using Redot;
using Redot.NativeInterop;

partial class ScriptBoilerplate
{
    /// <inheritdoc/>
    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    protected override void SaveRedotObjectData(global::Redot.Bridge.RedotSerializationInfo info)
    {
        base.SaveRedotObjectData(info);
        info.AddProperty(PropertyName.@_nodePath, global::Redot.Variant.From<global::Redot.NodePath>(this.@_nodePath));
        info.AddProperty(PropertyName.@_velocity, global::Redot.Variant.From<int>(this.@_velocity));
    }
    /// <inheritdoc/>
    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    protected override void RestoreRedotObjectData(global::Redot.Bridge.RedotSerializationInfo info)
    {
        base.RestoreRedotObjectData(info);
        if (info.TryGetProperty(PropertyName.@_nodePath, out var _value__nodePath))
            this.@_nodePath = _value__nodePath.As<global::Redot.NodePath>();
        if (info.TryGetProperty(PropertyName.@_velocity, out var _value__velocity))
            this.@_velocity = _value__velocity.As<int>();
    }
}
