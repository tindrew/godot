using Redot;

public abstract partial class AbstractGenericNode<[MustBeVariant] T> : Node
{
    [Export] // This should be included, but without type hints.
    public Redot.Collections.Array<T> MyArray { get; set; } = new();
}
