using Redot;
using Redot.NativeInterop;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace RedotTools.Internals
{
    public static class Globals
    {
        public static float EditorScale => Internal.redot_icall_Globals_EditorScale();

        // ReSharper disable once UnusedMethodReturnValue.Global
        public static Variant GlobalDef(string setting, Variant defaultValue, bool restartIfChanged = false)
        {
            using redot_string settingIn = Marshaling.ConvertStringToNative(setting);
            using redot_variant defaultValueIn = defaultValue.CopyNativeVariant();
            Internal.redot_icall_Globals_GlobalDef(settingIn, defaultValueIn, restartIfChanged,
                out redot_variant result);
            return Variant.CreateTakingOwnershipOfDisposableValue(result);
        }

        // ReSharper disable once UnusedMethodReturnValue.Global
        public static Variant EditorDef(string setting, Variant defaultValue, bool restartIfChanged = false)
        {
            using redot_string settingIn = Marshaling.ConvertStringToNative(setting);
            using redot_variant defaultValueIn = defaultValue.CopyNativeVariant();
            Internal.redot_icall_Globals_EditorDef(settingIn, defaultValueIn, restartIfChanged,
                out redot_variant result);
            return Variant.CreateTakingOwnershipOfDisposableValue(result);
        }

        public static Shortcut EditorDefShortcut(string setting, string name, Key keycode = Key.None, bool physical = false)
        {
            using redot_string settingIn = Marshaling.ConvertStringToNative(setting);
            using redot_string nameIn = Marshaling.ConvertStringToNative(name);
            Internal.redot_icall_Globals_EditorDefShortcut(settingIn, nameIn, keycode, physical.ToRedotBool(), out redot_variant result);
            return (Shortcut)Variant.CreateTakingOwnershipOfDisposableValue(result);
        }

        public static Shortcut EditorGetShortcut(string setting)
        {
            using redot_string settingIn = Marshaling.ConvertStringToNative(setting);
            Internal.redot_icall_Globals_EditorGetShortcut(settingIn, out redot_variant result);
            return (Shortcut)Variant.CreateTakingOwnershipOfDisposableValue(result);
        }

        public static void EditorShortcutOverride(string setting, string feature, Key keycode = Key.None, bool physical = false)
        {
            using redot_string settingIn = Marshaling.ConvertStringToNative(setting);
            using redot_string featureIn = Marshaling.ConvertStringToNative(feature);
            Internal.redot_icall_Globals_EditorShortcutOverride(settingIn, featureIn, keycode, physical.ToRedotBool());
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public static string TTR(this string text)
        {
            using redot_string textIn = Marshaling.ConvertStringToNative(text);
            Internal.redot_icall_Globals_TTR(textIn, out redot_string dest);
            using (dest)
                return Marshaling.ConvertStringToManaged(dest);
        }
    }
}
