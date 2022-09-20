extends Node

var player
var camera
# Called when the node enters the scene tree for the first time.
func _ready():
	camera = load("res://Prefabs/PlayerCamera.tscn").instantiate()

var spawned = false

func spawn():
	spawned = true
	add_child(camera)
	camera.player = player
	

func despawn():
	spawned = false
	remove_child(camera)
	camera.player = player

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta):
	if player != null and spawned:
		player.move_direction.x = Input.get_action_strength("right") - Input.get_action_strength("left")
		player.move_direction.z = Input.get_action_strength("back") - Input.get_action_strength("forward")
		player.move_direction = player.move_direction.rotated(Vector3.UP, camera.rotation.y).normalized()
		player.should_jump = Input.is_action_just_pressed("jump")
