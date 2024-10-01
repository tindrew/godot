#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable IDE1006 // Naming rule violation
// ReSharper disable InconsistentNaming

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Redot.NativeInterop
{
    // NOTES:
    // ref structs cannot implement interfaces, but they still work in `using` directives if they declare Dispose()

    public static class RedotBoolExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Redot_bool ToRedotBool(this bool @bool)
        {
            return *(Redot_bool*)&@bool;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool ToBool(this Redot_bool RedotBool)
        {
            return *(bool*)&RedotBool;
        }
    }

    // Apparently a struct with a byte is not blittable? It crashes when calling a UnmanagedCallersOnly function ptr.
    public enum Redot_bool : byte
    {
        True = 1,
        False = 0
    }

    [StructLayout(LayoutKind.Sequential)]
    public ref struct Redot_ref
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal readonly unsafe Redot_ref* GetUnsafeAddress()
            => (Redot_ref*)Unsafe.AsPointer(ref Unsafe.AsRef(in _reference));

        private IntPtr _reference;

        public void Dispose()
        {
            if (_reference == IntPtr.Zero)
                return;
            NativeFuncs.Redotsharp_ref_destroy(ref this);
            _reference = IntPtr.Zero;
        }

        public readonly IntPtr Reference
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _reference;
        }

        public readonly bool IsNull
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _reference == IntPtr.Zero;
        }
    }

    public enum Redot_variant_call_error_error
    {
        Redot_CALL_ERROR_CALL_OK = 0,
        Redot_CALL_ERROR_CALL_ERROR_INVALID_METHOD,
        Redot_CALL_ERROR_CALL_ERROR_INVALID_ARGUMENT,
        Redot_CALL_ERROR_CALL_ERROR_TOO_MANY_ARGUMENTS,
        Redot_CALL_ERROR_CALL_ERROR_TOO_FEW_ARGUMENTS,
        Redot_CALL_ERROR_CALL_ERROR_INSTANCE_IS_NULL,
        Redot_CALL_ERROR_CALL_ERROR_METHOD_NOT_CONST,
    }

    [StructLayout(LayoutKind.Sequential)]
    public ref struct Redot_variant_call_error
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal readonly unsafe Redot_variant_call_error* GetUnsafeAddress()
            => (Redot_variant_call_error*)Unsafe.AsPointer(ref Unsafe.AsRef(in error));

        private Redot_variant_call_error_error error;
        private int argument;
        private int expected;

        public Redot_variant_call_error_error Error
        {
            readonly get => error;
            set => error = value;
        }

        public int Argument
        {
            readonly get => argument;
            set => argument = value;
        }

        public Redot.Variant.Type Expected
        {
            readonly get => (Redot.Variant.Type)expected;
            set => expected = (int)value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public ref struct Redot_csharp_type_info
    {
        private Redot_string _className;
        private Redot_string _iconPath;
        private Redot_bool _isTool;
        private Redot_bool _isGlobalClass;
        private Redot_bool _isAbstract;
        private Redot_bool _isConstructedGenericType;
        private Redot_bool _isGenericTypeDefinition;

        public Redot_string ClassName
        {
            readonly get => _className;
            set => _className = value;
        }

        public Redot_string IconPath
        {
            readonly get => _iconPath;
            set => _iconPath = value;
        }

        public Redot_bool IsTool
        {
            readonly get => _isTool;
            set => _isTool = value;
        }

        public Redot_bool IsGlobalClass
        {
            readonly get => _isGlobalClass;
            set => _isGlobalClass = value;
        }

        public Redot_bool IsAbstract
        {
            readonly get => _isAbstract;
            set => _isAbstract = value;
        }

        public Redot_bool IsConstructedGenericType
        {
            readonly get => _isConstructedGenericType;
            set => _isConstructedGenericType = value;
        }

        public Redot_bool IsGenericTypeDefinition
        {
            readonly get => _isGenericTypeDefinition;
            set => _isGenericTypeDefinition = value;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public ref struct Redot_variant
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal readonly unsafe Redot_variant* GetUnsafeAddress()
            => (Redot_variant*)Unsafe.AsPointer(ref Unsafe.AsRef(in _typeField));

        // Variant.Type is generated as an enum of type long, so we can't use for the field as it must only take 32-bits.
        private int _typeField;

        // There's padding here

        private Redot_variant_data _data;

        [StructLayout(LayoutKind.Explicit)]
        private unsafe ref struct Redot_variant_data
        {
            [FieldOffset(0)] public Redot_bool _bool;
            [FieldOffset(0)] public long _int;
            [FieldOffset(0)] public double _float;
            [FieldOffset(0)] public Transform2D* _transform2d;
            [FieldOffset(0)] public Aabb* _aabb;
            [FieldOffset(0)] public Basis* _basis;
            [FieldOffset(0)] public Transform3D* _transform3d;
            [FieldOffset(0)] public Projection* _projection;
            [FieldOffset(0)] private Redot_variant_data_mem _mem;

            // The following fields are not in the C++ union, but this is how they're stored in _mem.
            [FieldOffset(0)] public Redot_string_name _m_string_name;
            [FieldOffset(0)] public Redot_string _m_string;
            [FieldOffset(0)] public Vector4 _m_vector4;
            [FieldOffset(0)] public Vector4I _m_vector4i;
            [FieldOffset(0)] public Vector3 _m_vector3;
            [FieldOffset(0)] public Vector3I _m_vector3i;
            [FieldOffset(0)] public Vector2 _m_vector2;
            [FieldOffset(0)] public Vector2I _m_vector2i;
            [FieldOffset(0)] public Rect2 _m_rect2;
            [FieldOffset(0)] public Rect2I _m_rect2i;
            [FieldOffset(0)] public Plane _m_plane;
            [FieldOffset(0)] public Quaternion _m_quaternion;
            [FieldOffset(0)] public Color _m_color;
            [FieldOffset(0)] public Redot_node_path _m_node_path;
            [FieldOffset(0)] public Rid _m_rid;
            [FieldOffset(0)] public Redot_variant_obj_data _m_obj_data;
            [FieldOffset(0)] public Redot_callable _m_callable;
            [FieldOffset(0)] public Redot_signal _m_signal;
            [FieldOffset(0)] public Redot_dictionary _m_dictionary;
            [FieldOffset(0)] public Redot_array _m_array;

            [StructLayout(LayoutKind.Sequential)]
            public struct Redot_variant_obj_data
            {
                public ulong id;
                public IntPtr obj;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct Redot_variant_data_mem
            {
#pragma warning disable 169
                private real_t _mem0;
                private real_t _mem1;
                private real_t _mem2;
                private real_t _mem3;
#pragma warning restore 169
            }
        }

        public Variant.Type Type
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => (Variant.Type)_typeField;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _typeField = (int)value;
        }

        public Redot_bool Bool
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => _data._bool;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _data._bool = value;
        }

        public long Int
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => _data._int;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _data._int = value;
        }

        public double Float
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => _data._float;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _data._float = value;
        }

        public readonly unsafe Transform2D* Transform2D
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _data._transform2d;
        }

        public readonly unsafe Aabb* Aabb
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _data._aabb;
        }

        public readonly unsafe Basis* Basis
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _data._basis;
        }

        public readonly unsafe Transform3D* Transform3D
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _data._transform3d;
        }

        public readonly unsafe Projection* Projection
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _data._projection;
        }

        public Redot_string_name StringName
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => _data._m_string_name;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _data._m_string_name = value;
        }

        public Redot_string String
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => _data._m_string;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _data._m_string = value;
        }

        public Vector4 Vector4
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => _data._m_vector4;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _data._m_vector4 = value;
        }

        public Vector4I Vector4I
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => _data._m_vector4i;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _data._m_vector4i = value;
        }

        public Vector3 Vector3
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => _data._m_vector3;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _data._m_vector3 = value;
        }

        public Vector3I Vector3I
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => _data._m_vector3i;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _data._m_vector3i = value;
        }

        public Vector2 Vector2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => _data._m_vector2;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _data._m_vector2 = value;
        }

        public Vector2I Vector2I
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => _data._m_vector2i;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _data._m_vector2i = value;
        }

        public Rect2 Rect2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => _data._m_rect2;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _data._m_rect2 = value;
        }

        public Rect2I Rect2I
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => _data._m_rect2i;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _data._m_rect2i = value;
        }

        public Plane Plane
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => _data._m_plane;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _data._m_plane = value;
        }

        public Quaternion Quaternion
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => _data._m_quaternion;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _data._m_quaternion = value;
        }

        public Color Color
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => _data._m_color;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _data._m_color = value;
        }

        public Redot_node_path NodePath
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => _data._m_node_path;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _data._m_node_path = value;
        }

        public Rid Rid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => _data._m_rid;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _data._m_rid = value;
        }

        public Redot_callable Callable
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => _data._m_callable;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _data._m_callable = value;
        }

        public Redot_signal Signal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => _data._m_signal;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _data._m_signal = value;
        }

        public Redot_dictionary Dictionary
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => _data._m_dictionary;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _data._m_dictionary = value;
        }

        public Redot_array Array
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => _data._m_array;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _data._m_array = value;
        }

        public readonly IntPtr Object
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _data._m_obj_data.obj;
        }

        public void Dispose()
        {
            switch (Type)
            {
                case Variant.Type.Nil:
                case Variant.Type.Bool:
                case Variant.Type.Int:
                case Variant.Type.Float:
                case Variant.Type.Vector2:
                case Variant.Type.Vector2I:
                case Variant.Type.Rect2:
                case Variant.Type.Rect2I:
                case Variant.Type.Vector3:
                case Variant.Type.Vector3I:
                case Variant.Type.Vector4:
                case Variant.Type.Vector4I:
                case Variant.Type.Plane:
                case Variant.Type.Quaternion:
                case Variant.Type.Color:
                case Variant.Type.Rid:
                    return;
            }

            NativeFuncs.Redotsharp_variant_destroy(ref this);
            Type = Variant.Type.Nil;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct movable
        {
            // Variant.Type is generated as an enum of type long, so we can't use for the field as it must only take 32-bits.
            [FieldOffset(0)] private int _typeField;

            // There's padding here

            [FieldOffset(8)] private Redot_variant_data.Redot_variant_data_mem _data;

            public static unsafe explicit operator movable(in Redot_variant value)
                => *(movable*)CustomUnsafe.AsPointer(ref CustomUnsafe.AsRef(value));

            public static unsafe explicit operator Redot_variant(movable value)
                => *(Redot_variant*)Unsafe.AsPointer(ref value);

            public unsafe ref Redot_variant DangerousSelfRef =>
                ref CustomUnsafe.AsRef((Redot_variant*)Unsafe.AsPointer(ref this));
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public ref struct Redot_string
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal readonly unsafe Redot_string* GetUnsafeAddress()
            => (Redot_string*)Unsafe.AsPointer(ref Unsafe.AsRef(in _ptr));

        private IntPtr _ptr;

        public void Dispose()
        {
            if (_ptr == IntPtr.Zero)
                return;
            NativeFuncs.Redotsharp_string_destroy(ref this);
            _ptr = IntPtr.Zero;
        }

        public readonly IntPtr Buffer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _ptr;
        }

        // Size including the null termination character
        public readonly unsafe int Size
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _ptr != IntPtr.Zero ? (int)(*((ulong*)_ptr - 1)) : 0;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public ref struct Redot_string_name
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal readonly unsafe Redot_string_name* GetUnsafeAddress()
            => (Redot_string_name*)Unsafe.AsPointer(ref Unsafe.AsRef(in _data));

        private IntPtr _data;

        public void Dispose()
        {
            if (_data == IntPtr.Zero)
                return;
            NativeFuncs.Redotsharp_string_name_destroy(ref this);
            _data = IntPtr.Zero;
        }

        public readonly bool IsAllocated
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _data != IntPtr.Zero;
        }

        public readonly bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            // This is all that's needed to check if it's empty. Equivalent to `== StringName()` in C++.
            get => _data == IntPtr.Zero;
        }

        public static bool operator ==(Redot_string_name left, Redot_string_name right)
        {
            return left._data == right._data;
        }

        public static bool operator !=(Redot_string_name left, Redot_string_name right)
        {
            return !(left == right);
        }

        public bool Equals(Redot_string_name other)
        {
            return _data == other._data;
        }

        public override bool Equals(object obj)
        {
            return obj is StringName s && s.Equals(this);
        }

        public override int GetHashCode()
        {
            return _data.GetHashCode();
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct movable
        {
            private IntPtr _data;

            public static unsafe explicit operator movable(in Redot_string_name value)
                => *(movable*)CustomUnsafe.AsPointer(ref CustomUnsafe.AsRef(value));

            public static unsafe explicit operator Redot_string_name(movable value)
                => *(Redot_string_name*)Unsafe.AsPointer(ref value);

            public unsafe ref Redot_string_name DangerousSelfRef =>
                ref CustomUnsafe.AsRef((Redot_string_name*)Unsafe.AsPointer(ref this));
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public ref struct Redot_node_path
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal readonly unsafe Redot_node_path* GetUnsafeAddress()
            => (Redot_node_path*)Unsafe.AsPointer(ref Unsafe.AsRef(in _data));

        private IntPtr _data;

        public void Dispose()
        {
            if (_data == IntPtr.Zero)
                return;
            NativeFuncs.Redotsharp_node_path_destroy(ref this);
            _data = IntPtr.Zero;
        }

        public readonly bool IsAllocated
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _data != IntPtr.Zero;
        }

        public readonly bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            // This is all that's needed to check if it's empty. It's what the `is_empty()` C++ method does.
            get => _data == IntPtr.Zero;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct movable
        {
            private IntPtr _data;

            public static unsafe explicit operator movable(in Redot_node_path value)
                => *(movable*)CustomUnsafe.AsPointer(ref CustomUnsafe.AsRef(value));

            public static unsafe explicit operator Redot_node_path(movable value)
                => *(Redot_node_path*)Unsafe.AsPointer(ref value);

            public unsafe ref Redot_node_path DangerousSelfRef =>
                ref CustomUnsafe.AsRef((Redot_node_path*)Unsafe.AsPointer(ref this));
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public ref struct Redot_signal
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal readonly unsafe Redot_signal* GetUnsafeAddress()
            => (Redot_signal*)Unsafe.AsPointer(ref Unsafe.AsRef(in _getUnsafeAddressHelper));

        [FieldOffset(0)] private byte _getUnsafeAddressHelper;

        [FieldOffset(0)] private Redot_string_name _name;

        // There's padding here on 32-bit

        [FieldOffset(8)] private ulong _objectId;

        public Redot_signal(Redot_string_name name, ulong objectId) : this()
        {
            _name = name;
            _objectId = objectId;
        }

        public Redot_string_name Name
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _name;
        }

        public ulong ObjectId
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _objectId;
        }

        public void Dispose()
        {
            if (!_name.IsAllocated)
                return;
            NativeFuncs.Redotsharp_signal_destroy(ref this);
            _name = default;
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public ref struct Redot_callable
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal readonly unsafe Redot_callable* GetUnsafeAddress()
            => (Redot_callable*)Unsafe.AsPointer(ref Unsafe.AsRef(in _getUnsafeAddressHelper));

        [FieldOffset(0)] private byte _getUnsafeAddressHelper;

        [FieldOffset(0)] private Redot_string_name _method;

        // There's padding here on 32-bit

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        [FieldOffset(8)] private ulong _objectId;
        [FieldOffset(8)] private IntPtr _custom;

        public Redot_callable(Redot_string_name method, ulong objectId) : this()
        {
            _method = method;
            _objectId = objectId;
        }

        public void Dispose()
        {
            // _custom needs freeing as well
            if (!_method.IsAllocated && _custom == IntPtr.Zero)
                return;
            NativeFuncs.Redotsharp_callable_destroy(ref this);
            _method = default;
            _custom = IntPtr.Zero;
        }
    }

    // A correctly constructed value needs to call the native default constructor to allocate `_p`.
    // Don't pass a C# default constructed `Redot_array` to native code, unless it's going to
    // be re-assigned a new value (the copy constructor checks if `_p` is null so that's fine).
    [StructLayout(LayoutKind.Explicit)]
    public ref struct Redot_array
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal readonly unsafe Redot_array* GetUnsafeAddress()
            => (Redot_array*)Unsafe.AsPointer(ref Unsafe.AsRef(in _getUnsafeAddressHelper));

        [FieldOffset(0)] private byte _getUnsafeAddressHelper;

        [FieldOffset(0)] private unsafe ArrayPrivate* _p;

        [StructLayout(LayoutKind.Sequential)]
        private struct ArrayPrivate
        {
            private uint _safeRefCount;

            public VariantVector _arrayVector;

            private unsafe Redot_variant* _readOnly;

            // There are more fields here, but we don't care as we never store this in C#

            public readonly int Size
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _arrayVector.Size;
            }

            public readonly unsafe bool IsReadOnly
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _readOnly != null;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct VariantVector
        {
            private IntPtr _writeProxy;
            public unsafe Redot_variant* _ptr;

            public readonly unsafe int Size
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _ptr != null ? (int)(*((ulong*)_ptr - 1)) : 0;
            }
        }

        public readonly unsafe Redot_variant* Elements
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _p->_arrayVector._ptr;
        }

        public readonly unsafe bool IsAllocated
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _p != null;
        }

        public readonly unsafe int Size
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _p != null ? _p->Size : 0;
        }

        public readonly unsafe bool IsReadOnly
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _p != null && _p->IsReadOnly;
        }

        public unsafe void Dispose()
        {
            if (_p == null)
                return;
            NativeFuncs.Redotsharp_array_destroy(ref this);
            _p = null;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct movable
        {
            private unsafe ArrayPrivate* _p;

            public static unsafe explicit operator movable(in Redot_array value)
                => *(movable*)CustomUnsafe.AsPointer(ref CustomUnsafe.AsRef(value));

            public static unsafe explicit operator Redot_array(movable value)
                => *(Redot_array*)Unsafe.AsPointer(ref value);

            public unsafe ref Redot_array DangerousSelfRef =>
                ref CustomUnsafe.AsRef((Redot_array*)Unsafe.AsPointer(ref this));
        }
    }

    // IMPORTANT:
    // A correctly constructed value needs to call the native default constructor to allocate `_p`.
    // Don't pass a C# default constructed `Redot_dictionary` to native code, unless it's going to
    // be re-assigned a new value (the copy constructor checks if `_p` is null so that's fine).
    [StructLayout(LayoutKind.Explicit)]
    public ref struct Redot_dictionary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal readonly unsafe Redot_dictionary* GetUnsafeAddress()
            => (Redot_dictionary*)Unsafe.AsPointer(ref Unsafe.AsRef(in _getUnsafeAddressHelper));

        [FieldOffset(0)] private byte _getUnsafeAddressHelper;

        [FieldOffset(0)] private unsafe DictionaryPrivate* _p;

        [StructLayout(LayoutKind.Sequential)]
        private struct DictionaryPrivate
        {
            private uint _safeRefCount;

            private unsafe Redot_variant* _readOnly;

            // There are more fields here, but we don't care as we never store this in C#

            public readonly unsafe bool IsReadOnly
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _readOnly != null;
            }
        }

        public readonly unsafe bool IsAllocated
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _p != null;
        }

        public readonly unsafe bool IsReadOnly
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _p != null && _p->IsReadOnly;
        }

        public unsafe void Dispose()
        {
            if (_p == null)
                return;
            NativeFuncs.Redotsharp_dictionary_destroy(ref this);
            _p = null;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct movable
        {
            private unsafe DictionaryPrivate* _p;

            public static unsafe explicit operator movable(in Redot_dictionary value)
                => *(movable*)CustomUnsafe.AsPointer(ref CustomUnsafe.AsRef(value));

            public static unsafe explicit operator Redot_dictionary(movable value)
                => *(Redot_dictionary*)Unsafe.AsPointer(ref value);

            public unsafe ref Redot_dictionary DangerousSelfRef =>
                ref CustomUnsafe.AsRef((Redot_dictionary*)Unsafe.AsPointer(ref this));
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public ref struct Redot_packed_byte_array
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal readonly unsafe Redot_packed_byte_array* GetUnsafeAddress()
            => (Redot_packed_byte_array*)Unsafe.AsPointer(ref Unsafe.AsRef(in _writeProxy));

        private IntPtr _writeProxy;
        private unsafe byte* _ptr;

        public unsafe void Dispose()
        {
            if (_ptr == null)
                return;
            NativeFuncs.Redotsharp_packed_byte_array_destroy(ref this);
            _ptr = null;
        }

        public readonly unsafe byte* Buffer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _ptr;
        }

        public readonly unsafe int Size
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _ptr != null ? (int)(*((ulong*)_ptr - 1)) : 0;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public ref struct Redot_packed_int32_array
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal readonly unsafe Redot_packed_int32_array* GetUnsafeAddress()
            => (Redot_packed_int32_array*)Unsafe.AsPointer(ref Unsafe.AsRef(in _writeProxy));

        private IntPtr _writeProxy;
        private unsafe int* _ptr;

        public unsafe void Dispose()
        {
            if (_ptr == null)
                return;
            NativeFuncs.Redotsharp_packed_int32_array_destroy(ref this);
            _ptr = null;
        }

        public readonly unsafe int* Buffer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _ptr;
        }

        public readonly unsafe int Size
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _ptr != null ? (int)(*((ulong*)_ptr - 1)) : 0;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public ref struct Redot_packed_int64_array
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal readonly unsafe Redot_packed_int64_array* GetUnsafeAddress()
            => (Redot_packed_int64_array*)Unsafe.AsPointer(ref Unsafe.AsRef(in _writeProxy));

        private IntPtr _writeProxy;
        private unsafe long* _ptr;

        public unsafe void Dispose()
        {
            if (_ptr == null)
                return;
            NativeFuncs.Redotsharp_packed_int64_array_destroy(ref this);
            _ptr = null;
        }

        public readonly unsafe long* Buffer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _ptr;
        }

        public readonly unsafe int Size
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _ptr != null ? (int)(*((ulong*)_ptr - 1)) : 0;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public ref struct Redot_packed_float32_array
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal readonly unsafe Redot_packed_float32_array* GetUnsafeAddress()
            => (Redot_packed_float32_array*)Unsafe.AsPointer(ref Unsafe.AsRef(in _writeProxy));

        private IntPtr _writeProxy;
        private unsafe float* _ptr;

        public unsafe void Dispose()
        {
            if (_ptr == null)
                return;
            NativeFuncs.Redotsharp_packed_float32_array_destroy(ref this);
            _ptr = null;
        }

        public readonly unsafe float* Buffer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _ptr;
        }

        public readonly unsafe int Size
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _ptr != null ? (int)(*((ulong*)_ptr - 1)) : 0;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public ref struct Redot_packed_float64_array
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal readonly unsafe Redot_packed_float64_array* GetUnsafeAddress()
            => (Redot_packed_float64_array*)Unsafe.AsPointer(ref Unsafe.AsRef(in _writeProxy));

        private IntPtr _writeProxy;
        private unsafe double* _ptr;

        public unsafe void Dispose()
        {
            if (_ptr == null)
                return;
            NativeFuncs.Redotsharp_packed_float64_array_destroy(ref this);
            _ptr = null;
        }

        public readonly unsafe double* Buffer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _ptr;
        }

        public readonly unsafe int Size
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _ptr != null ? (int)(*((ulong*)_ptr - 1)) : 0;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public ref struct Redot_packed_string_array
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal readonly unsafe Redot_packed_string_array* GetUnsafeAddress()
            => (Redot_packed_string_array*)Unsafe.AsPointer(ref Unsafe.AsRef(in _writeProxy));

        private IntPtr _writeProxy;
        private unsafe Redot_string* _ptr;

        public unsafe void Dispose()
        {
            if (_ptr == null)
                return;
            NativeFuncs.Redotsharp_packed_string_array_destroy(ref this);
            _ptr = null;
        }

        public readonly unsafe Redot_string* Buffer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _ptr;
        }

        public readonly unsafe int Size
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _ptr != null ? (int)(*((ulong*)_ptr - 1)) : 0;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public ref struct Redot_packed_vector2_array
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal readonly unsafe Redot_packed_vector2_array* GetUnsafeAddress()
            => (Redot_packed_vector2_array*)Unsafe.AsPointer(ref Unsafe.AsRef(in _writeProxy));

        private IntPtr _writeProxy;
        private unsafe Vector2* _ptr;

        public unsafe void Dispose()
        {
            if (_ptr == null)
                return;
            NativeFuncs.Redotsharp_packed_vector2_array_destroy(ref this);
            _ptr = null;
        }

        public readonly unsafe Vector2* Buffer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _ptr;
        }

        public readonly unsafe int Size
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _ptr != null ? (int)(*((ulong*)_ptr - 1)) : 0;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public ref struct Redot_packed_vector3_array
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal readonly unsafe Redot_packed_vector3_array* GetUnsafeAddress()
            => (Redot_packed_vector3_array*)Unsafe.AsPointer(ref Unsafe.AsRef(in _writeProxy));

        private IntPtr _writeProxy;
        private unsafe Vector3* _ptr;

        public unsafe void Dispose()
        {
            if (_ptr == null)
                return;
            NativeFuncs.Redotsharp_packed_vector3_array_destroy(ref this);
            _ptr = null;
        }

        public readonly unsafe Vector3* Buffer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _ptr;
        }

        public readonly unsafe int Size
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _ptr != null ? (int)(*((ulong*)_ptr - 1)) : 0;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    // ReSharper disable once InconsistentNaming
    public ref struct Redot_packed_vector4_array
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal readonly unsafe Redot_packed_vector4_array* GetUnsafeAddress()
            => (Redot_packed_vector4_array*)Unsafe.AsPointer(ref Unsafe.AsRef(in _writeProxy));

        private IntPtr _writeProxy;
        private unsafe Vector4* _ptr;

        public unsafe void Dispose()
        {
            if (_ptr == null)
                return;
            NativeFuncs.Redotsharp_packed_vector4_array_destroy(ref this);
            _ptr = null;
        }

        public readonly unsafe Vector4* Buffer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _ptr;
        }

        public readonly unsafe int Size
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _ptr != null ? (int)(*((ulong*)_ptr - 1)) : 0;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    // ReSharper disable once InconsistentNaming
    public ref struct Redot_packed_color_array
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal readonly unsafe Redot_packed_color_array* GetUnsafeAddress()
            => (Redot_packed_color_array*)Unsafe.AsPointer(ref Unsafe.AsRef(in _writeProxy));

        private IntPtr _writeProxy;
        private unsafe Color* _ptr;

        public unsafe void Dispose()
        {
            if (_ptr == null)
                return;
            NativeFuncs.Redotsharp_packed_color_array_destroy(ref this);
            _ptr = null;
        }

        public readonly unsafe Color* Buffer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _ptr;
        }

        public readonly unsafe int Size
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _ptr != null ? (int)(*((ulong*)_ptr - 1)) : 0;
        }
    }

    public enum Redot_error_handler_type
    {
        ERR_HANDLER_ERROR = 0,
        ERR_HANDLER_WARNING,
        ERR_HANDLER_SCRIPT,
        ERR_HANDLER_SHADER,
    }
}
