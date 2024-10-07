using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Redot.SourceGenerators
{
    [Generator]
    public class ScriptSerializationGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.IsRedotSourceGeneratorDisabled("ScriptSerialization"))
                return;

            INamedTypeSymbol[] redotClasses = context
                .Compilation.SyntaxTrees
                .SelectMany(tree =>
                    tree.GetRoot().DescendantNodes()
                        .OfType<ClassDeclarationSyntax>()
                        .SelectRedotScriptClasses(context.Compilation)
                        // Report and skip non-partial classes
                        .Where(x =>
                        {
                            if (x.cds.IsPartial())
                            {
                                if (x.cds.IsNested() && !x.cds.AreAllOuterTypesPartial(out _))
                                {
                                    return false;
                                }

                                return true;
                            }

                            return false;
                        })
                        .Select(x => x.symbol)
                )
                .Distinct<INamedTypeSymbol>(SymbolEqualityComparer.Default)
                .ToArray();

            if (redotClasses.Length > 0)
            {
                var typeCache = new MarshalUtils.TypeCache(context.Compilation);

                foreach (var redotClass in redotClasses)
                {
                    VisitRedotScriptClass(context, typeCache, redotClass);
                }
            }
        }

        private static void VisitRedotScriptClass(
            GeneratorExecutionContext context,
            MarshalUtils.TypeCache typeCache,
            INamedTypeSymbol symbol
        )
        {
            INamespaceSymbol namespaceSymbol = symbol.ContainingNamespace;
            string classNs = namespaceSymbol != null && !namespaceSymbol.IsGlobalNamespace ?
                namespaceSymbol.FullQualifiedNameOmitGlobal() :
                string.Empty;
            bool hasNamespace = classNs.Length != 0;

            bool isInnerClass = symbol.ContainingType != null;

            string uniqueHint = symbol.FullQualifiedNameOmitGlobal().SanitizeQualifiedNameForUniqueHint()
                                + "_ScriptSerialization.generated";

            var source = new StringBuilder();

            source.Append("using Redot;\n");
            source.Append("using Redot.NativeInterop;\n");
            source.Append("\n");

            if (hasNamespace)
            {
                source.Append("namespace ");
                source.Append(classNs);
                source.Append(" {\n\n");
            }

            if (isInnerClass)
            {
                var containingType = symbol.ContainingType;
                AppendPartialContainingTypeDeclarations(containingType);

                void AppendPartialContainingTypeDeclarations(INamedTypeSymbol? containingType)
                {
                    if (containingType == null)
                        return;

                    AppendPartialContainingTypeDeclarations(containingType.ContainingType);

                    source.Append("partial ");
                    source.Append(containingType.GetDeclarationKeyword());
                    source.Append(" ");
                    source.Append(containingType.NameWithTypeParameters());
                    source.Append("\n{\n");
                }
            }

            source.Append("partial class ");
            source.Append(symbol.NameWithTypeParameters());
            source.Append("\n{\n");

            var members = symbol.GetMembers();

            var propertySymbols = members
                .Where(s => !s.IsStatic && s.Kind == SymbolKind.Property)
                .Cast<IPropertySymbol>()
                .Where(s => !s.IsIndexer && s.ExplicitInterfaceImplementations.Length == 0);

            var fieldSymbols = members
                .Where(s => !s.IsStatic && s.Kind == SymbolKind.Field && !s.IsImplicitlyDeclared)
                .Cast<IFieldSymbol>();

            // TODO: We should still restore read-only properties after reloading assembly. Two possible ways: reflection or turn RestoreRedotObjectData into a constructor overload.
            // Ignore properties without a getter, without a setter or with an init-only setter. Redot properties must be both readable and writable.
            var redotClassProperties = propertySymbols.Where(property => !(property.IsReadOnly || property.IsWriteOnly || property.SetMethod!.IsInitOnly))
                .WhereIsRedotCompatibleType(typeCache)
                .ToArray();
            var redotClassFields = fieldSymbols.Where(property => !property.IsReadOnly)
                .WhereIsRedotCompatibleType(typeCache)
                .ToArray();

            var signalDelegateSymbols = members
                .Where(s => s.Kind == SymbolKind.NamedType)
                .Cast<INamedTypeSymbol>()
                .Where(namedTypeSymbol => namedTypeSymbol.TypeKind == TypeKind.Delegate)
                .Where(s => s.GetAttributes()
                    .Any(a => a.AttributeClass?.IsRedotSignalAttribute() ?? false));

            List<RedotSignalDelegateData> redotSignalDelegates = new();

            foreach (var signalDelegateSymbol in signalDelegateSymbols)
            {
                if (!signalDelegateSymbol.Name.EndsWith(ScriptSignalsGenerator.SignalDelegateSuffix))
                    continue;

                string signalName = signalDelegateSymbol.Name;
                signalName = signalName.Substring(0,
                    signalName.Length - ScriptSignalsGenerator.SignalDelegateSuffix.Length);

                var invokeMethodData = signalDelegateSymbol
                    .DelegateInvokeMethod?.HasRedotCompatibleSignature(typeCache);

                if (invokeMethodData == null)
                    continue;

                redotSignalDelegates.Add(new(signalName, signalDelegateSymbol, invokeMethodData.Value));
            }

            source.Append("    /// <inheritdoc/>\n");
            source.Append("    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]\n");
            source.Append(
                "    protected override void SaveRedotObjectData(global::Redot.Bridge.RedotSerializationInfo info)\n    {\n");
            source.Append("        base.SaveRedotObjectData(info);\n");

            // Save properties

            foreach (var property in redotClassProperties)
            {
                string propertyName = property.PropertySymbol.Name;

                source.Append("        info.AddProperty(PropertyName.@")
                    .Append(propertyName)
                    .Append(", ")
                    .AppendManagedToVariantExpr(string.Concat("this.@", propertyName),
                        property.PropertySymbol.Type, property.Type)
                    .Append(");\n");
            }

            // Save fields

            foreach (var field in redotClassFields)
            {
                string fieldName = field.FieldSymbol.Name;

                source.Append("        info.AddProperty(PropertyName.@")
                    .Append(fieldName)
                    .Append(", ")
                    .AppendManagedToVariantExpr(string.Concat("this.@", fieldName),
                        field.FieldSymbol.Type, field.Type)
                    .Append(");\n");
            }

            // Save signal events

            foreach (var signalDelegate in redotSignalDelegates)
            {
                string signalName = signalDelegate.Name;

                source.Append("        info.AddSignalEventDelegate(SignalName.@")
                    .Append(signalName)
                    .Append(", this.backing_")
                    .Append(signalName)
                    .Append(");\n");
            }

            source.Append("    }\n");

            source.Append("    /// <inheritdoc/>\n");
            source.Append("    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]\n");
            source.Append(
                "    protected override void RestoreRedotObjectData(global::Redot.Bridge.RedotSerializationInfo info)\n    {\n");
            source.Append("        base.RestoreRedotObjectData(info);\n");

            // Restore properties

            foreach (var property in redotClassProperties)
            {
                string propertyName = property.PropertySymbol.Name;

                source.Append("        if (info.TryGetProperty(PropertyName.@")
                    .Append(propertyName)
                    .Append(", out var _value_")
                    .Append(propertyName)
                    .Append("))\n")
                    .Append("            this.@")
                    .Append(propertyName)
                    .Append(" = ")
                    .AppendVariantToManagedExpr(string.Concat("_value_", propertyName),
                        property.PropertySymbol.Type, property.Type)
                    .Append(";\n");
            }

            // Restore fields

            foreach (var field in redotClassFields)
            {
                string fieldName = field.FieldSymbol.Name;

                source.Append("        if (info.TryGetProperty(PropertyName.@")
                    .Append(fieldName)
                    .Append(", out var _value_")
                    .Append(fieldName)
                    .Append("))\n")
                    .Append("            this.@")
                    .Append(fieldName)
                    .Append(" = ")
                    .AppendVariantToManagedExpr(string.Concat("_value_", fieldName),
                        field.FieldSymbol.Type, field.Type)
                    .Append(";\n");
            }

            // Restore signal events

            foreach (var signalDelegate in redotSignalDelegates)
            {
                string signalName = signalDelegate.Name;
                string signalDelegateQualifiedName = signalDelegate.DelegateSymbol.FullQualifiedNameIncludeGlobal();

                source.Append("        if (info.TryGetSignalEventDelegate<")
                    .Append(signalDelegateQualifiedName)
                    .Append(">(SignalName.@")
                    .Append(signalName)
                    .Append(", out var _value_")
                    .Append(signalName)
                    .Append("))\n")
                    .Append("            this.backing_")
                    .Append(signalName)
                    .Append(" = _value_")
                    .Append(signalName)
                    .Append(";\n");
            }

            source.Append("    }\n");

            source.Append("}\n"); // partial class

            if (isInnerClass)
            {
                var containingType = symbol.ContainingType;

                while (containingType != null)
                {
                    source.Append("}\n"); // outer class

                    containingType = containingType.ContainingType;
                }
            }

            if (hasNamespace)
            {
                source.Append("\n}\n");
            }

            context.AddSource(uniqueHint, SourceText.From(source.ToString(), Encoding.UTF8));
        }
    }
}
