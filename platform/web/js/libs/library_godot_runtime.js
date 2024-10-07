/**************************************************************************/
/*  library_redot_runtime.js                                              */
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

const RedotRuntime = {
	$RedotRuntime: {
		/*
		 * Functions
		 */
		get_func: function (ptr) {
			return wasmTable.get(ptr);
		},

		/*
		 * Prints
		 */
		error: function () {
			err.apply(null, Array.from(arguments)); // eslint-disable-line no-undef
		},

		print: function () {
			out.apply(null, Array.from(arguments)); // eslint-disable-line no-undef
		},

		/*
		 * Memory
		 */
		malloc: function (p_size) {
			return _malloc(p_size);
		},

		free: function (p_ptr) {
			_free(p_ptr);
		},

		getHeapValue: function (p_ptr, p_type) {
			return getValue(p_ptr, p_type);
		},

		setHeapValue: function (p_ptr, p_value, p_type) {
			setValue(p_ptr, p_value, p_type);
		},

		heapSub: function (p_heap, p_ptr, p_len) {
			const bytes = p_heap.BYTES_PER_ELEMENT;
			return p_heap.subarray(p_ptr / bytes, p_ptr / bytes + p_len);
		},

		heapSlice: function (p_heap, p_ptr, p_len) {
			const bytes = p_heap.BYTES_PER_ELEMENT;
			return p_heap.slice(p_ptr / bytes, p_ptr / bytes + p_len);
		},

		heapCopy: function (p_dst, p_src, p_ptr) {
			const bytes = p_src.BYTES_PER_ELEMENT;
			return p_dst.set(p_src, p_ptr / bytes);
		},

		/*
		 * Strings
		 */
		parseString: function (p_ptr) {
			return UTF8ToString(p_ptr);
		},

		parseStringArray: function (p_ptr, p_size) {
			const strings = [];
			const ptrs = RedotRuntime.heapSub(HEAP32, p_ptr, p_size); // TODO wasm64
			ptrs.forEach(function (ptr) {
				strings.push(RedotRuntime.parseString(ptr));
			});
			return strings;
		},

		strlen: function (p_str) {
			return lengthBytesUTF8(p_str);
		},

		allocString: function (p_str) {
			const length = RedotRuntime.strlen(p_str) + 1;
			const c_str = RedotRuntime.malloc(length);
			stringToUTF8(p_str, c_str, length);
			return c_str;
		},

		allocStringArray: function (p_strings) {
			const size = p_strings.length;
			const c_ptr = RedotRuntime.malloc(size * 4);
			for (let i = 0; i < size; i++) {
				HEAP32[(c_ptr >> 2) + i] = RedotRuntime.allocString(p_strings[i]);
			}
			return c_ptr;
		},

		freeStringArray: function (p_ptr, p_len) {
			for (let i = 0; i < p_len; i++) {
				RedotRuntime.free(HEAP32[(p_ptr >> 2) + i]);
			}
			RedotRuntime.free(p_ptr);
		},

		stringToHeap: function (p_str, p_ptr, p_len) {
			return stringToUTF8Array(p_str, HEAP8, p_ptr, p_len);
		},
	},
};
autoAddDeps(RedotRuntime, '$RedotRuntime');
mergeInto(LibraryManager.library, RedotRuntime);
