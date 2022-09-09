extends Entity
class_name Enemy

# Called when the node enters the scene tree for the first time.

@onready var navAgent = $Navigation


func _physics_process(delta):
	super._physics_process(delta)
	if not started:
		return
	if len(Constants.players) > 0 and Constants.players[0] != null:
		navAgent.set_target_location(Constants.players[0].global_transform.origin)
		if (Constants.players[0].global_transform.origin - global_transform.origin).length() > 2:
			move_direction = ( navAgent.get_next_location() - global_transform.origin).normalized() *0.5
		else:
			move_direction = Vector3.ZERO
