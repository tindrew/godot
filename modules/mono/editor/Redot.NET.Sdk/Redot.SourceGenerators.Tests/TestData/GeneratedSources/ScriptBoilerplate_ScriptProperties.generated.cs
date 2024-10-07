using Redot;
using Redot.NativeInterop;

partial class ScriptBoilerplate
{
#pragma warning disable CS0109 // Disable warning about redundant 'new' keyword
    /// <summary>
    /// Cached StringNames for the properties and fields contained in this class, for fast lookup.
    /// </summary>
    public new class PropertyName : global::Redot.Node.PropertyName {
        /// <summary>
        /// Cached name for the '_nodePath' field.
        /// </summary>
        public new static readonly global::Redot.StringName @_nodePath = "_nodePath";
        /// <summary>
        /// Cached name for the '_velocity' field.
        /// </summary>
        public new static readonly global::Redot.StringName @_velocity = "_velocity";
    }
    /// <inheritdoc/>
    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    protected override bool SetRedotClassPropertyValue(in redot_string_name name, in redot_variant value)
    {
        if (name == PropertyName.@_nodePath) {
            this.@_nodePath = global::Redot.NativeInterop.VariantUtils.ConvertTo<global::Redot.NodePath>(value);
            return true;
        }
        if (name == PropertyName.@_velocity) {
            this.@_velocity = global::Redot.NativeInterop.VariantUtils.ConvertTo<int>(value);
            return true;
        }
        return base.SetRedotClassPropertyValue(name, value);
    }
    /// <inheritdoc/>
    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    protected override bool GetRedotClassPropertyValue(in redot_string_name name, out redot_variant value)
    {
        if (name == PropertyName.@_nodePath) {
            value = global::Redot.NativeInterop.VariantUtils.CreateFrom<global::Redot.NodePath>(this.@_nodePath);
            return true;
        }
        if (name == PropertyName.@_velocity) {
            value = global::Redot.NativeInterop.VariantUtils.CreateFrom<int>(this.@_velocity);
            return true;
        }
        return base.GetRedotClassPropertyValue(name, out value);
    }
    /// <summary>
    /// Get the property information for all the properties declared in this class.
    /// This method is used by Redot to register the available properties in the editor.
    /// Do not call this method.
    /// </summary>
    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    internal new static global::System.Collections.Generic.List<global::Redot.Bridge.PropertyInfo> GetRedotPropertyList()
    {
        var properties = new global::System.Collections.Generic.List<global::Redot.Bridge.PropertyInfo>();
        properties.Add(new(type: (global::Redot.Variant.Type)22, name: PropertyName.@_nodePath, hint: (global::Redot.PropertyHint)0, hintString: "", usage: (global::Redot.PropertyUsageFlags)4096, exported: false));
        properties.Add(new(type: (global::Redot.Variant.Type)2, name: PropertyName.@_velocity, hint: (global::Redot.PropertyHint)0, hintString: "", usage: (global::Redot.PropertyUsageFlags)4096, exported: false));
        return properties;
    }
#pragma warning restore CS0109
}
