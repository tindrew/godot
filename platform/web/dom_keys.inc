/**************************************************************************/
/*  dom_keys.inc                                                          */
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

#include "core/os/keyboard.h"

// See https://w3c.github.io/uievents-code/#code-value-tables
Key dom_code2redot_scancode(EM_UTF8 const p_code[32], EM_UTF8 const p_key[32], bool p_physical) {
#define DOM2redot(p_str, p_redot_code)                                                                \
	if (memcmp((const void *)p_str, (void *)(p_physical ? p_code : p_key), strlen(p_str) + 1) == 0) { \
		return Key::p_redot_code;                                                                     \
	}

	// Numpad section.
	DOM2redot("NumLock", NUMLOCK);
	DOM2redot("Numpad0", KP_0);
	DOM2redot("Numpad1", KP_1);
	DOM2redot("Numpad2", KP_2);
	DOM2redot("Numpad3", KP_3);
	DOM2redot("Numpad4", KP_4);
	DOM2redot("Numpad5", KP_5);
	DOM2redot("Numpad6", KP_6);
	DOM2redot("Numpad7", KP_7);
	DOM2redot("Numpad8", KP_8);
	DOM2redot("Numpad9", KP_9);
	DOM2redot("NumpadAdd", KP_ADD);
	DOM2redot("NumpadBackspace", BACKSPACE);
	DOM2redot("Clear", CLEAR); // NumLock on macOS.
	DOM2redot("NumpadClear", CLEAR);
	DOM2redot("NumpadClearEntry", CLEAR);
	//DOM2redot("NumpadComma", UNKNOWN);
	DOM2redot("NumpadDecimal", KP_PERIOD);
	DOM2redot("NumpadDivide", KP_DIVIDE);
	DOM2redot("NumpadEnter", KP_ENTER);
	DOM2redot("NumpadEqual", EQUAL);
	//DOM2redot("NumpadHash", UNKNOWN);
	//DOM2redot("NumpadMemoryAdd", UNKNOWN);
	//DOM2redot("NumpadMemoryClear", UNKNOWN);
	//DOM2redot("NumpadMemoryRecall", UNKNOWN);
	//DOM2redot("NumpadMemoryStore", UNKNOWN);
	//DOM2redot("NumpadMemorySubtract", UNKNOWN);
	DOM2redot("NumpadMultiply", KP_MULTIPLY);
	DOM2redot("NumpadParenLeft", PARENLEFT);
	DOM2redot("NumpadParenRight", PARENRIGHT);
	DOM2redot("NumpadStar", KP_MULTIPLY); // or ASTERISK ?
	DOM2redot("NumpadSubtract", KP_SUBTRACT);

	// Alphanumeric section.
	DOM2redot("Backquote", QUOTELEFT);
	DOM2redot("Backslash", BACKSLASH);
	DOM2redot("BracketLeft", BRACKETLEFT);
	DOM2redot("BracketRight", BRACKETRIGHT);
	DOM2redot("Comma", COMMA);
	DOM2redot("Digit0", KEY_0);
	DOM2redot("Digit1", KEY_1);
	DOM2redot("Digit2", KEY_2);
	DOM2redot("Digit3", KEY_3);
	DOM2redot("Digit4", KEY_4);
	DOM2redot("Digit5", KEY_5);
	DOM2redot("Digit6", KEY_6);
	DOM2redot("Digit7", KEY_7);
	DOM2redot("Digit8", KEY_8);
	DOM2redot("Digit9", KEY_9);
	DOM2redot("Equal", EQUAL);
	DOM2redot("IntlBackslash", BACKSLASH);
	//DOM2redot("IntlRo", UNKNOWN);
	DOM2redot("IntlYen", YEN);

	DOM2redot("KeyA", A);
	DOM2redot("KeyB", B);
	DOM2redot("KeyC", C);
	DOM2redot("KeyD", D);
	DOM2redot("KeyE", E);
	DOM2redot("KeyF", F);
	DOM2redot("KeyG", G);
	DOM2redot("KeyH", H);
	DOM2redot("KeyI", I);
	DOM2redot("KeyJ", J);
	DOM2redot("KeyK", K);
	DOM2redot("KeyL", L);
	DOM2redot("KeyM", M);
	DOM2redot("KeyN", N);
	DOM2redot("KeyO", O);
	DOM2redot("KeyP", P);
	DOM2redot("KeyQ", Q);
	DOM2redot("KeyR", R);
	DOM2redot("KeyS", S);
	DOM2redot("KeyT", T);
	DOM2redot("KeyU", U);
	DOM2redot("KeyV", V);
	DOM2redot("KeyW", W);
	DOM2redot("KeyX", X);
	DOM2redot("KeyY", Y);
	DOM2redot("KeyZ", Z);

	DOM2redot("Minus", MINUS);
	DOM2redot("Period", PERIOD);
	DOM2redot("Quote", APOSTROPHE);
	DOM2redot("Semicolon", SEMICOLON);
	DOM2redot("Slash", SLASH);

	// Functional keys in the Alphanumeric section.
	DOM2redot("Alt", ALT);
	DOM2redot("AltLeft", ALT);
	DOM2redot("AltRight", ALT);
	DOM2redot("Backspace", BACKSPACE);
	DOM2redot("CapsLock", CAPSLOCK);
	DOM2redot("ContextMenu", MENU);
	DOM2redot("Control", CTRL);
	DOM2redot("ControlLeft", CTRL);
	DOM2redot("ControlRight", CTRL);
	DOM2redot("Enter", ENTER);
	DOM2redot("Meta", META);
	DOM2redot("MetaLeft", META);
	DOM2redot("MetaRight", META);
	DOM2redot("OSLeft", META); // Command on macOS.
	DOM2redot("OSRight", META); // Command on macOS.
	DOM2redot("Shift", SHIFT);
	DOM2redot("ShiftLeft", SHIFT);
	DOM2redot("ShiftRight", SHIFT);
	DOM2redot("Space", SPACE);
	DOM2redot("Tab", TAB);

	// ControlPad section.
	DOM2redot("Delete", KEY_DELETE);
	DOM2redot("End", END);
	DOM2redot("Help", HELP);
	DOM2redot("Home", HOME);
	DOM2redot("Insert", INSERT);
	DOM2redot("PageDown", PAGEDOWN);
	DOM2redot("PageUp", PAGEUP);

	// ArrowPad section.
	DOM2redot("ArrowDown", DOWN);
	DOM2redot("ArrowLeft", LEFT);
	DOM2redot("ArrowRight", RIGHT);
	DOM2redot("ArrowUp", UP);

	// Function section.
	DOM2redot("Escape", ESCAPE);
	DOM2redot("F1", F1);
	DOM2redot("F2", F2);
	DOM2redot("F3", F3);
	DOM2redot("F4", F4);
	DOM2redot("F5", F5);
	DOM2redot("F6", F6);
	DOM2redot("F7", F7);
	DOM2redot("F8", F8);
	DOM2redot("F9", F9);
	DOM2redot("F10", F10);
	DOM2redot("F11", F11);
	DOM2redot("F12", F12);
	//DOM2redot("Fn", UNKNOWN); // never actually fired, but included in the standard draft.
	//DOM2redot("FnLock", UNKNOWN);
	DOM2redot("PrintScreen", PRINT);
	DOM2redot("ScrollLock", SCROLLLOCK);
	DOM2redot("Pause", PAUSE);

	// Media keys section.
	DOM2redot("BrowserBack", BACK);
	DOM2redot("BrowserFavorites", FAVORITES);
	DOM2redot("BrowserForward", FORWARD);
	DOM2redot("BrowserHome", OPENURL);
	DOM2redot("BrowserRefresh", REFRESH);
	DOM2redot("BrowserSearch", SEARCH);
	DOM2redot("BrowserStop", STOP);
	//DOM2redot("Eject", UNKNOWN);
	DOM2redot("LaunchApp1", LAUNCH0);
	DOM2redot("LaunchApp2", LAUNCH1);
	DOM2redot("LaunchMail", LAUNCHMAIL);
	DOM2redot("MediaPlayPause", MEDIAPLAY);
	DOM2redot("MediaSelect", LAUNCHMEDIA);
	DOM2redot("MediaStop", MEDIASTOP);
	DOM2redot("MediaTrackNext", MEDIANEXT);
	DOM2redot("MediaTrackPrevious", MEDIAPREVIOUS);
	//DOM2redot("Power", UNKNOWN);
	//DOM2redot("Sleep", UNKNOWN);
	DOM2redot("AudioVolumeDown", VOLUMEDOWN);
	DOM2redot("AudioVolumeMute", VOLUMEMUTE);
	DOM2redot("AudioVolumeUp", VOLUMEUP);
	//DOM2redot("WakeUp", UNKNOWN);

	// Printable ASCII.
	uint8_t b0 = (uint8_t)p_key[0];
	uint8_t b1 = (uint8_t)p_key[1];
	if (b0 >= 0x20 && b0 < 0x7F) { // ASCII.
		if (b0 > 0x60 && b0 < 0x7B) { // Lowercase ASCII.
			b0 -= 32;
		}
		return (Key)b0;
	} else if (b0 == 0xC2 && b1 == 0xA5) {
		return Key::YEN;
	} else if (b0 == 0xC2 && b1 == 0xA7) {
		return Key::SECTION;
	}

	return Key::NONE;
#undef DOM2redot
}

KeyLocation dom_code2redot_key_location(EM_UTF8 const p_code[32]) {
#define DOM2redot(m_str, m_redot_code)                                         \
	if (memcmp((const void *)m_str, (void *)p_code, strlen(m_str) + 1) == 0) { \
		return KeyLocation::m_redot_code;                                      \
	}

	DOM2redot("AltLeft", LEFT);
	DOM2redot("AltRight", RIGHT);
	DOM2redot("ControlLeft", LEFT);
	DOM2redot("ControlRight", RIGHT);
	DOM2redot("MetaLeft", LEFT);
	DOM2redot("MetaRight", RIGHT);
	DOM2redot("OSLeft", LEFT);
	DOM2redot("OSRight", RIGHT);
	DOM2redot("ShiftLeft", LEFT);
	DOM2redot("ShiftRight", RIGHT);

	return KeyLocation::UNSPECIFIED;
#undef DOM2redot
}
