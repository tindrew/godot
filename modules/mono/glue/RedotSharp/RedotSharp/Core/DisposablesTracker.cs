using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using Redot.NativeInterop;

#nullable enable

namespace Redot
{
    internal static class DisposablesTracker
    {
        [UnmanagedCallersOnly]
        internal static void OnRedotShuttingDown()
        {
            try
            {
                OnRedotShuttingDownImpl();
            }
            catch (Exception e)
            {
                ExceptionUtils.LogException(e);
            }
        }

        private static void OnRedotShuttingDownImpl()
        {
            bool isStdoutVerbose;

            try
            {
                isStdoutVerbose = OS.IsStdOutVerbose();
            }
            catch (ObjectDisposedException)
            {
                // OS singleton already disposed. Maybe OnUnloading was called twice.
                isStdoutVerbose = false;
            }

            if (isStdoutVerbose)
                GD.Print("Unloading: Disposing tracked instances...");

            // Dispose Redot Objects first, and only then dispose other disposables
            // like StringName, NodePath, Redot.Collections.Array/Dictionary, etc.
            // The Redot Object Dispose() method may need any of the later instances.

            foreach (WeakReference<RedotObject> item in RedotObjectInstances.Keys)
            {
                if (item.TryGetTarget(out RedotObject? self))
                    self.Dispose();
            }

            foreach (WeakReference<IDisposable> item in OtherInstances.Keys)
            {
                if (item.TryGetTarget(out IDisposable? self))
                    self.Dispose();
            }

            if (isStdoutVerbose)
                GD.Print("Unloading: Finished disposing tracked instances.");
        }

        private static ConcurrentDictionary<WeakReference<RedotObject>, byte> RedotObjectInstances { get; } =
            new();

        private static ConcurrentDictionary<WeakReference<IDisposable>, byte> OtherInstances { get; } =
            new();

        public static WeakReference<RedotObject> RegisterRedotObject(RedotObject redotObject)
        {
            var weakReferenceToSelf = new WeakReference<RedotObject>(redotObject);
            RedotObjectInstances.TryAdd(weakReferenceToSelf, 0);
            return weakReferenceToSelf;
        }

        public static WeakReference<IDisposable> RegisterDisposable(IDisposable disposable)
        {
            var weakReferenceToSelf = new WeakReference<IDisposable>(disposable);
            OtherInstances.TryAdd(weakReferenceToSelf, 0);
            return weakReferenceToSelf;
        }

        public static void UnregisterRedotObject(RedotObject redotObject, WeakReference<RedotObject> weakReferenceToSelf)
        {
            if (!RedotObjectInstances.TryRemove(weakReferenceToSelf, out _))
                throw new ArgumentException("Redot Object not registered.", nameof(weakReferenceToSelf));
        }

        public static void UnregisterDisposable(WeakReference<IDisposable> weakReference)
        {
            if (!OtherInstances.TryRemove(weakReference, out _))
                throw new ArgumentException("Disposable not registered.", nameof(weakReference));
        }
    }
}
