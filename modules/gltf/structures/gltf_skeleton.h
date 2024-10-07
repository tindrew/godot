/**************************************************************************/
/*  gltf_skeleton.h                                                       */
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

#ifndef GLTF_SKELETON_H
#define GLTF_SKELETON_H

#include "../gltf_defines.h"

#include "core/io/resource.h"
#include "scene/3d/bone_attachment_3d.h"
#include "scene/3d/skeleton_3d.h"

class GLTFSkeleton : public Resource {
	GDCLASS(GLTFSkeleton, Resource);
	friend class GLTFDocument;
	friend class SkinTool;
	friend class FBXDocument;

private:
	// The *synthesized* skeletons joints
	Vector<GLTFNodeIndex> joints;

	// The roots of the skeleton. If there are multiple, each root must have the
	// same parent (ie roots are siblings)
	Vector<GLTFNodeIndex> roots;

	// The created Skeleton3D for the scene
	Skeleton3D *redot_skeleton = nullptr;

	// Set of unique bone names for the skeleton
	HashSet<String> unique_names;

	HashMap<int32_t, GLTFNodeIndex> redot_bone_node;

	Vector<BoneAttachment3D *> bone_attachments;

protected:
	static void _bind_methods();

public:
	Vector<GLTFNodeIndex> get_joints();
	void set_joints(Vector<GLTFNodeIndex> p_joints);

	Vector<GLTFNodeIndex> get_roots();
	void set_roots(Vector<GLTFNodeIndex> p_roots);

	Skeleton3D *get_redot_skeleton();

	// Skeleton *get_redot_skeleton() {
	// 	return redot_skeleton;
	// }
	// void set_redot_skeleton(Skeleton p_*redot_skeleton) {
	// 	redot_skeleton = p_redot_skeleton;
	// }

	TypedArray<String> get_unique_names();
	void set_unique_names(TypedArray<String> p_unique_names);

	//RBMap<int32_t, GLTFNodeIndex> get_redot_bone_node() {
	//	return redot_bone_node;
	//}
	//void set_redot_bone_node(const RBMap<int32_t, GLTFNodeIndex> &p_redot_bone_node) {
	//	redot_bone_node = p_redot_bone_node;
	//}
	Dictionary get_redot_bone_node();
	void set_redot_bone_node(Dictionary p_indict);

	//Dictionary get_redot_bone_node() {
	//	return VariantConversion::to_dict(redot_bone_node);
	//}
	//void set_redot_bone_node(Dictionary p_indict) {
	//	VariantConversion::set_from_dict(redot_bone_node, p_indict);
	//}

	BoneAttachment3D *get_bone_attachment(int idx);

	int32_t get_bone_attachment_count();
};

#endif // GLTF_SKELETON_H
