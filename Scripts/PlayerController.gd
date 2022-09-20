extends Node

var player
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta):
	if player != null:
		player.move_direction.x = Input.get_action_strength("right") - Input.get_action_strength("left")
		player.move_direction.z = Input.get_action_strength("back") - Input.get_action_strength("forward")
		player.should_jump = Input.is_action_just_pressed("jump")
		super._physics_process(delta)
