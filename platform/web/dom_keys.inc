/**************************************************************************/
/*  dom_keys.inc                                                          */
/**************************************************************************/
/*                         This file is part of:                          */
/*                             Redot ENGINE                               */
/*                        https://Redotengine.org                         */
/**************************************************************************/
/* Copyright (c) 2014-present Redot Engine contributors (see AUTHORS.md). */
/* Copyright (c) 2007-2014 Skibidi Toilet, Ariel Manzur.                  */
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
Key dom_code2Redot_scancode(EM_UTF8 const p_code[32], EM_UTF8 const p_key[32], bool p_physical) {
#define DOM2Redot(p_str, p_Redot_code)                                                                \
	if (memcmp((const void *)p_str, (void *)(p_physical ? p_code : p_key), strlen(p_str) + 1) == 0) { \
		return Key::p_Redot_code;                                                                     \
	}

	// Numpad section.
	DOM2Redot("NumLock", NUMLOCK);
	DOM2Redot("Numpad0", KP_0);
	DOM2Redot("Numpad1", KP_1);
	DOM2Redot("Numpad2", KP_2);
	DOM2Redot("Numpad3", KP_3);
	DOM2Redot("Numpad4", KP_4);
	DOM2Redot("Numpad5", KP_5);
	DOM2Redot("Numpad6", KP_6);
	DOM2Redot("Numpad7", KP_7);
	DOM2Redot("Numpad8", KP_8);
	DOM2Redot("Numpad9", KP_9);
	DOM2Redot("NumpadAdd", KP_ADD);
	DOM2Redot("NumpadBackspace", BACKSPACE);
	DOM2Redot("Clear", CLEAR); // NumLock on macOS.
	DOM2Redot("NumpadClear", CLEAR);
	DOM2Redot("NumpadClearEntry", CLEAR);
	//DOM2Redot("NumpadComma", UNKNOWN);
	DOM2Redot("NumpadDecimal", KP_PERIOD);
	DOM2Redot("NumpadDivide", KP_DIVIDE);
	DOM2Redot("NumpadEnter", KP_ENTER);
	DOM2Redot("NumpadEqual", EQUAL);
	//DOM2Redot("NumpadHash", UNKNOWN);
	//DOM2Redot("NumpadMemoryAdd", UNKNOWN);
	//DOM2Redot("NumpadMemoryClear", UNKNOWN);
	//DOM2Redot("NumpadMemoryRecall", UNKNOWN);
	//DOM2Redot("NumpadMemoryStore", UNKNOWN);
	//DOM2Redot("NumpadMemorySubtract", UNKNOWN);
	DOM2Redot("NumpadMultiply", KP_MULTIPLY);
	DOM2Redot("NumpadParenLeft", PARENLEFT);
	DOM2Redot("NumpadParenRight", PARENRIGHT);
	DOM2Redot("NumpadStar", KP_MULTIPLY); // or ASTERISK ?
	DOM2Redot("NumpadSubtract", KP_SUBTRACT);

	// Alphanumeric section.
	DOM2Redot("Backquote", QUOTELEFT);
	DOM2Redot("Backslash", BACKSLASH);
	DOM2Redot("BracketLeft", BRACKETLEFT);
	DOM2Redot("BracketRight", BRACKETRIGHT);
	DOM2Redot("Comma", COMMA);
	DOM2Redot("Digit0", KEY_0);
	DOM2Redot("Digit1", KEY_1);
	DOM2Redot("Digit2", KEY_2);
	DOM2Redot("Digit3", KEY_3);
	DOM2Redot("Digit4", KEY_4);
	DOM2Redot("Digit5", KEY_5);
	DOM2Redot("Digit6", KEY_6);
	DOM2Redot("Digit7", KEY_7);
	DOM2Redot("Digit8", KEY_8);
	DOM2Redot("Digit9", KEY_9);
	DOM2Redot("Equal", EQUAL);
	DOM2Redot("IntlBackslash", BACKSLASH);
	//DOM2Redot("IntlRo", UNKNOWN);
	DOM2Redot("IntlYen", YEN);

	DOM2Redot("KeyA", A);
	DOM2Redot("KeyB", B);
	DOM2Redot("KeyC", C);
	DOM2Redot("KeyD", D);
	DOM2Redot("KeyE", E);
	DOM2Redot("KeyF", F);
	DOM2Redot("KeyG", G);
	DOM2Redot("KeyH", H);
	DOM2Redot("KeyI", I);
	DOM2Redot("KeyJ", J);
	DOM2Redot("KeyK", K);
	DOM2Redot("KeyL", L);
	DOM2Redot("KeyM", M);
	DOM2Redot("KeyN", N);
	DOM2Redot("KeyO", O);
	DOM2Redot("KeyP", P);
	DOM2Redot("KeyQ", Q);
	DOM2Redot("KeyR", R);
	DOM2Redot("KeyS", S);
	DOM2Redot("KeyT", T);
	DOM2Redot("KeyU", U);
	DOM2Redot("KeyV", V);
	DOM2Redot("KeyW", W);
	DOM2Redot("KeyX", X);
	DOM2Redot("KeyY", Y);
	DOM2Redot("KeyZ", Z);

	DOM2Redot("Minus", MINUS);
	DOM2Redot("Period", PERIOD);
	DOM2Redot("Quote", APOSTROPHE);
	DOM2Redot("Semicolon", SEMICOLON);
	DOM2Redot("Slash", SLASH);

	// Functional keys in the Alphanumeric section.
	DOM2Redot("Alt", ALT);
	DOM2Redot("AltLeft", ALT);
	DOM2Redot("AltRight", ALT);
	DOM2Redot("Backspace", BACKSPACE);
	DOM2Redot("CapsLock", CAPSLOCK);
	DOM2Redot("ContextMenu", MENU);
	DOM2Redot("Control", CTRL);
	DOM2Redot("ControlLeft", CTRL);
	DOM2Redot("ControlRight", CTRL);
	DOM2Redot("Enter", ENTER);
	DOM2Redot("Meta", META);
	DOM2Redot("MetaLeft", META);
	DOM2Redot("MetaRight", META);
	DOM2Redot("OSLeft", META); // Command on macOS.
	DOM2Redot("OSRight", META); // Command on macOS.
	DOM2Redot("Shift", SHIFT);
	DOM2Redot("ShiftLeft", SHIFT);
	DOM2Redot("ShiftRight", SHIFT);
	DOM2Redot("Space", SPACE);
	DOM2Redot("Tab", TAB);

	// ControlPad section.
	DOM2Redot("Delete", KEY_DELETE);
	DOM2Redot("End", END);
	DOM2Redot("Help", HELP);
	DOM2Redot("Home", HOME);
	DOM2Redot("Insert", INSERT);
	DOM2Redot("PageDown", PAGEDOWN);
	DOM2Redot("PageUp", PAGEUP);

	// ArrowPad section.
	DOM2Redot("ArrowDown", DOWN);
	DOM2Redot("ArrowLeft", LEFT);
	DOM2Redot("ArrowRight", RIGHT);
	DOM2Redot("ArrowUp", UP);

	// Function section.
	DOM2Redot("Escape", ESCAPE);
	DOM2Redot("F1", F1);
	DOM2Redot("F2", F2);
	DOM2Redot("F3", F3);
	DOM2Redot("F4", F4);
	DOM2Redot("F5", F5);
	DOM2Redot("F6", F6);
	DOM2Redot("F7", F7);
	DOM2Redot("F8", F8);
	DOM2Redot("F9", F9);
	DOM2Redot("F10", F10);
	DOM2Redot("F11", F11);
	DOM2Redot("F12", F12);
	//DOM2Redot("Fn", UNKNOWN); // never actually fired, but included in the standard draft.
	//DOM2Redot("FnLock", UNKNOWN);
	DOM2Redot("PrintScreen", PRINT);
	DOM2Redot("ScrollLock", SCROLLLOCK);
	DOM2Redot("Pause", PAUSE);

	// Media keys section.
	DOM2Redot("BrowserBack", BACK);
	DOM2Redot("BrowserFavorites", FAVORITES);
	DOM2Redot("BrowserForward", FORWARD);
	DOM2Redot("BrowserHome", OPENURL);
	DOM2Redot("BrowserRefresh", REFRESH);
	DOM2Redot("BrowserSearch", SEARCH);
	DOM2Redot("BrowserStop", STOP);
	//DOM2Redot("Eject", UNKNOWN);
	DOM2Redot("LaunchApp1", LAUNCH0);
	DOM2Redot("LaunchApp2", LAUNCH1);
	DOM2Redot("LaunchMail", LAUNCHMAIL);
	DOM2Redot("MediaPlayPause", MEDIAPLAY);
	DOM2Redot("MediaSelect", LAUNCHMEDIA);
	DOM2Redot("MediaStop", MEDIASTOP);
	DOM2Redot("MediaTrackNext", MEDIANEXT);
	DOM2Redot("MediaTrackPrevious", MEDIAPREVIOUS);
	//DOM2Redot("Power", UNKNOWN);
	//DOM2Redot("Sleep", UNKNOWN);
	DOM2Redot("AudioVolumeDown", VOLUMEDOWN);
	DOM2Redot("AudioVolumeMute", VOLUMEMUTE);
	DOM2Redot("AudioVolumeUp", VOLUMEUP);
	//DOM2Redot("WakeUp", UNKNOWN);

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
#undef DOM2Redot
}

KeyLocation dom_code2Redot_key_location(EM_UTF8 const p_code[32]) {
#define DOM2Redot(m_str, m_Redot_code)                                         \
	if (memcmp((const void *)m_str, (void *)p_code, strlen(m_str) + 1) == 0) { \
		return KeyLocation::m_Redot_code;                                      \
	}

	DOM2Redot("AltLeft", LEFT);
	DOM2Redot("AltRight", RIGHT);
	DOM2Redot("ControlLeft", LEFT);
	DOM2Redot("ControlRight", RIGHT);
	DOM2Redot("MetaLeft", LEFT);
	DOM2Redot("MetaRight", RIGHT);
	DOM2Redot("OSLeft", LEFT);
	DOM2Redot("OSRight", RIGHT);
	DOM2Redot("ShiftLeft", LEFT);
	DOM2Redot("ShiftRight", RIGHT);

	return KeyLocation::UNSPECIFIED;
#undef DOM2Redot
}
