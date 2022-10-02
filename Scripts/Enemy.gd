extends Entity
class_name Enemy

# Called when the node enters the scene tree for the first time.

@onready var navAgent = $Navigation
@onready var target = $Target
var _timer = null
func _ready():
	super._ready()
	remove_child(target)
	navAgent.ignore_y = false
	get_parent().add_child(target)
	setup_calculation_routine()

func setup_calculation_routine():
	_timer = Timer.new()
	add_child(_timer)
	_timer.timeout.connect(try_strike)
	_timer.set_wait_time(1.0)
	_timer.set_one_shot(false) # Make sure it loops
	_timer.start()

func try_strike():
	print("Trying to Strike")
	on_entity_melee_strike.emit(15)
	

func _physics_process(delta):
	super._physics_process(delta)
	if not started:
		return
	if len(Constants.players) > 0 and Constants.players[0] != null:
		target.global_transform.origin	=	Constants.players[0].global_transform.origin
	move_direction = Vector3.ZERO
	auto_navigate()




func auto_navigate():
	navAgent.set_target_location(target.global_transform.origin)
	#var canreach = navAgent.is_target_reachable() and not navAgent.is_target_reached()
	var canreach = true
	if canreach:
		move_direction = ( navAgent.get_next_location() - global_transform.origin).normalized() *0.5
	navAgent.set_velocity(_velocity)

func remove():
	target.queue_free()
	super.remove()
