/**************************************************************************/
/*  interop_types.h                                                       */
/**************************************************************************/
/*                         This file is part of:                          */
/*                             REDOT ENGINE                               */
/*                        https://redotengine.org                         */
/**************************************************************************/
/* Copyright (c) 2014-present Redot Engine contributors (see AUTHORS.md). */
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

#define redot_VARIANT_SIZE (sizeof(real_t) * 4 + sizeof(int64_t))

typedef struct {
	uint8_t _dont_touch_that[redot_VARIANT_SIZE];
} redot_variant;

#define redot_ARRAY_SIZE sizeof(void *)

typedef struct {
	uint8_t _dont_touch_that[redot_ARRAY_SIZE];
} redot_array;

#define redot_DICTIONARY_SIZE sizeof(void *)

typedef struct {
	uint8_t _dont_touch_that[redot_DICTIONARY_SIZE];
} redot_dictionary;

#define redot_STRING_SIZE sizeof(void *)

typedef struct {
	uint8_t _dont_touch_that[redot_STRING_SIZE];
} redot_string;

#define redot_STRING_NAME_SIZE sizeof(void *)

typedef struct {
	uint8_t _dont_touch_that[redot_STRING_NAME_SIZE];
} redot_string_name;

#define redot_PACKED_ARRAY_SIZE (2 * sizeof(void *))

typedef struct {
	uint8_t _dont_touch_that[redot_PACKED_ARRAY_SIZE];
} redot_packed_array;

#define redot_VECTOR2_SIZE (sizeof(real_t) * 2)

typedef struct {
	uint8_t _dont_touch_that[redot_VECTOR2_SIZE];
} redot_vector2;

#define redot_VECTOR2I_SIZE (sizeof(int32_t) * 2)

typedef struct {
	uint8_t _dont_touch_that[redot_VECTOR2I_SIZE];
} redot_vector2i;

#define redot_RECT2_SIZE (sizeof(real_t) * 4)

typedef struct redot_rect2 {
	uint8_t _dont_touch_that[redot_RECT2_SIZE];
} redot_rect2;

#define redot_RECT2I_SIZE (sizeof(int32_t) * 4)

typedef struct redot_rect2i {
	uint8_t _dont_touch_that[redot_RECT2I_SIZE];
} redot_rect2i;

#define redot_VECTOR3_SIZE (sizeof(real_t) * 3)

typedef struct {
	uint8_t _dont_touch_that[redot_VECTOR3_SIZE];
} redot_vector3;

#define redot_VECTOR3I_SIZE (sizeof(int32_t) * 3)

typedef struct {
	uint8_t _dont_touch_that[redot_VECTOR3I_SIZE];
} redot_vector3i;

#define redot_TRANSFORM2D_SIZE (sizeof(real_t) * 6)

typedef struct {
	uint8_t _dont_touch_that[redot_TRANSFORM2D_SIZE];
} redot_transform2d;

#define redot_VECTOR4_SIZE (sizeof(real_t) * 4)

typedef struct {
	uint8_t _dont_touch_that[redot_VECTOR4_SIZE];
} redot_vector4;

#define redot_VECTOR4I_SIZE (sizeof(int32_t) * 4)

typedef struct {
	uint8_t _dont_touch_that[redot_VECTOR4I_SIZE];
} redot_vector4i;

#define redot_PLANE_SIZE (sizeof(real_t) * 4)

typedef struct {
	uint8_t _dont_touch_that[redot_PLANE_SIZE];
} redot_plane;

#define redot_QUATERNION_SIZE (sizeof(real_t) * 4)

typedef struct {
	uint8_t _dont_touch_that[redot_QUATERNION_SIZE];
} redot_quaternion;

#define redot_AABB_SIZE (sizeof(real_t) * 6)

typedef struct {
	uint8_t _dont_touch_that[redot_AABB_SIZE];
} redot_aabb;

#define redot_BASIS_SIZE (sizeof(real_t) * 9)

typedef struct {
	uint8_t _dont_touch_that[redot_BASIS_SIZE];
} redot_basis;

#define redot_TRANSFORM3D_SIZE (sizeof(real_t) * 12)

typedef struct {
	uint8_t _dont_touch_that[redot_TRANSFORM3D_SIZE];
} redot_transform3d;

#define redot_PROJECTION_SIZE (sizeof(real_t) * 4 * 4)

typedef struct {
	uint8_t _dont_touch_that[redot_PROJECTION_SIZE];
} redot_projection;

// Colors should always use 32-bit floats, so don't use real_t here.
#define redot_COLOR_SIZE (sizeof(float) * 4)

typedef struct {
	uint8_t _dont_touch_that[redot_COLOR_SIZE];
} redot_color;

#define redot_NODE_PATH_SIZE sizeof(void *)

typedef struct {
	uint8_t _dont_touch_that[redot_NODE_PATH_SIZE];
} redot_node_path;

#define redot_RID_SIZE sizeof(uint64_t)

typedef struct {
	uint8_t _dont_touch_that[redot_RID_SIZE];
} redot_rid;

// Alignment hardcoded in `core/variant/callable.h`.
#define redot_CALLABLE_SIZE (16)

typedef struct {
	uint8_t _dont_touch_that[redot_CALLABLE_SIZE];
} redot_callable;

// Alignment hardcoded in `core/variant/callable.h`.
#define redot_SIGNAL_SIZE (16)

typedef struct {
	uint8_t _dont_touch_that[redot_SIGNAL_SIZE];
} redot_signal;

#ifdef __cplusplus
}
#endif

#endif // INTEROP_TYPES_H
