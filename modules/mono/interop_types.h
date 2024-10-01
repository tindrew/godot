/**************************************************************************/
/*  interop_types.h                                                       */
/**************************************************************************/
/*                         This file is part of:                          */
/*                             Redot ENGINE                               */
/*                        https://Redotengine.org                         */
/**************************************************************************/
/* Copyright (c) 2014-present Redot Engine contributors (see AUTHORS.md). */
/* Copyright (c) 2007-2014 Juan Linietsky, Ariel Manzur.                  */
/*                                                                        */
/* Permission is hereby granted, free of charge, to any person obtaining  */
/* a copy of this software and associated documentation files (the        */
/* "Software"), to deal in the Software without restriction, including    */
/* without limitation the rights to use, copy, modify, merge, publish,    */
/* distribute, sublicense, and/or sell copies of the Software, and to     */
/* permit persons to whom the Software is furnished to do so, subject to  */
/* the following conditions:                                              */
/*                                                                        */
/* The above copyright notice and this permission notice shall be         */
/* included in all copies or substantial portions of the Software.        */
/*                                                                        */
/* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,        */
/* EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF     */
/* MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. */
/* IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY   */
/* CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,   */
/* TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE      */
/* SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                 */
/**************************************************************************/

#ifndef INTEROP_TYPES_H
#define INTEROP_TYPES_H

#include "core/math/math_defs.h"

#ifdef __cplusplus
extern "C" {
#endif

#include <stdbool.h>
#include <stdint.h>

// This is taken from the old GDNative, which was removed.

#define Redot_VARIANT_SIZE (sizeof(real_t) * 4 + sizeof(int64_t))

typedef struct {
	uint8_t _dont_touch_that[Redot_VARIANT_SIZE];
} Redot_variant;

#define Redot_ARRAY_SIZE sizeof(void *)

typedef struct {
	uint8_t _dont_touch_that[Redot_ARRAY_SIZE];
} Redot_array;

#define Redot_DICTIONARY_SIZE sizeof(void *)

typedef struct {
	uint8_t _dont_touch_that[Redot_DICTIONARY_SIZE];
} Redot_dictionary;

#define Redot_STRING_SIZE sizeof(void *)

typedef struct {
	uint8_t _dont_touch_that[Redot_STRING_SIZE];
} Redot_string;

#define Redot_STRING_NAME_SIZE sizeof(void *)

typedef struct {
	uint8_t _dont_touch_that[Redot_STRING_NAME_SIZE];
} Redot_string_name;

#define Redot_PACKED_ARRAY_SIZE (2 * sizeof(void *))

typedef struct {
	uint8_t _dont_touch_that[Redot_PACKED_ARRAY_SIZE];
} Redot_packed_array;

#define Redot_VECTOR2_SIZE (sizeof(real_t) * 2)

typedef struct {
	uint8_t _dont_touch_that[Redot_VECTOR2_SIZE];
} Redot_vector2;

#define Redot_VECTOR2I_SIZE (sizeof(int32_t) * 2)

typedef struct {
	uint8_t _dont_touch_that[Redot_VECTOR2I_SIZE];
} Redot_vector2i;

#define Redot_RECT2_SIZE (sizeof(real_t) * 4)

typedef struct Redot_rect2 {
	uint8_t _dont_touch_that[Redot_RECT2_SIZE];
} Redot_rect2;

#define Redot_RECT2I_SIZE (sizeof(int32_t) * 4)

typedef struct Redot_rect2i {
	uint8_t _dont_touch_that[Redot_RECT2I_SIZE];
} Redot_rect2i;

#define Redot_VECTOR3_SIZE (sizeof(real_t) * 3)

typedef struct {
	uint8_t _dont_touch_that[Redot_VECTOR3_SIZE];
} Redot_vector3;

#define Redot_VECTOR3I_SIZE (sizeof(int32_t) * 3)

typedef struct {
	uint8_t _dont_touch_that[Redot_VECTOR3I_SIZE];
} Redot_vector3i;

#define Redot_TRANSFORM2D_SIZE (sizeof(real_t) * 6)

typedef struct {
	uint8_t _dont_touch_that[Redot_TRANSFORM2D_SIZE];
} Redot_transform2d;

#define Redot_VECTOR4_SIZE (sizeof(real_t) * 4)

typedef struct {
	uint8_t _dont_touch_that[Redot_VECTOR4_SIZE];
} Redot_vector4;

#define Redot_VECTOR4I_SIZE (sizeof(int32_t) * 4)

typedef struct {
	uint8_t _dont_touch_that[Redot_VECTOR4I_SIZE];
} Redot_vector4i;

#define Redot_PLANE_SIZE (sizeof(real_t) * 4)

typedef struct {
	uint8_t _dont_touch_that[Redot_PLANE_SIZE];
} Redot_plane;

#define Redot_QUATERNION_SIZE (sizeof(real_t) * 4)

typedef struct {
	uint8_t _dont_touch_that[Redot_QUATERNION_SIZE];
} Redot_quaternion;

#define Redot_AABB_SIZE (sizeof(real_t) * 6)

typedef struct {
	uint8_t _dont_touch_that[Redot_AABB_SIZE];
} Redot_aabb;

#define Redot_BASIS_SIZE (sizeof(real_t) * 9)

typedef struct {
	uint8_t _dont_touch_that[Redot_BASIS_SIZE];
} Redot_basis;

#define Redot_TRANSFORM3D_SIZE (sizeof(real_t) * 12)

typedef struct {
	uint8_t _dont_touch_that[Redot_TRANSFORM3D_SIZE];
} Redot_transform3d;

#define Redot_PROJECTION_SIZE (sizeof(real_t) * 4 * 4)

typedef struct {
	uint8_t _dont_touch_that[Redot_PROJECTION_SIZE];
} Redot_projection;

// Colors should always use 32-bit floats, so don't use real_t here.
#define Redot_COLOR_SIZE (sizeof(float) * 4)

typedef struct {
	uint8_t _dont_touch_that[Redot_COLOR_SIZE];
} Redot_color;

#define Redot_NODE_PATH_SIZE sizeof(void *)

typedef struct {
	uint8_t _dont_touch_that[Redot_NODE_PATH_SIZE];
} Redot_node_path;

#define Redot_RID_SIZE sizeof(uint64_t)

typedef struct {
	uint8_t _dont_touch_that[Redot_RID_SIZE];
} Redot_rid;

// Alignment hardcoded in `core/variant/callable.h`.
#define Redot_CALLABLE_SIZE (16)

typedef struct {
	uint8_t _dont_touch_that[Redot_CALLABLE_SIZE];
} Redot_callable;

// Alignment hardcoded in `core/variant/callable.h`.
#define Redot_SIGNAL_SIZE (16)

typedef struct {
	uint8_t _dont_touch_that[Redot_SIGNAL_SIZE];
} Redot_signal;

#ifdef __cplusplus
}
#endif

#endif // INTEROP_TYPES_H
