using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Redot.SourceGenerators
{
    [Generator]
    public class ScriptSignalsGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.IsRedotSourceGeneratorDisabled("ScriptSignals"))
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

        internal static string SignalDelegateSuffix = "EventHandler";

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
                                + "_ScriptSignals.generated";

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

            var signalDelegateSymbols = members
                .Where(s => s.Kind == SymbolKind.NamedType)
                .Cast<INamedTypeSymbol>()
                .Where(namedTypeSymbol => namedTypeSymbol.TypeKind == TypeKind.Delegate)
                .Where(s => s.GetAttributes()
                    .Any(a => a.AttributeClass?.IsRedotSignalAttribute() ?? false));

            List<RedotSignalDelegateData> redotSignalDelegates = new();

            foreach (var signalDelegateSymbol in signalDelegateSymbols)
            {
                if (!signalDelegateSymbol.Name.EndsWith(SignalDelegateSuffix))
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        Common.SignalDelegateMissingSuffixRule,
                        signalDelegateSymbol.Locations.FirstLocationWithSourceTreeOrDefault(),
                        signalDelegateSymbol.ToDisplayString()
                    ));
                    continue;
                }

                string signalName = signalDelegateSymbol.Name;
                signalName = signalName.Substring(0, signalName.Length - SignalDelegateSuffix.Length);

                var invokeMethodData = signalDelegateSymbol
                    .DelegateInvokeMethod?.HasRedotCompatibleSignature(typeCache);

                if (invokeMethodData == null)
                {
                    if (signalDelegateSymbol.DelegateInvokeMethod is IMethodSymbol methodSymbol)
                    {
                        foreach (var parameter in methodSymbol.Parameters)
                        {
                            if (parameter.RefKind != RefKind.None)
                            {
                                context.ReportDiagnostic(Diagnostic.Create(
                                    Common.SignalParameterTypeNotSupportedRule,
                                    parameter.Locations.FirstLocationWithSourceTreeOrDefault(),
                                    parameter.ToDisplayString()
                                ));
                                continue;
                            }

                            var marshalType = MarshalUtils.ConvertManagedTypeToMarshalType(parameter.Type, typeCache);
                            if (marshalType == null)
                            {
                                context.ReportDiagnostic(Diagnostic.Create(
                                    Common.SignalParameterTypeNotSupportedRule,
                                    parameter.Locations.FirstLocationWithSourceTreeOrDefault(),
                                    parameter.ToDisplayString()
                                ));
                            }
                        }

                        if (!methodSymbol.ReturnsVoid)
                        {
                            context.ReportDiagnostic(Diagnostic.Create(
                                Common.SignalDelegateSignatureMustReturnVoidRule,
                                signalDelegateSymbol.Locations.FirstLocationWithSourceTreeOrDefault(),
                                signalDelegateSymbol.ToDisplayString()
                            ));
                        }
                    }

                    continue;
                }

                redotSignalDelegates.Add(new(signalName, signalDelegateSymbol, invokeMethodData.Value));
            }

            source.Append("#pragma warning disable CS0109 // Disable warning about redundant 'new' keyword\n");

            source.Append("    /// <summary>\n")
                .Append("    /// Cached StringNames for the signals contained in this class, for fast lookup.\n")
                .Append("    /// </summary>\n");

            source.Append(
                $"    public new class SignalName : {symbol.BaseType!.FullQualifiedNameIncludeGlobal()}.SignalName {{\n");

            // Generate cached StringNames for methods and properties, for fast lookup

            foreach (var signalDelegate in redotSignalDelegates)
            {
                string signalName = signalDelegate.Name;

                source.Append("        /// <summary>\n")
                    .Append("        /// Cached name for the '")
                    .Append(signalName)
                    .Append("' signal.\n")
                    .Append("        /// </summary>\n");

                source.Append("        public new static readonly global::Redot.StringName @");
                source.Append(signalName);
                source.Append(" = \"");
                source.Append(signalName);
                source.Append("\";\n");
            }

            source.Append("    }\n"); // class RedotInternal

            // Generate GetRedotSignalList

            if (redotSignalDelegates.Count > 0)
            {
                const string ListType = "global::System.Collections.Generic.List<global::Redot.Bridge.MethodInfo>";

                source.Append("    /// <summary>\n")
                    .Append("    /// Get the signal information for all the signals declared in this class.\n")
                    .Append("    /// This method is used by Redot to register the available signals in the editor.\n")
                    .Append("    /// Do not call this method.\n")
                    .Append("    /// </summary>\n");

                source.Append("    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]\n");

                source.Append("    internal new static ")
                    .Append(ListType)
                    .Append(" GetRedotSignalList()\n    {\n");

                source.Append("        var signals = new ")
                    .Append(ListType)
                    .Append("(")
                    .Append(redotSignalDelegates.Count)
                    .Append(");\n");

                foreach (var signalDelegateData in redotSignalDelegates)
                {
                    var methodInfo = DetermineMethodInfo(signalDelegateData);
                    AppendMethodInfo(source, methodInfo);
                }

                source.Append("        return signals;\n");
                source.Append("    }\n");
            }

            source.Append("#pragma warning restore CS0109\n");

            // Generate signal event

            foreach (var signalDelegate in redotSignalDelegates)
            {
                string signalName = signalDelegate.Name;

                // TODO: Hide backing event from code-completion and debugger
                // The reason we have a backing field is to hide the invoke method from the event,
                // as it doesn't emit the signal, only the event delegates. This can confuse users.
                // Maybe we should directly connect the delegates, as we do with native signals?
                source.Append("    private ")
                    .Append(signalDelegate.DelegateSymbol.FullQualifiedNameIncludeGlobal())
                    .Append(" backing_")
                    .Append(signalName)
                    .Append(";\n");

                source.Append(
                    $"    /// <inheritdoc cref=\"{signalDelegate.DelegateSymbol.FullQualifiedNameIncludeGlobal()}\"/>\n");

                source.Append($"    {signalDelegate.DelegateSymbol.GetAccessibilityKeyword()} event ")
                    .Append(signalDelegate.DelegateSymbol.FullQualifiedNameIncludeGlobal())
                    .Append(" @")
                    .Append(signalName)
                    .Append(" {\n")
                    .Append("        add => backing_")
                    .Append(signalName)
                    .Append(" += value;\n")
                    .Append("        remove => backing_")
                    .Append(signalName)
                    .Append(" -= value;\n")
                    .Append("}\n");

                // Generate On{EventName} method to raise the event

                var invokeMethodSymbol = signalDelegate.InvokeMethodData.Method;
                int paramCount = invokeMethodSymbol.Parameters.Length;

                string raiseMethodModifiers = signalDelegate.DelegateSymbol.ContainingType.IsSealed ?
                    "private" :
                    "protected";

                source.Append($"    {raiseMethodModifiers} void On{signalName}(");
                for (int i = 0; i < paramCount; i++)
                {
                    var paramSymbol = invokeMethodSymbol.Parameters[i];
                    source.Append($"{paramSymbol.Type.FullQualifiedNameIncludeGlobal()} {paramSymbol.Name}");
                    if (i < paramCount - 1)
                    {
                        source.Append(", ");
                    }
                }
                source.Append(")\n");
                source.Append("    {\n");
                source.Append($"        EmitSignal(SignalName.{signalName}");
                foreach (var paramSymbol in invokeMethodSymbol.Parameters)
                {
                    // Enums must be converted to the underlying type before they can be implicitly converted to Variant
                    if (paramSymbol.Type.TypeKind == TypeKind.Enum)
                    {
                        var underlyingType = ((INamedTypeSymbol)paramSymbol.Type).EnumUnderlyingType;
                        source.Append($", ({underlyingType.FullQualifiedNameIncludeGlobal()}){paramSymbol.Name}");
                        continue;
                    }

                    source.Append($", {paramSymbol.Name}");
                }
                source.Append(");\n");
                source.Append("    }\n");
            }

            // Generate RaiseRedotClassSignalCallbacks

            if (redotSignalDelegates.Count > 0)
            {
                source.Append("    /// <inheritdoc/>\n");
                source.Append("    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]\n");
                source.Append(
                    "    protected override void RaiseRedotClassSignalCallbacks(in redot_string_name signal, ");
                source.Append("NativeVariantPtrArgs args)\n    {\n");

                foreach (var signal in redotSignalDelegates)
                {
                    GenerateSignalEventInvoker(signal, source);
                }

                source.Append("        base.RaiseRedotClassSignalCallbacks(signal, args);\n");

                source.Append("    }\n");
            }

            // Generate HasRedotClassSignal

            if (redotSignalDelegates.Count > 0)
            {
                source.Append("    /// <inheritdoc/>\n");
                source.Append("    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]\n");
                source.Append(
                    "    protected override bool HasRedotClassSignal(in redot_string_name signal)\n    {\n");

                foreach (var signal in redotSignalDelegates)
                {
                    GenerateHasSignalEntry(signal.Name, source);
                }

                source.Append("        return base.HasRedotClassSignal(signal);\n");

                source.Append("    }\n");
            }

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

        private static void AppendMethodInfo(StringBuilder source, MethodInfo methodInfo)
        {
            source.Append("        signals.Add(new(name: SignalName.@")
                .Append(methodInfo.Name)
                .Append(", returnVal: ");

            AppendPropertyInfo(source, methodInfo.ReturnVal);

            source.Append(", flags: (global::Redot.MethodFlags)")
                .Append((int)methodInfo.Flags)
                .Append(", arguments: ");

            if (methodInfo.Arguments is { Count: > 0 })
            {
                source.Append("new() { ");

                foreach (var param in methodInfo.Arguments)
                {
                    AppendPropertyInfo(source, param);

                    // C# allows colon after the last element
                    source.Append(", ");
                }

                source.Append(" }");
            }
            else
            {
                source.Append("null");
            }

            source.Append(", defaultArguments: null));\n");
        }

        private static void AppendPropertyInfo(StringBuilder source, PropertyInfo propertyInfo)
        {
            source.Append("new(type: (global::Redot.Variant.Type)")
                .Append((int)propertyInfo.Type)
                .Append(", name: \"")
                .Append(propertyInfo.Name)
                .Append("\", hint: (global::Redot.PropertyHint)")
                .Append((int)propertyInfo.Hint)
                .Append(", hintString: \"")
                .Append(propertyInfo.HintString)
                .Append("\", usage: (global::Redot.PropertyUsageFlags)")
                .Append((int)propertyInfo.Usage)
                .Append(", exported: ")
                .Append(propertyInfo.Exported ? "true" : "false");
            if (propertyInfo.ClassName != null)
            {
                source.Append(", className: new global::Redot.StringName(\"")
                    .Append(propertyInfo.ClassName)
                    .Append("\")");
            }
            source.Append(")");
        }

        private static MethodInfo DetermineMethodInfo(RedotSignalDelegateData signalDelegateData)
        {
            var invokeMethodData = signalDelegateData.InvokeMethodData;

            PropertyInfo returnVal;

            if (invokeMethodData.RetType != null)
            {
                returnVal = DeterminePropertyInfo(invokeMethodData.RetType.Value.MarshalType,
                    invokeMethodData.RetType.Value.TypeSymbol,
                    name: string.Empty);
            }
            else
            {
                returnVal = new PropertyInfo(VariantType.Nil, string.Empty, PropertyHint.None,
                    hintString: null, PropertyUsageFlags.Default, exported: false);
            }

            int paramCount = invokeMethodData.ParamTypes.Length;

            List<PropertyInfo>? arguments;

            if (paramCount > 0)
            {
                arguments = new(capacity: paramCount);

                for (int i = 0; i < paramCount; i++)
                {
                    arguments.Add(DeterminePropertyInfo(invokeMethodData.ParamTypes[i],
                        invokeMethodData.Method.Parameters[i].Type,
                        name: invokeMethodData.Method.Parameters[i].Name));
                }
            }
            else
            {
                arguments = null;
            }

            return new MethodInfo(signalDelegateData.Name, returnVal, MethodFlags.Default, arguments,
                defaultArguments: null);
        }

        private static PropertyInfo DeterminePropertyInfo(MarshalType marshalType, ITypeSymbol typeSymbol, string name)
        {
            var memberVariantType = MarshalUtils.ConvertMarshalTypeToVariantType(marshalType)!.Value;

            var propUsage = PropertyUsageFlags.Default;

            if (memberVariantType == VariantType.Nil)
                propUsage |= PropertyUsageFlags.NilIsVariant;

            string? className = null;
            if (memberVariantType == VariantType.Object && typeSymbol is INamedTypeSymbol namedTypeSymbol)
            {
                className = namedTypeSymbol.GetRedotScriptNativeClassName();
            }

            return new PropertyInfo(memberVariantType, name,
                PropertyHint.None, string.Empty, propUsage, className, exported: false);
        }

        private static void GenerateHasSignalEntry(
            string signalName,
            StringBuilder source
        )
        {
            source.Append("        ");
            source.Append("if (signal == SignalName.@");
            source.Append(signalName);
            source.Append(") {\n           return true;\n        }\n");
        }

        private static void GenerateSignalEventInvoker(
            RedotSignalDelegateData signal,
            StringBuilder source
        )
        {
            string signalName = signal.Name;
            var invokeMethodData = signal.InvokeMethodData;

            source.Append("        if (signal == SignalName.@");
            source.Append(signalName);
            source.Append(" && args.Count == ");
            source.Append(invokeMethodData.ParamTypes.Length);
            source.Append(") {\n");
            source.Append("            backing_");
            source.Append(signalName);
            source.Append("?.Invoke(");

            for (int i = 0; i < invokeMethodData.ParamTypes.Length; i++)
            {
                if (i != 0)
                    source.Append(", ");

                source.AppendNativeVariantToManagedExpr(string.Concat("args[", i.ToString(), "]"),
                    invokeMethodData.ParamTypeSymbols[i], invokeMethodData.ParamTypes[i]);
            }

            source.Append(");\n");

            source.Append("            return;\n");

            source.Append("        }\n");
        }
    }
}
