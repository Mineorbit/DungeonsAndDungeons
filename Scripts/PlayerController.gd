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
	
var is_active = false
func set_active(active):
	print("Setting "+str(active))
	is_active = active

func despawn():
	spawned = false
	remove_child(camera)
	camera.player = player

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta):
	if player != null and spawned and is_active:
		
		var input_direction = Vector3.ZERO
		input_direction.x = Input.get_action_strength("right") - Input.get_action_strength("left")
		input_direction.z = Input.get_action_strength("back") - Input.get_action_strength("forward")
		input_direction = input_direction.rotated(Vector3.UP, camera.rotation.y).normalized()
		
		
		player.should_jump = Input.is_action_just_pressed("jump")
		
		player.move_direction.x = input_direction.x
		player.move_direction.z = input_direction.z