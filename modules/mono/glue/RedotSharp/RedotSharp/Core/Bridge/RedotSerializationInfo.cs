using System;
using System.Diagnostics.CodeAnalysis;
using Redot.NativeInterop;

namespace Redot.Bridge;

public sealed class RedotSerializationInfo : IDisposable
{
    private readonly Collections.Dictionary _properties;
    private readonly Collections.Dictionary _signalEvents;

    public void Dispose()
    {
        _properties?.Dispose();
        _signalEvents?.Dispose();

        GC.SuppressFinalize(this);
    }

    private RedotSerializationInfo(in redot_dictionary properties, in redot_dictionary signalEvents)
    {
        _properties = Collections.Dictionary.CreateTakingOwnershipOfDisposableValue(properties);
        _signalEvents = Collections.Dictionary.CreateTakingOwnershipOfDisposableValue(signalEvents);
    }

    internal static RedotSerializationInfo CreateCopyingBorrowed(
        in redot_dictionary properties, in redot_dictionary signalEvents)
    {
        return new(NativeFuncs.redotsharp_dictionary_new_copy(properties),
            NativeFuncs.redotsharp_dictionary_new_copy(signalEvents));
    }

    public void AddProperty(StringName name, Variant value)
    {
        _properties[name] = value;
    }

    public bool TryGetProperty(StringName name, out Variant value)
    {
        return _properties.TryGetValue(name, out value);
    }

    public void AddSignalEventDelegate(StringName name, Delegate eventDelegate)
    {
        var serializedData = new Collections.Array();

        if (DelegateUtils.TrySerializeDelegate(eventDelegate, serializedData))
        {
            _signalEvents[name] = serializedData;
        }
        else if (OS.IsStdOutVerbose())
        {
            Console.WriteLine($"Failed to serialize event signal delegate: {name}");
        }
    }

    public bool TryGetSignalEventDelegate<T>(StringName name, [MaybeNullWhen(false)] out T value)
        where T : Delegate
    {
        if (_signalEvents.TryGetValue(name, out Variant serializedData))
        {
            if (DelegateUtils.TryDeserializeDelegate(serializedData.AsRedotArray(), out var eventDelegate))
            {
                value = eventDelegate as T;

                if (value == null)
                {
                    Console.WriteLine($"Cannot cast the deserialized event signal delegate: {name}. " +
                                      $"Expected '{typeof(T).FullName}'; got '{eventDelegate.GetType().FullName}'.");
                    return false;
                }

                return true;
            }
            else if (OS.IsStdOutVerbose())
            {
                Console.WriteLine($"Failed to deserialize event signal delegate: {name}");
            }

            value = null;
            return false;
        }

        value = null;
        return false;
    }
}
