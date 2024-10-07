#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable IDE1006 // Naming rule violation
// ReSharper disable InconsistentNaming

namespace Redot.NativeInterop
{
    public static partial class NativeFuncs
    {
        public static redot_variant redotsharp_variant_new_copy(in redot_variant src)
        {
            switch (src.Type)
            {
                case Variant.Type.Nil:
                    return default;
                case Variant.Type.Bool:
                    return new redot_variant() { Bool = src.Bool, Type = Variant.Type.Bool };
                case Variant.Type.Int:
                    return new redot_variant() { Int = src.Int, Type = Variant.Type.Int };
                case Variant.Type.Float:
                    return new redot_variant() { Float = src.Float, Type = Variant.Type.Float };
                case Variant.Type.Vector2:
                    return new redot_variant() { Vector2 = src.Vector2, Type = Variant.Type.Vector2 };
                case Variant.Type.Vector2I:
                    return new redot_variant() { Vector2I = src.Vector2I, Type = Variant.Type.Vector2I };
                case Variant.Type.Rect2:
                    return new redot_variant() { Rect2 = src.Rect2, Type = Variant.Type.Rect2 };
                case Variant.Type.Rect2I:
                    return new redot_variant() { Rect2I = src.Rect2I, Type = Variant.Type.Rect2I };
                case Variant.Type.Vector3:
                    return new redot_variant() { Vector3 = src.Vector3, Type = Variant.Type.Vector3 };
                case Variant.Type.Vector3I:
                    return new redot_variant() { Vector3I = src.Vector3I, Type = Variant.Type.Vector3I };
                case Variant.Type.Vector4:
                    return new redot_variant() { Vector4 = src.Vector4, Type = Variant.Type.Vector4 };
                case Variant.Type.Vector4I:
                    return new redot_variant() { Vector4I = src.Vector4I, Type = Variant.Type.Vector4I };
                case Variant.Type.Plane:
                    return new redot_variant() { Plane = src.Plane, Type = Variant.Type.Plane };
                case Variant.Type.Quaternion:
                    return new redot_variant() { Quaternion = src.Quaternion, Type = Variant.Type.Quaternion };
                case Variant.Type.Color:
                    return new redot_variant() { Color = src.Color, Type = Variant.Type.Color };
                case Variant.Type.Rid:
                    return new redot_variant() { Rid = src.Rid, Type = Variant.Type.Rid };
            }

            redotsharp_variant_new_copy(out redot_variant ret, src);
            return ret;
        }

        public static redot_string_name redotsharp_string_name_new_copy(in redot_string_name src)
        {
            if (src.IsEmpty)
                return default;
            redotsharp_string_name_new_copy(out redot_string_name ret, src);
            return ret;
        }

        public static redot_node_path redotsharp_node_path_new_copy(in redot_node_path src)
        {
            if (src.IsEmpty)
                return default;
            redotsharp_node_path_new_copy(out redot_node_path ret, src);
            return ret;
        }

        public static redot_array redotsharp_array_new()
        {
            redotsharp_array_new(out redot_array ret);
            return ret;
        }

        public static redot_array redotsharp_array_new_copy(in redot_array src)
        {
            redotsharp_array_new_copy(out redot_array ret, src);
            return ret;
        }

        public static redot_dictionary redotsharp_dictionary_new()
        {
            redotsharp_dictionary_new(out redot_dictionary ret);
            return ret;
        }

        public static redot_dictionary redotsharp_dictionary_new_copy(in redot_dictionary src)
        {
            redotsharp_dictionary_new_copy(out redot_dictionary ret, src);
            return ret;
        }

        public static redot_string_name redotsharp_string_name_new_from_string(string name)
        {
            using redot_string src = Marshaling.ConvertStringToNative(name);
            redotsharp_string_name_new_from_string(out redot_string_name ret, src);
            return ret;
        }

        public static redot_node_path redotsharp_node_path_new_from_string(string name)
        {
            using redot_string src = Marshaling.ConvertStringToNative(name);
            redotsharp_node_path_new_from_string(out redot_node_path ret, src);
            return ret;
        }
    }
}
