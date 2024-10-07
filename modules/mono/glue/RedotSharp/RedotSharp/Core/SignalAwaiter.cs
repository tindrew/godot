using System;
using System.Runtime.InteropServices;
using Redot.NativeInterop;

namespace Redot
{
    public class SignalAwaiter : IAwaiter<Variant[]>, IAwaitable<Variant[]>
    {
        private bool _completed;
        private Variant[] _result;
        private Action _continuation;

        public SignalAwaiter(RedotObject source, StringName signal, RedotObject target)
        {
            var awaiterGcHandle = CustomGCHandle.AllocStrong(this);
            using redot_string_name signalSrc = NativeFuncs.redotsharp_string_name_new_copy(
                (redot_string_name)(signal?.NativeValue ?? default));
            NativeFuncs.redotsharp_internal_signal_awaiter_connect(RedotObject.GetPtr(source), in signalSrc,
                RedotObject.GetPtr(target), GCHandle.ToIntPtr(awaiterGcHandle));
        }

        public bool IsCompleted => _completed;

        public void OnCompleted(Action continuation)
        {
            _continuation = continuation;
        }

        public Variant[] GetResult() => _result;

        public IAwaiter<Variant[]> GetAwaiter() => this;

        [UnmanagedCallersOnly]
        internal static unsafe void SignalCallback(IntPtr awaiterGCHandlePtr, redot_variant** args, int argCount,
            redot_bool* outAwaiterIsNull)
        {
            try
            {
                var awaiter = (SignalAwaiter)GCHandle.FromIntPtr(awaiterGCHandlePtr).Target;

                if (awaiter == null)
                {
                    *outAwaiterIsNull = redot_bool.True;
                    return;
                }

                *outAwaiterIsNull = redot_bool.False;

                awaiter._completed = true;

                Variant[] signalArgs = new Variant[argCount];

                for (int i = 0; i < argCount; i++)
                    signalArgs[i] = Variant.CreateCopyingBorrowed(*args[i]);

                awaiter._result = signalArgs;

                awaiter._continuation?.Invoke();
            }
            catch (Exception e)
            {
                ExceptionUtils.LogException(e);
                *outAwaiterIsNull = redot_bool.False;
            }
        }
    }
}
