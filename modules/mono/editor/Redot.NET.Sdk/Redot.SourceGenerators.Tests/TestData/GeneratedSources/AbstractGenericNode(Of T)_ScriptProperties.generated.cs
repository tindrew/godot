using Redot;
using Redot.NativeInterop;

partial class AbstractGenericNode<T>
{
#pragma warning disable CS0109 // Disable warning about redundant 'new' keyword
    /// <summary>
    /// Cached StringNames for the properties and fields contained in this class, for fast lookup.
    /// </summary>
    public new class PropertyName : global::Redot.Node.PropertyName {
        /// <summary>
        /// Cached name for the 'MyArray' property.
        /// </summary>
        public new static readonly global::Redot.StringName @MyArray = "MyArray";
    }
    /// <inheritdoc/>
    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    protected override bool SetRedotClassPropertyValue(in redot_string_name name, in redot_variant value)
    {
        if (name == PropertyName.@MyArray) {
            this.@MyArray = global::Redot.NativeInterop.VariantUtils.ConvertToArray<T>(value);
            return true;
        }
        return base.SetRedotClassPropertyValue(name, value);
    }
    /// <inheritdoc/>
    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    protected override bool GetRedotClassPropertyValue(in redot_string_name name, out redot_variant value)
    {
        if (name == PropertyName.@MyArray) {
            value = global::Redot.NativeInterop.VariantUtils.CreateFromArray(this.@MyArray);
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
        properties.Add(new(type: (global::Redot.Variant.Type)28, name: PropertyName.@MyArray, hint: (global::Redot.PropertyHint)0, hintString: "", usage: (global::Redot.PropertyUsageFlags)4102, exported: true));
        return properties;
    }
#pragma warning restore CS0109
}
