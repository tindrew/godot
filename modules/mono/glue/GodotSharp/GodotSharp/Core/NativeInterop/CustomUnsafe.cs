using System.Runtime.CompilerServices;

namespace Redot.NativeInterop;

// Ref structs are not allowed as generic type parameters, so we can't use Unsafe.AsPointer<T>/AsRef<T>.
// As a workaround we create our own overloads for our structs with some tricks under the hood.

public static class CustomUnsafe
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_ref* AsPointer(ref Redot_ref value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_ref* ReadOnlyRefAsPointer(in Redot_ref value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_ref AsRef(Redot_ref* source)
        => ref *source;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_ref AsRef(in Redot_ref source)
        => ref *ReadOnlyRefAsPointer(in source);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_variant_call_error* AsPointer(ref Redot_variant_call_error value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_variant_call_error* ReadOnlyRefAsPointer(in Redot_variant_call_error value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_variant_call_error AsRef(Redot_variant_call_error* source)
        => ref *source;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_variant_call_error AsRef(in Redot_variant_call_error source)
        => ref *ReadOnlyRefAsPointer(in source);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_variant* AsPointer(ref Redot_variant value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_variant* ReadOnlyRefAsPointer(in Redot_variant value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_variant AsRef(Redot_variant* source)
        => ref *source;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_variant AsRef(in Redot_variant source)
        => ref *ReadOnlyRefAsPointer(in source);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_string* AsPointer(ref Redot_string value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_string* ReadOnlyRefAsPointer(in Redot_string value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_string AsRef(Redot_string* source)
        => ref *source;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_string AsRef(in Redot_string source)
        => ref *ReadOnlyRefAsPointer(in source);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_string_name* AsPointer(ref Redot_string_name value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_string_name* ReadOnlyRefAsPointer(in Redot_string_name value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_string_name AsRef(Redot_string_name* source)
        => ref *source;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_string_name AsRef(in Redot_string_name source)
        => ref *ReadOnlyRefAsPointer(in source);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_node_path* AsPointer(ref Redot_node_path value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_node_path* ReadOnlyRefAsPointer(in Redot_node_path value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_node_path AsRef(Redot_node_path* source)
        => ref *source;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_node_path AsRef(in Redot_node_path source)
        => ref *ReadOnlyRefAsPointer(in source);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_signal* AsPointer(ref Redot_signal value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_signal* ReadOnlyRefAsPointer(in Redot_signal value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_signal AsRef(Redot_signal* source)
        => ref *source;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_signal AsRef(in Redot_signal source)
        => ref *ReadOnlyRefAsPointer(in source);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_callable* AsPointer(ref Redot_callable value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_callable* ReadOnlyRefAsPointer(in Redot_callable value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_callable AsRef(Redot_callable* source)
        => ref *source;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_callable AsRef(in Redot_callable source)
        => ref *ReadOnlyRefAsPointer(in source);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_array* AsPointer(ref Redot_array value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_array* ReadOnlyRefAsPointer(in Redot_array value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_array AsRef(Redot_array* source)
        => ref *source;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_array AsRef(in Redot_array source)
        => ref *ReadOnlyRefAsPointer(in source);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_dictionary* AsPointer(ref Redot_dictionary value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_dictionary* ReadOnlyRefAsPointer(in Redot_dictionary value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_dictionary AsRef(Redot_dictionary* source)
        => ref *source;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_dictionary AsRef(in Redot_dictionary source)
        => ref *ReadOnlyRefAsPointer(in source);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_packed_byte_array* AsPointer(ref Redot_packed_byte_array value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_packed_byte_array* ReadOnlyRefAsPointer(in Redot_packed_byte_array value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_packed_byte_array AsRef(Redot_packed_byte_array* source)
        => ref *source;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_packed_byte_array AsRef(in Redot_packed_byte_array source)
        => ref *ReadOnlyRefAsPointer(in source);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_packed_int32_array* AsPointer(ref Redot_packed_int32_array value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_packed_int32_array* ReadOnlyRefAsPointer(in Redot_packed_int32_array value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_packed_int32_array AsRef(Redot_packed_int32_array* source)
        => ref *source;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_packed_int32_array AsRef(in Redot_packed_int32_array source)
        => ref *ReadOnlyRefAsPointer(in source);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_packed_int64_array* AsPointer(ref Redot_packed_int64_array value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_packed_int64_array* ReadOnlyRefAsPointer(in Redot_packed_int64_array value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_packed_int64_array AsRef(Redot_packed_int64_array* source)
        => ref *source;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_packed_int64_array AsRef(in Redot_packed_int64_array source)
        => ref *ReadOnlyRefAsPointer(in source);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_packed_float32_array* AsPointer(ref Redot_packed_float32_array value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_packed_float32_array* ReadOnlyRefAsPointer(in Redot_packed_float32_array value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_packed_float32_array AsRef(Redot_packed_float32_array* source)
        => ref *source;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_packed_float32_array AsRef(in Redot_packed_float32_array source)
        => ref *ReadOnlyRefAsPointer(in source);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_packed_float64_array* AsPointer(ref Redot_packed_float64_array value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_packed_float64_array* ReadOnlyRefAsPointer(in Redot_packed_float64_array value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_packed_float64_array AsRef(Redot_packed_float64_array* source)
        => ref *source;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_packed_float64_array AsRef(in Redot_packed_float64_array source)
        => ref *ReadOnlyRefAsPointer(in source);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_packed_string_array* AsPointer(ref Redot_packed_string_array value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_packed_string_array* ReadOnlyRefAsPointer(in Redot_packed_string_array value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_packed_string_array AsRef(Redot_packed_string_array* source)
        => ref *source;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_packed_string_array AsRef(in Redot_packed_string_array source)
        => ref *ReadOnlyRefAsPointer(in source);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_packed_vector2_array* AsPointer(ref Redot_packed_vector2_array value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_packed_vector2_array* ReadOnlyRefAsPointer(in Redot_packed_vector2_array value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_packed_vector2_array AsRef(Redot_packed_vector2_array* source)
        => ref *source;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_packed_vector2_array AsRef(in Redot_packed_vector2_array source)
        => ref *ReadOnlyRefAsPointer(in source);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_packed_vector3_array* AsPointer(ref Redot_packed_vector3_array value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_packed_vector3_array* ReadOnlyRefAsPointer(in Redot_packed_vector3_array value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_packed_vector3_array AsRef(Redot_packed_vector3_array* source)
        => ref *source;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_packed_vector3_array AsRef(in Redot_packed_vector3_array source)
        => ref *ReadOnlyRefAsPointer(in source);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_packed_vector4_array* AsPointer(ref Redot_packed_vector4_array value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_packed_vector4_array* ReadOnlyRefAsPointer(in Redot_packed_vector4_array value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_packed_vector4_array AsRef(Redot_packed_vector4_array* source)
        => ref *source;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_packed_vector4_array AsRef(in Redot_packed_vector4_array source)
        => ref *ReadOnlyRefAsPointer(in source);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_packed_color_array* AsPointer(ref Redot_packed_color_array value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Redot_packed_color_array* ReadOnlyRefAsPointer(in Redot_packed_color_array value)
        => value.GetUnsafeAddress();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_packed_color_array AsRef(Redot_packed_color_array* source)
        => ref *source;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref Redot_packed_color_array AsRef(in Redot_packed_color_array source)
        => ref *ReadOnlyRefAsPointer(in source);
}
