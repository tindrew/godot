using Redot;
using Redot.NativeInterop;

partial struct OuterClass
{
partial class NestedClass
{
#pragma warning disable CS0109 // Disable warning about redundant 'new' keyword
    /// <summary>
    /// Cached StringNames for the methods contained in this class, for fast lookup.
    /// </summary>
    public new class MethodName : global::Redot.RefCounted.MethodName {
        /// <summary>
        /// Cached name for the '_Get' method.
        /// </summary>
        public new static readonly global::Redot.StringName @_Get = "_Get";
    }
    /// <summary>
    /// Get the method information for all the methods declared in this class.
    /// This method is used by Redot to register the available methods in the editor.
    /// Do not call this method.
    /// </summary>
    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    internal new static global::System.Collections.Generic.List<global::Redot.Bridge.MethodInfo> GetRedotMethodList()
    {
        var methods = new global::System.Collections.Generic.List<global::Redot.Bridge.MethodInfo>(1);
        methods.Add(new(name: MethodName.@_Get, returnVal: new(type: (global::Redot.Variant.Type)0, name: "", hint: (global::Redot.PropertyHint)0, hintString: "", usage: (global::Redot.PropertyUsageFlags)131078, exported: false), flags: (global::Redot.MethodFlags)1, arguments: new() { new(type: (global::Redot.Variant.Type)21, name: "property", hint: (global::Redot.PropertyHint)0, hintString: "", usage: (global::Redot.PropertyUsageFlags)6, exported: false),  }, defaultArguments: null));
        return methods;
    }
#pragma warning restore CS0109
    /// <inheritdoc/>
    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    protected override bool InvokeRedotClassMethod(in redot_string_name method, NativeVariantPtrArgs args, out redot_variant ret)
    {
        if (method == MethodName.@_Get && args.Count == 1) {
            var callRet = @_Get(global::Redot.NativeInterop.VariantUtils.ConvertTo<global::Redot.StringName>(args[0]));
            ret = global::Redot.NativeInterop.VariantUtils.CreateFrom<global::Redot.Variant>(callRet);
            return true;
        }
        return base.InvokeRedotClassMethod(method, args, out ret);
    }
    /// <inheritdoc/>
    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    protected override bool HasRedotClassMethod(in redot_string_name method)
    {
        if (method == MethodName.@_Get) {
           return true;
        }
        return base.HasRedotClassMethod(method);
    }
}
}
