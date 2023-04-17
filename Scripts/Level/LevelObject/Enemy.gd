extends Entity
class_name Enemy

# Called when the node enters the scene tree for the first time.

@onready var navAgent: NavigationAgent3D = $Navigation
@onready var target = $Target
@onready var strikeArea: Area3D = $StrikeArea

@export_range(0.1,10,0.1) var minimum_distance_to_target: float = 1
@export var visibilityDistance = 8



func _ready():
	super._ready()
	remove_child(target)
	navAgent.ignore_y = false
	get_parent().add_child(target)



func _physics_process(delta):
	track_target()
	plan_route()
	auto_navigate(delta)
	super._physics_process(delta)
	navAgent.set_velocity(_velocity)

var target_entity = null
var follow_target = false




func target_entity_in_front():
	if target_entity == null:
		return false
	return (target_entity.global_transform.origin-global_transform.origin).dot(global_transform.basis.x) > 0.25




func track_target():
	if target_entity != null and target_entity_in_front():
		target.global_transform.origin = target_entity.global_transform.origin

func distance_to_target_entity():
	return (target_entity.global_transform.origin-global_transform.origin).length()

func distance_to_target():
	return (target.global_transform.origin-global_transform.origin).length()

func plan_route():
	if (is_inside_tree() and target.is_inside_tree()):
		if target_entity_in_front() and distance_to_target()<visibilityDistance:
			navAgent.target_position = target.global_transform.origin

var immediate_target_pos = Vector3.ZERO

func auto_navigate(delta):
	immediate_target_pos = navAgent.get_next_path_position()
	var canreach = navAgent.is_target_reachable() and not navAgent.is_target_reached()
	if canreach and distance_to_target() > minimum_distance_to_target and follow_target:
		move_direction = ( immediate_target_pos - global_transform.origin).normalized() *0.5
	else:
		move_direction = Vector3.ZERO



func on_navigation_velocity_computed(safe_velocity):
	move_direction = safe_velocity


func remove():
	#target.queue_free()
	super.remove()
