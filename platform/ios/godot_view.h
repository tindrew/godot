/**************************************************************************/
/*  Redot_view.h                                                          */
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

#import <UIKit/UIKit.h>

class String;

@class RedotView;
@protocol DisplayLayer;
@protocol RedotViewRendererProtocol;

@protocol RedotViewDelegate

- (BOOL)RedotViewFinishedSetup:(RedotView *)view;

@end

@interface RedotView : UIView

@property(assign, nonatomic) id<RedotViewRendererProtocol> renderer;
@property(assign, nonatomic) id<RedotViewDelegate> delegate;

@property(assign, readonly, nonatomic) BOOL isActive;

@property(assign, nonatomic) BOOL useCADisplayLink;
@property(strong, readonly, nonatomic) CALayer<DisplayLayer> *renderingLayer;
@property(assign, readonly, nonatomic) BOOL canRender;

@property(assign, nonatomic) NSTimeInterval renderingInterval;

- (CALayer<DisplayLayer> *)initializeRenderingForDriver:(NSString *)driverName;
- (void)stopRendering;
- (void)startRendering;

@end
