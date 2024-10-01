using System;
using System.Runtime.InteropServices;
using Redot.NativeInterop;

namespace Redot.Bridge
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct ManagedCallbacks
    {
        // @formatter:off
        public delegate* unmanaged<IntPtr, Redot_variant**, int, Redot_bool*, void> SignalAwaiter_SignalCallback;
        public delegate* unmanaged<IntPtr, void*, Redot_variant**, int, Redot_variant*, void> DelegateUtils_InvokeWithVariantArgs;
        public delegate* unmanaged<IntPtr, IntPtr, Redot_bool> DelegateUtils_DelegateEquals;
        public delegate* unmanaged<IntPtr, int> DelegateUtils_DelegateHash;
        public delegate* unmanaged<IntPtr, Redot_bool*, int> DelegateUtils_GetArgumentCount;
        public delegate* unmanaged<IntPtr, Redot_array*, Redot_bool> DelegateUtils_TrySerializeDelegateWithGCHandle;
        public delegate* unmanaged<Redot_array*, IntPtr*, Redot_bool> DelegateUtils_TryDeserializeDelegateWithGCHandle;
        public delegate* unmanaged<void> ScriptManagerBridge_FrameCallback;
        public delegate* unmanaged<Redot_string_name*, IntPtr, IntPtr> ScriptManagerBridge_CreateManagedForRedotObjectBinding;
        public delegate* unmanaged<IntPtr, IntPtr, Redot_variant**, int, Redot_bool> ScriptManagerBridge_CreateManagedForRedotObjectScriptInstance;
        public delegate* unmanaged<IntPtr, Redot_string_name*, void> ScriptManagerBridge_GetScriptNativeName;
        public delegate* unmanaged<Redot_string*, Redot_string*, Redot_string*, Redot_string*, void> ScriptManagerBridge_GetGlobalClassName;
        public delegate* unmanaged<IntPtr, IntPtr, void> ScriptManagerBridge_SetRedotObjectPtr;
        public delegate* unmanaged<IntPtr, Redot_string_name*, Redot_variant**, int, Redot_bool*, void> ScriptManagerBridge_RaiseEventSignal;
        public delegate* unmanaged<IntPtr, IntPtr, Redot_bool> ScriptManagerBridge_ScriptIsOrInherits;
        public delegate* unmanaged<IntPtr, Redot_string*, Redot_bool> ScriptManagerBridge_AddScriptBridge;
        public delegate* unmanaged<Redot_string*, Redot_ref*, void> ScriptManagerBridge_GetOrCreateScriptBridgeForPath;
        public delegate* unmanaged<IntPtr, void> ScriptManagerBridge_RemoveScriptBridge;
        public delegate* unmanaged<IntPtr, Redot_bool> ScriptManagerBridge_TryReloadRegisteredScriptWithClass;
        public delegate* unmanaged<IntPtr, Redot_csharp_type_info*, Redot_array*, Redot_dictionary*, Redot_dictionary*, Redot_ref*, void> ScriptManagerBridge_UpdateScriptClassInfo;
        public delegate* unmanaged<IntPtr, IntPtr*, Redot_bool, Redot_bool> ScriptManagerBridge_SwapGCHandleForType;
        public delegate* unmanaged<IntPtr, delegate* unmanaged<IntPtr, Redot_string*, void*, int, void>, void> ScriptManagerBridge_GetPropertyInfoList;
        public delegate* unmanaged<IntPtr, delegate* unmanaged<IntPtr, void*, int, void>, void> ScriptManagerBridge_GetPropertyDefaultValues;
        public delegate* unmanaged<IntPtr, Redot_string_name*, Redot_variant**, int, Redot_variant_call_error*, Redot_variant*, Redot_bool> ScriptManagerBridge_CallStatic;
        public delegate* unmanaged<IntPtr, Redot_string_name*, Redot_variant**, int, Redot_variant_call_error*, Redot_variant*, Redot_bool> CSharpInstanceBridge_Call;
        public delegate* unmanaged<IntPtr, Redot_string_name*, Redot_variant*, Redot_bool> CSharpInstanceBridge_Set;
        public delegate* unmanaged<IntPtr, Redot_string_name*, Redot_variant*, Redot_bool> CSharpInstanceBridge_Get;
        public delegate* unmanaged<IntPtr, Redot_bool, void> CSharpInstanceBridge_CallDispose;
        public delegate* unmanaged<IntPtr, Redot_string*, Redot_bool*, void> CSharpInstanceBridge_CallToString;
        public delegate* unmanaged<IntPtr, Redot_string_name*, Redot_bool> CSharpInstanceBridge_HasMethodUnknownParams;
        public delegate* unmanaged<IntPtr, Redot_dictionary*, Redot_dictionary*, void> CSharpInstanceBridge_SerializeState;
        public delegate* unmanaged<IntPtr, Redot_dictionary*, Redot_dictionary*, void> CSharpInstanceBridge_DeserializeState;
        public delegate* unmanaged<IntPtr, void> GCHandleBridge_FreeGCHandle;
        public delegate* unmanaged<IntPtr, Redot_bool> GCHandleBridge_GCHandleIsTargetCollectible;
        public delegate* unmanaged<void*, void> DebuggingUtils_GetCurrentStackInfo;
        public delegate* unmanaged<void> DisposablesTracker_OnRedotShuttingDown;
        public delegate* unmanaged<Redot_bool, void> GD_OnCoreApiAssemblyLoaded;
        // @formatter:on

        public static ManagedCallbacks Create()
        {
            return new()
            {
                // @formatter:off
                SignalAwaiter_SignalCallback = &SignalAwaiter.SignalCallback,
                DelegateUtils_InvokeWithVariantArgs = &DelegateUtils.InvokeWithVariantArgs,
                DelegateUtils_DelegateEquals = &DelegateUtils.DelegateEquals,
                DelegateUtils_DelegateHash = &DelegateUtils.DelegateHash,
                DelegateUtils_GetArgumentCount = &DelegateUtils.GetArgumentCount,
                DelegateUtils_TrySerializeDelegateWithGCHandle = &DelegateUtils.TrySerializeDelegateWithGCHandle,
                DelegateUtils_TryDeserializeDelegateWithGCHandle = &DelegateUtils.TryDeserializeDelegateWithGCHandle,
                ScriptManagerBridge_FrameCallback = &ScriptManagerBridge.FrameCallback,
                ScriptManagerBridge_CreateManagedForRedotObjectBinding = &ScriptManagerBridge.CreateManagedForRedotObjectBinding,
                ScriptManagerBridge_CreateManagedForRedotObjectScriptInstance = &ScriptManagerBridge.CreateManagedForRedotObjectScriptInstance,
                ScriptManagerBridge_GetScriptNativeName = &ScriptManagerBridge.GetScriptNativeName,
                ScriptManagerBridge_GetGlobalClassName = &ScriptManagerBridge.GetGlobalClassName,
                ScriptManagerBridge_SetRedotObjectPtr = &ScriptManagerBridge.SetRedotObjectPtr,
                ScriptManagerBridge_RaiseEventSignal = &ScriptManagerBridge.RaiseEventSignal,
                ScriptManagerBridge_ScriptIsOrInherits = &ScriptManagerBridge.ScriptIsOrInherits,
                ScriptManagerBridge_AddScriptBridge = &ScriptManagerBridge.AddScriptBridge,
                ScriptManagerBridge_GetOrCreateScriptBridgeForPath = &ScriptManagerBridge.GetOrCreateScriptBridgeForPath,
                ScriptManagerBridge_RemoveScriptBridge = &ScriptManagerBridge.RemoveScriptBridge,
                ScriptManagerBridge_TryReloadRegisteredScriptWithClass = &ScriptManagerBridge.TryReloadRegisteredScriptWithClass,
                ScriptManagerBridge_UpdateScriptClassInfo = &ScriptManagerBridge.UpdateScriptClassInfo,
                ScriptManagerBridge_SwapGCHandleForType = &ScriptManagerBridge.SwapGCHandleForType,
                ScriptManagerBridge_GetPropertyInfoList = &ScriptManagerBridge.GetPropertyInfoList,
                ScriptManagerBridge_GetPropertyDefaultValues = &ScriptManagerBridge.GetPropertyDefaultValues,
                ScriptManagerBridge_CallStatic = &ScriptManagerBridge.CallStatic,
                CSharpInstanceBridge_Call = &CSharpInstanceBridge.Call,
                CSharpInstanceBridge_Set = &CSharpInstanceBridge.Set,
                CSharpInstanceBridge_Get = &CSharpInstanceBridge.Get,
                CSharpInstanceBridge_CallDispose = &CSharpInstanceBridge.CallDispose,
                CSharpInstanceBridge_CallToString = &CSharpInstanceBridge.CallToString,
                CSharpInstanceBridge_HasMethodUnknownParams = &CSharpInstanceBridge.HasMethodUnknownParams,
                CSharpInstanceBridge_SerializeState = &CSharpInstanceBridge.SerializeState,
                CSharpInstanceBridge_DeserializeState = &CSharpInstanceBridge.DeserializeState,
                GCHandleBridge_FreeGCHandle = &GCHandleBridge.FreeGCHandle,
                GCHandleBridge_GCHandleIsTargetCollectible = &GCHandleBridge.GCHandleIsTargetCollectible,
                DebuggingUtils_GetCurrentStackInfo = &DebuggingUtils.GetCurrentStackInfo,
                DisposablesTracker_OnRedotShuttingDown = &DisposablesTracker.OnRedotShuttingDown,
                GD_OnCoreApiAssemblyLoaded = &GD.OnCoreApiAssemblyLoaded,
                // @formatter:on
            };
        }

        public static void Create(IntPtr outManagedCallbacks)
            => *(ManagedCallbacks*)outManagedCallbacks = Create();
    }
}
