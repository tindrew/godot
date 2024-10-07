/**************************************************************************/
/*  library_redot_input.js                                                */
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

/*
 * IME API helper.
 */

const RedotIME = {
	$RedotIME__deps: ['$RedotRuntime', '$RedotEventListeners'],
	$RedotIME__postset: 'RedotOS.atexit(function(resolve, reject) { RedotIME.clear(); resolve(); });',
	$RedotIME: {
		ime: null,
		active: false,

		getModifiers: function (evt) {
			return (evt.shiftKey + 0) + ((evt.altKey + 0) << 1) + ((evt.ctrlKey + 0) << 2) + ((evt.metaKey + 0) << 3);
		},

		ime_active: function (active) {
			function focus_timer() {
				RedotIME.active = true;
				RedotIME.ime.focus();
			}

			if (RedotIME.ime) {
				if (active) {
					RedotIME.ime.style.display = 'block';
					setInterval(focus_timer, 100);
				} else {
					RedotIME.ime.style.display = 'none';
					RedotConfig.canvas.focus();
					RedotIME.active = false;
				}
			}
		},

		ime_position: function (x, y) {
			if (RedotIME.ime) {
				const canvas = RedotConfig.canvas;
				const rect = canvas.getBoundingClientRect();
				const rw = canvas.width / rect.width;
				const rh = canvas.height / rect.height;
				const clx = (x / rw) + rect.x;
				const cly = (y / rh) + rect.y;

				RedotIME.ime.style.left = `${clx}px`;
				RedotIME.ime.style.top = `${cly}px`;
			}
		},

		init: function (ime_cb, key_cb, code, key) {
			function key_event_cb(pressed, evt) {
				const modifiers = RedotIME.getModifiers(evt);
				RedotRuntime.stringToHeap(evt.code, code, 32);
				RedotRuntime.stringToHeap(evt.key, key, 32);
				key_cb(pressed, evt.repeat, modifiers);
				evt.preventDefault();
			}
			function ime_event_cb(event) {
				if (RedotIME.ime) {
					if (event.type === 'compositionstart') {
						ime_cb(0, null);
						RedotIME.ime.innerHTML = '';
					} else if (event.type === 'compositionupdate') {
						const ptr = RedotRuntime.allocString(event.data);
						ime_cb(1, ptr);
						RedotRuntime.free(ptr);
					} else if (event.type === 'compositionend') {
						const ptr = RedotRuntime.allocString(event.data);
						ime_cb(2, ptr);
						RedotRuntime.free(ptr);
						RedotIME.ime.innerHTML = '';
					}
				}
			}

			const ime = document.createElement('div');
			ime.className = 'ime';
			ime.style.background = 'none';
			ime.style.opacity = 0.0;
			ime.style.position = 'fixed';
			ime.style.textAlign = 'left';
			ime.style.fontSize = '1px';
			ime.style.left = '0px';
			ime.style.top = '0px';
			ime.style.width = '100%';
			ime.style.height = '40px';
			ime.style.pointerEvents = 'none';
			ime.style.display = 'none';
			ime.contentEditable = 'true';

			RedotEventListeners.add(ime, 'compositionstart', ime_event_cb, false);
			RedotEventListeners.add(ime, 'compositionupdate', ime_event_cb, false);
			RedotEventListeners.add(ime, 'compositionend', ime_event_cb, false);
			RedotEventListeners.add(ime, 'keydown', key_event_cb.bind(null, 1), false);
			RedotEventListeners.add(ime, 'keyup', key_event_cb.bind(null, 0), false);

			ime.onblur = function () {
				this.style.display = 'none';
				RedotConfig.canvas.focus();
				RedotIME.active = false;
			};

			RedotConfig.canvas.parentElement.appendChild(ime);
			RedotIME.ime = ime;
		},

		clear: function () {
			if (RedotIME.ime) {
				RedotIME.ime.remove();
				RedotIME.ime = null;
			}
		},
	},
};
mergeInto(LibraryManager.library, RedotIME);

/*
 * Gamepad API helper.
 */
const RedotInputGamepads = {
	$RedotInputGamepads__deps: ['$RedotRuntime', '$RedotEventListeners'],
	$RedotInputGamepads: {
		samples: [],

		get_pads: function () {
			try {
				// Will throw in iframe when permission is denied.
				// Will throw/warn in the future for insecure contexts.
				// See https://github.com/w3c/gamepad/pull/120
				const pads = navigator.getGamepads();
				if (pads) {
					return pads;
				}
				return [];
			} catch (e) {
				return [];
			}
		},

		get_samples: function () {
			return RedotInputGamepads.samples;
		},

		get_sample: function (index) {
			const samples = RedotInputGamepads.samples;
			return index < samples.length ? samples[index] : null;
		},

		sample: function () {
			const pads = RedotInputGamepads.get_pads();
			const samples = [];
			for (let i = 0; i < pads.length; i++) {
				const pad = pads[i];
				if (!pad) {
					samples.push(null);
					continue;
				}
				const s = {
					standard: pad.mapping === 'standard',
					buttons: [],
					axes: [],
					connected: pad.connected,
				};
				for (let b = 0; b < pad.buttons.length; b++) {
					s.buttons.push(pad.buttons[b].value);
				}
				for (let a = 0; a < pad.axes.length; a++) {
					s.axes.push(pad.axes[a]);
				}
				samples.push(s);
			}
			RedotInputGamepads.samples = samples;
		},

		init: function (onchange) {
			RedotInputGamepads.samples = [];
			function add(pad) {
				const guid = RedotInputGamepads.get_guid(pad);
				const c_id = RedotRuntime.allocString(pad.id);
				const c_guid = RedotRuntime.allocString(guid);
				onchange(pad.index, 1, c_id, c_guid);
				RedotRuntime.free(c_id);
				RedotRuntime.free(c_guid);
			}
			const pads = RedotInputGamepads.get_pads();
			for (let i = 0; i < pads.length; i++) {
				// Might be reserved space.
				if (pads[i]) {
					add(pads[i]);
				}
			}
			RedotEventListeners.add(window, 'gamepadconnected', function (evt) {
				if (evt.gamepad) {
					add(evt.gamepad);
				}
			}, false);
			RedotEventListeners.add(window, 'gamepaddisconnected', function (evt) {
				if (evt.gamepad) {
					onchange(evt.gamepad.index, 0);
				}
			}, false);
		},

		get_guid: function (pad) {
			if (pad.mapping) {
				return pad.mapping;
			}
			const ua = navigator.userAgent;
			let os = 'Unknown';
			if (ua.indexOf('Android') >= 0) {
				os = 'Android';
			} else if (ua.indexOf('Linux') >= 0) {
				os = 'Linux';
			} else if (ua.indexOf('iPhone') >= 0) {
				os = 'iOS';
			} else if (ua.indexOf('Macintosh') >= 0) {
				// Updated iPads will fall into this category.
				os = 'MacOSX';
			} else if (ua.indexOf('Windows') >= 0) {
				os = 'Windows';
			}

			const id = pad.id;
			// Chrom* style: NAME (Vendor: xxxx Product: xxxx).
			const exp1 = /vendor: ([0-9a-f]{4}) product: ([0-9a-f]{4})/i;
			// Firefox/Safari style (Safari may remove leading zeroes).
			const exp2 = /^([0-9a-f]+)-([0-9a-f]+)-/i;
			let vendor = '';
			let product = '';
			if (exp1.test(id)) {
				const match = exp1.exec(id);
				vendor = match[1].padStart(4, '0');
				product = match[2].padStart(4, '0');
			} else if (exp2.test(id)) {
				const match = exp2.exec(id);
				vendor = match[1].padStart(4, '0');
				product = match[2].padStart(4, '0');
			}
			if (!vendor || !product) {
				return `${os}Unknown`;
			}
			return os + vendor + product;
		},
	},
};
mergeInto(LibraryManager.library, RedotInputGamepads);

/*
 * Drag and drop helper.
 * This is pretty big, but basically detect dropped files on RedotConfig.canvas,
 * process them one by one (recursively for directories), and copies them to
 * the temporary FS path '/tmp/drop-[random]/' so it can be emitted as a redot
 * event (that requires a string array of paths).
 *
 * NOTE: The temporary files are removed after the callback. This means that
 * deferred callbacks won't be able to access the files.
 */
const RedotInputDragDrop = {
	$RedotInputDragDrop__deps: ['$FS', '$RedotFS'],
	$RedotInputDragDrop: {
		promises: [],
		pending_files: [],

		add_entry: function (entry) {
			if (entry.isDirectory) {
				RedotInputDragDrop.add_dir(entry);
			} else if (entry.isFile) {
				RedotInputDragDrop.add_file(entry);
			} else {
				RedotRuntime.error('Unrecognized entry...', entry);
			}
		},

		add_dir: function (entry) {
			RedotInputDragDrop.promises.push(new Promise(function (resolve, reject) {
				const reader = entry.createReader();
				reader.readEntries(function (entries) {
					for (let i = 0; i < entries.length; i++) {
						RedotInputDragDrop.add_entry(entries[i]);
					}
					resolve();
				});
			}));
		},

		add_file: function (entry) {
			RedotInputDragDrop.promises.push(new Promise(function (resolve, reject) {
				entry.file(function (file) {
					const reader = new FileReader();
					reader.onload = function () {
						const f = {
							'path': file.relativePath || file.webkitRelativePath,
							'name': file.name,
							'type': file.type,
							'size': file.size,
							'data': reader.result,
						};
						if (!f['path']) {
							f['path'] = f['name'];
						}
						RedotInputDragDrop.pending_files.push(f);
						resolve();
					};
					reader.onerror = function () {
						RedotRuntime.print('Error reading file');
						reject();
					};
					reader.readAsArrayBuffer(file);
				}, function (err) {
					RedotRuntime.print('Error!');
					reject();
				});
			}));
		},

		process: function (resolve, reject) {
			if (RedotInputDragDrop.promises.length === 0) {
				resolve();
				return;
			}
			RedotInputDragDrop.promises.pop().then(function () {
				setTimeout(function () {
					RedotInputDragDrop.process(resolve, reject);
				}, 0);
			});
		},

		_process_event: function (ev, callback) {
			ev.preventDefault();
			if (ev.dataTransfer.items) {
				// Use DataTransferItemList interface to access the file(s)
				for (let i = 0; i < ev.dataTransfer.items.length; i++) {
					const item = ev.dataTransfer.items[i];
					let entry = null;
					if ('getAsEntry' in item) {
						entry = item.getAsEntry();
					} else if ('webkitGetAsEntry' in item) {
						entry = item.webkitGetAsEntry();
					}
					if (entry) {
						RedotInputDragDrop.add_entry(entry);
					}
				}
			} else {
				RedotRuntime.error('File upload not supported');
			}
			new Promise(RedotInputDragDrop.process).then(function () {
				const DROP = `/tmp/drop-${parseInt(Math.random() * (1 << 30), 10)}/`;
				const drops = [];
				const files = [];
				FS.mkdir(DROP.slice(0, -1)); // Without trailing slash
				RedotInputDragDrop.pending_files.forEach((elem) => {
					const path = elem['path'];
					RedotFS.copy_to_fs(DROP + path, elem['data']);
					let idx = path.indexOf('/');
					if (idx === -1) {
						// Root file
						drops.push(DROP + path);
					} else {
						// Subdir
						const sub = path.substr(0, idx);
						idx = sub.indexOf('/');
						if (idx < 0 && drops.indexOf(DROP + sub) === -1) {
							drops.push(DROP + sub);
						}
					}
					files.push(DROP + path);
				});
				RedotInputDragDrop.promises = [];
				RedotInputDragDrop.pending_files = [];
				callback(drops);
				if (RedotConfig.persistent_drops) {
					// Delay removal at exit.
					RedotOS.atexit(function (resolve, reject) {
						RedotInputDragDrop.remove_drop(files, DROP);
						resolve();
					});
				} else {
					RedotInputDragDrop.remove_drop(files, DROP);
				}
			});
		},

		remove_drop: function (files, drop_path) {
			const dirs = [drop_path.substr(0, drop_path.length - 1)];
			// Remove temporary files
			files.forEach(function (file) {
				FS.unlink(file);
				let dir = file.replace(drop_path, '');
				let idx = dir.lastIndexOf('/');
				while (idx > 0) {
					dir = dir.substr(0, idx);
					if (dirs.indexOf(drop_path + dir) === -1) {
						dirs.push(drop_path + dir);
					}
					idx = dir.lastIndexOf('/');
				}
			});
			// Remove dirs.
			dirs.sort(function (a, b) {
				const al = (a.match(/\//g) || []).length;
				const bl = (b.match(/\//g) || []).length;
				if (al > bl) {
					return -1;
				} else if (al < bl) {
					return 1;
				}
				return 0;
			}).forEach(function (dir) {
				FS.rmdir(dir);
			});
		},

		handler: function (callback) {
			return function (ev) {
				RedotInputDragDrop._process_event(ev, callback);
			};
		},
	},
};
mergeInto(LibraryManager.library, RedotInputDragDrop);

/*
 * Redot exposed input functions.
 */
const RedotInput = {
	$RedotInput__deps: ['$RedotRuntime', '$RedotConfig', '$RedotEventListeners', '$RedotInputGamepads', '$RedotInputDragDrop', '$RedotIME'],
	$RedotInput: {
		getModifiers: function (evt) {
			return (evt.shiftKey + 0) + ((evt.altKey + 0) << 1) + ((evt.ctrlKey + 0) << 2) + ((evt.metaKey + 0) << 3);
		},
		computePosition: function (evt, rect) {
			const canvas = RedotConfig.canvas;
			const rw = canvas.width / rect.width;
			const rh = canvas.height / rect.height;
			const x = (evt.clientX - rect.x) * rw;
			const y = (evt.clientY - rect.y) * rh;
			return [x, y];
		},
	},

	/*
	 * Mouse API
	 */
	redot_js_input_mouse_move_cb__proxy: 'sync',
	redot_js_input_mouse_move_cb__sig: 'vi',
	redot_js_input_mouse_move_cb: function (callback) {
		const func = RedotRuntime.get_func(callback);
		const canvas = RedotConfig.canvas;
		function move_cb(evt) {
			const rect = canvas.getBoundingClientRect();
			const pos = RedotInput.computePosition(evt, rect);
			// Scale movement
			const rw = canvas.width / rect.width;
			const rh = canvas.height / rect.height;
			const rel_pos_x = evt.movementX * rw;
			const rel_pos_y = evt.movementY * rh;
			const modifiers = RedotInput.getModifiers(evt);
			func(pos[0], pos[1], rel_pos_x, rel_pos_y, modifiers);
		}
		RedotEventListeners.add(window, 'mousemove', move_cb, false);
	},

	redot_js_input_mouse_wheel_cb__proxy: 'sync',
	redot_js_input_mouse_wheel_cb__sig: 'vi',
	redot_js_input_mouse_wheel_cb: function (callback) {
		const func = RedotRuntime.get_func(callback);
		function wheel_cb(evt) {
			if (func(evt['deltaX'] || 0, evt['deltaY'] || 0)) {
				evt.preventDefault();
			}
		}
		RedotEventListeners.add(RedotConfig.canvas, 'wheel', wheel_cb, false);
	},

	redot_js_input_mouse_button_cb__proxy: 'sync',
	redot_js_input_mouse_button_cb__sig: 'vi',
	redot_js_input_mouse_button_cb: function (callback) {
		const func = RedotRuntime.get_func(callback);
		const canvas = RedotConfig.canvas;
		function button_cb(p_pressed, evt) {
			const rect = canvas.getBoundingClientRect();
			const pos = RedotInput.computePosition(evt, rect);
			const modifiers = RedotInput.getModifiers(evt);
			// Since the event is consumed, focus manually.
			// NOTE: The iframe container may not have focus yet, so focus even when already active.
			if (p_pressed) {
				RedotConfig.canvas.focus();
			}
			if (func(p_pressed, evt.button, pos[0], pos[1], modifiers)) {
				evt.preventDefault();
			}
		}
		RedotEventListeners.add(canvas, 'mousedown', button_cb.bind(null, 1), false);
		RedotEventListeners.add(window, 'mouseup', button_cb.bind(null, 0), false);
	},

	/*
	 * Touch API
	 */
	redot_js_input_touch_cb__proxy: 'sync',
	redot_js_input_touch_cb__sig: 'viii',
	redot_js_input_touch_cb: function (callback, ids, coords) {
		const func = RedotRuntime.get_func(callback);
		const canvas = RedotConfig.canvas;
		function touch_cb(type, evt) {
			// Since the event is consumed, focus manually.
			// NOTE: The iframe container may not have focus yet, so focus even when already active.
			if (type === 0) {
				RedotConfig.canvas.focus();
			}
			const rect = canvas.getBoundingClientRect();
			const touches = evt.changedTouches;
			for (let i = 0; i < touches.length; i++) {
				const touch = touches[i];
				const pos = RedotInput.computePosition(touch, rect);
				RedotRuntime.setHeapValue(coords + (i * 2) * 8, pos[0], 'double');
				RedotRuntime.setHeapValue(coords + (i * 2 + 1) * 8, pos[1], 'double');
				RedotRuntime.setHeapValue(ids + i * 4, touch.identifier, 'i32');
			}
			func(type, touches.length);
			if (evt.cancelable) {
				evt.preventDefault();
			}
		}
		RedotEventListeners.add(canvas, 'touchstart', touch_cb.bind(null, 0), false);
		RedotEventListeners.add(canvas, 'touchend', touch_cb.bind(null, 1), false);
		RedotEventListeners.add(canvas, 'touchcancel', touch_cb.bind(null, 1), false);
		RedotEventListeners.add(canvas, 'touchmove', touch_cb.bind(null, 2), false);
	},

	/*
	 * Key API
	 */
	redot_js_input_key_cb__proxy: 'sync',
	redot_js_input_key_cb__sig: 'viii',
	redot_js_input_key_cb: function (callback, code, key) {
		const func = RedotRuntime.get_func(callback);
		function key_cb(pressed, evt) {
			const modifiers = RedotInput.getModifiers(evt);
			RedotRuntime.stringToHeap(evt.code, code, 32);
			RedotRuntime.stringToHeap(evt.key, key, 32);
			func(pressed, evt.repeat, modifiers);
			evt.preventDefault();
		}
		RedotEventListeners.add(RedotConfig.canvas, 'keydown', key_cb.bind(null, 1), false);
		RedotEventListeners.add(RedotConfig.canvas, 'keyup', key_cb.bind(null, 0), false);
	},

	/*
	 * IME API
	 */
	redot_js_set_ime_active__proxy: 'sync',
	redot_js_set_ime_active__sig: 'vi',
	redot_js_set_ime_active: function (p_active) {
		RedotIME.ime_active(p_active);
	},

	redot_js_set_ime_position__proxy: 'sync',
	redot_js_set_ime_position__sig: 'vii',
	redot_js_set_ime_position: function (p_x, p_y) {
		RedotIME.ime_position(p_x, p_y);
	},

	redot_js_set_ime_cb__proxy: 'sync',
	redot_js_set_ime_cb__sig: 'viiii',
	redot_js_set_ime_cb: function (p_ime_cb, p_key_cb, code, key) {
		const ime_cb = RedotRuntime.get_func(p_ime_cb);
		const key_cb = RedotRuntime.get_func(p_key_cb);
		RedotIME.init(ime_cb, key_cb, code, key);
	},

	redot_js_is_ime_focused__proxy: 'sync',
	redot_js_is_ime_focused__sig: 'i',
	redot_js_is_ime_focused: function () {
		return RedotIME.active;
	},

	/*
	 * Gamepad API
	 */
	redot_js_input_gamepad_cb__proxy: 'sync',
	redot_js_input_gamepad_cb__sig: 'vi',
	redot_js_input_gamepad_cb: function (change_cb) {
		const onchange = RedotRuntime.get_func(change_cb);
		RedotInputGamepads.init(onchange);
	},

	redot_js_input_gamepad_sample_count__proxy: 'sync',
	redot_js_input_gamepad_sample_count__sig: 'i',
	redot_js_input_gamepad_sample_count: function () {
		return RedotInputGamepads.get_samples().length;
	},

	redot_js_input_gamepad_sample__proxy: 'sync',
	redot_js_input_gamepad_sample__sig: 'i',
	redot_js_input_gamepad_sample: function () {
		RedotInputGamepads.sample();
		return 0;
	},

	redot_js_input_gamepad_sample_get__proxy: 'sync',
	redot_js_input_gamepad_sample_get__sig: 'iiiiiii',
	redot_js_input_gamepad_sample_get: function (p_index, r_btns, r_btns_num, r_axes, r_axes_num, r_standard) {
		const sample = RedotInputGamepads.get_sample(p_index);
		if (!sample || !sample.connected) {
			return 1;
		}
		const btns = sample.buttons;
		const btns_len = btns.length < 16 ? btns.length : 16;
		for (let i = 0; i < btns_len; i++) {
			RedotRuntime.setHeapValue(r_btns + (i << 2), btns[i], 'float');
		}
		RedotRuntime.setHeapValue(r_btns_num, btns_len, 'i32');
		const axes = sample.axes;
		const axes_len = axes.length < 10 ? axes.length : 10;
		for (let i = 0; i < axes_len; i++) {
			RedotRuntime.setHeapValue(r_axes + (i << 2), axes[i], 'float');
		}
		RedotRuntime.setHeapValue(r_axes_num, axes_len, 'i32');
		const is_standard = sample.standard ? 1 : 0;
		RedotRuntime.setHeapValue(r_standard, is_standard, 'i32');
		return 0;
	},

	/*
	 * Drag/Drop API
	 */
	redot_js_input_drop_files_cb__proxy: 'sync',
	redot_js_input_drop_files_cb__sig: 'vi',
	redot_js_input_drop_files_cb: function (callback) {
		const func = RedotRuntime.get_func(callback);
		const dropFiles = function (files) {
			const args = files || [];
			if (!args.length) {
				return;
			}
			const argc = args.length;
			const argv = RedotRuntime.allocStringArray(args);
			func(argv, argc);
			RedotRuntime.freeStringArray(argv, argc);
		};
		const canvas = RedotConfig.canvas;
		RedotEventListeners.add(canvas, 'dragover', function (ev) {
			// Prevent default behavior (which would try to open the file(s))
			ev.preventDefault();
		}, false);
		RedotEventListeners.add(canvas, 'drop', RedotInputDragDrop.handler(dropFiles));
	},

	/* Paste API */
	redot_js_input_paste_cb__proxy: 'sync',
	redot_js_input_paste_cb__sig: 'vi',
	redot_js_input_paste_cb: function (callback) {
		const func = RedotRuntime.get_func(callback);
		RedotEventListeners.add(window, 'paste', function (evt) {
			const text = evt.clipboardData.getData('text');
			const ptr = RedotRuntime.allocString(text);
			func(ptr);
			RedotRuntime.free(ptr);
		}, false);
	},

	redot_js_input_vibrate_handheld__proxy: 'sync',
	redot_js_input_vibrate_handheld__sig: 'vi',
	redot_js_input_vibrate_handheld: function (p_duration_ms) {
		if (typeof navigator.vibrate !== 'function') {
			RedotRuntime.print('This browser does not support vibration.');
		} else {
			navigator.vibrate(p_duration_ms);
		}
	},
};

autoAddDeps(RedotInput, '$RedotInput');
mergeInto(LibraryManager.library, RedotInput);
