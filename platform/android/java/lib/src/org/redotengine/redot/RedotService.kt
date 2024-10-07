package org.redotengine.redot

import android.app.Service
import android.content.Intent
import android.os.Binder
import android.os.IBinder
import android.util.Log

/**
 * Redot service responsible for hosting the Redot engine instance.
 *
 * Note: Still in development, so it's made private and inaccessible until completed.
 */
private class RedotService : Service() {

	companion object {
		private val TAG = RedotService::class.java.simpleName
	}

	private var boundIntent: Intent? = null
	private val redot by lazy {
		Redot(applicationContext)
	}

	override fun onCreate() {
		super.onCreate()
	}

	override fun onDestroy() {
		super.onDestroy()
	}

	override fun onBind(intent: Intent?): IBinder? {
		if (boundIntent != null) {
			Log.d(TAG, "RedotService already bound")
			return null
		}

		boundIntent = intent
		return RedotHandle(redot)
	}

	override fun onRebind(intent: Intent?) {
		super.onRebind(intent)
	}

	override fun onUnbind(intent: Intent?): Boolean {
		return super.onUnbind(intent)
	}

	override fun onTaskRemoved(rootIntent: Intent?) {
		super.onTaskRemoved(rootIntent)
	}

	class RedotHandle(val redot: Redot) : Binder()
}
