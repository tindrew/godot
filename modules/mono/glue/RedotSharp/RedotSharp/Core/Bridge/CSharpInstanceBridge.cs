using System;
using System.Runtime.InteropServices;
using Redot.NativeInterop;

namespace Redot.Bridge
{
    internal static class CSharpInstanceBridge
    {
        [UnmanagedCallersOnly]
        internal static unsafe redot_bool Call(IntPtr redotObjectGCHandle, redot_string_name* method,
            redot_variant** args, int argCount, redot_variant_call_error* refCallError, redot_variant* ret)
        {
            try
            {
                var redotObject = (RedotObject)GCHandle.FromIntPtr(redotObjectGCHandle).Target;

                if (redotObject == null)
                {
                    *ret = default;
                    (*refCallError).Error = redot_variant_call_error_error.redot_CALL_ERROR_CALL_ERROR_INSTANCE_IS_NULL;
                    return redot_bool.False;
                }

                bool methodInvoked = redotObject.InvokeRedotClassMethod(CustomUnsafe.AsRef(method),
                    new NativeVariantPtrArgs(args, argCount), out redot_variant retValue);

                if (!methodInvoked)
                {
                    *ret = default;
                    // This is important, as it tells Object::call that no method was called.
                    // Otherwise, it would prevent Object::call from calling native methods.
                    (*refCallError).Error = redot_variant_call_error_error.redot_CALL_ERROR_CALL_ERROR_INVALID_METHOD;
                    return redot_bool.False;
                }

                *ret = retValue;
                return redot_bool.True;
            }
            catch (Exception e)
            {
                ExceptionUtils.LogException(e);
                *ret = default;
                return redot_bool.False;
            }
        }

        [UnmanagedCallersOnly]
        internal static unsafe redot_bool Set(IntPtr redotObjectGCHandle, redot_string_name* name, redot_variant* value)
        {
            try
            {
                var redotObject = (RedotObject)GCHandle.FromIntPtr(redotObjectGCHandle).Target;

                if (redotObject == null)
                    throw new InvalidOperationException();

                if (redotObject.SetRedotClassPropertyValue(CustomUnsafe.AsRef(name), CustomUnsafe.AsRef(value)))
                {
                    return redot_bool.True;
                }

                var nameManaged = StringName.CreateTakingOwnershipOfDisposableValue(
                    NativeFuncs.redotsharp_string_name_new_copy(CustomUnsafe.AsRef(name)));

                Variant valueManaged = Variant.CreateCopyingBorrowed(*value);

                return redotObject._Set(nameManaged, valueManaged).ToRedotBool();
            }
            catch (Exception e)
            {
                ExceptionUtils.LogException(e);
                return redot_bool.False;
            }
        }

        [UnmanagedCallersOnly]
        internal static unsafe redot_bool Get(IntPtr redotObjectGCHandle, redot_string_name* name,
            redot_variant* outRet)
        {
            try
            {
                var redotObject = (RedotObject)GCHandle.FromIntPtr(redotObjectGCHandle).Target;

                if (redotObject == null)
                    throw new InvalidOperationException();

                // Properties
                if (redotObject.GetRedotClassPropertyValue(CustomUnsafe.AsRef(name), out redot_variant outRetValue))
                {
                    *outRet = outRetValue;
                    return redot_bool.True;
                }

                // Signals
                if (redotObject.HasRedotClassSignal(CustomUnsafe.AsRef(name)))
                {
                    redot_signal signal = new redot_signal(NativeFuncs.redotsharp_string_name_new_copy(*name), redotObject.GetInstanceId());
                    *outRet = VariantUtils.CreateFromSignalTakingOwnershipOfDisposableValue(signal);
                    return redot_bool.True;
                }

                // Methods
                if (redotObject.HasRedotClassMethod(CustomUnsafe.AsRef(name)))
                {
                    redot_callable method = new redot_callable(NativeFuncs.redotsharp_string_name_new_copy(*name), redotObject.GetInstanceId());
                    *outRet = VariantUtils.CreateFromCallableTakingOwnershipOfDisposableValue(method);
                    return redot_bool.True;
                }

                var nameManaged = StringName.CreateTakingOwnershipOfDisposableValue(
                    NativeFuncs.redotsharp_string_name_new_copy(CustomUnsafe.AsRef(name)));

                Variant ret = redotObject._Get(nameManaged);

                if (ret.VariantType == Variant.Type.Nil)
                {
                    *outRet = default;
                    return redot_bool.False;
                }

                *outRet = ret.CopyNativeVariant();
                return redot_bool.True;
            }
            catch (Exception e)
            {
                ExceptionUtils.LogException(e);
                *outRet = default;
                return redot_bool.False;
            }
        }

        [UnmanagedCallersOnly]
        internal static void CallDispose(IntPtr redotObjectGCHandle, redot_bool okIfNull)
        {
            try
            {
                var redotObject = (RedotObject)GCHandle.FromIntPtr(redotObjectGCHandle).Target;

                if (okIfNull.ToBool())
                    redotObject?.Dispose();
                else
                    redotObject!.Dispose();
            }
            catch (Exception e)
            {
                ExceptionUtils.LogException(e);
            }
        }

        [UnmanagedCallersOnly]
        internal static unsafe void CallToString(IntPtr redotObjectGCHandle, redot_string* outRes, redot_bool* outValid)
        {
            try
            {
                var self = (RedotObject)GCHandle.FromIntPtr(redotObjectGCHandle).Target;

                if (self == null)
                {
                    *outRes = default;
                    *outValid = redot_bool.False;
                    return;
                }

                var resultStr = self.ToString();

                if (resultStr == null)
                {
                    *outRes = default;
                    *outValid = redot_bool.False;
                    return;
                }

                *outRes = Marshaling.ConvertStringToNative(resultStr);
                *outValid = redot_bool.True;
            }
            catch (Exception e)
            {
                ExceptionUtils.LogException(e);
                *outRes = default;
                *outValid = redot_bool.False;
            }
        }

        [UnmanagedCallersOnly]
        internal static unsafe redot_bool HasMethodUnknownParams(IntPtr redotObjectGCHandle, redot_string_name* method)
        {
            try
            {
                var redotObject = (RedotObject)GCHandle.FromIntPtr(redotObjectGCHandle).Target;

                if (redotObject == null)
                    return redot_bool.False;

                return redotObject.HasRedotClassMethod(CustomUnsafe.AsRef(method)).ToRedotBool();
            }
            catch (Exception e)
            {
                ExceptionUtils.LogException(e);
                return redot_bool.False;
            }
        }

        [UnmanagedCallersOnly]
        internal static unsafe void SerializeState(
            IntPtr redotObjectGCHandle,
            redot_dictionary* propertiesState,
            redot_dictionary* signalEventsState
        )
        {
            try
            {
                var redotObject = (RedotObject)GCHandle.FromIntPtr(redotObjectGCHandle).Target;

                if (redotObject == null)
                    return;

                // Call OnBeforeSerialize

                // ReSharper disable once SuspiciousTypeConversion.Global
                if (redotObject is ISerializationListener serializationListener)
                    serializationListener.OnBeforeSerialize();

                // Save instance state

                using var info = RedotSerializationInfo.CreateCopyingBorrowed(
                    *propertiesState, *signalEventsState);

                redotObject.SaveRedotObjectData(info);
            }
            catch (Exception e)
            {
                ExceptionUtils.LogException(e);
            }
        }

        [UnmanagedCallersOnly]
        internal static unsafe void DeserializeState(
            IntPtr redotObjectGCHandle,
            redot_dictionary* propertiesState,
            redot_dictionary* signalEventsState
        )
        {
            try
            {
                var redotObject = (RedotObject)GCHandle.FromIntPtr(redotObjectGCHandle).Target;

                if (redotObject == null)
                    return;

                // Restore instance state

                using var info = RedotSerializationInfo.CreateCopyingBorrowed(
                    *propertiesState, *signalEventsState);

                redotObject.RestoreRedotObjectData(info);

                // Call OnAfterDeserialize

                // ReSharper disable once SuspiciousTypeConversion.Global
                if (redotObject is ISerializationListener serializationListener)
                    serializationListener.OnAfterDeserialize();
            }
            catch (Exception e)
            {
                ExceptionUtils.LogException(e);
            }
        }
    }
}
