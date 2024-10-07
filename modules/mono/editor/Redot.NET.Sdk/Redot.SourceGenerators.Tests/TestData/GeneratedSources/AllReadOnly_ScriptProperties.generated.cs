using Redot;
using Redot.NativeInterop;

partial class AllReadOnly
{
#pragma warning disable CS0109 // Disable warning about redundant 'new' keyword
    /// <summary>
    /// Cached StringNames for the properties and fields contained in this class, for fast lookup.
    /// </summary>
    public new class PropertyName : global::Redot.RedotObject.PropertyName {
        /// <summary>
        /// Cached name for the 'ReadOnlyAutoProperty' property.
        /// </summary>
        public new static readonly global::Redot.StringName @ReadOnlyAutoProperty = "ReadOnlyAutoProperty";
        /// <summary>
        /// Cached name for the 'ReadOnlyProperty' property.
        /// </summary>
        public new static readonly global::Redot.StringName @ReadOnlyProperty = "ReadOnlyProperty";
        /// <summary>
        /// Cached name for the 'InitOnlyAutoProperty' property.
        /// </summary>
        public new static readonly global::Redot.StringName @InitOnlyAutoProperty = "InitOnlyAutoProperty";
        /// <summary>
        /// Cached name for the 'ReadOnlyField' field.
        /// </summary>
        public new static readonly global::Redot.StringName @ReadOnlyField = "ReadOnlyField";
    }
    /// <inheritdoc/>
    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    protected override bool GetRedotClassPropertyValue(in redot_string_name name, out redot_variant value)
    {
        if (name == PropertyName.@ReadOnlyAutoProperty) {
            value = global::Redot.NativeInterop.VariantUtils.CreateFrom<string>(this.@ReadOnlyAutoProperty);
            return true;
        }
        if (name == PropertyName.@ReadOnlyProperty) {
            value = global::Redot.NativeInterop.VariantUtils.CreateFrom<string>(this.@ReadOnlyProperty);
            return true;
        }
        if (name == PropertyName.@InitOnlyAutoProperty) {
            value = global::Redot.NativeInterop.VariantUtils.CreateFrom<string>(this.@InitOnlyAutoProperty);
            return true;
        }
        if (name == PropertyName.@ReadOnlyField) {
            value = global::Redot.NativeInterop.VariantUtils.CreateFrom<string>(this.@ReadOnlyField);
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
        properties.Add(new(type: (global::Redot.Variant.Type)4, name: PropertyName.@ReadOnlyField, hint: (global::Redot.PropertyHint)0, hintString: "", usage: (global::Redot.PropertyUsageFlags)4096, exported: false));
        properties.Add(new(type: (global::Redot.Variant.Type)4, name: PropertyName.@ReadOnlyAutoProperty, hint: (global::Redot.PropertyHint)0, hintString: "", usage: (global::Redot.PropertyUsageFlags)4096, exported: false));
        properties.Add(new(type: (global::Redot.Variant.Type)4, name: PropertyName.@ReadOnlyProperty, hint: (global::Redot.PropertyHint)0, hintString: "", usage: (global::Redot.PropertyUsageFlags)4096, exported: false));
        properties.Add(new(type: (global::Redot.Variant.Type)4, name: PropertyName.@InitOnlyAutoProperty, hint: (global::Redot.PropertyHint)0, hintString: "", usage: (global::Redot.PropertyUsageFlags)4096, exported: false));
        return properties;
    }
#pragma warning restore CS0109
}
