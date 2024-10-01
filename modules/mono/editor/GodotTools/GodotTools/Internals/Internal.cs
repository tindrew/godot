#pragma warning disable IDE1006 // Naming rule violation
// ReSharper disable InconsistentNaming

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Redot;
using Redot.NativeInterop;
using Redot.SourceGenerators.Internal;
using RedotTools.IdeMessaging.Requests;

namespace RedotTools.Internals
{
    [GenerateUnmanagedCallbacks(typeof(InternalUnmanagedCallbacks))]
    internal static partial class Internal
    {
        public const string CSharpLanguageType = "CSharpScript";
        public const string CSharpLanguageExtension = ".cs";

        public static string FullExportTemplatesDir
        {
            get
            {
                Redot_icall_Internal_FullExportTemplatesDir(out Redot_string dest);
                using (dest)
                    return Marshaling.ConvertStringToManaged(dest);
            }
        }

        public static string SimplifyRedotPath(this string path) => Redot.StringExtensions.SimplifyPath(path);

        public static bool IsMacOSAppBundleInstalled(string bundleId)
        {
            using Redot_string bundleIdIn = Marshaling.ConvertStringToNative(bundleId);
            return Redot_icall_Internal_IsMacOSAppBundleInstalled(bundleIdIn);
        }

        public static bool LipOCreateFile(string outputPath, string[] files)
        {
            using Redot_string outputPathIn = Marshaling.ConvertStringToNative(outputPath);
            using Redot_packed_string_array filesIn = Marshaling.ConvertSystemArrayToNativePackedStringArray(files);
            return Redot_icall_Internal_LipOCreateFile(outputPathIn, filesIn);
        }

        public static bool RedotIs32Bits() => Redot_icall_Internal_RedotIs32Bits();

        public static bool RedotIsRealTDouble() => Redot_icall_Internal_RedotIsRealTDouble();

        public static void RedotMainIteration() => Redot_icall_Internal_RedotMainIteration();

        public static bool IsAssembliesReloadingNeeded() => Redot_icall_Internal_IsAssembliesReloadingNeeded();

        public static void ReloadAssemblies(bool softReload) => Redot_icall_Internal_ReloadAssemblies(softReload);

        public static void EditorDebuggerNodeReloadScripts() => Redot_icall_Internal_EditorDebuggerNodeReloadScripts();

        public static bool ScriptEditorEdit(Resource resource, int line, int col, bool grabFocus = true) =>
            Redot_icall_Internal_ScriptEditorEdit(resource.NativeInstance, line, col, grabFocus);

        public static void EditorNodeShowScriptScreen() => Redot_icall_Internal_EditorNodeShowScriptScreen();

        public static void EditorRunPlay() => Redot_icall_Internal_EditorRunPlay();

        public static void EditorRunStop() => Redot_icall_Internal_EditorRunStop();

        public static void EditorPlugin_AddControlToEditorRunBar(Control control) =>
            Redot_icall_Internal_EditorPlugin_AddControlToEditorRunBar(control.NativeInstance);

        public static void ScriptEditorDebugger_ReloadScripts() =>
            Redot_icall_Internal_ScriptEditorDebugger_ReloadScripts();

        public static string[] CodeCompletionRequest(CodeCompletionRequest.CompletionKind kind,
            string scriptFile)
        {
            using Redot_string scriptFileIn = Marshaling.ConvertStringToNative(scriptFile);
            Redot_icall_Internal_CodeCompletionRequest((int)kind, scriptFileIn, out Redot_packed_string_array res);
            using (res)
                return Marshaling.ConvertNativePackedStringArrayToSystemArray(res);
        }

        #region Internal

        private static bool initialized = false;

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Global
        internal static unsafe void Initialize(IntPtr unmanagedCallbacks, int unmanagedCallbacksSize)
        {
            if (initialized)
                throw new InvalidOperationException("Already initialized.");
            initialized = true;

            if (unmanagedCallbacksSize != sizeof(InternalUnmanagedCallbacks))
                throw new ArgumentException("Unmanaged callbacks size mismatch.", nameof(unmanagedCallbacksSize));

            _unmanagedCallbacks = Unsafe.AsRef<InternalUnmanagedCallbacks>((void*)unmanagedCallbacks);
        }

        private partial struct InternalUnmanagedCallbacks
        {
        }

        /*
         * IMPORTANT:
         * The order of the methods defined in NativeFuncs must match the order
         * in the array defined at the bottom of 'editor/editor_internal_calls.cpp'.
         */

        public static partial void Redot_icall_RedotSharpDirs_ResMetadataDir(out Redot_string r_dest);

        public static partial void Redot_icall_RedotSharpDirs_MonoUserDir(out Redot_string r_dest);

        public static partial void Redot_icall_RedotSharpDirs_BuildLogsDirs(out Redot_string r_dest);

        public static partial void Redot_icall_RedotSharpDirs_DataEditorToolsDir(out Redot_string r_dest);

        public static partial void Redot_icall_RedotSharpDirs_CSharpProjectName(out Redot_string r_dest);

        public static partial void Redot_icall_EditorProgress_Create(in Redot_string task, in Redot_string label,
            int amount, bool canCancel);

        public static partial void Redot_icall_EditorProgress_Dispose(in Redot_string task);

        public static partial bool Redot_icall_EditorProgress_Step(in Redot_string task, in Redot_string state,
            int step,
            bool forceRefresh);

        private static partial void Redot_icall_Internal_FullExportTemplatesDir(out Redot_string dest);

        private static partial bool Redot_icall_Internal_IsMacOSAppBundleInstalled(in Redot_string bundleId);

        private static partial bool Redot_icall_Internal_LipOCreateFile(in Redot_string outputPath, in Redot_packed_string_array files);

        private static partial bool Redot_icall_Internal_RedotIs32Bits();

        private static partial bool Redot_icall_Internal_RedotIsRealTDouble();

        private static partial void Redot_icall_Internal_RedotMainIteration();

        private static partial bool Redot_icall_Internal_IsAssembliesReloadingNeeded();

        private static partial void Redot_icall_Internal_ReloadAssemblies(bool softReload);

        private static partial void Redot_icall_Internal_EditorDebuggerNodeReloadScripts();

        private static partial bool Redot_icall_Internal_ScriptEditorEdit(IntPtr resource, int line, int col,
            bool grabFocus);

        private static partial void Redot_icall_Internal_EditorNodeShowScriptScreen();

        private static partial void Redot_icall_Internal_EditorRunPlay();

        private static partial void Redot_icall_Internal_EditorRunStop();

        private static partial void Redot_icall_Internal_EditorPlugin_AddControlToEditorRunBar(IntPtr p_control);

        private static partial void Redot_icall_Internal_ScriptEditorDebugger_ReloadScripts();

        private static partial void Redot_icall_Internal_CodeCompletionRequest(int kind, in Redot_string scriptFile,
            out Redot_packed_string_array res);

        public static partial float Redot_icall_Globals_EditorScale();

        public static partial void Redot_icall_Globals_GlobalDef(in Redot_string setting, in Redot_variant defaultValue,
            bool restartIfChanged, out Redot_variant result);

        public static partial void Redot_icall_Globals_EditorDef(in Redot_string setting, in Redot_variant defaultValue,
            bool restartIfChanged, out Redot_variant result);

        public static partial void
            Redot_icall_Globals_EditorDefShortcut(in Redot_string setting, in Redot_string name, Key keycode, Redot_bool physical, out Redot_variant result);

        public static partial void
            Redot_icall_Globals_EditorGetShortcut(in Redot_string setting, out Redot_variant result);

        public static partial void
            Redot_icall_Globals_EditorShortcutOverride(in Redot_string setting, in Redot_string feature, Key keycode, Redot_bool physical);

        public static partial void Redot_icall_Globals_TTR(in Redot_string text, out Redot_string dest);

        public static partial void Redot_icall_Utils_OS_GetPlatformName(out Redot_string dest);

        public static partial bool Redot_icall_Utils_OS_UnixFileHasExecutableAccess(in Redot_string filePath);

        #endregion
    }
}
