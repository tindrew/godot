using System;

namespace Redot.SourceGenerators.Sample
{
    public partial class AllWriteOnly : RedotObject
    {
        private bool _writeOnlyBackingField = false;
        public bool WriteOnlyProperty { set => _writeOnlyBackingField = value; }
    }
}
