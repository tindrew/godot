using Redot;
using Redot.NativeInterop;

partial struct OuterClass
{
partial class NestedClass
{
    /// <inheritdoc/>
    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    protected override void SaveRedotObjectData(global::Redot.Bridge.RedotSerializationInfo info)
    {
        base.SaveRedotObjectData(info);
    }
    /// <inheritdoc/>
    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    protected override void RestoreRedotObjectData(global::Redot.Bridge.RedotSerializationInfo info)
    {
        base.RestoreRedotObjectData(info);
    }
}
}
