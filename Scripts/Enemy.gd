extends Entity
class_name Enemy

# Called when the node enters the scene tree for the first time.

@onready var navAgent = $Navigation
@onready var target = $Target

func _ready():
	super._ready()
	remove_child(target)
	get_parent().add_child(target)

func _physics_process(delta):
	super._physics_process(delta)
	if not started:
		return
		
	
	if len(Constants.players) > 0 and Constants.players[0] != null:
		target.global_transform.origin	=	Constants.players[0].global_transform.origin
	move_direction = Vector3.ZERO
	print(str(navAgent.is_target_reachable())+" "+str(not navAgent.is_target_reached()))
	navAgent.set_target_location(target.global_transform.origin)
	if navAgent.is_target_reachable() and not navAgent.is_target_reached():
		move_direction = ( navAgent.get_next_location() - global_transform.origin).normalized() *0.5
		print(move_direction)


func remove():
	target.queue_free()
	super.remove()
