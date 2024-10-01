partial class ExportedProperties
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
        var values = new global::System.Collections.Generic.Dictionary<global::Redot.StringName, global::Redot.Variant>(64);
        string __NotGenerateComplexLamdaProperty_default_value = default;
        values.Add(PropertyName.@NotGenerateComplexLamdaProperty, global::Redot.Variant.From<string>(__NotGenerateComplexLamdaProperty_default_value));
        string __NotGenerateLamdaNoFieldProperty_default_value = default;
        values.Add(PropertyName.@NotGenerateLamdaNoFieldProperty, global::Redot.Variant.From<string>(__NotGenerateLamdaNoFieldProperty_default_value));
        string __NotGenerateComplexReturnProperty_default_value = default;
        values.Add(PropertyName.@NotGenerateComplexReturnProperty, global::Redot.Variant.From<string>(__NotGenerateComplexReturnProperty_default_value));
        string __NotGenerateReturnsProperty_default_value = default;
        values.Add(PropertyName.@NotGenerateReturnsProperty, global::Redot.Variant.From<string>(__NotGenerateReturnsProperty_default_value));
        string __FullPropertyString_default_value = "FullPropertyString";
        values.Add(PropertyName.@FullPropertyString, global::Redot.Variant.From<string>(__FullPropertyString_default_value));
        string __FullPropertyString_Complex_default_value = new string("FullPropertyString_Complex")   + global::System.Convert.ToInt32("1");
        values.Add(PropertyName.@FullPropertyString_Complex, global::Redot.Variant.From<string>(__FullPropertyString_Complex_default_value));
        string __LamdaPropertyString_default_value = "LamdaPropertyString";
        values.Add(PropertyName.@LamdaPropertyString, global::Redot.Variant.From<string>(__LamdaPropertyString_default_value));
        bool __PropertyBoolean_default_value = true;
        values.Add(PropertyName.@PropertyBoolean, global::Redot.Variant.From<bool>(__PropertyBoolean_default_value));
        char __PropertyChar_default_value = 'f';
        values.Add(PropertyName.@PropertyChar, global::Redot.Variant.From<char>(__PropertyChar_default_value));
        sbyte __PropertySByte_default_value = 10;
        values.Add(PropertyName.@PropertySByte, global::Redot.Variant.From<sbyte>(__PropertySByte_default_value));
        short __PropertyInt16_default_value = 10;
        values.Add(PropertyName.@PropertyInt16, global::Redot.Variant.From<short>(__PropertyInt16_default_value));
        int __PropertyInt32_default_value = 10;
        values.Add(PropertyName.@PropertyInt32, global::Redot.Variant.From<int>(__PropertyInt32_default_value));
        long __PropertyInt64_default_value = 10;
        values.Add(PropertyName.@PropertyInt64, global::Redot.Variant.From<long>(__PropertyInt64_default_value));
        byte __PropertyByte_default_value = 10;
        values.Add(PropertyName.@PropertyByte, global::Redot.Variant.From<byte>(__PropertyByte_default_value));
        ushort __PropertyUInt16_default_value = 10;
        values.Add(PropertyName.@PropertyUInt16, global::Redot.Variant.From<ushort>(__PropertyUInt16_default_value));
        uint __PropertyUInt32_default_value = 10;
        values.Add(PropertyName.@PropertyUInt32, global::Redot.Variant.From<uint>(__PropertyUInt32_default_value));
        ulong __PropertyUInt64_default_value = 10;
        values.Add(PropertyName.@PropertyUInt64, global::Redot.Variant.From<ulong>(__PropertyUInt64_default_value));
        float __PropertySingle_default_value = 10;
        values.Add(PropertyName.@PropertySingle, global::Redot.Variant.From<float>(__PropertySingle_default_value));
        double __PropertyDouble_default_value = 10;
        values.Add(PropertyName.@PropertyDouble, global::Redot.Variant.From<double>(__PropertyDouble_default_value));
        string __PropertyString_default_value = "foo";
        values.Add(PropertyName.@PropertyString, global::Redot.Variant.From<string>(__PropertyString_default_value));
        global::Redot.Vector2 __PropertyVector2_default_value = new(10f, 10f);
        values.Add(PropertyName.@PropertyVector2, global::Redot.Variant.From<global::Redot.Vector2>(__PropertyVector2_default_value));
        global::Redot.Vector2I __PropertyVector2I_default_value = global::Redot.Vector2I.Up;
        values.Add(PropertyName.@PropertyVector2I, global::Redot.Variant.From<global::Redot.Vector2I>(__PropertyVector2I_default_value));
        global::Redot.Rect2 __PropertyRect2_default_value = new(new global::Redot.Vector2(10f, 10f), new global::Redot.Vector2(10f, 10f));
        values.Add(PropertyName.@PropertyRect2, global::Redot.Variant.From<global::Redot.Rect2>(__PropertyRect2_default_value));
        global::Redot.Rect2I __PropertyRect2I_default_value = new(new global::Redot.Vector2I(10, 10), new global::Redot.Vector2I(10, 10));
        values.Add(PropertyName.@PropertyRect2I, global::Redot.Variant.From<global::Redot.Rect2I>(__PropertyRect2I_default_value));
        global::Redot.Transform2D __PropertyTransform2D_default_value = global::Redot.Transform2D.Identity;
        values.Add(PropertyName.@PropertyTransform2D, global::Redot.Variant.From<global::Redot.Transform2D>(__PropertyTransform2D_default_value));
        global::Redot.Vector3 __PropertyVector3_default_value = new(10f, 10f, 10f);
        values.Add(PropertyName.@PropertyVector3, global::Redot.Variant.From<global::Redot.Vector3>(__PropertyVector3_default_value));
        global::Redot.Vector3I __PropertyVector3I_default_value = global::Redot.Vector3I.Back;
        values.Add(PropertyName.@PropertyVector3I, global::Redot.Variant.From<global::Redot.Vector3I>(__PropertyVector3I_default_value));
        global::Redot.Basis __PropertyBasis_default_value = new global::Redot.Basis(global::Redot.Quaternion.Identity);
        values.Add(PropertyName.@PropertyBasis, global::Redot.Variant.From<global::Redot.Basis>(__PropertyBasis_default_value));
        global::Redot.Quaternion __PropertyQuaternion_default_value = new global::Redot.Quaternion(global::Redot.Basis.Identity);
        values.Add(PropertyName.@PropertyQuaternion, global::Redot.Variant.From<global::Redot.Quaternion>(__PropertyQuaternion_default_value));
        global::Redot.Transform3D __PropertyTransform3D_default_value = global::Redot.Transform3D.Identity;
        values.Add(PropertyName.@PropertyTransform3D, global::Redot.Variant.From<global::Redot.Transform3D>(__PropertyTransform3D_default_value));
        global::Redot.Vector4 __PropertyVector4_default_value = new(10f, 10f, 10f, 10f);
        values.Add(PropertyName.@PropertyVector4, global::Redot.Variant.From<global::Redot.Vector4>(__PropertyVector4_default_value));
        global::Redot.Vector4I __PropertyVector4I_default_value = global::Redot.Vector4I.One;
        values.Add(PropertyName.@PropertyVector4I, global::Redot.Variant.From<global::Redot.Vector4I>(__PropertyVector4I_default_value));
        global::Redot.Projection __PropertyProjection_default_value = global::Redot.Projection.Identity;
        values.Add(PropertyName.@PropertyProjection, global::Redot.Variant.From<global::Redot.Projection>(__PropertyProjection_default_value));
        global::Redot.Aabb __PropertyAabb_default_value = new global::Redot.Aabb(10f, 10f, 10f, new global::Redot.Vector3(1f, 1f, 1f));
        values.Add(PropertyName.@PropertyAabb, global::Redot.Variant.From<global::Redot.Aabb>(__PropertyAabb_default_value));
        global::Redot.Color __PropertyColor_default_value = global::Redot.Colors.Aquamarine;
        values.Add(PropertyName.@PropertyColor, global::Redot.Variant.From<global::Redot.Color>(__PropertyColor_default_value));
        global::Redot.Plane __PropertyPlane_default_value = global::Redot.Plane.PlaneXZ;
        values.Add(PropertyName.@PropertyPlane, global::Redot.Variant.From<global::Redot.Plane>(__PropertyPlane_default_value));
        global::Redot.Callable __PropertyCallable_default_value = new global::Redot.Callable(global::Redot.Engine.GetMainLoop(), "_process");
        values.Add(PropertyName.@PropertyCallable, global::Redot.Variant.From<global::Redot.Callable>(__PropertyCallable_default_value));
        global::Redot.Signal __PropertySignal_default_value = new global::Redot.Signal(global::Redot.Engine.GetMainLoop(), "Propertylist_changed");
        values.Add(PropertyName.@PropertySignal, global::Redot.Variant.From<global::Redot.Signal>(__PropertySignal_default_value));
        global::ExportedProperties.MyEnum __PropertyEnum_default_value = global::ExportedProperties.MyEnum.C;
        values.Add(PropertyName.@PropertyEnum, global::Redot.Variant.From<global::ExportedProperties.MyEnum>(__PropertyEnum_default_value));
        global::ExportedProperties.MyFlagsEnum __PropertyFlagsEnum_default_value = global::ExportedProperties.MyFlagsEnum.C;
        values.Add(PropertyName.@PropertyFlagsEnum, global::Redot.Variant.From<global::ExportedProperties.MyFlagsEnum>(__PropertyFlagsEnum_default_value));
        byte[] __PropertyByteArray_default_value = { 0, 1, 2, 3, 4, 5, 6  };
        values.Add(PropertyName.@PropertyByteArray, global::Redot.Variant.From<byte[]>(__PropertyByteArray_default_value));
        int[] __PropertyInt32Array_default_value = { 0, 1, 2, 3, 4, 5, 6  };
        values.Add(PropertyName.@PropertyInt32Array, global::Redot.Variant.From<int[]>(__PropertyInt32Array_default_value));
        long[] __PropertyInt64Array_default_value = { 0, 1, 2, 3, 4, 5, 6  };
        values.Add(PropertyName.@PropertyInt64Array, global::Redot.Variant.From<long[]>(__PropertyInt64Array_default_value));
        float[] __PropertySingleArray_default_value = { 0f, 1f, 2f, 3f, 4f, 5f, 6f  };
        values.Add(PropertyName.@PropertySingleArray, global::Redot.Variant.From<float[]>(__PropertySingleArray_default_value));
        double[] __PropertyDoubleArray_default_value = { 0d, 1d, 2d, 3d, 4d, 5d, 6d  };
        values.Add(PropertyName.@PropertyDoubleArray, global::Redot.Variant.From<double[]>(__PropertyDoubleArray_default_value));
        string[] __PropertyStringArray_default_value = { "foo", "bar"  };
        values.Add(PropertyName.@PropertyStringArray, global::Redot.Variant.From<string[]>(__PropertyStringArray_default_value));
        string[] __PropertyStringArrayEnum_default_value = { "foo", "bar"  };
        values.Add(PropertyName.@PropertyStringArrayEnum, global::Redot.Variant.From<string[]>(__PropertyStringArrayEnum_default_value));
        global::Redot.Vector2[] __PropertyVector2Array_default_value = { global::Redot.Vector2.Up, global::Redot.Vector2.Down, global::Redot.Vector2.Left, global::Redot.Vector2.Right   };
        values.Add(PropertyName.@PropertyVector2Array, global::Redot.Variant.From<global::Redot.Vector2[]>(__PropertyVector2Array_default_value));
        global::Redot.Vector3[] __PropertyVector3Array_default_value = { global::Redot.Vector3.Up, global::Redot.Vector3.Down, global::Redot.Vector3.Left, global::Redot.Vector3.Right   };
        values.Add(PropertyName.@PropertyVector3Array, global::Redot.Variant.From<global::Redot.Vector3[]>(__PropertyVector3Array_default_value));
        global::Redot.Color[] __PropertyColorArray_default_value = { global::Redot.Colors.Aqua, global::Redot.Colors.Aquamarine, global::Redot.Colors.Azure, global::Redot.Colors.Beige   };
        values.Add(PropertyName.@PropertyColorArray, global::Redot.Variant.From<global::Redot.Color[]>(__PropertyColorArray_default_value));
        global::Redot.RedotObject[] __PropertyRedotObjectOrDerivedArray_default_value = { null  };
        values.Add(PropertyName.@PropertyRedotObjectOrDerivedArray, global::Redot.Variant.CreateFrom(__PropertyRedotObjectOrDerivedArray_default_value));
        global::Redot.StringName[] __field_StringNameArray_default_value = { "foo", "bar"  };
        values.Add(PropertyName.@field_StringNameArray, global::Redot.Variant.From<global::Redot.StringName[]>(__field_StringNameArray_default_value));
        global::Redot.NodePath[] __field_NodePathArray_default_value = { "foo", "bar"  };
        values.Add(PropertyName.@field_NodePathArray, global::Redot.Variant.From<global::Redot.NodePath[]>(__field_NodePathArray_default_value));
        global::Redot.Rid[] __field_RidArray_default_value = { default, default, default  };
        values.Add(PropertyName.@field_RidArray, global::Redot.Variant.From<global::Redot.Rid[]>(__field_RidArray_default_value));
        global::Redot.Variant __PropertyVariant_default_value = "foo";
        values.Add(PropertyName.@PropertyVariant, global::Redot.Variant.From<global::Redot.Variant>(__PropertyVariant_default_value));
        global::Redot.RedotObject __PropertyRedotObjectOrDerived_default_value = default;
        values.Add(PropertyName.@PropertyRedotObjectOrDerived, global::Redot.Variant.From<global::Redot.RedotObject>(__PropertyRedotObjectOrDerived_default_value));
        global::Redot.Texture __PropertyRedotResourceTexture_default_value = default;
        values.Add(PropertyName.@PropertyRedotResourceTexture, global::Redot.Variant.From<global::Redot.Texture>(__PropertyRedotResourceTexture_default_value));
        global::Redot.StringName __PropertyStringName_default_value = new global::Redot.StringName("foo");
        values.Add(PropertyName.@PropertyStringName, global::Redot.Variant.From<global::Redot.StringName>(__PropertyStringName_default_value));
        global::Redot.NodePath __PropertyNodePath_default_value = new global::Redot.NodePath("foo");
        values.Add(PropertyName.@PropertyNodePath, global::Redot.Variant.From<global::Redot.NodePath>(__PropertyNodePath_default_value));
        global::Redot.Rid __PropertyRid_default_value = default;
        values.Add(PropertyName.@PropertyRid, global::Redot.Variant.From<global::Redot.Rid>(__PropertyRid_default_value));
        global::Redot.Collections.Dictionary __PropertyRedotDictionary_default_value = new()  { { "foo", 10  }, { global::Redot.Vector2.Up, global::Redot.Colors.Chocolate   }  };
        values.Add(PropertyName.@PropertyRedotDictionary, global::Redot.Variant.From<global::Redot.Collections.Dictionary>(__PropertyRedotDictionary_default_value));
        global::Redot.Collections.Array __PropertyRedotArray_default_value = new()  { "foo", 10, global::Redot.Vector2.Up, global::Redot.Colors.Chocolate   };
        values.Add(PropertyName.@PropertyRedotArray, global::Redot.Variant.From<global::Redot.Collections.Array>(__PropertyRedotArray_default_value));
        global::Redot.Collections.Dictionary<string, bool> __PropertyRedotGenericDictionary_default_value = new()  { { "foo", true  }, { "bar", false  }  };
        values.Add(PropertyName.@PropertyRedotGenericDictionary, global::Redot.Variant.CreateFrom(__PropertyRedotGenericDictionary_default_value));
        global::Redot.Collections.Array<int> __PropertyRedotGenericArray_default_value = new()  { 0, 1, 2, 3, 4, 5, 6  };
        values.Add(PropertyName.@PropertyRedotGenericArray, global::Redot.Variant.CreateFrom(__PropertyRedotGenericArray_default_value));
        return values;
    }
#endif // TOOLS
#pragma warning restore CS0109
}
