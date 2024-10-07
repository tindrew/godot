using Redot;

// This works because it inherits from RedotObject.
[GlobalClass]
public partial class CustomGlobalClass1 : RedotObject
{

}

// This works because it inherits from an object that inherits from RedotObject
[GlobalClass]
public partial class CustomGlobalClass2 : Node
{

}

// This raises a GD0401 diagnostic error: global classes must inherit from RedotObject
[GlobalClass]
public partial class {|GD0401:CustomGlobalClass3|}
{

}
