using Redot;
using Redot.NativeInterop;

partial class Methods
{
#pragma warning disable CS0109 // Disable warning about redundant 'new' keyword
    /// <summary>
    /// Cached StringNames for the methods contained in this class, for fast lookup.
    /// </summary>
    public new class MethodName : global::Redot.RedotObject.MethodName {
        /// <summary>
        /// Cached name for the 'MethodWithOverload' method.
        /// </summary>
        public new static readonly global::Redot.StringName @MethodWithOverload = "MethodWithOverload";
    }
    /// <summary>
    /// Get the method information for all the methods declared in this class.
    /// This method is used by Redot to register the available methods in the editor.
    /// Do not call this method.
    /// </summary>
    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    internal new static global::System.Collections.Generic.List<global::Redot.Bridge.MethodInfo> GetRedotMethodList()
    {
        var methods = new global::System.Collections.Generic.List<global::Redot.Bridge.MethodInfo>(3);
        methods.Add(new(name: MethodName.@MethodWithOverload, returnVal: new(type: (global::Redot.Variant.Type)0, name: "", hint: (global::Redot.PropertyHint)0, hintString: "", usage: (global::Redot.PropertyUsageFlags)6, exported: false), flags: (global::Redot.MethodFlags)1, arguments: null, defaultArguments: null));
        methods.Add(new(name: MethodName.@MethodWithOverload, returnVal: new(type: (global::Redot.Variant.Type)0, name: "", hint: (global::Redot.PropertyHint)0, hintString: "", usage: (global::Redot.PropertyUsageFlags)6, exported: false), flags: (global::Redot.MethodFlags)1, arguments: new() { new(type: (global::Redot.Variant.Type)2, name: "a", hint: (global::Redot.PropertyHint)0, hintString: "", usage: (global::Redot.PropertyUsageFlags)6, exported: false),  }, defaultArguments: null));
        methods.Add(new(name: MethodName.@MethodWithOverload, returnVal: new(type: (global::Redot.Variant.Type)0, name: "", hint: (global::Redot.PropertyHint)0, hintString: "", usage: (global::Redot.PropertyUsageFlags)6, exported: false), flags: (global::Redot.MethodFlags)1, arguments: new() { new(type: (global::Redot.Variant.Type)2, name: "a", hint: (global::Redot.PropertyHint)0, hintString: "", usage: (global::Redot.PropertyUsageFlags)6, exported: false), new(type: (global::Redot.Variant.Type)2, name: "b", hint: (global::Redot.PropertyHint)0, hintString: "", usage: (global::Redot.PropertyUsageFlags)6, exported: false),  }, defaultArguments: null));
        return methods;
    }
#pragma warning restore CS0109
    /// <inheritdoc/>
    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    protected override bool InvokeRedotClassMethod(in redot_string_name method, NativeVariantPtrArgs args, out redot_variant ret)
    {
        if (method == MethodName.@MethodWithOverload && args.Count == 0) {
            @MethodWithOverload();
            ret = default;
            return true;
        }
        if (method == MethodName.@MethodWithOverload && args.Count == 1) {
            @MethodWithOverload(global::Redot.NativeInterop.VariantUtils.ConvertTo<int>(args[0]));
            ret = default;
            return true;
        }
        if (method == MethodName.@MethodWithOverload && args.Count == 2) {
            @MethodWithOverload(global::Redot.NativeInterop.VariantUtils.ConvertTo<int>(args[0]), global::Redot.NativeInterop.VariantUtils.ConvertTo<int>(args[1]));
            ret = default;
            return true;
        }
        return base.InvokeRedotClassMethod(method, args, out ret);
    }
    /// <inheritdoc/>
    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    protected override bool HasRedotClassMethod(in redot_string_name method)
    {
        if (method == MethodName.@MethodWithOverload) {
           return true;
        }
        return base.HasRedotClassMethod(method);
    }
}
