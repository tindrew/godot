using System;
using System.Runtime.InteropServices;
using Redot.NativeInterop;

namespace Redot
{
    public static class Dispatcher
    {
        internal static RedotTaskScheduler DefaultRedotTaskScheduler;

        internal static void InitializeDefaultRedotTaskScheduler()
        {
            DefaultRedotTaskScheduler?.Dispose();
            DefaultRedotTaskScheduler = new RedotTaskScheduler();
        }

        public static RedotSynchronizationContext SynchronizationContext => DefaultRedotTaskScheduler.Context;
    }
}
