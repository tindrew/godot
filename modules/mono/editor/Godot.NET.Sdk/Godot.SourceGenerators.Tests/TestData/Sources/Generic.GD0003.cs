using Redot;

public partial class Generic<T> : RedotObject
{
    private int _field;
}

// Generic again but different generic parameters
public partial class {|GD0003:Generic|}<T, R> : RedotObject
{
    private int _field;
}

// Generic again but without generic parameters
public partial class {|GD0003:Generic|} : RedotObject
{
    private int _field;
}
