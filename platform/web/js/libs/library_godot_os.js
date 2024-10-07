/**************************************************************************/
/*  library_redot_os.js                                                   */
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

const IDHandler = {
	$IDHandler: {
		_last_id: 0,
		_references: {},

		get: function (p_id) {
			return IDHandler._references[p_id];
		},

		add: function (p_data) {
			const id = ++IDHandler._last_id;
			IDHandler._references[id] = p_data;
			return id;
		},

		remove: function (p_id) {
			delete IDHandler._references[p_id];
		},
	},
};

autoAddDeps(IDHandler, '$IDHandler');
mergeInto(LibraryManager.library, IDHandler);

const RedotConfig = {
	$RedotConfig__postset: 'Module["initConfig"] = RedotConfig.init_config;',
	$RedotConfig__deps: ['$RedotRuntime'],
	$RedotConfig: {
		canvas: null,
		locale: 'en',
		canvas_resize_policy: 2, // Adaptive
		virtual_keyboard: false,
		persistent_drops: false,
		on_execute: null,
		on_exit: null,

		init_config: function (p_opts) {
			RedotConfig.canvas_resize_policy = p_opts['canvasResizePolicy'];
			RedotConfig.canvas = p_opts['canvas'];
			RedotConfig.locale = p_opts['locale'] || RedotConfig.locale;
			RedotConfig.virtual_keyboard = p_opts['virtualKeyboard'];
			RedotConfig.persistent_drops = !!p_opts['persistentDrops'];
			RedotConfig.on_execute = p_opts['onExecute'];
			RedotConfig.on_exit = p_opts['onExit'];
			if (p_opts['focusCanvas']) {
				RedotConfig.canvas.focus();
			}
		},

		locate_file: function (file) {
			return Module['locateFile'](file);
		},
		clear: function () {
			RedotConfig.canvas = null;
			RedotConfig.locale = 'en';
			RedotConfig.canvas_resize_policy = 2;
			RedotConfig.virtual_keyboard = false;
			RedotConfig.persistent_drops = false;
			RedotConfig.on_execute = null;
			RedotConfig.on_exit = null;
		},
	},

	redot_js_config_canvas_id_get__proxy: 'sync',
	redot_js_config_canvas_id_get__sig: 'vii',
	redot_js_config_canvas_id_get: function (p_ptr, p_ptr_max) {
		RedotRuntime.stringToHeap(`#${RedotConfig.canvas.id}`, p_ptr, p_ptr_max);
	},

	redot_js_config_locale_get__proxy: 'sync',
	redot_js_config_locale_get__sig: 'vii',
	redot_js_config_locale_get: function (p_ptr, p_ptr_max) {
		RedotRuntime.stringToHeap(RedotConfig.locale, p_ptr, p_ptr_max);
	},
};

autoAddDeps(RedotConfig, '$RedotConfig');
mergeInto(LibraryManager.library, RedotConfig);

const RedotFS = {
	$RedotFS__deps: ['$FS', '$IDBFS', '$RedotRuntime'],
	$RedotFS__postset: [
		'Module["initFS"] = RedotFS.init;',
		'Module["copyToFS"] = RedotFS.copy_to_fs;',
	].join(''),
	$RedotFS: {
		// ERRNO_CODES works every odd version of emscripten, but this will break too eventually.
		ENOENT: 44,
		_idbfs: false,
		_syncing: false,
		_mount_points: [],

		is_persistent: function () {
			return RedotFS._idbfs ? 1 : 0;
		},

		// Initialize redot file system, setting up persistent paths.
		// Returns a promise that resolves when the FS is ready.
		// We keep track of mount_points, so that we can properly close the IDBFS
		// since emscripten is not doing it by itself. (emscripten GH#12516).
		init: function (persistentPaths) {
			RedotFS._idbfs = false;
			if (!Array.isArray(persistentPaths)) {
				return Promise.reject(new Error('Persistent paths must be an array'));
			}
			if (!persistentPaths.length) {
				return Promise.resolve();
			}
			RedotFS._mount_points = persistentPaths.slice();

			function createRecursive(dir) {
				try {
					FS.stat(dir);
				} catch (e) {
					if (e.errno !== RedotFS.ENOENT) {
						// Let mkdirTree throw in case, we cannot trust the above check.
						RedotRuntime.error(e);
					}
					FS.mkdirTree(dir);
				}
			}

			RedotFS._mount_points.forEach(function (path) {
				createRecursive(path);
				FS.mount(IDBFS, {}, path);
			});
			return new Promise(function (resolve, reject) {
				FS.syncfs(true, function (err) {
					if (err) {
						RedotFS._mount_points = [];
						RedotFS._idbfs = false;
						RedotRuntime.print(`IndexedDB not available: ${err.message}`);
					} else {
						RedotFS._idbfs = true;
					}
					resolve(err);
				});
			});
		},

		// Deinit redot file system, making sure to unmount file systems, and close IDBFS(s).
		deinit: function () {
			RedotFS._mount_points.forEach(function (path) {
				try {
					FS.unmount(path);
				} catch (e) {
					RedotRuntime.print('Already unmounted', e);
				}
				if (RedotFS._idbfs && IDBFS.dbs[path]) {
					IDBFS.dbs[path].close();
					delete IDBFS.dbs[path];
				}
			});
			RedotFS._mount_points = [];
			RedotFS._idbfs = false;
			RedotFS._syncing = false;
		},

		sync: function () {
			if (RedotFS._syncing) {
				RedotRuntime.error('Already syncing!');
				return Promise.resolve();
			}
			RedotFS._syncing = true;
			return new Promise(function (resolve, reject) {
				FS.syncfs(false, function (error) {
					if (error) {
						RedotRuntime.error(`Failed to save IDB file system: ${error.message}`);
					}
					RedotFS._syncing = false;
					resolve(error);
				});
			});
		},

		// Copies a buffer to the internal file system. Creating directories recursively.
		copy_to_fs: function (path, buffer) {
			const idx = path.lastIndexOf('/');
			let dir = '/';
			if (idx > 0) {
				dir = path.slice(0, idx);
			}
			try {
				FS.stat(dir);
			} catch (e) {
				if (e.errno !== RedotFS.ENOENT) {
					// Let mkdirTree throw in case, we cannot trust the above check.
					RedotRuntime.error(e);
				}
				FS.mkdirTree(dir);
			}
			FS.writeFile(path, new Uint8Array(buffer));
		},
	},
};
mergeInto(LibraryManager.library, RedotFS);

const RedotOS = {
	$RedotOS__deps: ['$RedotRuntime', '$RedotConfig', '$RedotFS'],
	$RedotOS__postset: [
		'Module["request_quit"] = function() { RedotOS.request_quit() };',
		'Module["onExit"] = RedotOS.cleanup;',
		'RedotOS._fs_sync_promise = Promise.resolve();',
	].join(''),
	$RedotOS: {
		request_quit: function () {},
		_async_cbs: [],
		_fs_sync_promise: null,

		atexit: function (p_promise_cb) {
			RedotOS._async_cbs.push(p_promise_cb);
		},

		cleanup: function (exit_code) {
			const cb = RedotConfig.on_exit;
			RedotFS.deinit();
			RedotConfig.clear();
			if (cb) {
				cb(exit_code);
			}
		},

		finish_async: function (callback) {
			RedotOS._fs_sync_promise.then(function (err) {
				const promises = [];
				RedotOS._async_cbs.forEach(function (cb) {
					promises.push(new Promise(cb));
				});
				return Promise.all(promises);
			}).then(function () {
				return RedotFS.sync(); // Final FS sync.
			}).then(function (err) {
				// Always deferred.
				setTimeout(function () {
					callback();
				}, 0);
			});
		},
	},

	redot_js_os_finish_async__proxy: 'sync',
	redot_js_os_finish_async__sig: 'vi',
	redot_js_os_finish_async: function (p_callback) {
		const func = RedotRuntime.get_func(p_callback);
		RedotOS.finish_async(func);
	},

	redot_js_os_request_quit_cb__proxy: 'sync',
	redot_js_os_request_quit_cb__sig: 'vi',
	redot_js_os_request_quit_cb: function (p_callback) {
		RedotOS.request_quit = RedotRuntime.get_func(p_callback);
	},

	redot_js_os_fs_is_persistent__proxy: 'sync',
	redot_js_os_fs_is_persistent__sig: 'i',
	redot_js_os_fs_is_persistent: function () {
		return RedotFS.is_persistent();
	},

	redot_js_os_fs_sync__proxy: 'sync',
	redot_js_os_fs_sync__sig: 'vi',
	redot_js_os_fs_sync: function (callback) {
		const func = RedotRuntime.get_func(callback);
		RedotOS._fs_sync_promise = RedotFS.sync();
		RedotOS._fs_sync_promise.then(function (err) {
			func();
		});
	},

	redot_js_os_has_feature__proxy: 'sync',
	redot_js_os_has_feature__sig: 'ii',
	redot_js_os_has_feature: function (p_ftr) {
		const ftr = RedotRuntime.parseString(p_ftr);
		const ua = navigator.userAgent;
		if (ftr === 'web_macos') {
			return (ua.indexOf('Mac') !== -1) ? 1 : 0;
		}
		if (ftr === 'web_windows') {
			return (ua.indexOf('Windows') !== -1) ? 1 : 0;
		}
		if (ftr === 'web_android') {
			return (ua.indexOf('Android') !== -1) ? 1 : 0;
		}
		if (ftr === 'web_ios') {
			return ((ua.indexOf('iPhone') !== -1) || (ua.indexOf('iPad') !== -1) || (ua.indexOf('iPod') !== -1)) ? 1 : 0;
		}
		if (ftr === 'web_linuxbsd') {
			return ((ua.indexOf('CrOS') !== -1) || (ua.indexOf('BSD') !== -1) || (ua.indexOf('Linux') !== -1) || (ua.indexOf('X11') !== -1)) ? 1 : 0;
		}
		return 0;
	},

	redot_js_os_execute__proxy: 'sync',
	redot_js_os_execute__sig: 'ii',
	redot_js_os_execute: function (p_json) {
		const json_args = RedotRuntime.parseString(p_json);
		const args = JSON.parse(json_args);
		if (RedotConfig.on_execute) {
			RedotConfig.on_execute(args);
			return 0;
		}
		return 1;
	},

	redot_js_os_shell_open__proxy: 'sync',
	redot_js_os_shell_open__sig: 'vi',
	redot_js_os_shell_open: function (p_uri) {
		window.open(RedotRuntime.parseString(p_uri), '_blank');
	},

	redot_js_os_hw_concurrency_get__proxy: 'sync',
	redot_js_os_hw_concurrency_get__sig: 'i',
	redot_js_os_hw_concurrency_get: function () {
		// TODO Redot core needs fixing to avoid spawning too many threads (> 24).
		const concurrency = navigator.hardwareConcurrency || 1;
		return concurrency < 2 ? concurrency : 2;
	},

	redot_js_os_download_buffer__proxy: 'sync',
	redot_js_os_download_buffer__sig: 'viiii',
	redot_js_os_download_buffer: function (p_ptr, p_size, p_name, p_mime) {
		const buf = RedotRuntime.heapSlice(HEAP8, p_ptr, p_size);
		const name = RedotRuntime.parseString(p_name);
		const mime = RedotRuntime.parseString(p_mime);
		const blob = new Blob([buf], { type: mime });
		const url = window.URL.createObjectURL(blob);
		const a = document.createElement('a');
		a.href = url;
		a.download = name;
		a.style.display = 'none';
		document.body.appendChild(a);
		a.click();
		a.remove();
		window.URL.revokeObjectURL(url);
	},
};

autoAddDeps(RedotOS, '$RedotOS');
mergeInto(LibraryManager.library, RedotOS);

/*
 * Redot event listeners.
 * Keeps track of registered event listeners so it can remove them on shutdown.
 */
const RedotEventListeners = {
	$RedotEventListeners__deps: ['$RedotOS'],
	$RedotEventListeners__postset: 'RedotOS.atexit(function(resolve, reject) { RedotEventListeners.clear(); resolve(); });',
	$RedotEventListeners: {
		handlers: [],

		has: function (target, event, method, capture) {
			return RedotEventListeners.handlers.findIndex(function (e) {
				return e.target === target && e.event === event && e.method === method && e.capture === capture;
			}) !== -1;
		},

		add: function (target, event, method, capture) {
			if (RedotEventListeners.has(target, event, method, capture)) {
				return;
			}
			function Handler(p_target, p_event, p_method, p_capture) {
				this.target = p_target;
				this.event = p_event;
				this.method = p_method;
				this.capture = p_capture;
			}
			RedotEventListeners.handlers.push(new Handler(target, event, method, capture));
			target.addEventListener(event, method, capture);
		},

		clear: function () {
			RedotEventListeners.handlers.forEach(function (h) {
				h.target.removeEventListener(h.event, h.method, h.capture);
			});
			RedotEventListeners.handlers.length = 0;
		},
	},
};
mergeInto(LibraryManager.library, RedotEventListeners);

const RedotPWA = {

	$RedotPWA__deps: ['$RedotRuntime', '$RedotEventListeners'],
	$RedotPWA: {
		hasUpdate: false,

		updateState: function (cb, reg) {
			if (!reg) {
				return;
			}
			if (!reg.active) {
				return;
			}
			if (reg.waiting) {
				RedotPWA.hasUpdate = true;
				cb();
			}
			RedotEventListeners.add(reg, 'updatefound', function () {
				const installing = reg.installing;
				RedotEventListeners.add(installing, 'statechange', function () {
					if (installing.state === 'installed') {
						RedotPWA.hasUpdate = true;
						cb();
					}
				});
			});
		},
	},

	redot_js_pwa_cb__proxy: 'sync',
	redot_js_pwa_cb__sig: 'vi',
	redot_js_pwa_cb: function (p_update_cb) {
		if ('serviceWorker' in navigator) {
			const cb = RedotRuntime.get_func(p_update_cb);
			navigator.serviceWorker.getRegistration().then(RedotPWA.updateState.bind(null, cb));
		}
	},

	redot_js_pwa_update__proxy: 'sync',
	redot_js_pwa_update__sig: 'i',
	redot_js_pwa_update: function () {
		if ('serviceWorker' in navigator && RedotPWA.hasUpdate) {
			navigator.serviceWorker.getRegistration().then(function (reg) {
				if (!reg || !reg.waiting) {
					return;
				}
				reg.waiting.postMessage('update');
			});
			return 0;
		}
		return 1;
	},
};

autoAddDeps(RedotPWA, '$RedotPWA');
mergeInto(LibraryManager.library, RedotPWA);
