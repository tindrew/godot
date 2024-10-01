/**************************************************************************/
/*  rendering_context_driver_vulkan_windows.h                             */
/**************************************************************************/
/*                         This file is part of:                          */
/*                             Redot ENGINE                               */
/*                        https://Redotengine.org                         */
/**************************************************************************/
/* Copyright (c) 2014-present Redot Engine contributors (see AUTHORS.md). */
/* Copyright (c) 2007-2014 Juan Linietsky, Ariel Manzur.                  */
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

#ifndef RENDERING_CONTEXT_DRIVER_VULKAN_WINDOWS_H
#define RENDERING_CONTEXT_DRIVER_VULKAN_WINDOWS_H

#ifdef VULKAN_ENABLED

#include "drivers/vulkan/rendering_context_driver_vulkan.h"

#define WIN32_LEAN_AND_MEAN
#include <windows.h>

class RenderingContextDriverVulkanWindows : public RenderingContextDriverVulkan {
private:
	const char *_get_platform_surface_extension() const override final;

protected:
	SurfaceID surface_create(const void *p_platform_data) override final;

public:
	struct WindowPlatformData {
		HWND window;
		HINSTANCE instance;
	};

	RenderingContextDriverVulkanWindows();
	~RenderingContextDriverVulkanWindows() override;
};

#endif // VULKAN_ENABLED

#endif // RENDERING_CONTEXT_DRIVER_VULKAN_WINDOWS_H
