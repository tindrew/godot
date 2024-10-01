using Redot;

// This works because it inherits from RedotObject and it doesn't have any generic type parameter.
[GlobalClass]
public partial class CustomGlobalClass : RedotObject
{

}

// This raises a GD0402 diagnostic error: global classes can't have any generic type parameter
[GlobalClass]
public partial class {|GD0402:CustomGlobalClass|}<T> : RedotObject
{

}
