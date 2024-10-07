/**************************************************************************/
/*  java_redot_lib_jni.h                                                  */
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

#ifndef JAVA_redot_LIB_JNI_H
#define JAVA_redot_LIB_JNI_H

#include <android/log.h>
#include <jni.h>

// These functions can be called from within JAVA and are the means by which our JAVA implementation calls back into our C++ code.
// See java/src/org/redotengine/redot/RedotLib.java for the JAVA side of this (yes that's why we have the long names)
extern "C" {
JNIEXPORT jboolean JNICALL Java_org_redotengine_redot_RedotLib_initialize(JNIEnv *env, jclass clazz, jobject p_activity, jobject p_redot_instance, jobject p_asset_manager, jobject p_redot_io, jobject p_net_utils, jobject p_directory_access_handler, jobject p_file_access_handler, jboolean p_use_apk_expansion);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_ondestroy(JNIEnv *env, jclass clazz);
JNIEXPORT jboolean JNICALL Java_org_redotengine_redot_RedotLib_setup(JNIEnv *env, jclass clazz, jobjectArray p_cmdline, jobject p_redot_tts);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_resize(JNIEnv *env, jclass clazz, jobject p_surface, jint p_width, jint p_height);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_newcontext(JNIEnv *env, jclass clazz, jobject p_surface);
JNIEXPORT jboolean JNICALL Java_org_redotengine_redot_RedotLib_step(JNIEnv *env, jclass clazz);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_ttsCallback(JNIEnv *env, jclass clazz, jint event, jint id, jint pos);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_back(JNIEnv *env, jclass clazz);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_dispatchMouseEvent(JNIEnv *env, jclass clazz, jint p_event_type, jint p_button_mask, jfloat p_x, jfloat p_y, jfloat p_delta_x, jfloat p_delta_y, jboolean p_double_click, jboolean p_source_mouse_relative, jfloat p_pressure, jfloat p_tilt_x, jfloat p_tilt_y);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_dispatchTouchEvent(JNIEnv *env, jclass clazz, jint ev, jint pointer, jint pointer_count, jfloatArray positions, jboolean p_double_tap);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_magnify(JNIEnv *env, jclass clazz, jfloat p_x, jfloat p_y, jfloat p_factor);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_pan(JNIEnv *env, jclass clazz, jfloat p_x, jfloat p_y, jfloat p_delta_x, jfloat p_delta_y);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_key(JNIEnv *env, jclass clazz, jint p_physical_keycode, jint p_unicode, jint p_key_label, jboolean p_pressed, jboolean p_echo);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_joybutton(JNIEnv *env, jclass clazz, jint p_device, jint p_button, jboolean p_pressed);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_joyaxis(JNIEnv *env, jclass clazz, jint p_device, jint p_axis, jfloat p_value);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_joyhat(JNIEnv *env, jclass clazz, jint p_device, jint p_hat_x, jint p_hat_y);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_joyconnectionchanged(JNIEnv *env, jclass clazz, jint p_device, jboolean p_connected, jstring p_name);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_accelerometer(JNIEnv *env, jclass clazz, jfloat x, jfloat y, jfloat z);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_gravity(JNIEnv *env, jclass clazz, jfloat x, jfloat y, jfloat z);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_magnetometer(JNIEnv *env, jclass clazz, jfloat x, jfloat y, jfloat z);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_gyroscope(JNIEnv *env, jclass clazz, jfloat x, jfloat y, jfloat z);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_focusin(JNIEnv *env, jclass clazz);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_focusout(JNIEnv *env, jclass clazz);
JNIEXPORT jstring JNICALL Java_org_redotengine_redot_RedotLib_getGlobal(JNIEnv *env, jclass clazz, jstring path);
JNIEXPORT jstring JNICALL Java_org_redotengine_redot_RedotLib_getEditorSetting(JNIEnv *env, jclass clazz, jstring p_setting_key);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_callobject(JNIEnv *env, jclass clazz, jlong ID, jstring method, jobjectArray params);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_calldeferred(JNIEnv *env, jclass clazz, jlong ID, jstring method, jobjectArray params);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_setVirtualKeyboardHeight(JNIEnv *env, jclass clazz, jint p_height);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_requestPermissionResult(JNIEnv *env, jclass clazz, jstring p_permission, jboolean p_result);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_onNightModeChanged(JNIEnv *env, jclass clazz);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_onRendererResumed(JNIEnv *env, jclass clazz);
JNIEXPORT void JNICALL Java_org_redotengine_redot_RedotLib_onRendererPaused(JNIEnv *env, jclass clazz);
JNIEXPORT jboolean JNICALL Java_org_redotengine_redot_RedotLib_shouldDispatchInputToRenderThread(JNIEnv *env, jclass clazz);
JNIEXPORT jstring JNICALL Java_org_redotengine_redot_RedotLib_getProjectResourceDir(JNIEnv *env, jclass clazz);
}

#endif // JAVA_redot_LIB_JNI_H
