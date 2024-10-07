namespace Redot.SourceGenerators.Sample
{
    public partial class AllReadOnly : RedotObject
    {
        public readonly string ReadonlyField = "foo";
        public string ReadonlyAutoProperty { get; } = "foo";
        public string ReadonlyProperty { get => "foo"; }
        public string InitonlyAutoProperty { get; init; }
    }
}
