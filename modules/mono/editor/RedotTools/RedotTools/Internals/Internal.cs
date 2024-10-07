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
                redot_icall_Internal_FullExportTemplatesDir(out redot_string dest);
                using (dest)
                    return Marshaling.ConvertStringToManaged(dest);
            }
        }

        public static string SimplifyRedotPath(this string path) => Redot.StringExtensions.SimplifyPath(path);

        public static bool IsMacOSAppBundleInstalled(string bundleId)
        {
            using redot_string bundleIdIn = Marshaling.ConvertStringToNative(bundleId);
            return redot_icall_Internal_IsMacOSAppBundleInstalled(bundleIdIn);
        }

        public static bool LipOCreateFile(string outputPath, string[] files)
        {
            using redot_string outputPathIn = Marshaling.ConvertStringToNative(outputPath);
            using redot_packed_string_array filesIn = Marshaling.ConvertSystemArrayToNativePackedStringArray(files);
            return redot_icall_Internal_LipOCreateFile(outputPathIn, filesIn);
        }

        public static bool RedotIs32Bits() => redot_icall_Internal_RedotIs32Bits();

        public static bool RedotIsRealTDouble() => redot_icall_Internal_RedotIsRealTDouble();

        public static void RedotMainIteration() => redot_icall_Internal_RedotMainIteration();

        public static bool IsAssembliesReloadingNeeded() => redot_icall_Internal_IsAssembliesReloadingNeeded();

        public static void ReloadAssemblies(bool softReload) => redot_icall_Internal_ReloadAssemblies(softReload);

        public static void EditorDebuggerNodeReloadScripts() => redot_icall_Internal_EditorDebuggerNodeReloadScripts();

        public static bool ScriptEditorEdit(Resource resource, int line, int col, bool grabFocus = true) =>
            redot_icall_Internal_ScriptEditorEdit(resource.NativeInstance, line, col, grabFocus);

        public static void EditorNodeShowScriptScreen() => redot_icall_Internal_EditorNodeShowScriptScreen();

        public static void EditorRunPlay() => redot_icall_Internal_EditorRunPlay();

        public static void EditorRunStop() => redot_icall_Internal_EditorRunStop();

        public static void EditorPlugin_AddControlToEditorRunBar(Control control) =>
            redot_icall_Internal_EditorPlugin_AddControlToEditorRunBar(control.NativeInstance);

        public static void ScriptEditorDebugger_ReloadScripts() =>
            redot_icall_Internal_ScriptEditorDebugger_ReloadScripts();

        public static string[] CodeCompletionRequest(CodeCompletionRequest.CompletionKind kind,
            string scriptFile)
        {
            using redot_string scriptFileIn = Marshaling.ConvertStringToNative(scriptFile);
            redot_icall_Internal_CodeCompletionRequest((int)kind, scriptFileIn, out redot_packed_string_array res);
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

        public static partial void redot_icall_RedotSharpDirs_ResMetadataDir(out redot_string r_dest);

        public static partial void redot_icall_RedotSharpDirs_MonoUserDir(out redot_string r_dest);

        public static partial void redot_icall_RedotSharpDirs_BuildLogsDirs(out redot_string r_dest);

        public static partial void redot_icall_RedotSharpDirs_DataEditorToolsDir(out redot_string r_dest);

        public static partial void redot_icall_RedotSharpDirs_CSharpProjectName(out redot_string r_dest);

        public static partial void redot_icall_EditorProgress_Create(in redot_string task, in redot_string label,
            int amount, bool canCancel);

        public static partial void redot_icall_EditorProgress_Dispose(in redot_string task);

        public static partial bool redot_icall_EditorProgress_Step(in redot_string task, in redot_string state,
            int step,
            bool forceRefresh);

        private static partial void redot_icall_Internal_FullExportTemplatesDir(out redot_string dest);

        private static partial bool redot_icall_Internal_IsMacOSAppBundleInstalled(in redot_string bundleId);

        private static partial bool redot_icall_Internal_LipOCreateFile(in redot_string outputPath, in redot_packed_string_array files);

        private static partial bool redot_icall_Internal_RedotIs32Bits();

        private static partial bool redot_icall_Internal_RedotIsRealTDouble();

        private static partial void redot_icall_Internal_RedotMainIteration();

        private static partial bool redot_icall_Internal_IsAssembliesReloadingNeeded();

        private static partial void redot_icall_Internal_ReloadAssemblies(bool softReload);

        private static partial void redot_icall_Internal_EditorDebuggerNodeReloadScripts();

        private static partial bool redot_icall_Internal_ScriptEditorEdit(IntPtr resource, int line, int col,
            bool grabFocus);

        private static partial void redot_icall_Internal_EditorNodeShowScriptScreen();

        private static partial void redot_icall_Internal_EditorRunPlay();

        private static partial void redot_icall_Internal_EditorRunStop();

        private static partial void redot_icall_Internal_EditorPlugin_AddControlToEditorRunBar(IntPtr p_control);

        private static partial void redot_icall_Internal_ScriptEditorDebugger_ReloadScripts();

        private static partial void redot_icall_Internal_CodeCompletionRequest(int kind, in redot_string scriptFile,
            out redot_packed_string_array res);

        public static partial float redot_icall_Globals_EditorScale();

        public static partial void redot_icall_Globals_GlobalDef(in redot_string setting, in redot_variant defaultValue,
            bool restartIfChanged, out redot_variant result);

        public static partial void redot_icall_Globals_EditorDef(in redot_string setting, in redot_variant defaultValue,
            bool restartIfChanged, out redot_variant result);

        public static partial void
            redot_icall_Globals_EditorDefShortcut(in redot_string setting, in redot_string name, Key keycode, redot_bool physical, out redot_variant result);

        public static partial void
            redot_icall_Globals_EditorGetShortcut(in redot_string setting, out redot_variant result);

        public static partial void
            redot_icall_Globals_EditorShortcutOverride(in redot_string setting, in redot_string feature, Key keycode, redot_bool physical);

        public static partial void redot_icall_Globals_TTR(in redot_string text, out redot_string dest);

        public static partial void redot_icall_Utils_OS_GetPlatformName(out redot_string dest);

        public static partial bool redot_icall_Utils_OS_UnixFileHasExecutableAccess(in redot_string filePath);

        #endregion
    }
}
