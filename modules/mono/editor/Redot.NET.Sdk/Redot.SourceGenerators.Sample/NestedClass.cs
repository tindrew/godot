using System;

namespace Redot.SourceGenerators.Sample;

public partial class NestedClass : RedotObject
{
    public partial class NestedClass2 : RedotObject
    {
        public partial class NestedClass3 : RedotObject
        {
            [Signal]
            public delegate void MySignalEventHandler(string str, int num);

            [Export] private String _fieldString = "foo";
            [Export] private String PropertyString { get; set; } = "foo";

            private void Method()
            {
            }
        }
    }
}
