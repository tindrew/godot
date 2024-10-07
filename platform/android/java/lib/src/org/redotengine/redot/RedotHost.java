/**************************************************************************/
/*  RedotHost.java                                                        */
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

import org.redotengine.redot.error.Error;
import org.redotengine.redot.plugin.RedotPlugin;

import android.app.Activity;

import androidx.annotation.NonNull;

import java.util.Collections;
import java.util.List;
import java.util.Set;

/**
 * Denotate a component (e.g: Activity, Fragment) that hosts the {@link Redot} engine.
 */
public interface RedotHost {
	/**
	 * Provides a set of command line parameters to setup the {@link Redot} engine.
	 */
	default List<String> getCommandLine() {
		return Collections.emptyList();
	}

	/**
	 * Invoked on the render thread when setup of the {@link Redot} engine is complete.
	 */
	default void onRedotSetupCompleted() {}

	/**
	 * Invoked on the render thread when the {@link Redot} engine main loop has started.
	 */
	default void onRedotMainLoopStarted() {}

	/**
	 * Invoked on the render thread to terminate the given {@link Redot} engine instance.
	 */
	default void onRedotForceQuit(Redot instance) {}

	/**
	 * Invoked on the render thread to terminate the {@link Redot} engine instance with the given id.
	 * @param redotInstanceId id of the Redot instance to terminate. See {@code onNewRedotInstanceRequested}
	 *
	 * @return true if successful, false otherwise.
	 */
	default boolean onRedotForceQuit(int redotInstanceId) {
		return false;
	}

	/**
	 * Invoked on the render thread when the Redot instance wants to be restarted. It's up to the host
	 * to perform the appropriate action(s).
	 */
	default void onRedotRestartRequested(Redot instance) {}

	/**
	 * Invoked on the render thread when a new Redot instance is requested. It's up to the host to
	 * perform the appropriate action(s).
	 *
	 * @param args Arguments used to initialize the new instance.
	 *
	 * @return the id of the new instance. See {@code onRedotForceQuit}
	 */
	default int onNewRedotInstanceRequested(String[] args) {
		return 0;
	}

	/**
	 * Provide access to the Activity hosting the {@link Redot} engine.
	 */
	Activity getActivity();

	/**
	 * Provide access to the hosted {@link Redot} engine.
	 */
	Redot getRedot();

	/**
	 * Returns a set of {@link RedotPlugin} to be registered with the hosted {@link Redot} engine.
	 */
	default Set<RedotPlugin> getHostPlugins(Redot engine) {
		return Collections.emptySet();
	}

	/**
	 * Signs the given Android apk
	 *
	 * @param inputPath Path to the apk that should be signed
	 * @param outputPath Path for the signed output apk; can be the same as inputPath
	 * @param keystorePath Path to the keystore to use for signing the apk
	 * @param keystoreUser Keystore user credential
	 * @param keystorePassword Keystore password credential
	 *
	 * @return {@link Error#OK} if signing is successful
	 */
	default Error signApk(@NonNull String inputPath, @NonNull String outputPath, @NonNull String keystorePath, @NonNull String keystoreUser, @NonNull String keystorePassword) {
		return Error.ERR_UNAVAILABLE;
	}

	/**
	 * Verifies the given Android apk is signed
	 *
	 * @param apkPath Path to the apk that should be verified
	 * @return {@link Error#OK} if verification was successful
	 */
	default Error verifyApk(@NonNull String apkPath) {
		return Error.ERR_UNAVAILABLE;
	}

	/**
	 * Returns whether the given feature tag is supported.
	 *
	 * @see <a href="https://docs.redotengine.org/en/stable/tutorials/export/feature_tags.html">Feature tags</a>
	 */
	default boolean supportsFeature(String featureTag) {
		return false;
	}
}
