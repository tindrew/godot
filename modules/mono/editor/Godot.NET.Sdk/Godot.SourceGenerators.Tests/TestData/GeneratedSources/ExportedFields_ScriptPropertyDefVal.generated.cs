partial class ExportedFields
{
#pragma warning disable CS0109 // Disable warning about redundant 'new' keyword
#if TOOLS
    /// <summary>
    /// Get the default values for all properties declared in this class.
    /// This method is used by Redot to determine the value that will be
    /// used by the inspector when resetting properties.
    /// Do not call this method.
    /// </summary>
    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    internal new static global::System.Collections.Generic.Dictionary<global::Redot.StringName, global::Redot.Variant> GetRedotPropertyDefaultValues()
    {
        var values = new global::System.Collections.Generic.Dictionary<global::Redot.StringName, global::Redot.Variant>(60);
        bool ___fieldBoolean_default_value = true;
        values.Add(PropertyName.@_fieldBoolean, global::Redot.Variant.From<bool>(___fieldBoolean_default_value));
        char ___fieldChar_default_value = 'f';
        values.Add(PropertyName.@_fieldChar, global::Redot.Variant.From<char>(___fieldChar_default_value));
        sbyte ___fieldSByte_default_value = 10;
        values.Add(PropertyName.@_fieldSByte, global::Redot.Variant.From<sbyte>(___fieldSByte_default_value));
        short ___fieldInt16_default_value = 10;
        values.Add(PropertyName.@_fieldInt16, global::Redot.Variant.From<short>(___fieldInt16_default_value));
        int ___fieldInt32_default_value = 10;
        values.Add(PropertyName.@_fieldInt32, global::Redot.Variant.From<int>(___fieldInt32_default_value));
        long ___fieldInt64_default_value = 10;
        values.Add(PropertyName.@_fieldInt64, global::Redot.Variant.From<long>(___fieldInt64_default_value));
        byte ___fieldByte_default_value = 10;
        values.Add(PropertyName.@_fieldByte, global::Redot.Variant.From<byte>(___fieldByte_default_value));
        ushort ___fieldUInt16_default_value = 10;
        values.Add(PropertyName.@_fieldUInt16, global::Redot.Variant.From<ushort>(___fieldUInt16_default_value));
        uint ___fieldUInt32_default_value = 10;
        values.Add(PropertyName.@_fieldUInt32, global::Redot.Variant.From<uint>(___fieldUInt32_default_value));
        ulong ___fieldUInt64_default_value = 10;
        values.Add(PropertyName.@_fieldUInt64, global::Redot.Variant.From<ulong>(___fieldUInt64_default_value));
        float ___fieldSingle_default_value = 10;
        values.Add(PropertyName.@_fieldSingle, global::Redot.Variant.From<float>(___fieldSingle_default_value));
        double ___fieldDouble_default_value = 10;
        values.Add(PropertyName.@_fieldDouble, global::Redot.Variant.From<double>(___fieldDouble_default_value));
        string ___fieldString_default_value = "foo";
        values.Add(PropertyName.@_fieldString, global::Redot.Variant.From<string>(___fieldString_default_value));
        global::Redot.Vector2 ___fieldVector2_default_value = new(10f, 10f);
        values.Add(PropertyName.@_fieldVector2, global::Redot.Variant.From<global::Redot.Vector2>(___fieldVector2_default_value));
        global::Redot.Vector2I ___fieldVector2I_default_value = global::Redot.Vector2I.Up;
        values.Add(PropertyName.@_fieldVector2I, global::Redot.Variant.From<global::Redot.Vector2I>(___fieldVector2I_default_value));
        global::Redot.Rect2 ___fieldRect2_default_value = new(new global::Redot.Vector2(10f, 10f), new global::Redot.Vector2(10f, 10f));
        values.Add(PropertyName.@_fieldRect2, global::Redot.Variant.From<global::Redot.Rect2>(___fieldRect2_default_value));
        global::Redot.Rect2I ___fieldRect2I_default_value = new(new global::Redot.Vector2I(10, 10), new global::Redot.Vector2I(10, 10));
        values.Add(PropertyName.@_fieldRect2I, global::Redot.Variant.From<global::Redot.Rect2I>(___fieldRect2I_default_value));
        global::Redot.Transform2D ___fieldTransform2D_default_value = global::Redot.Transform2D.Identity;
        values.Add(PropertyName.@_fieldTransform2D, global::Redot.Variant.From<global::Redot.Transform2D>(___fieldTransform2D_default_value));
        global::Redot.Vector3 ___fieldVector3_default_value = new(10f, 10f, 10f);
        values.Add(PropertyName.@_fieldVector3, global::Redot.Variant.From<global::Redot.Vector3>(___fieldVector3_default_value));
        global::Redot.Vector3I ___fieldVector3I_default_value = global::Redot.Vector3I.Back;
        values.Add(PropertyName.@_fieldVector3I, global::Redot.Variant.From<global::Redot.Vector3I>(___fieldVector3I_default_value));
        global::Redot.Basis ___fieldBasis_default_value = new global::Redot.Basis(global::Redot.Quaternion.Identity);
        values.Add(PropertyName.@_fieldBasis, global::Redot.Variant.From<global::Redot.Basis>(___fieldBasis_default_value));
        global::Redot.Quaternion ___fieldQuaternion_default_value = new global::Redot.Quaternion(global::Redot.Basis.Identity);
        values.Add(PropertyName.@_fieldQuaternion, global::Redot.Variant.From<global::Redot.Quaternion>(___fieldQuaternion_default_value));
        global::Redot.Transform3D ___fieldTransform3D_default_value = global::Redot.Transform3D.Identity;
        values.Add(PropertyName.@_fieldTransform3D, global::Redot.Variant.From<global::Redot.Transform3D>(___fieldTransform3D_default_value));
        global::Redot.Vector4 ___fieldVector4_default_value = new(10f, 10f, 10f, 10f);
        values.Add(PropertyName.@_fieldVector4, global::Redot.Variant.From<global::Redot.Vector4>(___fieldVector4_default_value));
        global::Redot.Vector4I ___fieldVector4I_default_value = global::Redot.Vector4I.One;
        values.Add(PropertyName.@_fieldVector4I, global::Redot.Variant.From<global::Redot.Vector4I>(___fieldVector4I_default_value));
        global::Redot.Projection ___fieldProjection_default_value = global::Redot.Projection.Identity;
        values.Add(PropertyName.@_fieldProjection, global::Redot.Variant.From<global::Redot.Projection>(___fieldProjection_default_value));
        global::Redot.Aabb ___fieldAabb_default_value = new global::Redot.Aabb(10f, 10f, 10f, new global::Redot.Vector3(1f, 1f, 1f));
        values.Add(PropertyName.@_fieldAabb, global::Redot.Variant.From<global::Redot.Aabb>(___fieldAabb_default_value));
        global::Redot.Color ___fieldColor_default_value = global::Redot.Colors.Aquamarine;
        values.Add(PropertyName.@_fieldColor, global::Redot.Variant.From<global::Redot.Color>(___fieldColor_default_value));
        global::Redot.Plane ___fieldPlane_default_value = global::Redot.Plane.PlaneXZ;
        values.Add(PropertyName.@_fieldPlane, global::Redot.Variant.From<global::Redot.Plane>(___fieldPlane_default_value));
        global::Redot.Callable ___fieldCallable_default_value = new global::Redot.Callable(global::Redot.Engine.GetMainLoop(), "_process");
        values.Add(PropertyName.@_fieldCallable, global::Redot.Variant.From<global::Redot.Callable>(___fieldCallable_default_value));
        global::Redot.Signal ___fieldSignal_default_value = new global::Redot.Signal(global::Redot.Engine.GetMainLoop(), "property_list_changed");
        values.Add(PropertyName.@_fieldSignal, global::Redot.Variant.From<global::Redot.Signal>(___fieldSignal_default_value));
        global::ExportedFields.MyEnum ___fieldEnum_default_value = global::ExportedFields.MyEnum.C;
        values.Add(PropertyName.@_fieldEnum, global::Redot.Variant.From<global::ExportedFields.MyEnum>(___fieldEnum_default_value));
        global::ExportedFields.MyFlagsEnum ___fieldFlagsEnum_default_value = global::ExportedFields.MyFlagsEnum.C;
        values.Add(PropertyName.@_fieldFlagsEnum, global::Redot.Variant.From<global::ExportedFields.MyFlagsEnum>(___fieldFlagsEnum_default_value));
        byte[] ___fieldByteArray_default_value = { 0, 1, 2, 3, 4, 5, 6  };
        values.Add(PropertyName.@_fieldByteArray, global::Redot.Variant.From<byte[]>(___fieldByteArray_default_value));
        int[] ___fieldInt32Array_default_value = { 0, 1, 2, 3, 4, 5, 6  };
        values.Add(PropertyName.@_fieldInt32Array, global::Redot.Variant.From<int[]>(___fieldInt32Array_default_value));
        long[] ___fieldInt64Array_default_value = { 0, 1, 2, 3, 4, 5, 6  };
        values.Add(PropertyName.@_fieldInt64Array, global::Redot.Variant.From<long[]>(___fieldInt64Array_default_value));
        float[] ___fieldSingleArray_default_value = { 0f, 1f, 2f, 3f, 4f, 5f, 6f  };
        values.Add(PropertyName.@_fieldSingleArray, global::Redot.Variant.From<float[]>(___fieldSingleArray_default_value));
        double[] ___fieldDoubleArray_default_value = { 0d, 1d, 2d, 3d, 4d, 5d, 6d  };
        values.Add(PropertyName.@_fieldDoubleArray, global::Redot.Variant.From<double[]>(___fieldDoubleArray_default_value));
        string[] ___fieldStringArray_default_value = { "foo", "bar"  };
        values.Add(PropertyName.@_fieldStringArray, global::Redot.Variant.From<string[]>(___fieldStringArray_default_value));
        string[] ___fieldStringArrayEnum_default_value = { "foo", "bar"  };
        values.Add(PropertyName.@_fieldStringArrayEnum, global::Redot.Variant.From<string[]>(___fieldStringArrayEnum_default_value));
        global::Redot.Vector2[] ___fieldVector2Array_default_value = { global::Redot.Vector2.Up, global::Redot.Vector2.Down, global::Redot.Vector2.Left, global::Redot.Vector2.Right   };
        values.Add(PropertyName.@_fieldVector2Array, global::Redot.Variant.From<global::Redot.Vector2[]>(___fieldVector2Array_default_value));
        global::Redot.Vector3[] ___fieldVector3Array_default_value = { global::Redot.Vector3.Up, global::Redot.Vector3.Down, global::Redot.Vector3.Left, global::Redot.Vector3.Right   };
        values.Add(PropertyName.@_fieldVector3Array, global::Redot.Variant.From<global::Redot.Vector3[]>(___fieldVector3Array_default_value));
        global::Redot.Color[] ___fieldColorArray_default_value = { global::Redot.Colors.Aqua, global::Redot.Colors.Aquamarine, global::Redot.Colors.Azure, global::Redot.Colors.Beige   };
        values.Add(PropertyName.@_fieldColorArray, global::Redot.Variant.From<global::Redot.Color[]>(___fieldColorArray_default_value));
        global::Redot.RedotObject[] ___fieldRedotObjectOrDerivedArray_default_value = { null  };
        values.Add(PropertyName.@_fieldRedotObjectOrDerivedArray, global::Redot.Variant.CreateFrom(___fieldRedotObjectOrDerivedArray_default_value));
        global::Redot.StringName[] ___fieldStringNameArray_default_value = { "foo", "bar"  };
        values.Add(PropertyName.@_fieldStringNameArray, global::Redot.Variant.From<global::Redot.StringName[]>(___fieldStringNameArray_default_value));
        global::Redot.NodePath[] ___fieldNodePathArray_default_value = { "foo", "bar"  };
        values.Add(PropertyName.@_fieldNodePathArray, global::Redot.Variant.From<global::Redot.NodePath[]>(___fieldNodePathArray_default_value));
        global::Redot.Rid[] ___fieldRidArray_default_value = { default, default, default  };
        values.Add(PropertyName.@_fieldRidArray, global::Redot.Variant.From<global::Redot.Rid[]>(___fieldRidArray_default_value));
        int[] ___fieldEmptyInt32Array_default_value = global::System.Array.Empty<int>();
        values.Add(PropertyName.@_fieldEmptyInt32Array, global::Redot.Variant.From<int[]>(___fieldEmptyInt32Array_default_value));
        int[] ___fieldArrayFromList_default_value = new global::System.Collections.Generic.List<int>(global::System.Array.Empty<int>()).ToArray();
        values.Add(PropertyName.@_fieldArrayFromList, global::Redot.Variant.From<int[]>(___fieldArrayFromList_default_value));
        global::Redot.Variant ___fieldVariant_default_value = "foo";
        values.Add(PropertyName.@_fieldVariant, global::Redot.Variant.From<global::Redot.Variant>(___fieldVariant_default_value));
        global::Redot.RedotObject ___fieldRedotObjectOrDerived_default_value = default;
        values.Add(PropertyName.@_fieldRedotObjectOrDerived, global::Redot.Variant.From<global::Redot.RedotObject>(___fieldRedotObjectOrDerived_default_value));
        global::Redot.Texture ___fieldRedotResourceTexture_default_value = default;
        values.Add(PropertyName.@_fieldRedotResourceTexture, global::Redot.Variant.From<global::Redot.Texture>(___fieldRedotResourceTexture_default_value));
        global::Redot.StringName ___fieldStringName_default_value = new global::Redot.StringName("foo");
        values.Add(PropertyName.@_fieldStringName, global::Redot.Variant.From<global::Redot.StringName>(___fieldStringName_default_value));
        global::Redot.NodePath ___fieldNodePath_default_value = new global::Redot.NodePath("foo");
        values.Add(PropertyName.@_fieldNodePath, global::Redot.Variant.From<global::Redot.NodePath>(___fieldNodePath_default_value));
        global::Redot.Rid ___fieldRid_default_value = default;
        values.Add(PropertyName.@_fieldRid, global::Redot.Variant.From<global::Redot.Rid>(___fieldRid_default_value));
        global::Redot.Collections.Dictionary ___fieldRedotDictionary_default_value = new()  { { "foo", 10  }, { global::Redot.Vector2.Up, global::Redot.Colors.Chocolate   }  };
        values.Add(PropertyName.@_fieldRedotDictionary, global::Redot.Variant.From<global::Redot.Collections.Dictionary>(___fieldRedotDictionary_default_value));
        global::Redot.Collections.Array ___fieldRedotArray_default_value = new()  { "foo", 10, global::Redot.Vector2.Up, global::Redot.Colors.Chocolate   };
        values.Add(PropertyName.@_fieldRedotArray, global::Redot.Variant.From<global::Redot.Collections.Array>(___fieldRedotArray_default_value));
        global::Redot.Collections.Dictionary<string, bool> ___fieldRedotGenericDictionary_default_value = new()  { { "foo", true  }, { "bar", false  }  };
        values.Add(PropertyName.@_fieldRedotGenericDictionary, global::Redot.Variant.CreateFrom(___fieldRedotGenericDictionary_default_value));
        global::Redot.Collections.Array<int> ___fieldRedotGenericArray_default_value = new()  { 0, 1, 2, 3, 4, 5, 6  };
        values.Add(PropertyName.@_fieldRedotGenericArray, global::Redot.Variant.CreateFrom(___fieldRedotGenericArray_default_value));
        long[] ___fieldEmptyInt64Array_default_value = global::System.Array.Empty<long>();
        values.Add(PropertyName.@_fieldEmptyInt64Array, global::Redot.Variant.From<long[]>(___fieldEmptyInt64Array_default_value));
        return values;
    }
#endif // TOOLS
#pragma warning restore CS0109
}
