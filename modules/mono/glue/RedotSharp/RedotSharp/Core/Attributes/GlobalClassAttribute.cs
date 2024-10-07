using System;

#nullable enable

namespace Redot
{
    /// <summary>
    /// Exposes the target class as a global script class to Redot Engine.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class GlobalClassAttribute : Attribute { }
}
