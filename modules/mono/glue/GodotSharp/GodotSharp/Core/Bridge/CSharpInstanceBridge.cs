using System;
using System.Runtime.InteropServices;
using Redot.NativeInterop;

namespace Redot.Bridge
{
    internal static class CSharpInstanceBridge
    {
        [UnmanagedCallersOnly]
        internal static unsafe Redot_bool Call(IntPtr RedotObjectGCHandle, Redot_string_name* method,
            Redot_variant** args, int argCount, Redot_variant_call_error* refCallError, Redot_variant* ret)
        {
            try
            {
                var RedotObject = (RedotObject)GCHandle.FromIntPtr(RedotObjectGCHandle).Target;

                if (RedotObject == null)
                {
                    *ret = default;
                    (*refCallError).Error = Redot_variant_call_error_error.Redot_CALL_ERROR_CALL_ERROR_INSTANCE_IS_NULL;
                    return Redot_bool.False;
                }

                bool methodInvoked = RedotObject.InvokeRedotClassMethod(CustomUnsafe.AsRef(method),
                    new NativeVariantPtrArgs(args, argCount), out Redot_variant retValue);

                if (!methodInvoked)
                {
                    *ret = default;
                    // This is important, as it tells Object::call that no method was called.
                    // Otherwise, it would prevent Object::call from calling native methods.
                    (*refCallError).Error = Redot_variant_call_error_error.Redot_CALL_ERROR_CALL_ERROR_INVALID_METHOD;
                    return Redot_bool.False;
                }

                *ret = retValue;
                return Redot_bool.True;
            }
            catch (Exception e)
            {
                ExceptionUtils.LogException(e);
                *ret = default;
                return Redot_bool.False;
            }
        }

        [UnmanagedCallersOnly]
        internal static unsafe Redot_bool Set(IntPtr RedotObjectGCHandle, Redot_string_name* name, Redot_variant* value)
        {
            try
            {
                var RedotObject = (RedotObject)GCHandle.FromIntPtr(RedotObjectGCHandle).Target;

                if (RedotObject == null)
                    throw new InvalidOperationException();

                if (RedotObject.SetRedotClassPropertyValue(CustomUnsafe.AsRef(name), CustomUnsafe.AsRef(value)))
                {
                    return Redot_bool.True;
                }

                var nameManaged = StringName.CreateTakingOwnershipOfDisposableValue(
                    NativeFuncs.Redotsharp_string_name_new_copy(CustomUnsafe.AsRef(name)));

                Variant valueManaged = Variant.CreateCopyingBorrowed(*value);

                return RedotObject._Set(nameManaged, valueManaged).ToRedotBool();
            }
            catch (Exception e)
            {
                ExceptionUtils.LogException(e);
                return Redot_bool.False;
            }
        }

        [UnmanagedCallersOnly]
        internal static unsafe Redot_bool Get(IntPtr RedotObjectGCHandle, Redot_string_name* name,
            Redot_variant* outRet)
        {
            try
            {
                var RedotObject = (RedotObject)GCHandle.FromIntPtr(RedotObjectGCHandle).Target;

                if (RedotObject == null)
                    throw new InvalidOperationException();

                // Properties
                if (RedotObject.GetRedotClassPropertyValue(CustomUnsafe.AsRef(name), out Redot_variant outRetValue))
                {
                    *outRet = outRetValue;
                    return Redot_bool.True;
                }

                // Signals
                if (RedotObject.HasRedotClassSignal(CustomUnsafe.AsRef(name)))
                {
                    Redot_signal signal = new Redot_signal(NativeFuncs.Redotsharp_string_name_new_copy(*name), RedotObject.GetInstanceId());
                    *outRet = VariantUtils.CreateFromSignalTakingOwnershipOfDisposableValue(signal);
                    return Redot_bool.True;
                }

                // Methods
                if (RedotObject.HasRedotClassMethod(CustomUnsafe.AsRef(name)))
                {
                    Redot_callable method = new Redot_callable(NativeFuncs.Redotsharp_string_name_new_copy(*name), RedotObject.GetInstanceId());
                    *outRet = VariantUtils.CreateFromCallableTakingOwnershipOfDisposableValue(method);
                    return Redot_bool.True;
                }

                var nameManaged = StringName.CreateTakingOwnershipOfDisposableValue(
                    NativeFuncs.Redotsharp_string_name_new_copy(CustomUnsafe.AsRef(name)));

                Variant ret = RedotObject._Get(nameManaged);

                if (ret.VariantType == Variant.Type.Nil)
                {
                    *outRet = default;
                    return Redot_bool.False;
                }

                *outRet = ret.CopyNativeVariant();
                return Redot_bool.True;
            }
            catch (Exception e)
            {
                ExceptionUtils.LogException(e);
                *outRet = default;
                return Redot_bool.False;
            }
        }

        [UnmanagedCallersOnly]
        internal static void CallDispose(IntPtr RedotObjectGCHandle, Redot_bool okIfNull)
        {
            try
            {
                var RedotObject = (RedotObject)GCHandle.FromIntPtr(RedotObjectGCHandle).Target;

                if (okIfNull.ToBool())
                    RedotObject?.Dispose();
                else
                    RedotObject!.Dispose();
            }
            catch (Exception e)
            {
                ExceptionUtils.LogException(e);
            }
        }

        [UnmanagedCallersOnly]
        internal static unsafe void CallToString(IntPtr RedotObjectGCHandle, Redot_string* outRes, Redot_bool* outValid)
        {
            try
            {
                var self = (RedotObject)GCHandle.FromIntPtr(RedotObjectGCHandle).Target;

                if (self == null)
                {
                    *outRes = default;
                    *outValid = Redot_bool.False;
                    return;
                }

                var resultStr = self.ToString();

                if (resultStr == null)
                {
                    *outRes = default;
                    *outValid = Redot_bool.False;
                    return;
                }

                *outRes = Marshaling.ConvertStringToNative(resultStr);
                *outValid = Redot_bool.True;
            }
            catch (Exception e)
            {
                ExceptionUtils.LogException(e);
                *outRes = default;
                *outValid = Redot_bool.False;
            }
        }

        [UnmanagedCallersOnly]
        internal static unsafe Redot_bool HasMethodUnknownParams(IntPtr RedotObjectGCHandle, Redot_string_name* method)
        {
            try
            {
                var RedotObject = (RedotObject)GCHandle.FromIntPtr(RedotObjectGCHandle).Target;

                if (RedotObject == null)
                    return Redot_bool.False;

                return RedotObject.HasRedotClassMethod(CustomUnsafe.AsRef(method)).ToRedotBool();
            }
            catch (Exception e)
            {
                ExceptionUtils.LogException(e);
                return Redot_bool.False;
            }
        }

        [UnmanagedCallersOnly]
        internal static unsafe void SerializeState(
            IntPtr RedotObjectGCHandle,
            Redot_dictionary* propertiesState,
            Redot_dictionary* signalEventsState
        )
        {
            try
            {
                var RedotObject = (RedotObject)GCHandle.FromIntPtr(RedotObjectGCHandle).Target;

                if (RedotObject == null)
                    return;

                // Call OnBeforeSerialize

                // ReSharper disable once SuspiciousTypeConversion.Global
                if (RedotObject is ISerializationListener serializationListener)
                    serializationListener.OnBeforeSerialize();

                // Save instance state

                using var info = RedotSerializationInfo.CreateCopyingBorrowed(
                    *propertiesState, *signalEventsState);

                RedotObject.SaveRedotObjectData(info);
            }
            catch (Exception e)
            {
                ExceptionUtils.LogException(e);
            }
        }

        [UnmanagedCallersOnly]
        internal static unsafe void DeserializeState(
            IntPtr RedotObjectGCHandle,
            Redot_dictionary* propertiesState,
            Redot_dictionary* signalEventsState
        )
        {
            try
            {
                var RedotObject = (RedotObject)GCHandle.FromIntPtr(RedotObjectGCHandle).Target;

                if (RedotObject == null)
                    return;

                // Restore instance state

                using var info = RedotSerializationInfo.CreateCopyingBorrowed(
                    *propertiesState, *signalEventsState);

                RedotObject.RestoreRedotObjectData(info);

                // Call OnAfterDeserialize

                // ReSharper disable once SuspiciousTypeConversion.Global
                if (RedotObject is ISerializationListener serializationListener)
                    serializationListener.OnAfterDeserialize();
            }
            catch (Exception e)
            {
                ExceptionUtils.LogException(e);
            }
        }
    }
}
