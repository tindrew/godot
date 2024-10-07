/**************************************************************************/
/*  RedotGLRenderView.java                                                */
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

package org.redotengine.redot;

import org.redotengine.redot.gl.GLSurfaceView;
import org.redotengine.redot.gl.RedotRenderer;
import org.redotengine.redot.input.RedotInputHandler;
import org.redotengine.redot.xr.XRMode;
import org.redotengine.redot.xr.ovr.OvrConfigChooser;
import org.redotengine.redot.xr.ovr.OvrContextFactory;
import org.redotengine.redot.xr.ovr.OvrWindowSurfaceFactory;
import org.redotengine.redot.xr.regular.RegularConfigChooser;
import org.redotengine.redot.xr.regular.RegularContextFactory;
import org.redotengine.redot.xr.regular.RegularFallbackConfigChooser;

import android.annotation.SuppressLint;
import android.content.res.AssetManager;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.PixelFormat;
import android.os.Build;
import android.text.TextUtils;
import android.util.SparseArray;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.PointerIcon;
import android.view.SurfaceView;

import androidx.annotation.Keep;

import java.io.InputStream;

/**
 * A simple GLSurfaceView sub-class that demonstrate how to perform
 * OpenGL ES 2.0 rendering into a GL Surface. Note the following important
 * details:
 *
 * - The class must use a custom context factory to enable 2.0 rendering.
 *   See ContextFactory class definition below.
 *
 * - The class must use a custom EGLConfigChooser to be able to select
 *   an EGLConfig that supports 3.0. This is done by providing a config
 *   specification to eglChooseConfig() that has the attribute
 *   EGL10.ELG_RENDERABLE_TYPE containing the EGL_OPENGL_ES2_BIT flag
 *   set. See ConfigChooser class definition below.
 *
 * - The class must select the surface's format, then choose an EGLConfig
 *   that matches it exactly (with regards to red/green/blue/alpha channels
 *   bit depths). Failure to do so would result in an EGL_BAD_MATCH error.
 */
class RedotGLRenderView extends GLSurfaceView implements RedotRenderView {
	private final RedotHost host;
	private final Redot redot;
	private final RedotInputHandler inputHandler;
	private final RedotRenderer redotRenderer;
	private final SparseArray<PointerIcon> customPointerIcons = new SparseArray<>();

	public RedotGLRenderView(RedotHost host, Redot redot, RedotInputHandler inputHandler, XRMode xrMode, boolean useDebugOpengl) {
		super(host.getActivity());

		this.host = host;
		this.redot = redot;
		this.inputHandler = inputHandler;
		this.redotRenderer = new RedotRenderer();
		if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.N) {
			setPointerIcon(PointerIcon.getSystemIcon(getContext(), PointerIcon.TYPE_DEFAULT));
		}
		init(xrMode, false, useDebugOpengl);
	}

	@Override
	public SurfaceView getView() {
		return this;
	}

	@Override
	public void queueOnRenderThread(Runnable event) {
		queueEvent(event);
	}

	@Override
	public void onActivityPaused() {
		queueEvent(() -> {
			RedotLib.focusout();
			// Pause the renderer
			redotRenderer.onActivityPaused();
		});
	}

	@Override
	public void onActivityStopped() {
		pauseGLThread();
	}

	@Override
	public void onActivityResumed() {
		queueEvent(() -> {
			// Resume the renderer
			redotRenderer.onActivityResumed();
			RedotLib.focusin();
		});
	}

	@Override
	public void onActivityStarted() {
		resumeGLThread();
	}

	@Override
	public void onActivityDestroyed() {
		requestRenderThreadExitAndWait();
	}

	@Override
	public RedotInputHandler getInputHandler() {
		return inputHandler;
	}

	@SuppressLint("ClickableViewAccessibility")
	@Override
	public boolean onTouchEvent(MotionEvent event) {
		super.onTouchEvent(event);
		return inputHandler.onTouchEvent(event);
	}

	@Override
	public boolean onKeyUp(final int keyCode, KeyEvent event) {
		return inputHandler.onKeyUp(keyCode, event) || super.onKeyUp(keyCode, event);
	}

	@Override
	public boolean onKeyDown(final int keyCode, KeyEvent event) {
		return inputHandler.onKeyDown(keyCode, event) || super.onKeyDown(keyCode, event);
	}

	@Override
	public boolean onGenericMotionEvent(MotionEvent event) {
		return inputHandler.onGenericMotionEvent(event) || super.onGenericMotionEvent(event);
	}

	@Override
	public boolean onCapturedPointerEvent(MotionEvent event) {
		return inputHandler.onGenericMotionEvent(event);
	}

	@Override
	public void onPointerCaptureChange(boolean hasCapture) {
		super.onPointerCaptureChange(hasCapture);
		inputHandler.onPointerCaptureChange(hasCapture);
	}

	@Override
	public void requestPointerCapture() {
		if (canCapturePointer()) {
			super.requestPointerCapture();
			inputHandler.onPointerCaptureChange(true);
		}
	}

	@Override
	public void releasePointerCapture() {
		super.releasePointerCapture();
		inputHandler.onPointerCaptureChange(false);
	}

	/**
	 * Used to configure the PointerIcon for the given type.
	 *
	 * Called from JNI
	 */
	@Keep
	@Override
	public void configurePointerIcon(int pointerType, String imagePath, float hotSpotX, float hotSpotY) {
		if (android.os.Build.VERSION.SDK_INT >= android.os.Build.VERSION_CODES.N) {
			try {
				Bitmap bitmap = null;
				if (!TextUtils.isEmpty(imagePath)) {
					if (redot.getDirectoryAccessHandler().filesystemFileExists(imagePath)) {
						// Try to load the bitmap from the file system
						bitmap = BitmapFactory.decodeFile(imagePath);
					} else if (redot.getDirectoryAccessHandler().assetsFileExists(imagePath)) {
						// Try to load the bitmap from the assets directory
						AssetManager am = getContext().getAssets();
						InputStream imageInputStream = am.open(imagePath);
						bitmap = BitmapFactory.decodeStream(imageInputStream);
					}
				}

				PointerIcon customPointerIcon = PointerIcon.create(bitmap, hotSpotX, hotSpotY);
				customPointerIcons.put(pointerType, customPointerIcon);
			} catch (Exception e) {
				// Reset the custom pointer icon
				customPointerIcons.delete(pointerType);
			}
		}
	}

	/**
	 * called from JNI to change pointer icon
	 */
	@Keep
	@Override
	public void setPointerIcon(int pointerType) {
		if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.N) {
			PointerIcon pointerIcon = customPointerIcons.get(pointerType);
			if (pointerIcon == null) {
				pointerIcon = PointerIcon.getSystemIcon(getContext(), pointerType);
			}
			setPointerIcon(pointerIcon);
		}
	}

	@Override
	public PointerIcon onResolvePointerIcon(MotionEvent me, int pointerIndex) {
		if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.N) {
			return getPointerIcon();
		}
		return super.onResolvePointerIcon(me, pointerIndex);
	}

	private void init(XRMode xrMode, boolean translucent, boolean useDebugOpengl) {
		setPreserveEGLContextOnPause(true);
		setFocusableInTouchMode(true);
		switch (xrMode) {
			case OPENXR:
				// Replace the default egl config chooser.
				setEGLConfigChooser(new OvrConfigChooser());

				// Replace the default context factory.
				setEGLContextFactory(new OvrContextFactory());

				// Replace the default window surface factory.
				setEGLWindowSurfaceFactory(new OvrWindowSurfaceFactory());
				break;

			case REGULAR:
			default:
				/* By default, GLSurfaceView() creates a RGB_565 opaque surface.
				 * If we want a translucent one, we should change the surface's
				 * format here, using PixelFormat.TRANSLUCENT for GL Surfaces
				 * is interpreted as any 32-bit surface with alpha by SurfaceFlinger.
				 */
				if (translucent) {
					this.getHolder().setFormat(PixelFormat.TRANSLUCENT);
				}

				/* Setup the context factory for 2.0 rendering.
				 * See ContextFactory class definition below
				 */
				setEGLContextFactory(new RegularContextFactory(useDebugOpengl));

				/* We need to choose an EGLConfig that matches the format of
				 * our surface exactly. This is going to be done in our
				 * custom config chooser. See ConfigChooser class definition
				 * below.
				 */

				setEGLConfigChooser(
						new RegularFallbackConfigChooser(8, 8, 8, 8, 24, 0,
								new RegularConfigChooser(8, 8, 8, 8, 16, 0)));
				break;
		}
	}

	@Override
	public void startRenderer() {
		/* Set the renderer responsible for frame rendering */
		setRenderer(redotRenderer);
	}
}
