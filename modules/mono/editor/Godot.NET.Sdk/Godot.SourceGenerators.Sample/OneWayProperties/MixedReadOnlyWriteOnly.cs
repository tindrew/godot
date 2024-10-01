namespace Redot.SourceGenerators.Sample
{
    public partial class MixedReadonlyWriteOnly : RedotObject
    {
        public readonly string ReadOnlyField = "foo";
        public string ReadOnlyAutoProperty { get; } = "foo";
        public string ReadOnlyProperty { get => "foo"; }
        public string InitOnlyAutoProperty { get; init; }

        private bool _writeOnlyBackingField = false;
        public bool WriteOnlyProperty { set => _writeOnlyBackingField = value; }
    }
}
