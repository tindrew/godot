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

        internal static partial redot_bool redotsharp_dotnet_module_is_initialized();

        public static partial IntPtr redotsharp_method_bind_get_method(in redot_string_name p_classname,
            in redot_string_name p_methodname);

        public static partial IntPtr redotsharp_method_bind_get_method_with_compatibility(
            in redot_string_name p_classname, in redot_string_name p_methodname, ulong p_hash);

        public static partial delegate* unmanaged<redot_bool, IntPtr> redotsharp_get_class_constructor(
            in redot_string_name p_classname);

        public static partial IntPtr redotsharp_engine_get_singleton(in redot_string p_name);


        internal static partial Error redotsharp_stack_info_vector_resize(
            ref DebuggingUtils.redot_stack_info_vector p_stack_info_vector, int p_size);

        internal static partial void redotsharp_stack_info_vector_destroy(
            ref DebuggingUtils.redot_stack_info_vector p_stack_info_vector);

        internal static partial void redotsharp_internal_editor_file_system_update_files(in redot_packed_string_array p_script_paths);

        internal static partial void redotsharp_internal_script_debugger_send_error(in redot_string p_func,
            in redot_string p_file, int p_line, in redot_string p_err, in redot_string p_descr,
            redot_error_handler_type p_type, in DebuggingUtils.redot_stack_info_vector p_stack_info_vector);

        internal static partial redot_bool redotsharp_internal_script_debugger_is_active();

        internal static partial IntPtr redotsharp_internal_object_get_associated_gchandle(IntPtr ptr);

        internal static partial void redotsharp_internal_object_disposed(IntPtr ptr, IntPtr gcHandleToFree);

        internal static partial void redotsharp_internal_refcounted_disposed(IntPtr ptr, IntPtr gcHandleToFree,
            redot_bool isFinalizer);

        internal static partial Error redotsharp_internal_signal_awaiter_connect(IntPtr source,
            in redot_string_name signal,
            IntPtr target, IntPtr awaiterHandlePtr);

        internal static partial void redotsharp_internal_tie_native_managed_to_unmanaged(IntPtr gcHandleIntPtr,
            IntPtr unmanaged, in redot_string_name nativeName, redot_bool refCounted);

        internal static partial void redotsharp_internal_tie_user_managed_to_unmanaged(IntPtr gcHandleIntPtr,
            IntPtr unmanaged, redot_ref* scriptPtr, redot_bool refCounted);

        internal static partial void redotsharp_internal_tie_managed_to_unmanaged_with_pre_setup(
            IntPtr gcHandleIntPtr, IntPtr unmanaged);

        internal static partial IntPtr redotsharp_internal_unmanaged_get_script_instance_managed(IntPtr p_unmanaged,
            out redot_bool r_has_cs_script_instance);

        internal static partial IntPtr redotsharp_internal_unmanaged_get_instance_binding_managed(IntPtr p_unmanaged);

        internal static partial IntPtr redotsharp_internal_unmanaged_instance_binding_create_managed(IntPtr p_unmanaged,
            IntPtr oldGCHandlePtr);

        internal static partial void redotsharp_internal_new_csharp_script(redot_ref* r_dest);

        internal static partial redot_bool redotsharp_internal_script_load(in redot_string p_path, redot_ref* r_dest);

        internal static partial void redotsharp_internal_reload_registered_script(IntPtr scriptPtr);

        internal static partial void redotsharp_array_filter_redot_objects_by_native(in redot_string_name p_native_name,
            in redot_array p_input, out redot_array r_output);

        internal static partial void redotsharp_array_filter_redot_objects_by_non_native(in redot_array p_input,
            out redot_array r_output);

        public static partial void redotsharp_ref_new_from_ref_counted_ptr(out redot_ref r_dest,
            IntPtr p_ref_counted_ptr);

        public static partial void redotsharp_ref_destroy(ref redot_ref p_instance);

        public static partial void redotsharp_string_name_new_from_string(out redot_string_name r_dest,
            in redot_string p_name);

        public static partial void redotsharp_node_path_new_from_string(out redot_node_path r_dest,
            in redot_string p_name);

        public static partial void
            redotsharp_string_name_as_string(out redot_string r_dest, in redot_string_name p_name);

        public static partial void redotsharp_node_path_as_string(out redot_string r_dest, in redot_node_path p_np);

        public static partial redot_packed_byte_array redotsharp_packed_byte_array_new_mem_copy(byte* p_src,
            int p_length);

        public static partial redot_packed_int32_array redotsharp_packed_int32_array_new_mem_copy(int* p_src,
            int p_length);

        public static partial redot_packed_int64_array redotsharp_packed_int64_array_new_mem_copy(long* p_src,
            int p_length);

        public static partial redot_packed_float32_array redotsharp_packed_float32_array_new_mem_copy(float* p_src,
            int p_length);

        public static partial redot_packed_float64_array redotsharp_packed_float64_array_new_mem_copy(double* p_src,
            int p_length);

        public static partial redot_packed_vector2_array redotsharp_packed_vector2_array_new_mem_copy(Vector2* p_src,
            int p_length);

        public static partial redot_packed_vector3_array redotsharp_packed_vector3_array_new_mem_copy(Vector3* p_src,
            int p_length);

        public static partial redot_packed_vector4_array redotsharp_packed_vector4_array_new_mem_copy(Vector4* p_src,
            int p_length);

        public static partial redot_packed_color_array redotsharp_packed_color_array_new_mem_copy(Color* p_src,
            int p_length);

        public static partial void redotsharp_packed_string_array_add(ref redot_packed_string_array r_dest,
            in redot_string p_element);

        public static partial void redotsharp_callable_new_with_delegate(IntPtr p_delegate_handle, IntPtr p_trampoline,
            IntPtr p_object, out redot_callable r_callable);

        internal static partial redot_bool redotsharp_callable_get_data_for_marshalling(in redot_callable p_callable,
            out IntPtr r_delegate_handle, out IntPtr r_trampoline, out IntPtr r_object, out redot_string_name r_name);

        internal static partial redot_variant redotsharp_callable_call(in redot_callable p_callable,
            redot_variant** p_args, int p_arg_count, out redot_variant_call_error p_call_error);

        internal static partial void redotsharp_callable_call_deferred(in redot_callable p_callable,
            redot_variant** p_args, int p_arg_count);

        internal static partial Color redotsharp_color_from_ok_hsl(float p_h, float p_s, float p_l, float p_alpha);

        // GDNative functions

        // gdnative.h

        public static partial void redotsharp_method_bind_ptrcall(IntPtr p_method_bind, IntPtr p_instance, void** p_args,
            void* p_ret);

        public static partial redot_variant redotsharp_method_bind_call(IntPtr p_method_bind, IntPtr p_instance,
            redot_variant** p_args, int p_arg_count, out redot_variant_call_error p_call_error);

        // variant.h

        public static partial void
            redotsharp_variant_new_string_name(out redot_variant r_dest, in redot_string_name p_s);

        public static partial void redotsharp_variant_new_copy(out redot_variant r_dest, in redot_variant p_src);

        public static partial void redotsharp_variant_new_node_path(out redot_variant r_dest, in redot_node_path p_np);

        public static partial void redotsharp_variant_new_object(out redot_variant r_dest, IntPtr p_obj);

        public static partial void redotsharp_variant_new_transform2d(out redot_variant r_dest, in Transform2D p_t2d);

        public static partial void redotsharp_variant_new_basis(out redot_variant r_dest, in Basis p_basis);

        public static partial void redotsharp_variant_new_transform3d(out redot_variant r_dest, in Transform3D p_trans);

        public static partial void redotsharp_variant_new_projection(out redot_variant r_dest, in Projection p_proj);

        public static partial void redotsharp_variant_new_aabb(out redot_variant r_dest, in Aabb p_aabb);

        public static partial void redotsharp_variant_new_dictionary(out redot_variant r_dest,
            in redot_dictionary p_dict);

        public static partial void redotsharp_variant_new_array(out redot_variant r_dest, in redot_array p_arr);

        public static partial void redotsharp_variant_new_packed_byte_array(out redot_variant r_dest,
            in redot_packed_byte_array p_pba);

        public static partial void redotsharp_variant_new_packed_int32_array(out redot_variant r_dest,
            in redot_packed_int32_array p_pia);

        public static partial void redotsharp_variant_new_packed_int64_array(out redot_variant r_dest,
            in redot_packed_int64_array p_pia);

        public static partial void redotsharp_variant_new_packed_float32_array(out redot_variant r_dest,
            in redot_packed_float32_array p_pra);

        public static partial void redotsharp_variant_new_packed_float64_array(out redot_variant r_dest,
            in redot_packed_float64_array p_pra);

        public static partial void redotsharp_variant_new_packed_string_array(out redot_variant r_dest,
            in redot_packed_string_array p_psa);

        public static partial void redotsharp_variant_new_packed_vector2_array(out redot_variant r_dest,
            in redot_packed_vector2_array p_pv2a);

        public static partial void redotsharp_variant_new_packed_vector3_array(out redot_variant r_dest,
            in redot_packed_vector3_array p_pv3a);

        public static partial void redotsharp_variant_new_packed_vector4_array(out redot_variant r_dest,
            in redot_packed_vector4_array p_pv4a);

        public static partial void redotsharp_variant_new_packed_color_array(out redot_variant r_dest,
            in redot_packed_color_array p_pca);

        public static partial redot_bool redotsharp_variant_as_bool(in redot_variant p_self);

        public static partial Int64 redotsharp_variant_as_int(in redot_variant p_self);

        public static partial double redotsharp_variant_as_float(in redot_variant p_self);

        public static partial redot_string redotsharp_variant_as_string(in redot_variant p_self);

        public static partial Vector2 redotsharp_variant_as_vector2(in redot_variant p_self);

        public static partial Vector2I redotsharp_variant_as_vector2i(in redot_variant p_self);

        public static partial Rect2 redotsharp_variant_as_rect2(in redot_variant p_self);

        public static partial Rect2I redotsharp_variant_as_rect2i(in redot_variant p_self);

        public static partial Vector3 redotsharp_variant_as_vector3(in redot_variant p_self);

        public static partial Vector3I redotsharp_variant_as_vector3i(in redot_variant p_self);

        public static partial Transform2D redotsharp_variant_as_transform2d(in redot_variant p_self);

        public static partial Vector4 redotsharp_variant_as_vector4(in redot_variant p_self);

        public static partial Vector4I redotsharp_variant_as_vector4i(in redot_variant p_self);

        public static partial Plane redotsharp_variant_as_plane(in redot_variant p_self);

        public static partial Quaternion redotsharp_variant_as_quaternion(in redot_variant p_self);

        public static partial Aabb redotsharp_variant_as_aabb(in redot_variant p_self);

        public static partial Basis redotsharp_variant_as_basis(in redot_variant p_self);

        public static partial Transform3D redotsharp_variant_as_transform3d(in redot_variant p_self);

        public static partial Projection redotsharp_variant_as_projection(in redot_variant p_self);

        public static partial Color redotsharp_variant_as_color(in redot_variant p_self);

        public static partial redot_string_name redotsharp_variant_as_string_name(in redot_variant p_self);

        public static partial redot_node_path redotsharp_variant_as_node_path(in redot_variant p_self);

        public static partial Rid redotsharp_variant_as_rid(in redot_variant p_self);

        public static partial redot_callable redotsharp_variant_as_callable(in redot_variant p_self);

        public static partial redot_signal redotsharp_variant_as_signal(in redot_variant p_self);

        public static partial redot_dictionary redotsharp_variant_as_dictionary(in redot_variant p_self);

        public static partial redot_array redotsharp_variant_as_array(in redot_variant p_self);

        public static partial redot_packed_byte_array redotsharp_variant_as_packed_byte_array(in redot_variant p_self);

        public static partial redot_packed_int32_array redotsharp_variant_as_packed_int32_array(in redot_variant p_self);

        public static partial redot_packed_int64_array redotsharp_variant_as_packed_int64_array(in redot_variant p_self);

        public static partial redot_packed_float32_array redotsharp_variant_as_packed_float32_array(
            in redot_variant p_self);

        public static partial redot_packed_float64_array redotsharp_variant_as_packed_float64_array(
            in redot_variant p_self);

        public static partial redot_packed_string_array redotsharp_variant_as_packed_string_array(
            in redot_variant p_self);

        public static partial redot_packed_vector2_array redotsharp_variant_as_packed_vector2_array(
            in redot_variant p_self);

        public static partial redot_packed_vector3_array redotsharp_variant_as_packed_vector3_array(
            in redot_variant p_self);

        public static partial redot_packed_vector4_array redotsharp_variant_as_packed_vector4_array(
            in redot_variant p_self);

        public static partial redot_packed_color_array redotsharp_variant_as_packed_color_array(in redot_variant p_self);

        public static partial redot_bool redotsharp_variant_equals(in redot_variant p_a, in redot_variant p_b);

        // string.h

        public static partial void redotsharp_string_new_with_utf16_chars(out redot_string r_dest, char* p_contents);

        // string_name.h

        public static partial void redotsharp_string_name_new_copy(out redot_string_name r_dest,
            in redot_string_name p_src);

        // node_path.h

        public static partial void redotsharp_node_path_new_copy(out redot_node_path r_dest, in redot_node_path p_src);

        // array.h

        public static partial void redotsharp_array_new(out redot_array r_dest);

        public static partial void redotsharp_array_new_copy(out redot_array r_dest, in redot_array p_src);

        public static partial redot_variant* redotsharp_array_ptrw(ref redot_array p_self);

        // dictionary.h

        public static partial void redotsharp_dictionary_new(out redot_dictionary r_dest);

        public static partial void redotsharp_dictionary_new_copy(out redot_dictionary r_dest,
            in redot_dictionary p_src);

        // destroy functions

        public static partial void redotsharp_packed_byte_array_destroy(ref redot_packed_byte_array p_self);

        public static partial void redotsharp_packed_int32_array_destroy(ref redot_packed_int32_array p_self);

        public static partial void redotsharp_packed_int64_array_destroy(ref redot_packed_int64_array p_self);

        public static partial void redotsharp_packed_float32_array_destroy(ref redot_packed_float32_array p_self);

        public static partial void redotsharp_packed_float64_array_destroy(ref redot_packed_float64_array p_self);

        public static partial void redotsharp_packed_string_array_destroy(ref redot_packed_string_array p_self);

        public static partial void redotsharp_packed_vector2_array_destroy(ref redot_packed_vector2_array p_self);

        public static partial void redotsharp_packed_vector3_array_destroy(ref redot_packed_vector3_array p_self);

        public static partial void redotsharp_packed_vector4_array_destroy(ref redot_packed_vector4_array p_self);

        public static partial void redotsharp_packed_color_array_destroy(ref redot_packed_color_array p_self);

        public static partial void redotsharp_variant_destroy(ref redot_variant p_self);

        public static partial void redotsharp_string_destroy(ref redot_string p_self);

        public static partial void redotsharp_string_name_destroy(ref redot_string_name p_self);

        public static partial void redotsharp_node_path_destroy(ref redot_node_path p_self);

        public static partial void redotsharp_signal_destroy(ref redot_signal p_self);

        public static partial void redotsharp_callable_destroy(ref redot_callable p_self);

        public static partial void redotsharp_array_destroy(ref redot_array p_self);

        public static partial void redotsharp_dictionary_destroy(ref redot_dictionary p_self);

        // Array

        public static partial int redotsharp_array_add(ref redot_array p_self, in redot_variant p_item);

        public static partial int redotsharp_array_add_range(ref redot_array p_self, in redot_array p_collection);

        public static partial int redotsharp_array_binary_search(ref redot_array p_self, int p_index, int p_count, in redot_variant p_value);

        public static partial void
            redotsharp_array_duplicate(ref redot_array p_self, redot_bool p_deep, out redot_array r_dest);

        public static partial void redotsharp_array_fill(ref redot_array p_self, in redot_variant p_value);

        public static partial int redotsharp_array_index_of(ref redot_array p_self, in redot_variant p_item, int p_index = 0);

        public static partial void redotsharp_array_insert(ref redot_array p_self, int p_index, in redot_variant p_item);

        public static partial int redotsharp_array_last_index_of(ref redot_array p_self, in redot_variant p_item, int p_index);

        public static partial void redotsharp_array_make_read_only(ref redot_array p_self);

        public static partial void redotsharp_array_max(ref redot_array p_self, out redot_variant r_value);

        public static partial void redotsharp_array_min(ref redot_array p_self, out redot_variant r_value);

        public static partial void redotsharp_array_pick_random(ref redot_array p_self, out redot_variant r_value);

        public static partial redot_bool redotsharp_array_recursive_equal(ref redot_array p_self, in redot_array p_other);

        public static partial void redotsharp_array_remove_at(ref redot_array p_self, int p_index);

        public static partial Error redotsharp_array_resize(ref redot_array p_self, int p_new_size);

        public static partial void redotsharp_array_reverse(ref redot_array p_self);

        public static partial void redotsharp_array_shuffle(ref redot_array p_self);

        public static partial void redotsharp_array_slice(ref redot_array p_self, int p_start, int p_end,
            int p_step, redot_bool p_deep, out redot_array r_dest);

        public static partial void redotsharp_array_sort(ref redot_array p_self);

        public static partial void redotsharp_array_to_string(ref redot_array p_self, out redot_string r_str);

        // Dictionary

        public static partial redot_bool redotsharp_dictionary_try_get_value(ref redot_dictionary p_self,
            in redot_variant p_key,
            out redot_variant r_value);

        public static partial void redotsharp_dictionary_set_value(ref redot_dictionary p_self, in redot_variant p_key,
            in redot_variant p_value);

        public static partial void redotsharp_dictionary_keys(ref redot_dictionary p_self, out redot_array r_dest);

        public static partial void redotsharp_dictionary_values(ref redot_dictionary p_self, out redot_array r_dest);

        public static partial int redotsharp_dictionary_count(ref redot_dictionary p_self);

        public static partial void redotsharp_dictionary_key_value_pair_at(ref redot_dictionary p_self, int p_index,
            out redot_variant r_key, out redot_variant r_value);

        public static partial void redotsharp_dictionary_add(ref redot_dictionary p_self, in redot_variant p_key,
            in redot_variant p_value);

        public static partial void redotsharp_dictionary_clear(ref redot_dictionary p_self);

        public static partial redot_bool redotsharp_dictionary_contains_key(ref redot_dictionary p_self,
            in redot_variant p_key);

        public static partial void redotsharp_dictionary_duplicate(ref redot_dictionary p_self, redot_bool p_deep,
            out redot_dictionary r_dest);

        public static partial void redotsharp_dictionary_merge(ref redot_dictionary p_self, in redot_dictionary p_dictionary, redot_bool p_overwrite);

        public static partial redot_bool redotsharp_dictionary_recursive_equal(ref redot_dictionary p_self, in redot_dictionary p_other);

        public static partial redot_bool redotsharp_dictionary_remove_key(ref redot_dictionary p_self,
            in redot_variant p_key);

        public static partial void redotsharp_dictionary_make_read_only(ref redot_dictionary p_self);

        public static partial void redotsharp_dictionary_to_string(ref redot_dictionary p_self, out redot_string r_str);

        // StringExtensions

        public static partial void redotsharp_string_simplify_path(in redot_string p_self,
            out redot_string r_simplified_path);

        public static partial void redotsharp_string_to_camel_case(in redot_string p_self,
            out redot_string r_camel_case);

        public static partial void redotsharp_string_to_pascal_case(in redot_string p_self,
            out redot_string r_pascal_case);

        public static partial void redotsharp_string_to_snake_case(in redot_string p_self,
            out redot_string r_snake_case);

        // NodePath

        public static partial void redotsharp_node_path_get_as_property_path(in redot_node_path p_self,
            ref redot_node_path r_dest);

        public static partial void redotsharp_node_path_get_concatenated_names(in redot_node_path p_self,
            out redot_string r_names);

        public static partial void redotsharp_node_path_get_concatenated_subnames(in redot_node_path p_self,
            out redot_string r_subnames);

        public static partial void redotsharp_node_path_get_name(in redot_node_path p_self, int p_idx,
            out redot_string r_name);

        public static partial int redotsharp_node_path_get_name_count(in redot_node_path p_self);

        public static partial void redotsharp_node_path_get_subname(in redot_node_path p_self, int p_idx,
            out redot_string r_subname);

        public static partial int redotsharp_node_path_get_subname_count(in redot_node_path p_self);

        public static partial redot_bool redotsharp_node_path_is_absolute(in redot_node_path p_self);

        public static partial redot_bool redotsharp_node_path_equals(in redot_node_path p_self, in redot_node_path p_other);

        public static partial int redotsharp_node_path_hash(in redot_node_path p_self);

        // GD, etc

        internal static partial void redotsharp_bytes_to_var(in redot_packed_byte_array p_bytes,
            redot_bool p_allow_objects,
            out redot_variant r_ret);

        internal static partial void redotsharp_convert(in redot_variant p_what, int p_type,
            out redot_variant r_ret);

        internal static partial int redotsharp_hash(in redot_variant p_var);

        internal static partial IntPtr redotsharp_instance_from_id(ulong p_instance_id);

        internal static partial void redotsharp_print(in redot_string p_what);

        public static partial void redotsharp_print_rich(in redot_string p_what);

        internal static partial void redotsharp_printerr(in redot_string p_what);

        internal static partial void redotsharp_printraw(in redot_string p_what);

        internal static partial void redotsharp_prints(in redot_string p_what);

        internal static partial void redotsharp_printt(in redot_string p_what);

        internal static partial float redotsharp_randf();

        internal static partial uint redotsharp_randi();

        internal static partial void redotsharp_randomize();

        internal static partial double redotsharp_randf_range(double from, double to);

        internal static partial double redotsharp_randfn(double mean, double deviation);

        internal static partial int redotsharp_randi_range(int from, int to);

        internal static partial uint redotsharp_rand_from_seed(ulong seed, out ulong newSeed);

        internal static partial void redotsharp_seed(ulong seed);

        internal static partial void redotsharp_weakref(IntPtr p_obj, out redot_ref r_weak_ref);

        internal static partial void redotsharp_str_to_var(in redot_string p_str, out redot_variant r_ret);

        internal static partial void redotsharp_var_to_bytes(in redot_variant p_what, redot_bool p_full_objects,
            out redot_packed_byte_array r_bytes);

        internal static partial void redotsharp_var_to_str(in redot_variant p_var, out redot_string r_ret);

        internal static partial void redotsharp_err_print_error(in redot_string p_function, in redot_string p_file, int p_line, in redot_string p_error, in redot_string p_message = default, redot_bool p_editor_notify = redot_bool.False, redot_error_handler_type p_type = redot_error_handler_type.ERR_HANDLER_ERROR);

        // Object

        public static partial void redotsharp_object_to_string(IntPtr ptr, out redot_string r_str);
    }
}
