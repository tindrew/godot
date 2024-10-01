#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable IDE1006 // Naming rule violation
// ReSharper disable InconsistentNaming

using System;
using System.Runtime.CompilerServices;
using Redot.SourceGenerators.Internal;


namespace Redot.NativeInterop
{
    /*
     * IMPORTANT:
     * The order of the methods defined in NativeFuncs must match the order
     * in the array defined at the bottom of 'glue/runtime_interop.cpp'.
     */

    [GenerateUnmanagedCallbacks(typeof(UnmanagedCallbacks))]
    public static unsafe partial class NativeFuncs
    {
        private static bool initialized;

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Global
        public static void Initialize(IntPtr unmanagedCallbacks, int unmanagedCallbacksSize)
        {
            if (initialized)
                throw new InvalidOperationException("Already initialized.");
            initialized = true;

            if (unmanagedCallbacksSize != sizeof(UnmanagedCallbacks))
                throw new ArgumentException("Unmanaged callbacks size mismatch.", nameof(unmanagedCallbacksSize));

            _unmanagedCallbacks = Unsafe.AsRef<UnmanagedCallbacks>((void*)unmanagedCallbacks);
        }

        private partial struct UnmanagedCallbacks
        {
        }

        // Custom functions

        internal static partial Redot_bool Redotsharp_dotnet_module_is_initialized();

        public static partial IntPtr Redotsharp_method_bind_get_method(in Redot_string_name p_classname,
            in Redot_string_name p_methodname);

        public static partial IntPtr Redotsharp_method_bind_get_method_with_compatibility(
            in Redot_string_name p_classname, in Redot_string_name p_methodname, ulong p_hash);

        public static partial delegate* unmanaged<Redot_bool, IntPtr> Redotsharp_get_class_constructor(
            in Redot_string_name p_classname);

        public static partial IntPtr Redotsharp_engine_get_singleton(in Redot_string p_name);


        internal static partial Error Redotsharp_stack_info_vector_resize(
            ref DebuggingUtils.Redot_stack_info_vector p_stack_info_vector, int p_size);

        internal static partial void Redotsharp_stack_info_vector_destroy(
            ref DebuggingUtils.Redot_stack_info_vector p_stack_info_vector);

        internal static partial void Redotsharp_internal_editor_file_system_update_files(in Redot_packed_string_array p_script_paths);

        internal static partial void Redotsharp_internal_script_debugger_send_error(in Redot_string p_func,
            in Redot_string p_file, int p_line, in Redot_string p_err, in Redot_string p_descr,
            Redot_error_handler_type p_type, in DebuggingUtils.Redot_stack_info_vector p_stack_info_vector);

        internal static partial Redot_bool Redotsharp_internal_script_debugger_is_active();

        internal static partial IntPtr Redotsharp_internal_object_get_associated_gchandle(IntPtr ptr);

        internal static partial void Redotsharp_internal_object_disposed(IntPtr ptr, IntPtr gcHandleToFree);

        internal static partial void Redotsharp_internal_refcounted_disposed(IntPtr ptr, IntPtr gcHandleToFree,
            Redot_bool isFinalizer);

        internal static partial Error Redotsharp_internal_signal_awaiter_connect(IntPtr source,
            in Redot_string_name signal,
            IntPtr target, IntPtr awaiterHandlePtr);

        internal static partial void Redotsharp_internal_tie_native_managed_to_unmanaged(IntPtr gcHandleIntPtr,
            IntPtr unmanaged, in Redot_string_name nativeName, Redot_bool refCounted);

        internal static partial void Redotsharp_internal_tie_user_managed_to_unmanaged(IntPtr gcHandleIntPtr,
            IntPtr unmanaged, Redot_ref* scriptPtr, Redot_bool refCounted);

        internal static partial void Redotsharp_internal_tie_managed_to_unmanaged_with_pre_setup(
            IntPtr gcHandleIntPtr, IntPtr unmanaged);

        internal static partial IntPtr Redotsharp_internal_unmanaged_get_script_instance_managed(IntPtr p_unmanaged,
            out Redot_bool r_has_cs_script_instance);

        internal static partial IntPtr Redotsharp_internal_unmanaged_get_instance_binding_managed(IntPtr p_unmanaged);

        internal static partial IntPtr Redotsharp_internal_unmanaged_instance_binding_create_managed(IntPtr p_unmanaged,
            IntPtr oldGCHandlePtr);

        internal static partial void Redotsharp_internal_new_csharp_script(Redot_ref* r_dest);

        internal static partial Redot_bool Redotsharp_internal_script_load(in Redot_string p_path, Redot_ref* r_dest);

        internal static partial void Redotsharp_internal_reload_registered_script(IntPtr scriptPtr);

        internal static partial void Redotsharp_array_filter_Redot_objects_by_native(in Redot_string_name p_native_name,
            in Redot_array p_input, out Redot_array r_output);

        internal static partial void Redotsharp_array_filter_Redot_objects_by_non_native(in Redot_array p_input,
            out Redot_array r_output);

        public static partial void Redotsharp_ref_new_from_ref_counted_ptr(out Redot_ref r_dest,
            IntPtr p_ref_counted_ptr);

        public static partial void Redotsharp_ref_destroy(ref Redot_ref p_instance);

        public static partial void Redotsharp_string_name_new_from_string(out Redot_string_name r_dest,
            in Redot_string p_name);

        public static partial void Redotsharp_node_path_new_from_string(out Redot_node_path r_dest,
            in Redot_string p_name);

        public static partial void
            Redotsharp_string_name_as_string(out Redot_string r_dest, in Redot_string_name p_name);

        public static partial void Redotsharp_node_path_as_string(out Redot_string r_dest, in Redot_node_path p_np);

        public static partial Redot_packed_byte_array Redotsharp_packed_byte_array_new_mem_copy(byte* p_src,
            int p_length);

        public static partial Redot_packed_int32_array Redotsharp_packed_int32_array_new_mem_copy(int* p_src,
            int p_length);

        public static partial Redot_packed_int64_array Redotsharp_packed_int64_array_new_mem_copy(long* p_src,
            int p_length);

        public static partial Redot_packed_float32_array Redotsharp_packed_float32_array_new_mem_copy(float* p_src,
            int p_length);

        public static partial Redot_packed_float64_array Redotsharp_packed_float64_array_new_mem_copy(double* p_src,
            int p_length);

        public static partial Redot_packed_vector2_array Redotsharp_packed_vector2_array_new_mem_copy(Vector2* p_src,
            int p_length);

        public static partial Redot_packed_vector3_array Redotsharp_packed_vector3_array_new_mem_copy(Vector3* p_src,
            int p_length);

        public static partial Redot_packed_vector4_array Redotsharp_packed_vector4_array_new_mem_copy(Vector4* p_src,
            int p_length);

        public static partial Redot_packed_color_array Redotsharp_packed_color_array_new_mem_copy(Color* p_src,
            int p_length);

        public static partial void Redotsharp_packed_string_array_add(ref Redot_packed_string_array r_dest,
            in Redot_string p_element);

        public static partial void Redotsharp_callable_new_with_delegate(IntPtr p_delegate_handle, IntPtr p_trampoline,
            IntPtr p_object, out Redot_callable r_callable);

        internal static partial Redot_bool Redotsharp_callable_get_data_for_marshalling(in Redot_callable p_callable,
            out IntPtr r_delegate_handle, out IntPtr r_trampoline, out IntPtr r_object, out Redot_string_name r_name);

        internal static partial Redot_variant Redotsharp_callable_call(in Redot_callable p_callable,
            Redot_variant** p_args, int p_arg_count, out Redot_variant_call_error p_call_error);

        internal static partial void Redotsharp_callable_call_deferred(in Redot_callable p_callable,
            Redot_variant** p_args, int p_arg_count);

        internal static partial Color Redotsharp_color_from_ok_hsl(float p_h, float p_s, float p_l, float p_alpha);

        // GDNative functions

        // gdnative.h

        public static partial void Redotsharp_method_bind_ptrcall(IntPtr p_method_bind, IntPtr p_instance, void** p_args,
            void* p_ret);

        public static partial Redot_variant Redotsharp_method_bind_call(IntPtr p_method_bind, IntPtr p_instance,
            Redot_variant** p_args, int p_arg_count, out Redot_variant_call_error p_call_error);

        // variant.h

        public static partial void
            Redotsharp_variant_new_string_name(out Redot_variant r_dest, in Redot_string_name p_s);

        public static partial void Redotsharp_variant_new_copy(out Redot_variant r_dest, in Redot_variant p_src);

        public static partial void Redotsharp_variant_new_node_path(out Redot_variant r_dest, in Redot_node_path p_np);

        public static partial void Redotsharp_variant_new_object(out Redot_variant r_dest, IntPtr p_obj);

        public static partial void Redotsharp_variant_new_transform2d(out Redot_variant r_dest, in Transform2D p_t2d);

        public static partial void Redotsharp_variant_new_basis(out Redot_variant r_dest, in Basis p_basis);

        public static partial void Redotsharp_variant_new_transform3d(out Redot_variant r_dest, in Transform3D p_trans);

        public static partial void Redotsharp_variant_new_projection(out Redot_variant r_dest, in Projection p_proj);

        public static partial void Redotsharp_variant_new_aabb(out Redot_variant r_dest, in Aabb p_aabb);

        public static partial void Redotsharp_variant_new_dictionary(out Redot_variant r_dest,
            in Redot_dictionary p_dict);

        public static partial void Redotsharp_variant_new_array(out Redot_variant r_dest, in Redot_array p_arr);

        public static partial void Redotsharp_variant_new_packed_byte_array(out Redot_variant r_dest,
            in Redot_packed_byte_array p_pba);

        public static partial void Redotsharp_variant_new_packed_int32_array(out Redot_variant r_dest,
            in Redot_packed_int32_array p_pia);

        public static partial void Redotsharp_variant_new_packed_int64_array(out Redot_variant r_dest,
            in Redot_packed_int64_array p_pia);

        public static partial void Redotsharp_variant_new_packed_float32_array(out Redot_variant r_dest,
            in Redot_packed_float32_array p_pra);

        public static partial void Redotsharp_variant_new_packed_float64_array(out Redot_variant r_dest,
            in Redot_packed_float64_array p_pra);

        public static partial void Redotsharp_variant_new_packed_string_array(out Redot_variant r_dest,
            in Redot_packed_string_array p_psa);

        public static partial void Redotsharp_variant_new_packed_vector2_array(out Redot_variant r_dest,
            in Redot_packed_vector2_array p_pv2a);

        public static partial void Redotsharp_variant_new_packed_vector3_array(out Redot_variant r_dest,
            in Redot_packed_vector3_array p_pv3a);

        public static partial void Redotsharp_variant_new_packed_vector4_array(out Redot_variant r_dest,
            in Redot_packed_vector4_array p_pv4a);

        public static partial void Redotsharp_variant_new_packed_color_array(out Redot_variant r_dest,
            in Redot_packed_color_array p_pca);

        public static partial Redot_bool Redotsharp_variant_as_bool(in Redot_variant p_self);

        public static partial Int64 Redotsharp_variant_as_int(in Redot_variant p_self);

        public static partial double Redotsharp_variant_as_float(in Redot_variant p_self);

        public static partial Redot_string Redotsharp_variant_as_string(in Redot_variant p_self);

        public static partial Vector2 Redotsharp_variant_as_vector2(in Redot_variant p_self);

        public static partial Vector2I Redotsharp_variant_as_vector2i(in Redot_variant p_self);

        public static partial Rect2 Redotsharp_variant_as_rect2(in Redot_variant p_self);

        public static partial Rect2I Redotsharp_variant_as_rect2i(in Redot_variant p_self);

        public static partial Vector3 Redotsharp_variant_as_vector3(in Redot_variant p_self);

        public static partial Vector3I Redotsharp_variant_as_vector3i(in Redot_variant p_self);

        public static partial Transform2D Redotsharp_variant_as_transform2d(in Redot_variant p_self);

        public static partial Vector4 Redotsharp_variant_as_vector4(in Redot_variant p_self);

        public static partial Vector4I Redotsharp_variant_as_vector4i(in Redot_variant p_self);

        public static partial Plane Redotsharp_variant_as_plane(in Redot_variant p_self);

        public static partial Quaternion Redotsharp_variant_as_quaternion(in Redot_variant p_self);

        public static partial Aabb Redotsharp_variant_as_aabb(in Redot_variant p_self);

        public static partial Basis Redotsharp_variant_as_basis(in Redot_variant p_self);

        public static partial Transform3D Redotsharp_variant_as_transform3d(in Redot_variant p_self);

        public static partial Projection Redotsharp_variant_as_projection(in Redot_variant p_self);

        public static partial Color Redotsharp_variant_as_color(in Redot_variant p_self);

        public static partial Redot_string_name Redotsharp_variant_as_string_name(in Redot_variant p_self);

        public static partial Redot_node_path Redotsharp_variant_as_node_path(in Redot_variant p_self);

        public static partial Rid Redotsharp_variant_as_rid(in Redot_variant p_self);

        public static partial Redot_callable Redotsharp_variant_as_callable(in Redot_variant p_self);

        public static partial Redot_signal Redotsharp_variant_as_signal(in Redot_variant p_self);

        public static partial Redot_dictionary Redotsharp_variant_as_dictionary(in Redot_variant p_self);

        public static partial Redot_array Redotsharp_variant_as_array(in Redot_variant p_self);

        public static partial Redot_packed_byte_array Redotsharp_variant_as_packed_byte_array(in Redot_variant p_self);

        public static partial Redot_packed_int32_array Redotsharp_variant_as_packed_int32_array(in Redot_variant p_self);

        public static partial Redot_packed_int64_array Redotsharp_variant_as_packed_int64_array(in Redot_variant p_self);

        public static partial Redot_packed_float32_array Redotsharp_variant_as_packed_float32_array(
            in Redot_variant p_self);

        public static partial Redot_packed_float64_array Redotsharp_variant_as_packed_float64_array(
            in Redot_variant p_self);

        public static partial Redot_packed_string_array Redotsharp_variant_as_packed_string_array(
            in Redot_variant p_self);

        public static partial Redot_packed_vector2_array Redotsharp_variant_as_packed_vector2_array(
            in Redot_variant p_self);

        public static partial Redot_packed_vector3_array Redotsharp_variant_as_packed_vector3_array(
            in Redot_variant p_self);

        public static partial Redot_packed_vector4_array Redotsharp_variant_as_packed_vector4_array(
            in Redot_variant p_self);

        public static partial Redot_packed_color_array Redotsharp_variant_as_packed_color_array(in Redot_variant p_self);

        public static partial Redot_bool Redotsharp_variant_equals(in Redot_variant p_a, in Redot_variant p_b);

        // string.h

        public static partial void Redotsharp_string_new_with_utf16_chars(out Redot_string r_dest, char* p_contents);

        // string_name.h

        public static partial void Redotsharp_string_name_new_copy(out Redot_string_name r_dest,
            in Redot_string_name p_src);

        // node_path.h

        public static partial void Redotsharp_node_path_new_copy(out Redot_node_path r_dest, in Redot_node_path p_src);

        // array.h

        public static partial void Redotsharp_array_new(out Redot_array r_dest);

        public static partial void Redotsharp_array_new_copy(out Redot_array r_dest, in Redot_array p_src);

        public static partial Redot_variant* Redotsharp_array_ptrw(ref Redot_array p_self);

        // dictionary.h

        public static partial void Redotsharp_dictionary_new(out Redot_dictionary r_dest);

        public static partial void Redotsharp_dictionary_new_copy(out Redot_dictionary r_dest,
            in Redot_dictionary p_src);

        // destroy functions

        public static partial void Redotsharp_packed_byte_array_destroy(ref Redot_packed_byte_array p_self);

        public static partial void Redotsharp_packed_int32_array_destroy(ref Redot_packed_int32_array p_self);

        public static partial void Redotsharp_packed_int64_array_destroy(ref Redot_packed_int64_array p_self);

        public static partial void Redotsharp_packed_float32_array_destroy(ref Redot_packed_float32_array p_self);

        public static partial void Redotsharp_packed_float64_array_destroy(ref Redot_packed_float64_array p_self);

        public static partial void Redotsharp_packed_string_array_destroy(ref Redot_packed_string_array p_self);

        public static partial void Redotsharp_packed_vector2_array_destroy(ref Redot_packed_vector2_array p_self);

        public static partial void Redotsharp_packed_vector3_array_destroy(ref Redot_packed_vector3_array p_self);

        public static partial void Redotsharp_packed_vector4_array_destroy(ref Redot_packed_vector4_array p_self);

        public static partial void Redotsharp_packed_color_array_destroy(ref Redot_packed_color_array p_self);

        public static partial void Redotsharp_variant_destroy(ref Redot_variant p_self);

        public static partial void Redotsharp_string_destroy(ref Redot_string p_self);

        public static partial void Redotsharp_string_name_destroy(ref Redot_string_name p_self);

        public static partial void Redotsharp_node_path_destroy(ref Redot_node_path p_self);

        public static partial void Redotsharp_signal_destroy(ref Redot_signal p_self);

        public static partial void Redotsharp_callable_destroy(ref Redot_callable p_self);

        public static partial void Redotsharp_array_destroy(ref Redot_array p_self);

        public static partial void Redotsharp_dictionary_destroy(ref Redot_dictionary p_self);

        // Array

        public static partial int Redotsharp_array_add(ref Redot_array p_self, in Redot_variant p_item);

        public static partial int Redotsharp_array_add_range(ref Redot_array p_self, in Redot_array p_collection);

        public static partial int Redotsharp_array_binary_search(ref Redot_array p_self, int p_index, int p_count, in Redot_variant p_value);

        public static partial void
            Redotsharp_array_duplicate(ref Redot_array p_self, Redot_bool p_deep, out Redot_array r_dest);

        public static partial void Redotsharp_array_fill(ref Redot_array p_self, in Redot_variant p_value);

        public static partial int Redotsharp_array_index_of(ref Redot_array p_self, in Redot_variant p_item, int p_index = 0);

        public static partial void Redotsharp_array_insert(ref Redot_array p_self, int p_index, in Redot_variant p_item);

        public static partial int Redotsharp_array_last_index_of(ref Redot_array p_self, in Redot_variant p_item, int p_index);

        public static partial void Redotsharp_array_make_read_only(ref Redot_array p_self);

        public static partial void Redotsharp_array_max(ref Redot_array p_self, out Redot_variant r_value);

        public static partial void Redotsharp_array_min(ref Redot_array p_self, out Redot_variant r_value);

        public static partial void Redotsharp_array_pick_random(ref Redot_array p_self, out Redot_variant r_value);

        public static partial Redot_bool Redotsharp_array_recursive_equal(ref Redot_array p_self, in Redot_array p_other);

        public static partial void Redotsharp_array_remove_at(ref Redot_array p_self, int p_index);

        public static partial Error Redotsharp_array_resize(ref Redot_array p_self, int p_new_size);

        public static partial void Redotsharp_array_reverse(ref Redot_array p_self);

        public static partial void Redotsharp_array_shuffle(ref Redot_array p_self);

        public static partial void Redotsharp_array_slice(ref Redot_array p_self, int p_start, int p_end,
            int p_step, Redot_bool p_deep, out Redot_array r_dest);

        public static partial void Redotsharp_array_sort(ref Redot_array p_self);

        public static partial void Redotsharp_array_to_string(ref Redot_array p_self, out Redot_string r_str);

        // Dictionary

        public static partial Redot_bool Redotsharp_dictionary_try_get_value(ref Redot_dictionary p_self,
            in Redot_variant p_key,
            out Redot_variant r_value);

        public static partial void Redotsharp_dictionary_set_value(ref Redot_dictionary p_self, in Redot_variant p_key,
            in Redot_variant p_value);

        public static partial void Redotsharp_dictionary_keys(ref Redot_dictionary p_self, out Redot_array r_dest);

        public static partial void Redotsharp_dictionary_values(ref Redot_dictionary p_self, out Redot_array r_dest);

        public static partial int Redotsharp_dictionary_count(ref Redot_dictionary p_self);

        public static partial void Redotsharp_dictionary_key_value_pair_at(ref Redot_dictionary p_self, int p_index,
            out Redot_variant r_key, out Redot_variant r_value);

        public static partial void Redotsharp_dictionary_add(ref Redot_dictionary p_self, in Redot_variant p_key,
            in Redot_variant p_value);

        public static partial void Redotsharp_dictionary_clear(ref Redot_dictionary p_self);

        public static partial Redot_bool Redotsharp_dictionary_contains_key(ref Redot_dictionary p_self,
            in Redot_variant p_key);

        public static partial void Redotsharp_dictionary_duplicate(ref Redot_dictionary p_self, Redot_bool p_deep,
            out Redot_dictionary r_dest);

        public static partial void Redotsharp_dictionary_merge(ref Redot_dictionary p_self, in Redot_dictionary p_dictionary, Redot_bool p_overwrite);

        public static partial Redot_bool Redotsharp_dictionary_recursive_equal(ref Redot_dictionary p_self, in Redot_dictionary p_other);

        public static partial Redot_bool Redotsharp_dictionary_remove_key(ref Redot_dictionary p_self,
            in Redot_variant p_key);

        public static partial void Redotsharp_dictionary_make_read_only(ref Redot_dictionary p_self);

        public static partial void Redotsharp_dictionary_to_string(ref Redot_dictionary p_self, out Redot_string r_str);

        // StringExtensions

        public static partial void Redotsharp_string_simplify_path(in Redot_string p_self,
            out Redot_string r_simplified_path);

        public static partial void Redotsharp_string_to_camel_case(in Redot_string p_self,
            out Redot_string r_camel_case);

        public static partial void Redotsharp_string_to_pascal_case(in Redot_string p_self,
            out Redot_string r_pascal_case);

        public static partial void Redotsharp_string_to_snake_case(in Redot_string p_self,
            out Redot_string r_snake_case);

        // NodePath

        public static partial void Redotsharp_node_path_get_as_property_path(in Redot_node_path p_self,
            ref Redot_node_path r_dest);

        public static partial void Redotsharp_node_path_get_concatenated_names(in Redot_node_path p_self,
            out Redot_string r_names);

        public static partial void Redotsharp_node_path_get_concatenated_subnames(in Redot_node_path p_self,
            out Redot_string r_subnames);

        public static partial void Redotsharp_node_path_get_name(in Redot_node_path p_self, int p_idx,
            out Redot_string r_name);

        public static partial int Redotsharp_node_path_get_name_count(in Redot_node_path p_self);

        public static partial void Redotsharp_node_path_get_subname(in Redot_node_path p_self, int p_idx,
            out Redot_string r_subname);

        public static partial int Redotsharp_node_path_get_subname_count(in Redot_node_path p_self);

        public static partial Redot_bool Redotsharp_node_path_is_absolute(in Redot_node_path p_self);

        public static partial Redot_bool Redotsharp_node_path_equals(in Redot_node_path p_self, in Redot_node_path p_other);

        public static partial int Redotsharp_node_path_hash(in Redot_node_path p_self);

        // GD, etc

        internal static partial void Redotsharp_bytes_to_var(in Redot_packed_byte_array p_bytes,
            Redot_bool p_allow_objects,
            out Redot_variant r_ret);

        internal static partial void Redotsharp_convert(in Redot_variant p_what, int p_type,
            out Redot_variant r_ret);

        internal static partial int Redotsharp_hash(in Redot_variant p_var);

        internal static partial IntPtr Redotsharp_instance_from_id(ulong p_instance_id);

        internal static partial void Redotsharp_print(in Redot_string p_what);

        public static partial void Redotsharp_print_rich(in Redot_string p_what);

        internal static partial void Redotsharp_printerr(in Redot_string p_what);

        internal static partial void Redotsharp_printraw(in Redot_string p_what);

        internal static partial void Redotsharp_prints(in Redot_string p_what);

        internal static partial void Redotsharp_printt(in Redot_string p_what);

        internal static partial float Redotsharp_randf();

        internal static partial uint Redotsharp_randi();

        internal static partial void Redotsharp_randomize();

        internal static partial double Redotsharp_randf_range(double from, double to);

        internal static partial double Redotsharp_randfn(double mean, double deviation);

        internal static partial int Redotsharp_randi_range(int from, int to);

        internal static partial uint Redotsharp_rand_from_seed(ulong seed, out ulong newSeed);

        internal static partial void Redotsharp_seed(ulong seed);

        internal static partial void Redotsharp_weakref(IntPtr p_obj, out Redot_ref r_weak_ref);

        internal static partial void Redotsharp_str_to_var(in Redot_string p_str, out Redot_variant r_ret);

        internal static partial void Redotsharp_var_to_bytes(in Redot_variant p_what, Redot_bool p_full_objects,
            out Redot_packed_byte_array r_bytes);

        internal static partial void Redotsharp_var_to_str(in Redot_variant p_var, out Redot_string r_ret);

        internal static partial void Redotsharp_err_print_error(in Redot_string p_function, in Redot_string p_file, int p_line, in Redot_string p_error, in Redot_string p_message = default, Redot_bool p_editor_notify = Redot_bool.False, Redot_error_handler_type p_type = Redot_error_handler_type.ERR_HANDLER_ERROR);

        // Object

        public static partial void Redotsharp_object_to_string(IntPtr ptr, out Redot_string r_str);
    }
}
