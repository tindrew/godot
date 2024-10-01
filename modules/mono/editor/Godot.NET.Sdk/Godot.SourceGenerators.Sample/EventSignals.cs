namespace Redot.SourceGenerators.Sample;

public partial class EventSignals : RedotObject
{
    [Signal]
    public delegate void MySignalEventHandler(string str, int num);
}
