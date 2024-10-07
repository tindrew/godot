using Redot;
using System;

public partial class ExportedFields : RedotObject
{
    // Note we use Array and not System.Array. This tests the generated namespace qualification.
    [Export] private Int64[] _fieldEmptyInt64Array = Array.Empty<Int64>();
}
