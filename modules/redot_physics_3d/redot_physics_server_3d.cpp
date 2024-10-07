/**************************************************************************/
/*  redot_physics_server_3d.cpp                                           */
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

#include "redot_physics_server_3d.h"

#include "redot_body_direct_state_3d.h"
#include "redot_broad_phase_3d_bvh.h"
#include "joints/redot_cone_twist_joint_3d.h"
#include "joints/redot_generic_6dof_joint_3d.h"
#include "joints/redot_hinge_joint_3d.h"
#include "joints/redot_pin_joint_3d.h"
#include "joints/redot_slider_joint_3d.h"

#include "core/debugger/engine_debugger.h"
#include "core/os/os.h"

#define FLUSH_QUERY_CHECK(m_object) \
	ERR_FAIL_COND_MSG(m_object->get_space() && flushing_queries, "Can't change this state while flushing queries. Use call_deferred() or set_deferred() to change monitoring state instead.");

RID RedotPhysicsServer3D::world_boundary_shape_create() {
	RedotShape3D *shape = memnew(RedotWorldBoundaryShape3D);
	RID rid = shape_owner.make_rid(shape);
	shape->set_self(rid);
	return rid;
}
RID RedotPhysicsServer3D::separation_ray_shape_create() {
	RedotShape3D *shape = memnew(RedotSeparationRayShape3D);
	RID rid = shape_owner.make_rid(shape);
	shape->set_self(rid);
	return rid;
}
RID RedotPhysicsServer3D::sphere_shape_create() {
	RedotShape3D *shape = memnew(RedotSphereShape3D);
	RID rid = shape_owner.make_rid(shape);
	shape->set_self(rid);
	return rid;
}
RID RedotPhysicsServer3D::box_shape_create() {
	RedotShape3D *shape = memnew(RedotBoxShape3D);
	RID rid = shape_owner.make_rid(shape);
	shape->set_self(rid);
	return rid;
}
RID RedotPhysicsServer3D::capsule_shape_create() {
	RedotShape3D *shape = memnew(RedotCapsuleShape3D);
	RID rid = shape_owner.make_rid(shape);
	shape->set_self(rid);
	return rid;
}
RID RedotPhysicsServer3D::cylinder_shape_create() {
	RedotShape3D *shape = memnew(RedotCylinderShape3D);
	RID rid = shape_owner.make_rid(shape);
	shape->set_self(rid);
	return rid;
}
RID RedotPhysicsServer3D::convex_polygon_shape_create() {
	RedotShape3D *shape = memnew(RedotConvexPolygonShape3D);
	RID rid = shape_owner.make_rid(shape);
	shape->set_self(rid);
	return rid;
}
RID RedotPhysicsServer3D::concave_polygon_shape_create() {
	RedotShape3D *shape = memnew(RedotConcavePolygonShape3D);
	RID rid = shape_owner.make_rid(shape);
	shape->set_self(rid);
	return rid;
}
RID RedotPhysicsServer3D::heightmap_shape_create() {
	RedotShape3D *shape = memnew(RedotHeightMapShape3D);
	RID rid = shape_owner.make_rid(shape);
	shape->set_self(rid);
	return rid;
}
RID RedotPhysicsServer3D::custom_shape_create() {
	ERR_FAIL_V(RID());
}

void RedotPhysicsServer3D::shape_set_data(RID p_shape, const Variant &p_data) {
	RedotShape3D *shape = shape_owner.get_or_null(p_shape);
	ERR_FAIL_NULL(shape);
	shape->set_data(p_data);
};

void RedotPhysicsServer3D::shape_set_custom_solver_bias(RID p_shape, real_t p_bias) {
	RedotShape3D *shape = shape_owner.get_or_null(p_shape);
	ERR_FAIL_NULL(shape);
	shape->set_custom_bias(p_bias);
}

PhysicsServer3D::ShapeType RedotPhysicsServer3D::shape_get_type(RID p_shape) const {
	const RedotShape3D *shape = shape_owner.get_or_null(p_shape);
	ERR_FAIL_NULL_V(shape, SHAPE_CUSTOM);
	return shape->get_type();
};

Variant RedotPhysicsServer3D::shape_get_data(RID p_shape) const {
	const RedotShape3D *shape = shape_owner.get_or_null(p_shape);
	ERR_FAIL_NULL_V(shape, Variant());
	ERR_FAIL_COND_V(!shape->is_configured(), Variant());
	return shape->get_data();
};

void RedotPhysicsServer3D::shape_set_margin(RID p_shape, real_t p_margin) {
}

real_t RedotPhysicsServer3D::shape_get_margin(RID p_shape) const {
	return 0.0;
}

real_t RedotPhysicsServer3D::shape_get_custom_solver_bias(RID p_shape) const {
	const RedotShape3D *shape = shape_owner.get_or_null(p_shape);
	ERR_FAIL_NULL_V(shape, 0);
	return shape->get_custom_bias();
}

RID RedotPhysicsServer3D::space_create() {
	RedotSpace3D *space = memnew(RedotSpace3D);
	RID id = space_owner.make_rid(space);
	space->set_self(id);
	RID area_id = area_create();
	RedotArea3D *area = area_owner.get_or_null(area_id);
	ERR_FAIL_NULL_V(area, RID());
	space->set_default_area(area);
	area->set_space(space);
	area->set_priority(-1);
	RID sgb = body_create();
	body_set_space(sgb, id);
	body_set_mode(sgb, BODY_MODE_STATIC);
	space->set_static_global_body(sgb);

	return id;
};

void RedotPhysicsServer3D::space_set_active(RID p_space, bool p_active) {
	RedotSpace3D *space = space_owner.get_or_null(p_space);
	ERR_FAIL_NULL(space);
	if (p_active) {
		active_spaces.insert(space);
	} else {
		active_spaces.erase(space);
	}
}

bool RedotPhysicsServer3D::space_is_active(RID p_space) const {
	const RedotSpace3D *space = space_owner.get_or_null(p_space);
	ERR_FAIL_NULL_V(space, false);

	return active_spaces.has(space);
}

void RedotPhysicsServer3D::space_set_param(RID p_space, SpaceParameter p_param, real_t p_value) {
	RedotSpace3D *space = space_owner.get_or_null(p_space);
	ERR_FAIL_NULL(space);

	space->set_param(p_param, p_value);
}

real_t RedotPhysicsServer3D::space_get_param(RID p_space, SpaceParameter p_param) const {
	const RedotSpace3D *space = space_owner.get_or_null(p_space);
	ERR_FAIL_NULL_V(space, 0);
	return space->get_param(p_param);
}

PhysicsDirectSpaceState3D *RedotPhysicsServer3D::space_get_direct_state(RID p_space) {
	RedotSpace3D *space = space_owner.get_or_null(p_space);
	ERR_FAIL_NULL_V(space, nullptr);
	ERR_FAIL_COND_V_MSG((using_threads && !doing_sync) || space->is_locked(), nullptr, "Space state is inaccessible right now, wait for iteration or physics process notification.");

	return space->get_direct_state();
}

void RedotPhysicsServer3D::space_set_debug_contacts(RID p_space, int p_max_contacts) {
	RedotSpace3D *space = space_owner.get_or_null(p_space);
	ERR_FAIL_NULL(space);
	space->set_debug_contacts(p_max_contacts);
}

Vector<Vector3> RedotPhysicsServer3D::space_get_contacts(RID p_space) const {
	RedotSpace3D *space = space_owner.get_or_null(p_space);
	ERR_FAIL_NULL_V(space, Vector<Vector3>());
	return space->get_debug_contacts();
}

int RedotPhysicsServer3D::space_get_contact_count(RID p_space) const {
	RedotSpace3D *space = space_owner.get_or_null(p_space);
	ERR_FAIL_NULL_V(space, 0);
	return space->get_debug_contact_count();
}

RID RedotPhysicsServer3D::area_create() {
	RedotArea3D *area = memnew(RedotArea3D);
	RID rid = area_owner.make_rid(area);
	area->set_self(rid);
	return rid;
}

void RedotPhysicsServer3D::area_set_space(RID p_area, RID p_space) {
	RedotArea3D *area = area_owner.get_or_null(p_area);
	ERR_FAIL_NULL(area);

	RedotSpace3D *space = nullptr;
	if (p_space.is_valid()) {
		space = space_owner.get_or_null(p_space);
		ERR_FAIL_NULL(space);
	}

	if (area->get_space() == space) {
		return; //pointless
	}

	area->clear_constraints();
	area->set_space(space);
}

RID RedotPhysicsServer3D::area_get_space(RID p_area) const {
	RedotArea3D *area = area_owner.get_or_null(p_area);
	ERR_FAIL_NULL_V(area, RID());

	RedotSpace3D *space = area->get_space();
	if (!space) {
		return RID();
	}
	return space->get_self();
}

void RedotPhysicsServer3D::area_add_shape(RID p_area, RID p_shape, const Transform3D &p_transform, bool p_disabled) {
	RedotArea3D *area = area_owner.get_or_null(p_area);
	ERR_FAIL_NULL(area);

	RedotShape3D *shape = shape_owner.get_or_null(p_shape);
	ERR_FAIL_NULL(shape);

	area->add_shape(shape, p_transform, p_disabled);
}

void RedotPhysicsServer3D::area_set_shape(RID p_area, int p_shape_idx, RID p_shape) {
	RedotArea3D *area = area_owner.get_or_null(p_area);
	ERR_FAIL_NULL(area);

	RedotShape3D *shape = shape_owner.get_or_null(p_shape);
	ERR_FAIL_NULL(shape);
	ERR_FAIL_COND(!shape->is_configured());

	area->set_shape(p_shape_idx, shape);
}

void RedotPhysicsServer3D::area_set_shape_transform(RID p_area, int p_shape_idx, const Transform3D &p_transform) {
	RedotArea3D *area = area_owner.get_or_null(p_area);
	ERR_FAIL_NULL(area);

	area->set_shape_transform(p_shape_idx, p_transform);
}

int RedotPhysicsServer3D::area_get_shape_count(RID p_area) const {
	RedotArea3D *area = area_owner.get_or_null(p_area);
	ERR_FAIL_NULL_V(area, -1);

	return area->get_shape_count();
}

RID RedotPhysicsServer3D::area_get_shape(RID p_area, int p_shape_idx) const {
	RedotArea3D *area = area_owner.get_or_null(p_area);
	ERR_FAIL_NULL_V(area, RID());

	RedotShape3D *shape = area->get_shape(p_shape_idx);
	ERR_FAIL_NULL_V(shape, RID());

	return shape->get_self();
}

Transform3D RedotPhysicsServer3D::area_get_shape_transform(RID p_area, int p_shape_idx) const {
	RedotArea3D *area = area_owner.get_or_null(p_area);
	ERR_FAIL_NULL_V(area, Transform3D());

	return area->get_shape_transform(p_shape_idx);
}

void RedotPhysicsServer3D::area_remove_shape(RID p_area, int p_shape_idx) {
	RedotArea3D *area = area_owner.get_or_null(p_area);
	ERR_FAIL_NULL(area);

	area->remove_shape(p_shape_idx);
}

void RedotPhysicsServer3D::area_clear_shapes(RID p_area) {
	RedotArea3D *area = area_owner.get_or_null(p_area);
	ERR_FAIL_NULL(area);

	while (area->get_shape_count()) {
		area->remove_shape(0);
	}
}

void RedotPhysicsServer3D::area_set_shape_disabled(RID p_area, int p_shape_idx, bool p_disabled) {
	RedotArea3D *area = area_owner.get_or_null(p_area);
	ERR_FAIL_NULL(area);
	ERR_FAIL_INDEX(p_shape_idx, area->get_shape_count());
	FLUSH_QUERY_CHECK(area);
	area->set_shape_disabled(p_shape_idx, p_disabled);
}

void RedotPhysicsServer3D::area_attach_object_instance_id(RID p_area, ObjectID p_id) {
	if (space_owner.owns(p_area)) {
		RedotSpace3D *space = space_owner.get_or_null(p_area);
		p_area = space->get_default_area()->get_self();
	}
	RedotArea3D *area = area_owner.get_or_null(p_area);
	ERR_FAIL_NULL(area);
	area->set_instance_id(p_id);
}

ObjectID RedotPhysicsServer3D::area_get_object_instance_id(RID p_area) const {
	if (space_owner.owns(p_area)) {
		RedotSpace3D *space = space_owner.get_or_null(p_area);
		p_area = space->get_default_area()->get_self();
	}
	RedotArea3D *area = area_owner.get_or_null(p_area);
	ERR_FAIL_NULL_V(area, ObjectID());
	return area->get_instance_id();
}

void RedotPhysicsServer3D::area_set_param(RID p_area, AreaParameter p_param, const Variant &p_value) {
	if (space_owner.owns(p_area)) {
		RedotSpace3D *space = space_owner.get_or_null(p_area);
		p_area = space->get_default_area()->get_self();
	}
	RedotArea3D *area = area_owner.get_or_null(p_area);
	ERR_FAIL_NULL(area);
	area->set_param(p_param, p_value);
};

void RedotPhysicsServer3D::area_set_transform(RID p_area, const Transform3D &p_transform) {
	RedotArea3D *area = area_owner.get_or_null(p_area);
	ERR_FAIL_NULL(area);
	area->set_transform(p_transform);
};

Variant RedotPhysicsServer3D::area_get_param(RID p_area, AreaParameter p_param) const {
	if (space_owner.owns(p_area)) {
		RedotSpace3D *space = space_owner.get_or_null(p_area);
		p_area = space->get_default_area()->get_self();
	}
	RedotArea3D *area = area_owner.get_or_null(p_area);
	ERR_FAIL_NULL_V(area, Variant());

	return area->get_param(p_param);
};

Transform3D RedotPhysicsServer3D::area_get_transform(RID p_area) const {
	RedotArea3D *area = area_owner.get_or_null(p_area);
	ERR_FAIL_NULL_V(area, Transform3D());

	return area->get_transform();
};

void RedotPhysicsServer3D::area_set_collision_layer(RID p_area, uint32_t p_layer) {
	RedotArea3D *area = area_owner.get_or_null(p_area);
	ERR_FAIL_NULL(area);

	area->set_collision_layer(p_layer);
}

uint32_t RedotPhysicsServer3D::area_get_collision_layer(RID p_area) const {
	RedotArea3D *area = area_owner.get_or_null(p_area);
	ERR_FAIL_NULL_V(area, 0);

	return area->get_collision_layer();
}

void RedotPhysicsServer3D::area_set_collision_mask(RID p_area, uint32_t p_mask) {
	RedotArea3D *area = area_owner.get_or_null(p_area);
	ERR_FAIL_NULL(area);

	area->set_collision_mask(p_mask);
}

uint32_t RedotPhysicsServer3D::area_get_collision_mask(RID p_area) const {
	RedotArea3D *area = area_owner.get_or_null(p_area);
	ERR_FAIL_NULL_V(area, 0);

	return area->get_collision_mask();
}

void RedotPhysicsServer3D::area_set_monitorable(RID p_area, bool p_monitorable) {
	RedotArea3D *area = area_owner.get_or_null(p_area);
	ERR_FAIL_NULL(area);
	FLUSH_QUERY_CHECK(area);

	area->set_monitorable(p_monitorable);
}

void RedotPhysicsServer3D::area_set_monitor_callback(RID p_area, const Callable &p_callback) {
	RedotArea3D *area = area_owner.get_or_null(p_area);
	ERR_FAIL_NULL(area);

	area->set_monitor_callback(p_callback.is_valid() ? p_callback : Callable());
}

void RedotPhysicsServer3D::area_set_ray_pickable(RID p_area, bool p_enable) {
	RedotArea3D *area = area_owner.get_or_null(p_area);
	ERR_FAIL_NULL(area);

	area->set_ray_pickable(p_enable);
}

void RedotPhysicsServer3D::area_set_area_monitor_callback(RID p_area, const Callable &p_callback) {
	RedotArea3D *area = area_owner.get_or_null(p_area);
	ERR_FAIL_NULL(area);

	area->set_area_monitor_callback(p_callback.is_valid() ? p_callback : Callable());
}

/* BODY API */

RID RedotPhysicsServer3D::body_create() {
	RedotBody3D *body = memnew(RedotBody3D);
	RID rid = body_owner.make_rid(body);
	body->set_self(rid);
	return rid;
};

void RedotPhysicsServer3D::body_set_space(RID p_body, RID p_space) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	RedotSpace3D *space = nullptr;
	if (p_space.is_valid()) {
		space = space_owner.get_or_null(p_space);
		ERR_FAIL_NULL(space);
	}

	if (body->get_space() == space) {
		return; //pointless
	}

	body->clear_constraint_map();
	body->set_space(space);
};

RID RedotPhysicsServer3D::body_get_space(RID p_body) const {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(body, RID());

	RedotSpace3D *space = body->get_space();
	if (!space) {
		return RID();
	}
	return space->get_self();
};

void RedotPhysicsServer3D::body_set_mode(RID p_body, BodyMode p_mode) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	body->set_mode(p_mode);
};

PhysicsServer3D::BodyMode RedotPhysicsServer3D::body_get_mode(RID p_body) const {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(body, BODY_MODE_STATIC);

	return body->get_mode();
};

void RedotPhysicsServer3D::body_add_shape(RID p_body, RID p_shape, const Transform3D &p_transform, bool p_disabled) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	RedotShape3D *shape = shape_owner.get_or_null(p_shape);
	ERR_FAIL_NULL(shape);

	body->add_shape(shape, p_transform, p_disabled);
}

void RedotPhysicsServer3D::body_set_shape(RID p_body, int p_shape_idx, RID p_shape) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	RedotShape3D *shape = shape_owner.get_or_null(p_shape);
	ERR_FAIL_NULL(shape);
	ERR_FAIL_COND(!shape->is_configured());

	body->set_shape(p_shape_idx, shape);
}
void RedotPhysicsServer3D::body_set_shape_transform(RID p_body, int p_shape_idx, const Transform3D &p_transform) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	body->set_shape_transform(p_shape_idx, p_transform);
}

int RedotPhysicsServer3D::body_get_shape_count(RID p_body) const {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(body, -1);

	return body->get_shape_count();
}

RID RedotPhysicsServer3D::body_get_shape(RID p_body, int p_shape_idx) const {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(body, RID());

	RedotShape3D *shape = body->get_shape(p_shape_idx);
	ERR_FAIL_NULL_V(shape, RID());

	return shape->get_self();
}

void RedotPhysicsServer3D::body_set_shape_disabled(RID p_body, int p_shape_idx, bool p_disabled) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);
	ERR_FAIL_INDEX(p_shape_idx, body->get_shape_count());
	FLUSH_QUERY_CHECK(body);

	body->set_shape_disabled(p_shape_idx, p_disabled);
}

Transform3D RedotPhysicsServer3D::body_get_shape_transform(RID p_body, int p_shape_idx) const {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(body, Transform3D());

	return body->get_shape_transform(p_shape_idx);
}

void RedotPhysicsServer3D::body_remove_shape(RID p_body, int p_shape_idx) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	body->remove_shape(p_shape_idx);
}

void RedotPhysicsServer3D::body_clear_shapes(RID p_body) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	while (body->get_shape_count()) {
		body->remove_shape(0);
	}
}

void RedotPhysicsServer3D::body_set_enable_continuous_collision_detection(RID p_body, bool p_enable) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	body->set_continuous_collision_detection(p_enable);
}

bool RedotPhysicsServer3D::body_is_continuous_collision_detection_enabled(RID p_body) const {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(body, false);

	return body->is_continuous_collision_detection_enabled();
}

void RedotPhysicsServer3D::body_set_collision_layer(RID p_body, uint32_t p_layer) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	body->set_collision_layer(p_layer);
}

uint32_t RedotPhysicsServer3D::body_get_collision_layer(RID p_body) const {
	const RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(body, 0);

	return body->get_collision_layer();
}

void RedotPhysicsServer3D::body_set_collision_mask(RID p_body, uint32_t p_mask) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	body->set_collision_mask(p_mask);
}

uint32_t RedotPhysicsServer3D::body_get_collision_mask(RID p_body) const {
	const RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(body, 0);

	return body->get_collision_mask();
}

void RedotPhysicsServer3D::body_set_collision_priority(RID p_body, real_t p_priority) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	body->set_collision_priority(p_priority);
}

real_t RedotPhysicsServer3D::body_get_collision_priority(RID p_body) const {
	const RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(body, 0);

	return body->get_collision_priority();
}

void RedotPhysicsServer3D::body_attach_object_instance_id(RID p_body, ObjectID p_id) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	if (body) {
		body->set_instance_id(p_id);
		return;
	}

	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	if (soft_body) {
		soft_body->set_instance_id(p_id);
		return;
	}

	ERR_FAIL_MSG("Invalid ID.");
}

ObjectID RedotPhysicsServer3D::body_get_object_instance_id(RID p_body) const {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(body, ObjectID());

	return body->get_instance_id();
}

void RedotPhysicsServer3D::body_set_user_flags(RID p_body, uint32_t p_flags) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);
}

uint32_t RedotPhysicsServer3D::body_get_user_flags(RID p_body) const {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(body, 0);

	return 0;
}

void RedotPhysicsServer3D::body_set_param(RID p_body, BodyParameter p_param, const Variant &p_value) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	body->set_param(p_param, p_value);
}

Variant RedotPhysicsServer3D::body_get_param(RID p_body, BodyParameter p_param) const {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(body, 0);

	return body->get_param(p_param);
}

void RedotPhysicsServer3D::body_reset_mass_properties(RID p_body) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	return body->reset_mass_properties();
}

void RedotPhysicsServer3D::body_set_state(RID p_body, BodyState p_state, const Variant &p_variant) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	body->set_state(p_state, p_variant);
}

Variant RedotPhysicsServer3D::body_get_state(RID p_body, BodyState p_state) const {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(body, Variant());

	return body->get_state(p_state);
}

void RedotPhysicsServer3D::body_apply_central_impulse(RID p_body, const Vector3 &p_impulse) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	_update_shapes();

	body->apply_central_impulse(p_impulse);
	body->wakeup();
}

void RedotPhysicsServer3D::body_apply_impulse(RID p_body, const Vector3 &p_impulse, const Vector3 &p_position) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	_update_shapes();

	body->apply_impulse(p_impulse, p_position);
	body->wakeup();
}

void RedotPhysicsServer3D::body_apply_torque_impulse(RID p_body, const Vector3 &p_impulse) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	_update_shapes();

	body->apply_torque_impulse(p_impulse);
	body->wakeup();
}

void RedotPhysicsServer3D::body_apply_central_force(RID p_body, const Vector3 &p_force) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	body->apply_central_force(p_force);
	body->wakeup();
}

void RedotPhysicsServer3D::body_apply_force(RID p_body, const Vector3 &p_force, const Vector3 &p_position) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	body->apply_force(p_force, p_position);
	body->wakeup();
}

void RedotPhysicsServer3D::body_apply_torque(RID p_body, const Vector3 &p_torque) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	body->apply_torque(p_torque);
	body->wakeup();
}

void RedotPhysicsServer3D::body_add_constant_central_force(RID p_body, const Vector3 &p_force) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	body->add_constant_central_force(p_force);
	body->wakeup();
}

void RedotPhysicsServer3D::body_add_constant_force(RID p_body, const Vector3 &p_force, const Vector3 &p_position) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	body->add_constant_force(p_force, p_position);
	body->wakeup();
}

void RedotPhysicsServer3D::body_add_constant_torque(RID p_body, const Vector3 &p_torque) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	body->add_constant_torque(p_torque);
	body->wakeup();
}

void RedotPhysicsServer3D::body_set_constant_force(RID p_body, const Vector3 &p_force) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	body->set_constant_force(p_force);
	if (!p_force.is_zero_approx()) {
		body->wakeup();
	}
}

Vector3 RedotPhysicsServer3D::body_get_constant_force(RID p_body) const {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(body, Vector3());
	return body->get_constant_force();
}

void RedotPhysicsServer3D::body_set_constant_torque(RID p_body, const Vector3 &p_torque) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	body->set_constant_torque(p_torque);
	if (!p_torque.is_zero_approx()) {
		body->wakeup();
	}
}

Vector3 RedotPhysicsServer3D::body_get_constant_torque(RID p_body) const {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(body, Vector3());

	return body->get_constant_torque();
}

void RedotPhysicsServer3D::body_set_axis_velocity(RID p_body, const Vector3 &p_axis_velocity) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	_update_shapes();

	Vector3 v = body->get_linear_velocity();
	Vector3 axis = p_axis_velocity.normalized();
	v -= axis * axis.dot(v);
	v += p_axis_velocity;
	body->set_linear_velocity(v);
	body->wakeup();
}

void RedotPhysicsServer3D::body_set_axis_lock(RID p_body, BodyAxis p_axis, bool p_lock) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	body->set_axis_lock(p_axis, p_lock);
	body->wakeup();
}

bool RedotPhysicsServer3D::body_is_axis_locked(RID p_body, BodyAxis p_axis) const {
	const RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(body, 0);
	return body->is_axis_locked(p_axis);
}

void RedotPhysicsServer3D::body_add_collision_exception(RID p_body, RID p_body_b) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	body->add_exception(p_body_b);
	body->wakeup();
};

void RedotPhysicsServer3D::body_remove_collision_exception(RID p_body, RID p_body_b) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	body->remove_exception(p_body_b);
	body->wakeup();
};

void RedotPhysicsServer3D::body_get_collision_exceptions(RID p_body, List<RID> *p_exceptions) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	for (int i = 0; i < body->get_exceptions().size(); i++) {
		p_exceptions->push_back(body->get_exceptions()[i]);
	}
};

void RedotPhysicsServer3D::body_set_contacts_reported_depth_threshold(RID p_body, real_t p_threshold) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);
};

real_t RedotPhysicsServer3D::body_get_contacts_reported_depth_threshold(RID p_body) const {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(body, 0);
	return 0;
};

void RedotPhysicsServer3D::body_set_omit_force_integration(RID p_body, bool p_omit) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);

	body->set_omit_force_integration(p_omit);
};

bool RedotPhysicsServer3D::body_is_omitting_force_integration(RID p_body) const {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(body, false);
	return body->get_omit_force_integration();
};

void RedotPhysicsServer3D::body_set_max_contacts_reported(RID p_body, int p_contacts) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);
	body->set_max_contacts_reported(p_contacts);
}

int RedotPhysicsServer3D::body_get_max_contacts_reported(RID p_body) const {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(body, -1);
	return body->get_max_contacts_reported();
}

void RedotPhysicsServer3D::body_set_state_sync_callback(RID p_body, const Callable &p_callable) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);
	body->set_state_sync_callback(p_callable);
}

void RedotPhysicsServer3D::body_set_force_integration_callback(RID p_body, const Callable &p_callable, const Variant &p_udata) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);
	body->set_force_integration_callback(p_callable, p_udata);
}

void RedotPhysicsServer3D::body_set_ray_pickable(RID p_body, bool p_enable) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(body);
	body->set_ray_pickable(p_enable);
}

bool RedotPhysicsServer3D::body_test_motion(RID p_body, const MotionParameters &p_parameters, MotionResult *r_result) {
	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(body, false);
	ERR_FAIL_NULL_V(body->get_space(), false);
	ERR_FAIL_COND_V(body->get_space()->is_locked(), false);

	_update_shapes();

	return body->get_space()->test_body_motion(body, p_parameters, r_result);
}

PhysicsDirectBodyState3D *RedotPhysicsServer3D::body_get_direct_state(RID p_body) {
	ERR_FAIL_COND_V_MSG((using_threads && !doing_sync), nullptr, "Body state is inaccessible right now, wait for iteration or physics process notification.");

	if (!body_owner.owns(p_body)) {
		return nullptr;
	}

	RedotBody3D *body = body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(body, nullptr);

	if (!body->get_space()) {
		return nullptr;
	}

	ERR_FAIL_COND_V_MSG(body->get_space()->is_locked(), nullptr, "Body state is inaccessible right now, wait for iteration or physics process notification.");

	return body->get_direct_state();
}

/* SOFT BODY */

RID RedotPhysicsServer3D::soft_body_create() {
	RedotSoftBody3D *soft_body = memnew(RedotSoftBody3D);
	RID rid = soft_body_owner.make_rid(soft_body);
	soft_body->set_self(rid);
	return rid;
}

void RedotPhysicsServer3D::soft_body_update_rendering_server(RID p_body, PhysicsServer3DRenderingServerHandler *p_rendering_server_handler) {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(soft_body);

	soft_body->update_rendering_server(p_rendering_server_handler);
}

void RedotPhysicsServer3D::soft_body_set_space(RID p_body, RID p_space) {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(soft_body);

	RedotSpace3D *space = nullptr;
	if (p_space.is_valid()) {
		space = space_owner.get_or_null(p_space);
		ERR_FAIL_NULL(space);
	}

	if (soft_body->get_space() == space) {
		return;
	}

	soft_body->set_space(space);
}

RID RedotPhysicsServer3D::soft_body_get_space(RID p_body) const {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(soft_body, RID());

	RedotSpace3D *space = soft_body->get_space();
	if (!space) {
		return RID();
	}
	return space->get_self();
}

void RedotPhysicsServer3D::soft_body_set_collision_layer(RID p_body, uint32_t p_layer) {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(soft_body);

	soft_body->set_collision_layer(p_layer);
}

uint32_t RedotPhysicsServer3D::soft_body_get_collision_layer(RID p_body) const {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(soft_body, 0);

	return soft_body->get_collision_layer();
}

void RedotPhysicsServer3D::soft_body_set_collision_mask(RID p_body, uint32_t p_mask) {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(soft_body);

	soft_body->set_collision_mask(p_mask);
}

uint32_t RedotPhysicsServer3D::soft_body_get_collision_mask(RID p_body) const {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(soft_body, 0);

	return soft_body->get_collision_mask();
}

void RedotPhysicsServer3D::soft_body_add_collision_exception(RID p_body, RID p_body_b) {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(soft_body);

	soft_body->add_exception(p_body_b);
}

void RedotPhysicsServer3D::soft_body_remove_collision_exception(RID p_body, RID p_body_b) {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(soft_body);

	soft_body->remove_exception(p_body_b);
}

void RedotPhysicsServer3D::soft_body_get_collision_exceptions(RID p_body, List<RID> *p_exceptions) {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(soft_body);

	for (int i = 0; i < soft_body->get_exceptions().size(); i++) {
		p_exceptions->push_back(soft_body->get_exceptions()[i]);
	}
}

void RedotPhysicsServer3D::soft_body_set_state(RID p_body, BodyState p_state, const Variant &p_variant) {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(soft_body);

	soft_body->set_state(p_state, p_variant);
}

Variant RedotPhysicsServer3D::soft_body_get_state(RID p_body, BodyState p_state) const {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(soft_body, Variant());

	return soft_body->get_state(p_state);
}

void RedotPhysicsServer3D::soft_body_set_transform(RID p_body, const Transform3D &p_transform) {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(soft_body);

	soft_body->set_state(BODY_STATE_TRANSFORM, p_transform);
}

void RedotPhysicsServer3D::soft_body_set_ray_pickable(RID p_body, bool p_enable) {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(soft_body);

	soft_body->set_ray_pickable(p_enable);
}

void RedotPhysicsServer3D::soft_body_set_simulation_precision(RID p_body, int p_simulation_precision) {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(soft_body);

	soft_body->set_iteration_count(p_simulation_precision);
}

int RedotPhysicsServer3D::soft_body_get_simulation_precision(RID p_body) const {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(soft_body, 0.f);

	return soft_body->get_iteration_count();
}

void RedotPhysicsServer3D::soft_body_set_total_mass(RID p_body, real_t p_total_mass) {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(soft_body);

	soft_body->set_total_mass(p_total_mass);
}

real_t RedotPhysicsServer3D::soft_body_get_total_mass(RID p_body) const {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(soft_body, 0.f);

	return soft_body->get_total_mass();
}

void RedotPhysicsServer3D::soft_body_set_linear_stiffness(RID p_body, real_t p_stiffness) {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(soft_body);

	soft_body->set_linear_stiffness(p_stiffness);
}

real_t RedotPhysicsServer3D::soft_body_get_linear_stiffness(RID p_body) const {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(soft_body, 0.f);

	return soft_body->get_linear_stiffness();
}

void RedotPhysicsServer3D::soft_body_set_pressure_coefficient(RID p_body, real_t p_pressure_coefficient) {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(soft_body);

	soft_body->set_pressure_coefficient(p_pressure_coefficient);
}

real_t RedotPhysicsServer3D::soft_body_get_pressure_coefficient(RID p_body) const {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(soft_body, 0.f);

	return soft_body->get_pressure_coefficient();
}

void RedotPhysicsServer3D::soft_body_set_damping_coefficient(RID p_body, real_t p_damping_coefficient) {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(soft_body);

	soft_body->set_damping_coefficient(p_damping_coefficient);
}

real_t RedotPhysicsServer3D::soft_body_get_damping_coefficient(RID p_body) const {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(soft_body, 0.f);

	return soft_body->get_damping_coefficient();
}

void RedotPhysicsServer3D::soft_body_set_drag_coefficient(RID p_body, real_t p_drag_coefficient) {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(soft_body);

	soft_body->set_drag_coefficient(p_drag_coefficient);
}

real_t RedotPhysicsServer3D::soft_body_get_drag_coefficient(RID p_body) const {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(soft_body, 0.f);

	return soft_body->get_drag_coefficient();
}

void RedotPhysicsServer3D::soft_body_set_mesh(RID p_body, RID p_mesh) {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(soft_body);

	soft_body->set_mesh(p_mesh);
}

AABB RedotPhysicsServer3D::soft_body_get_bounds(RID p_body) const {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(soft_body, AABB());

	return soft_body->get_bounds();
}

void RedotPhysicsServer3D::soft_body_move_point(RID p_body, int p_point_index, const Vector3 &p_global_position) {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(soft_body);

	soft_body->set_vertex_position(p_point_index, p_global_position);
}

Vector3 RedotPhysicsServer3D::soft_body_get_point_global_position(RID p_body, int p_point_index) const {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(soft_body, Vector3());

	return soft_body->get_vertex_position(p_point_index);
}

void RedotPhysicsServer3D::soft_body_remove_all_pinned_points(RID p_body) {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(soft_body);

	soft_body->unpin_all_vertices();
}

void RedotPhysicsServer3D::soft_body_pin_point(RID p_body, int p_point_index, bool p_pin) {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL(soft_body);

	if (p_pin) {
		soft_body->pin_vertex(p_point_index);
	} else {
		soft_body->unpin_vertex(p_point_index);
	}
}

bool RedotPhysicsServer3D::soft_body_is_point_pinned(RID p_body, int p_point_index) const {
	RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_body);
	ERR_FAIL_NULL_V(soft_body, false);

	return soft_body->is_vertex_pinned(p_point_index);
}

/* JOINT API */

RID RedotPhysicsServer3D::joint_create() {
	RedotJoint3D *joint = memnew(RedotJoint3D);
	RID rid = joint_owner.make_rid(joint);
	joint->set_self(rid);
	return rid;
}

void RedotPhysicsServer3D::joint_clear(RID p_joint) {
	RedotJoint3D *joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL(joint);
	if (joint->get_type() != JOINT_TYPE_MAX) {
		RedotJoint3D *empty_joint = memnew(RedotJoint3D);
		empty_joint->copy_settings_from(joint);

		joint_owner.replace(p_joint, empty_joint);
		memdelete(joint);
	}
}

void RedotPhysicsServer3D::joint_make_pin(RID p_joint, RID p_body_A, const Vector3 &p_local_A, RID p_body_B, const Vector3 &p_local_B) {
	RedotBody3D *body_A = body_owner.get_or_null(p_body_A);
	ERR_FAIL_NULL(body_A);

	if (!p_body_B.is_valid()) {
		ERR_FAIL_NULL(body_A->get_space());
		p_body_B = body_A->get_space()->get_static_global_body();
	}

	RedotBody3D *body_B = body_owner.get_or_null(p_body_B);
	ERR_FAIL_NULL(body_B);

	ERR_FAIL_COND(body_A == body_B);

	RedotJoint3D *prev_joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL(prev_joint);

	RedotJoint3D *joint = memnew(RedotPinJoint3D(body_A, p_local_A, body_B, p_local_B));

	joint->copy_settings_from(prev_joint);
	joint_owner.replace(p_joint, joint);
	memdelete(prev_joint);
}

void RedotPhysicsServer3D::pin_joint_set_param(RID p_joint, PinJointParam p_param, real_t p_value) {
	RedotJoint3D *joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL(joint);
	ERR_FAIL_COND(joint->get_type() != JOINT_TYPE_PIN);
	RedotPinJoint3D *pin_joint = static_cast<RedotPinJoint3D *>(joint);
	pin_joint->set_param(p_param, p_value);
}

real_t RedotPhysicsServer3D::pin_joint_get_param(RID p_joint, PinJointParam p_param) const {
	RedotJoint3D *joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL_V(joint, 0);
	ERR_FAIL_COND_V(joint->get_type() != JOINT_TYPE_PIN, 0);
	RedotPinJoint3D *pin_joint = static_cast<RedotPinJoint3D *>(joint);
	return pin_joint->get_param(p_param);
}

void RedotPhysicsServer3D::pin_joint_set_local_a(RID p_joint, const Vector3 &p_A) {
	RedotJoint3D *joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL(joint);
	ERR_FAIL_COND(joint->get_type() != JOINT_TYPE_PIN);
	RedotPinJoint3D *pin_joint = static_cast<RedotPinJoint3D *>(joint);
	pin_joint->set_pos_a(p_A);
}

Vector3 RedotPhysicsServer3D::pin_joint_get_local_a(RID p_joint) const {
	RedotJoint3D *joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL_V(joint, Vector3());
	ERR_FAIL_COND_V(joint->get_type() != JOINT_TYPE_PIN, Vector3());
	RedotPinJoint3D *pin_joint = static_cast<RedotPinJoint3D *>(joint);
	return pin_joint->get_position_a();
}

void RedotPhysicsServer3D::pin_joint_set_local_b(RID p_joint, const Vector3 &p_B) {
	RedotJoint3D *joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL(joint);
	ERR_FAIL_COND(joint->get_type() != JOINT_TYPE_PIN);
	RedotPinJoint3D *pin_joint = static_cast<RedotPinJoint3D *>(joint);
	pin_joint->set_pos_b(p_B);
}

Vector3 RedotPhysicsServer3D::pin_joint_get_local_b(RID p_joint) const {
	RedotJoint3D *joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL_V(joint, Vector3());
	ERR_FAIL_COND_V(joint->get_type() != JOINT_TYPE_PIN, Vector3());
	RedotPinJoint3D *pin_joint = static_cast<RedotPinJoint3D *>(joint);
	return pin_joint->get_position_b();
}

void RedotPhysicsServer3D::joint_make_hinge(RID p_joint, RID p_body_A, const Transform3D &p_frame_A, RID p_body_B, const Transform3D &p_frame_B) {
	RedotBody3D *body_A = body_owner.get_or_null(p_body_A);
	ERR_FAIL_NULL(body_A);

	if (!p_body_B.is_valid()) {
		ERR_FAIL_NULL(body_A->get_space());
		p_body_B = body_A->get_space()->get_static_global_body();
	}

	RedotBody3D *body_B = body_owner.get_or_null(p_body_B);
	ERR_FAIL_NULL(body_B);

	ERR_FAIL_COND(body_A == body_B);

	RedotJoint3D *prev_joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL(prev_joint);

	RedotJoint3D *joint = memnew(RedotHingeJoint3D(body_A, body_B, p_frame_A, p_frame_B));

	joint->copy_settings_from(prev_joint);
	joint_owner.replace(p_joint, joint);
	memdelete(prev_joint);
}

void RedotPhysicsServer3D::joint_make_hinge_simple(RID p_joint, RID p_body_A, const Vector3 &p_pivot_A, const Vector3 &p_axis_A, RID p_body_B, const Vector3 &p_pivot_B, const Vector3 &p_axis_B) {
	RedotBody3D *body_A = body_owner.get_or_null(p_body_A);
	ERR_FAIL_NULL(body_A);

	if (!p_body_B.is_valid()) {
		ERR_FAIL_NULL(body_A->get_space());
		p_body_B = body_A->get_space()->get_static_global_body();
	}

	RedotBody3D *body_B = body_owner.get_or_null(p_body_B);
	ERR_FAIL_NULL(body_B);

	ERR_FAIL_COND(body_A == body_B);

	RedotJoint3D *prev_joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL(prev_joint);

	RedotJoint3D *joint = memnew(RedotHingeJoint3D(body_A, body_B, p_pivot_A, p_pivot_B, p_axis_A, p_axis_B));

	joint->copy_settings_from(prev_joint);
	joint_owner.replace(p_joint, joint);
	memdelete(prev_joint);
}

void RedotPhysicsServer3D::hinge_joint_set_param(RID p_joint, HingeJointParam p_param, real_t p_value) {
	RedotJoint3D *joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL(joint);
	ERR_FAIL_COND(joint->get_type() != JOINT_TYPE_HINGE);
	RedotHingeJoint3D *hinge_joint = static_cast<RedotHingeJoint3D *>(joint);
	hinge_joint->set_param(p_param, p_value);
}

real_t RedotPhysicsServer3D::hinge_joint_get_param(RID p_joint, HingeJointParam p_param) const {
	RedotJoint3D *joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL_V(joint, 0);
	ERR_FAIL_COND_V(joint->get_type() != JOINT_TYPE_HINGE, 0);
	RedotHingeJoint3D *hinge_joint = static_cast<RedotHingeJoint3D *>(joint);
	return hinge_joint->get_param(p_param);
}

void RedotPhysicsServer3D::hinge_joint_set_flag(RID p_joint, HingeJointFlag p_flag, bool p_enabled) {
	RedotJoint3D *joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL(joint);
	ERR_FAIL_COND(joint->get_type() != JOINT_TYPE_HINGE);
	RedotHingeJoint3D *hinge_joint = static_cast<RedotHingeJoint3D *>(joint);
	hinge_joint->set_flag(p_flag, p_enabled);
}

bool RedotPhysicsServer3D::hinge_joint_get_flag(RID p_joint, HingeJointFlag p_flag) const {
	RedotJoint3D *joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL_V(joint, false);
	ERR_FAIL_COND_V(joint->get_type() != JOINT_TYPE_HINGE, false);
	RedotHingeJoint3D *hinge_joint = static_cast<RedotHingeJoint3D *>(joint);
	return hinge_joint->get_flag(p_flag);
}

void RedotPhysicsServer3D::joint_set_solver_priority(RID p_joint, int p_priority) {
	RedotJoint3D *joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL(joint);
	joint->set_priority(p_priority);
}

int RedotPhysicsServer3D::joint_get_solver_priority(RID p_joint) const {
	RedotJoint3D *joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL_V(joint, 0);
	return joint->get_priority();
}

void RedotPhysicsServer3D::joint_disable_collisions_between_bodies(RID p_joint, bool p_disable) {
	RedotJoint3D *joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL(joint);

	joint->disable_collisions_between_bodies(p_disable);

	if (2 == joint->get_body_count()) {
		RedotBody3D *body_a = *joint->get_body_ptr();
		RedotBody3D *body_b = *(joint->get_body_ptr() + 1);

		if (p_disable) {
			body_add_collision_exception(body_a->get_self(), body_b->get_self());
			body_add_collision_exception(body_b->get_self(), body_a->get_self());
		} else {
			body_remove_collision_exception(body_a->get_self(), body_b->get_self());
			body_remove_collision_exception(body_b->get_self(), body_a->get_self());
		}
	}
}

bool RedotPhysicsServer3D::joint_is_disabled_collisions_between_bodies(RID p_joint) const {
	RedotJoint3D *joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL_V(joint, true);

	return joint->is_disabled_collisions_between_bodies();
}

RedotPhysicsServer3D::JointType RedotPhysicsServer3D::joint_get_type(RID p_joint) const {
	RedotJoint3D *joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL_V(joint, JOINT_TYPE_PIN);
	return joint->get_type();
}

void RedotPhysicsServer3D::joint_make_slider(RID p_joint, RID p_body_A, const Transform3D &p_local_frame_A, RID p_body_B, const Transform3D &p_local_frame_B) {
	RedotBody3D *body_A = body_owner.get_or_null(p_body_A);
	ERR_FAIL_NULL(body_A);

	if (!p_body_B.is_valid()) {
		ERR_FAIL_NULL(body_A->get_space());
		p_body_B = body_A->get_space()->get_static_global_body();
	}

	RedotBody3D *body_B = body_owner.get_or_null(p_body_B);
	ERR_FAIL_NULL(body_B);

	ERR_FAIL_COND(body_A == body_B);

	RedotJoint3D *prev_joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL(prev_joint);

	RedotJoint3D *joint = memnew(RedotSliderJoint3D(body_A, body_B, p_local_frame_A, p_local_frame_B));

	joint->copy_settings_from(prev_joint);
	joint_owner.replace(p_joint, joint);
	memdelete(prev_joint);
}

void RedotPhysicsServer3D::slider_joint_set_param(RID p_joint, SliderJointParam p_param, real_t p_value) {
	RedotJoint3D *joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL(joint);
	ERR_FAIL_COND(joint->get_type() != JOINT_TYPE_SLIDER);
	RedotSliderJoint3D *slider_joint = static_cast<RedotSliderJoint3D *>(joint);
	slider_joint->set_param(p_param, p_value);
}

real_t RedotPhysicsServer3D::slider_joint_get_param(RID p_joint, SliderJointParam p_param) const {
	RedotJoint3D *joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL_V(joint, 0);
	ERR_FAIL_COND_V(joint->get_type() != JOINT_TYPE_CONE_TWIST, 0);
	RedotSliderJoint3D *slider_joint = static_cast<RedotSliderJoint3D *>(joint);
	return slider_joint->get_param(p_param);
}

void RedotPhysicsServer3D::joint_make_cone_twist(RID p_joint, RID p_body_A, const Transform3D &p_local_frame_A, RID p_body_B, const Transform3D &p_local_frame_B) {
	RedotBody3D *body_A = body_owner.get_or_null(p_body_A);
	ERR_FAIL_NULL(body_A);

	if (!p_body_B.is_valid()) {
		ERR_FAIL_NULL(body_A->get_space());
		p_body_B = body_A->get_space()->get_static_global_body();
	}

	RedotBody3D *body_B = body_owner.get_or_null(p_body_B);
	ERR_FAIL_NULL(body_B);

	ERR_FAIL_COND(body_A == body_B);

	RedotJoint3D *prev_joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL(prev_joint);

	RedotJoint3D *joint = memnew(RedotConeTwistJoint3D(body_A, body_B, p_local_frame_A, p_local_frame_B));

	joint->copy_settings_from(prev_joint);
	joint_owner.replace(p_joint, joint);
	memdelete(prev_joint);
}

void RedotPhysicsServer3D::cone_twist_joint_set_param(RID p_joint, ConeTwistJointParam p_param, real_t p_value) {
	RedotJoint3D *joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL(joint);
	ERR_FAIL_COND(joint->get_type() != JOINT_TYPE_CONE_TWIST);
	RedotConeTwistJoint3D *cone_twist_joint = static_cast<RedotConeTwistJoint3D *>(joint);
	cone_twist_joint->set_param(p_param, p_value);
}

real_t RedotPhysicsServer3D::cone_twist_joint_get_param(RID p_joint, ConeTwistJointParam p_param) const {
	RedotJoint3D *joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL_V(joint, 0);
	ERR_FAIL_COND_V(joint->get_type() != JOINT_TYPE_CONE_TWIST, 0);
	RedotConeTwistJoint3D *cone_twist_joint = static_cast<RedotConeTwistJoint3D *>(joint);
	return cone_twist_joint->get_param(p_param);
}

void RedotPhysicsServer3D::joint_make_generic_6dof(RID p_joint, RID p_body_A, const Transform3D &p_local_frame_A, RID p_body_B, const Transform3D &p_local_frame_B) {
	RedotBody3D *body_A = body_owner.get_or_null(p_body_A);
	ERR_FAIL_NULL(body_A);

	if (!p_body_B.is_valid()) {
		ERR_FAIL_NULL(body_A->get_space());
		p_body_B = body_A->get_space()->get_static_global_body();
	}

	RedotBody3D *body_B = body_owner.get_or_null(p_body_B);
	ERR_FAIL_NULL(body_B);

	ERR_FAIL_COND(body_A == body_B);

	RedotJoint3D *prev_joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL(prev_joint);

	RedotJoint3D *joint = memnew(RedotGeneric6DOFJoint3D(body_A, body_B, p_local_frame_A, p_local_frame_B, true));

	joint->copy_settings_from(prev_joint);
	joint_owner.replace(p_joint, joint);
	memdelete(prev_joint);
}

void RedotPhysicsServer3D::generic_6dof_joint_set_param(RID p_joint, Vector3::Axis p_axis, G6DOFJointAxisParam p_param, real_t p_value) {
	RedotJoint3D *joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL(joint);
	ERR_FAIL_COND(joint->get_type() != JOINT_TYPE_6DOF);
	RedotGeneric6DOFJoint3D *generic_6dof_joint = static_cast<RedotGeneric6DOFJoint3D *>(joint);
	generic_6dof_joint->set_param(p_axis, p_param, p_value);
}

real_t RedotPhysicsServer3D::generic_6dof_joint_get_param(RID p_joint, Vector3::Axis p_axis, G6DOFJointAxisParam p_param) const {
	RedotJoint3D *joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL_V(joint, 0);
	ERR_FAIL_COND_V(joint->get_type() != JOINT_TYPE_6DOF, 0);
	RedotGeneric6DOFJoint3D *generic_6dof_joint = static_cast<RedotGeneric6DOFJoint3D *>(joint);
	return generic_6dof_joint->get_param(p_axis, p_param);
}

void RedotPhysicsServer3D::generic_6dof_joint_set_flag(RID p_joint, Vector3::Axis p_axis, G6DOFJointAxisFlag p_flag, bool p_enable) {
	RedotJoint3D *joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL(joint);
	ERR_FAIL_COND(joint->get_type() != JOINT_TYPE_6DOF);
	RedotGeneric6DOFJoint3D *generic_6dof_joint = static_cast<RedotGeneric6DOFJoint3D *>(joint);
	generic_6dof_joint->set_flag(p_axis, p_flag, p_enable);
}

bool RedotPhysicsServer3D::generic_6dof_joint_get_flag(RID p_joint, Vector3::Axis p_axis, G6DOFJointAxisFlag p_flag) const {
	RedotJoint3D *joint = joint_owner.get_or_null(p_joint);
	ERR_FAIL_NULL_V(joint, false);
	ERR_FAIL_COND_V(joint->get_type() != JOINT_TYPE_6DOF, false);
	RedotGeneric6DOFJoint3D *generic_6dof_joint = static_cast<RedotGeneric6DOFJoint3D *>(joint);
	return generic_6dof_joint->get_flag(p_axis, p_flag);
}

void RedotPhysicsServer3D::free(RID p_rid) {
	_update_shapes(); //just in case

	if (shape_owner.owns(p_rid)) {
		RedotShape3D *shape = shape_owner.get_or_null(p_rid);

		while (shape->get_owners().size()) {
			RedotShapeOwner3D *so = shape->get_owners().begin()->key;
			so->remove_shape(shape);
		}

		shape_owner.free(p_rid);
		memdelete(shape);
	} else if (body_owner.owns(p_rid)) {
		RedotBody3D *body = body_owner.get_or_null(p_rid);

		body->set_space(nullptr);

		while (body->get_shape_count()) {
			body->remove_shape(0);
		}

		body_owner.free(p_rid);
		memdelete(body);
	} else if (soft_body_owner.owns(p_rid)) {
		RedotSoftBody3D *soft_body = soft_body_owner.get_or_null(p_rid);

		soft_body->set_space(nullptr);

		soft_body_owner.free(p_rid);
		memdelete(soft_body);
	} else if (area_owner.owns(p_rid)) {
		RedotArea3D *area = area_owner.get_or_null(p_rid);

		area->set_space(nullptr);

		while (area->get_shape_count()) {
			area->remove_shape(0);
		}

		area_owner.free(p_rid);
		memdelete(area);
	} else if (space_owner.owns(p_rid)) {
		RedotSpace3D *space = space_owner.get_or_null(p_rid);

		while (space->get_objects().size()) {
			RedotCollisionObject3D *co = static_cast<RedotCollisionObject3D *>(*space->get_objects().begin());
			co->set_space(nullptr);
		}

		active_spaces.erase(space);
		free(space->get_default_area()->get_self());
		free(space->get_static_global_body());

		space_owner.free(p_rid);
		memdelete(space);
	} else if (joint_owner.owns(p_rid)) {
		RedotJoint3D *joint = joint_owner.get_or_null(p_rid);

		joint_owner.free(p_rid);
		memdelete(joint);

	} else {
		ERR_FAIL_MSG("Invalid ID.");
	}
}

void RedotPhysicsServer3D::set_active(bool p_active) {
	active = p_active;
}

void RedotPhysicsServer3D::init() {
	stepper = memnew(RedotStep3D);
}

void RedotPhysicsServer3D::step(real_t p_step) {
	if (!active) {
		return;
	}

	_update_shapes();

	island_count = 0;
	active_objects = 0;
	collision_pairs = 0;
	for (const RedotSpace3D *E : active_spaces) {
		stepper->step(const_cast<RedotSpace3D *>(E), p_step);
		island_count += E->get_island_count();
		active_objects += E->get_active_objects();
		collision_pairs += E->get_collision_pairs();
	}
}

void RedotPhysicsServer3D::sync() {
	doing_sync = true;
}

void RedotPhysicsServer3D::flush_queries() {
	if (!active) {
		return;
	}

	flushing_queries = true;

	uint64_t time_beg = OS::get_singleton()->get_ticks_usec();

	for (const RedotSpace3D *E : active_spaces) {
		RedotSpace3D *space = const_cast<RedotSpace3D *>(E);
		space->call_queries();
	}

	flushing_queries = false;

	if (EngineDebugger::is_profiling("servers")) {
		uint64_t total_time[RedotSpace3D::ELAPSED_TIME_MAX];
		static const char *time_name[RedotSpace3D::ELAPSED_TIME_MAX] = {
			"integrate_forces",
			"generate_islands",
			"setup_constraints",
			"solve_constraints",
			"integrate_velocities"
		};

		for (int i = 0; i < RedotSpace3D::ELAPSED_TIME_MAX; i++) {
			total_time[i] = 0;
		}

		for (const RedotSpace3D *E : active_spaces) {
			for (int i = 0; i < RedotSpace3D::ELAPSED_TIME_MAX; i++) {
				total_time[i] += E->get_elapsed_time(RedotSpace3D::ElapsedTime(i));
			}
		}

		Array values;
		values.resize(RedotSpace3D::ELAPSED_TIME_MAX * 2);
		for (int i = 0; i < RedotSpace3D::ELAPSED_TIME_MAX; i++) {
			values[i * 2 + 0] = time_name[i];
			values[i * 2 + 1] = USEC_TO_SEC(total_time[i]);
		}
		values.push_back("flush_queries");
		values.push_back(USEC_TO_SEC(OS::get_singleton()->get_ticks_usec() - time_beg));

		values.push_front("physics_3d");
		EngineDebugger::profiler_add_frame_data("servers", values);
	}
}

void RedotPhysicsServer3D::end_sync() {
	doing_sync = false;
}

void RedotPhysicsServer3D::finish() {
	memdelete(stepper);
}

int RedotPhysicsServer3D::get_process_info(ProcessInfo p_info) {
	switch (p_info) {
		case INFO_ACTIVE_OBJECTS: {
			return active_objects;
		} break;
		case INFO_COLLISION_PAIRS: {
			return collision_pairs;
		} break;
		case INFO_ISLAND_COUNT: {
			return island_count;
		} break;
	}

	return 0;
}

void RedotPhysicsServer3D::_update_shapes() {
	while (pending_shape_update_list.first()) {
		pending_shape_update_list.first()->self()->_shape_changed();
		pending_shape_update_list.remove(pending_shape_update_list.first());
	}
}

void RedotPhysicsServer3D::_shape_col_cbk(const Vector3 &p_point_A, int p_index_A, const Vector3 &p_point_B, int p_index_B, const Vector3 &normal, void *p_userdata) {
	CollCbkData *cbk = static_cast<CollCbkData *>(p_userdata);

	if (cbk->max == 0) {
		return;
	}

	if (cbk->amount == cbk->max) {
		//find least deep
		real_t min_depth = 1e20;
		int min_depth_idx = 0;
		for (int i = 0; i < cbk->amount; i++) {
			real_t d = cbk->ptr[i * 2 + 0].distance_squared_to(cbk->ptr[i * 2 + 1]);
			if (d < min_depth) {
				min_depth = d;
				min_depth_idx = i;
			}
		}

		real_t d = p_point_A.distance_squared_to(p_point_B);
		if (d < min_depth) {
			return;
		}
		cbk->ptr[min_depth_idx * 2 + 0] = p_point_A;
		cbk->ptr[min_depth_idx * 2 + 1] = p_point_B;

	} else {
		cbk->ptr[cbk->amount * 2 + 0] = p_point_A;
		cbk->ptr[cbk->amount * 2 + 1] = p_point_B;
		cbk->amount++;
	}
}

RedotPhysicsServer3D *RedotPhysicsServer3D::redot_singleton = nullptr;
RedotPhysicsServer3D::RedotPhysicsServer3D(bool p_using_threads) {
	redot_singleton = this;
	RedotBroadPhase3D::create_func = RedotBroadPhase3DBVH::_create;

	using_threads = p_using_threads;
};
