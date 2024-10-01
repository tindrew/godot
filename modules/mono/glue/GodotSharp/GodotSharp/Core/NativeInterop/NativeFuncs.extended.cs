#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable IDE1006 // Naming rule violation
// ReSharper disable InconsistentNaming

namespace Redot.NativeInterop
{
    public static partial class NativeFuncs
    {
        public static Redot_variant Redotsharp_variant_new_copy(in Redot_variant src)
        {
            switch (src.Type)
            {
                case Variant.Type.Nil:
                    return default;
                case Variant.Type.Bool:
                    return new Redot_variant() { Bool = src.Bool, Type = Variant.Type.Bool };
                case Variant.Type.Int:
                    return new Redot_variant() { Int = src.Int, Type = Variant.Type.Int };
                case Variant.Type.Float:
                    return new Redot_variant() { Float = src.Float, Type = Variant.Type.Float };
                case Variant.Type.Vector2:
                    return new Redot_variant() { Vector2 = src.Vector2, Type = Variant.Type.Vector2 };
                case Variant.Type.Vector2I:
                    return new Redot_variant() { Vector2I = src.Vector2I, Type = Variant.Type.Vector2I };
                case Variant.Type.Rect2:
                    return new Redot_variant() { Rect2 = src.Rect2, Type = Variant.Type.Rect2 };
                case Variant.Type.Rect2I:
                    return new Redot_variant() { Rect2I = src.Rect2I, Type = Variant.Type.Rect2I };
                case Variant.Type.Vector3:
                    return new Redot_variant() { Vector3 = src.Vector3, Type = Variant.Type.Vector3 };
                case Variant.Type.Vector3I:
                    return new Redot_variant() { Vector3I = src.Vector3I, Type = Variant.Type.Vector3I };
                case Variant.Type.Vector4:
                    return new Redot_variant() { Vector4 = src.Vector4, Type = Variant.Type.Vector4 };
                case Variant.Type.Vector4I:
                    return new Redot_variant() { Vector4I = src.Vector4I, Type = Variant.Type.Vector4I };
                case Variant.Type.Plane:
                    return new Redot_variant() { Plane = src.Plane, Type = Variant.Type.Plane };
                case Variant.Type.Quaternion:
                    return new Redot_variant() { Quaternion = src.Quaternion, Type = Variant.Type.Quaternion };
                case Variant.Type.Color:
                    return new Redot_variant() { Color = src.Color, Type = Variant.Type.Color };
                case Variant.Type.Rid:
                    return new Redot_variant() { Rid = src.Rid, Type = Variant.Type.Rid };
            }

            Redotsharp_variant_new_copy(out Redot_variant ret, src);
            return ret;
        }

        public static Redot_string_name Redotsharp_string_name_new_copy(in Redot_string_name src)
        {
            if (src.IsEmpty)
                return default;
            Redotsharp_string_name_new_copy(out Redot_string_name ret, src);
            return ret;
        }

        public static Redot_node_path Redotsharp_node_path_new_copy(in Redot_node_path src)
        {
            if (src.IsEmpty)
                return default;
            Redotsharp_node_path_new_copy(out Redot_node_path ret, src);
            return ret;
        }

        public static Redot_array Redotsharp_array_new()
        {
            Redotsharp_array_new(out Redot_array ret);
            return ret;
        }

        public static Redot_array Redotsharp_array_new_copy(in Redot_array src)
        {
            Redotsharp_array_new_copy(out Redot_array ret, src);
            return ret;
        }

        public static Redot_dictionary Redotsharp_dictionary_new()
        {
            Redotsharp_dictionary_new(out Redot_dictionary ret);
            return ret;
        }

        public static Redot_dictionary Redotsharp_dictionary_new_copy(in Redot_dictionary src)
        {
            Redotsharp_dictionary_new_copy(out Redot_dictionary ret, src);
            return ret;
        }

        public static Redot_string_name Redotsharp_string_name_new_from_string(string name)
        {
            using Redot_string src = Marshaling.ConvertStringToNative(name);
            Redotsharp_string_name_new_from_string(out Redot_string_name ret, src);
            return ret;
        }

        public static Redot_node_path Redotsharp_node_path_new_from_string(string name)
        {
            using Redot_string src = Marshaling.ConvertStringToNative(name);
            Redotsharp_node_path_new_from_string(out Redot_node_path ret, src);
            return ret;
        }
    }
}
