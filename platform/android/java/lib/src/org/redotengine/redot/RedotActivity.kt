/**************************************************************************/
/*  RedotActivity.kt                                                      */
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

package org.redotengine.redot

import android.app.Activity
import android.content.Intent
import android.content.pm.PackageManager
import android.os.Bundle
import android.util.Log
import androidx.annotation.CallSuper
import androidx.annotation.LayoutRes
import androidx.fragment.app.FragmentActivity
import org.redotengine.redot.utils.PermissionsUtil
import org.redotengine.redot.utils.ProcessPhoenix

/**
 * Base abstract activity for Android apps intending to use Redot as the primary screen.
 *
 * Also a reference implementation for how to setup and use the [RedotFragment] fragment
 * within an Android app.
 */
abstract class RedotActivity : FragmentActivity(), RedotHost {

	companion object {
		private val TAG = RedotActivity::class.java.simpleName

		@JvmStatic
		protected val EXTRA_NEW_LAUNCH = "new_launch_requested"
	}

	/**
	 * Interaction with the [Redot] object is delegated to the [RedotFragment] class.
	 */
	protected var redotFragment: RedotFragment? = null
		private set

	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)
		setContentView(getRedotAppLayout())

		handleStartIntent(intent, true)

		val currentFragment = supportFragmentManager.findFragmentById(R.id.redot_fragment_container)
		if (currentFragment is RedotFragment) {
			Log.v(TAG, "Reusing existing Redot fragment instance.")
			redotFragment = currentFragment
		} else {
			Log.v(TAG, "Creating new Redot fragment instance.")
			redotFragment = initRedotInstance()
			supportFragmentManager.beginTransaction().replace(R.id.redot_fragment_container, redotFragment!!).setPrimaryNavigationFragment(redotFragment).commitNowAllowingStateLoss()
		}
	}

	@LayoutRes
	protected open fun getRedotAppLayout() = R.layout.redot_app_layout

	override fun onDestroy() {
		Log.v(TAG, "Destroying RedotActivity $this...")
		super.onDestroy()
	}

	override fun onRedotForceQuit(instance: Redot) {
		runOnUiThread { terminateRedotInstance(instance) }
	}

	private fun terminateRedotInstance(instance: Redot) {
		redotFragment?.let {
			if (instance === it.redot) {
				Log.v(TAG, "Force quitting Redot instance")
				ProcessPhoenix.forceQuit(this)
			}
		}
	}

	override fun onRedotRestartRequested(instance: Redot) {
		runOnUiThread {
			redotFragment?.let {
				if (instance === it.redot) {
					// It's very hard to properly de-initialize Redot on Android to restart the game
					// from scratch. Therefore, we need to kill the whole app process and relaunch it.
					//
					// Restarting only the activity, wouldn't be enough unless it did proper cleanup (including
					// releasing and reloading native libs or resetting their state somehow and clearing static data).
					Log.v(TAG, "Restarting Redot instance...")
					ProcessPhoenix.triggerRebirth(this)
				}
			}
		}
	}

	override fun onNewIntent(newIntent: Intent) {
		super.onNewIntent(newIntent)
		intent = newIntent

		handleStartIntent(newIntent, false)

		redotFragment?.onNewIntent(newIntent)
	}

	private fun handleStartIntent(intent: Intent, newLaunch: Boolean) {
		if (!newLaunch) {
			val newLaunchRequested = intent.getBooleanExtra(EXTRA_NEW_LAUNCH, false)
			if (newLaunchRequested) {
				Log.d(TAG, "New launch requested, restarting..")
				val restartIntent = Intent(intent).putExtra(EXTRA_NEW_LAUNCH, false)
				ProcessPhoenix.triggerRebirth(this, restartIntent)
				return
			}
		}
	}

	@CallSuper
	override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
		super.onActivityResult(requestCode, resultCode, data)
		redotFragment?.onActivityResult(requestCode, resultCode, data)
	}

	@CallSuper
	override fun onRequestPermissionsResult(requestCode: Int, permissions: Array<String>, grantResults: IntArray) {
		super.onRequestPermissionsResult(requestCode, permissions, grantResults)
		redotFragment?.onRequestPermissionsResult(requestCode, permissions, grantResults)

		// Logging the result of permission requests
		if (requestCode == PermissionsUtil.REQUEST_ALL_PERMISSION_REQ_CODE || requestCode == PermissionsUtil.REQUEST_SINGLE_PERMISSION_REQ_CODE) {
			Log.d(TAG, "Received permissions request result..")
			for (i in permissions.indices) {
				val permissionGranted = grantResults[i] == PackageManager.PERMISSION_GRANTED
				Log.d(TAG, "Permission ${permissions[i]} ${if (permissionGranted) { "granted"} else { "denied" }}")
			}
		}
	}

	override fun onBackPressed() {
		redotFragment?.onBackPressed() ?: super.onBackPressed()
	}

	override fun getActivity(): Activity? {
		return this
	}

	override fun getRedot(): Redot? {
		return redotFragment?.redot
	}

	/**
	 * Used to initialize the Redot fragment instance in [onCreate].
	 */
	protected open fun initRedotInstance(): RedotFragment {
		return RedotFragment()
	}
}
