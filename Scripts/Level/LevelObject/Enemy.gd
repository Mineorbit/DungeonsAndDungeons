extends Entity
class_name Enemy

# Called when the node enters the scene tree for the first time.

@onready var navAgent: NavigationAgent3D = $Navigation
@onready var target = $Target
@onready var strikeArea: Area3D = $StrikeArea
var _timer = null

func _ready():
	super._ready()
	remove_child(target)
	navAgent.ignore_y = false
	get_parent().add_child(target)


func try_strike():
	if strikeArea.get_overlapping_bodies().size() > 0:
		on_entity_melee_strike.emit(15)
	
var strike_time = 0

func _physics_process(delta):
	track_target()
	plan_route()
	strike_time += delta
	if strike_time > 1:
		try_strike()
		strike_time = 0
	auto_navigate(delta)

var target_entity = null
var follow_target = false

func track_target():
	if target_entity != null:
		target.global_transform.origin = target_entity.global_transform.origin

func plan_route():
	if (is_inside_tree() and target.is_inside_tree()):
		navAgent.target_position = target.global_transform.origin

var immediate_target_pos = Vector3.ZERO

func auto_navigate(delta):
	immediate_target_pos = navAgent.get_next_path_position()
	var canreach = navAgent.is_target_reachable() and not navAgent.is_target_reached()
	if canreach:
		move_direction = ( immediate_target_pos - global_transform.origin).normalized() *0.5
	else:
		move_direction = Vector3.ZERO
	super._physics_process(delta)
	navAgent.set_velocity(_velocity)

func remove():
	#target.queue_free()
	super.remove()
