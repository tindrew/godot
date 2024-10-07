/**************************************************************************/
/*  openxr_meta_controller_extension.h                                    */
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

#ifndef OPENXR_META_CONTROLLER_EXTENSION_H
#define OPENXR_META_CONTROLLER_EXTENSION_H

#include "openxr_extension_wrapper.h"

class OpenXRMetaControllerExtension : public OpenXRExtensionWrapper {
public:
	enum MetaControllers {
		META_TOUCH_PROXIMITY, // Proximity extensions for normal touch controllers
		META_TOUCH_PRO, // Touch controller for the Quest Pro
		META_TOUCH_PLUS, // Touch controller for the Quest Plus
		META_MAX_CONTROLLERS
	};

	virtual HashMap<String, bool *> get_requested_extensions() override;

	bool is_available(MetaControllers p_type);

	virtual void on_register_metadata() override;

private:
	bool available[META_MAX_CONTROLLERS] = { false, false, false };
};

#endif // OPENXR_META_CONTROLLER_EXTENSION_H
