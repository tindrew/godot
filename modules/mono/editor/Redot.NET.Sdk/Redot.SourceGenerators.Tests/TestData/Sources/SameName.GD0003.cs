using Redot;

namespace NamespaceA
{
    partial class SameName : RedotObject
    {
        private int _field;
    }
}

// SameName again but different namespace
namespace NamespaceB
{
    partial class {|GD0003:SameName|} : RedotObject
    {
        private int _field;
    }
}
