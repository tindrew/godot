/**************************************************************************/
/*  view_controller.mm                                                    */
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

#import "view_controller.h"

#import "display_server_ios.h"
#import "redot_view.h"
#import "redot_view_renderer.h"
#import "key_mapping_ios.h"
#import "keyboard_input_view.h"
#import "os_ios.h"

#include "core/config/project_settings.h"

#import <AVFoundation/AVFoundation.h>
#import <GameController/GameController.h>

@interface ViewController () <RedotViewDelegate>

@property(strong, nonatomic) RedotViewRenderer *renderer;
@property(strong, nonatomic) RedotKeyboardInputView *keyboardView;

@property(strong, nonatomic) UIView *redotLoadingOverlay;

@end

@implementation ViewController

- (RedotView *)redotView {
	return (RedotView *)self.view;
}

- (void)pressesBegan:(NSSet<UIPress *> *)presses withEvent:(UIPressesEvent *)event {
	[super pressesBegan:presses withEvent:event];

	if (!DisplayServerIOS::get_singleton() || DisplayServerIOS::get_singleton()->is_keyboard_active()) {
		return;
	}
	if (@available(iOS 13.4, *)) {
		for (UIPress *press in presses) {
			String u32lbl = String::utf8([press.key.charactersIgnoringModifiers UTF8String]);
			String u32text = String::utf8([press.key.characters UTF8String]);
			Key key = KeyMappingIOS::remap_key(press.key.keyCode);

			if (press.key.keyCode == 0 && u32text.is_empty() && u32lbl.is_empty()) {
				continue;
			}

			char32_t us = 0;
			if (!u32lbl.is_empty() && !u32lbl.begins_with("UIKey")) {
				us = u32lbl[0];
			}

			KeyLocation location = KeyMappingIOS::key_location(press.key.keyCode);

			if (!u32text.is_empty() && !u32text.begins_with("UIKey")) {
				for (int i = 0; i < u32text.length(); i++) {
					const char32_t c = u32text[i];
					DisplayServerIOS::get_singleton()->key(fix_keycode(us, key), c, fix_key_label(us, key), key, press.key.modifierFlags, true, location);
				}
			} else {
				DisplayServerIOS::get_singleton()->key(fix_keycode(us, key), 0, fix_key_label(us, key), key, press.key.modifierFlags, true, location);
			}
		}
	}
}

- (void)pressesEnded:(NSSet<UIPress *> *)presses withEvent:(UIPressesEvent *)event {
	[super pressesEnded:presses withEvent:event];

	if (!DisplayServerIOS::get_singleton() || DisplayServerIOS::get_singleton()->is_keyboard_active()) {
		return;
	}
	if (@available(iOS 13.4, *)) {
		for (UIPress *press in presses) {
			String u32lbl = String::utf8([press.key.charactersIgnoringModifiers UTF8String]);
			Key key = KeyMappingIOS::remap_key(press.key.keyCode);

			if (press.key.keyCode == 0 && u32lbl.is_empty()) {
				continue;
			}

			char32_t us = 0;
			if (!u32lbl.is_empty() && !u32lbl.begins_with("UIKey")) {
				us = u32lbl[0];
			}

			KeyLocation location = KeyMappingIOS::key_location(press.key.keyCode);

			DisplayServerIOS::get_singleton()->key(fix_keycode(us, key), 0, fix_key_label(us, key), key, press.key.modifierFlags, false, location);
		}
	}
}

- (void)loadView {
	RedotView *view = [[RedotView alloc] init];
	RedotViewRenderer *renderer = [[RedotViewRenderer alloc] init];

	self.renderer = renderer;
	self.view = view;

	view.renderer = self.renderer;
	view.delegate = self;
}

- (instancetype)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil {
	self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];

	if (self) {
		[self redot_commonInit];
	}

	return self;
}

- (instancetype)initWithCoder:(NSCoder *)coder {
	self = [super initWithCoder:coder];

	if (self) {
		[self redot_commonInit];
	}

	return self;
}

- (void)redot_commonInit {
	// Initialize view controller values.
}

- (void)didReceiveMemoryWarning {
	[super didReceiveMemoryWarning];
	print_verbose("Did receive memory warning!");
}

- (void)viewDidLoad {
	[super viewDidLoad];

	[self observeKeyboard];
	[self displayLoadingOverlay];

	[self setNeedsUpdateOfScreenEdgesDeferringSystemGestures];
}

- (void)observeKeyboard {
	print_verbose("Setting up keyboard input view.");
	self.keyboardView = [RedotKeyboardInputView new];
	[self.view addSubview:self.keyboardView];

	print_verbose("Adding observer for keyboard show/hide.");
	[[NSNotificationCenter defaultCenter]
			addObserver:self
			   selector:@selector(keyboardOnScreen:)
				   name:UIKeyboardDidShowNotification
				 object:nil];
	[[NSNotificationCenter defaultCenter]
			addObserver:self
			   selector:@selector(keyboardHidden:)
				   name:UIKeyboardDidHideNotification
				 object:nil];
}

- (void)displayLoadingOverlay {
	NSBundle *bundle = [NSBundle mainBundle];
	NSString *storyboardName = @"Launch Screen";

	if ([bundle pathForResource:storyboardName ofType:@"storyboardc"] == nil) {
		return;
	}

	UIStoryboard *launchStoryboard = [UIStoryboard storyboardWithName:storyboardName bundle:bundle];

	UIViewController *controller = [launchStoryboard instantiateInitialViewController];
	self.redotLoadingOverlay = controller.view;
	self.redotLoadingOverlay.frame = self.view.bounds;
	self.redotLoadingOverlay.autoresizingMask = UIViewAutoresizingFlexibleHeight | UIViewAutoresizingFlexibleWidth;

	[self.view addSubview:self.redotLoadingOverlay];
}

- (BOOL)redotViewFinishedSetup:(RedotView *)view {
	[self.redotLoadingOverlay removeFromSuperview];
	self.redotLoadingOverlay = nil;

	return YES;
}

- (void)dealloc {
	self.keyboardView = nil;

	self.renderer = nil;

	if (self.redotLoadingOverlay) {
		[self.redotLoadingOverlay removeFromSuperview];
		self.redotLoadingOverlay = nil;
	}

	[[NSNotificationCenter defaultCenter] removeObserver:self];
}

// MARK: Orientation

- (UIRectEdge)preferredScreenEdgesDeferringSystemGestures {
	if (GLOBAL_GET("display/window/ios/suppress_ui_gesture")) {
		return UIRectEdgeAll;
	} else {
		return UIRectEdgeNone;
	}
}

- (BOOL)shouldAutorotate {
	if (!DisplayServerIOS::get_singleton()) {
		return NO;
	}

	switch (DisplayServerIOS::get_singleton()->screen_get_orientation(DisplayServer::SCREEN_OF_MAIN_WINDOW)) {
		case DisplayServer::SCREEN_SENSOR:
		case DisplayServer::SCREEN_SENSOR_LANDSCAPE:
		case DisplayServer::SCREEN_SENSOR_PORTRAIT:
			return YES;
		default:
			return NO;
	}
}

- (UIInterfaceOrientationMask)supportedInterfaceOrientations {
	if (!DisplayServerIOS::get_singleton()) {
		return UIInterfaceOrientationMaskAll;
	}

	switch (DisplayServerIOS::get_singleton()->screen_get_orientation(DisplayServer::SCREEN_OF_MAIN_WINDOW)) {
		case DisplayServer::SCREEN_PORTRAIT:
			return UIInterfaceOrientationMaskPortrait;
		case DisplayServer::SCREEN_REVERSE_LANDSCAPE:
			if (UIDevice.currentDevice.userInterfaceIdiom == UIUserInterfaceIdiomPad) {
				return UIInterfaceOrientationMaskLandscapeLeft;
			} else {
				return UIInterfaceOrientationMaskLandscapeRight;
			}
		case DisplayServer::SCREEN_REVERSE_PORTRAIT:
			return UIInterfaceOrientationMaskPortraitUpsideDown;
		case DisplayServer::SCREEN_SENSOR_LANDSCAPE:
			return UIInterfaceOrientationMaskLandscape;
		case DisplayServer::SCREEN_SENSOR_PORTRAIT:
			return UIInterfaceOrientationMaskPortrait | UIInterfaceOrientationMaskPortraitUpsideDown;
		case DisplayServer::SCREEN_SENSOR:
			return UIInterfaceOrientationMaskAll;
		case DisplayServer::SCREEN_LANDSCAPE:
			if (UIDevice.currentDevice.userInterfaceIdiom == UIUserInterfaceIdiomPad) {
				return UIInterfaceOrientationMaskLandscapeRight;
			} else {
				return UIInterfaceOrientationMaskLandscapeLeft;
			}
	}
}

- (BOOL)prefersStatusBarHidden {
	if (GLOBAL_GET("display/window/ios/hide_status_bar")) {
		return YES;
	} else {
		return NO;
	}
}

- (BOOL)prefersHomeIndicatorAutoHidden {
	if (GLOBAL_GET("display/window/ios/hide_home_indicator")) {
		return YES;
	} else {
		return NO;
	}
}

// MARK: Keyboard

- (void)keyboardOnScreen:(NSNotification *)notification {
	NSDictionary *info = notification.userInfo;
	NSValue *value = info[UIKeyboardFrameEndUserInfoKey];

	CGRect rawFrame = [value CGRectValue];
	CGRect keyboardFrame = [self.view convertRect:rawFrame fromView:nil];

	if (DisplayServerIOS::get_singleton()) {
		DisplayServerIOS::get_singleton()->virtual_keyboard_set_height(keyboardFrame.size.height);
	}
}

- (void)keyboardHidden:(NSNotification *)notification {
	if (DisplayServerIOS::get_singleton()) {
		DisplayServerIOS::get_singleton()->virtual_keyboard_set_height(0);
	}
}

@end
