extends Node3D

@onready var CameraPosition = $Spring/CameraPosition
@onready var Camera = $Camera


#@export var player: Node:
#	get:
#		return player
#	set(value):
#		player = value
#		if value == null:
#			Camera.current = false
#			player_to_follow_exists = false
#			Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
#		else:
#			Constants.PlayerCamera = self
#			Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
#			Camera.current = true
#			player_to_follow_exists = true
#			update_camera_target_position()
#			update_camera_rigging(0)
#			_move_camera()
#			player.tree_exiting.connect(func():
#				player = null)
#			#player.on_entity_despawn.connect(func():player_to_follow_exists = false)
#			player.on_entity_aiming.connect(ChangeMovementState)

var player_to_follow_exists = false
var mouse_sensitivity := 0.005
@onready var player = get_parent().player
@onready var camera_target = $CameraTarget

func activate():
	player = get_parent().player
	Constants.PlayerCamera = self
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	Camera.current = true
	player_to_follow_exists = true
	update_camera_rigging(0)
	player.tree_exiting.connect(func():
		deactivate())
	camera_target = $CameraTarget
	camera_target.prepare_camera_target(player)
	player.on_entity_despawn.connect(func():deactivate())

func deactivate():
	Camera.current = false


func _ready() -> void:
	print("Camera is ready")
	Camera.set_as_top_level(true)

var offset = 0

	# this changes the relative position of the camera, instead need to change relative position of PlayerCamera, i.e., self
	#Camera.top_level = true


var last_input_time = 0

#should be higher than inputrate framerate
var tolerance: float = 0.04

@export var controllerCameraSpeed: float = 6

var dir: Vector2 = Vector2.ZERO

var s = 0

var t = 0


func move_camera_rig(vec: Vector2) -> void:
		s -= vec.y * mouse_sensitivity
		s = clamp(s,-0.9,0.3)
		global_rotation.x = s
		
		
		t -= vec.x * mouse_sensitivity
		
		global_rotation.y = t
		global_rotation.z = 0


var camera_ideal_target_position = Vector3.ZERO
var target_position = Vector3.ZERO

# interpolate camera position between current position and the target Position because networking is slow
func move_camera():
	var t = 0.75
	Camera.global_transform.origin =t*Camera.global_transform.origin + (1-t) *CameraPosition.global_transform.origin
	Camera.look_at(camera_target.global_transform.origin)


func get_camera_target_position():
	return player.global_transform.origin + Vector3.UP*0.75 + player.basis.x*offset

func _process(delta):
	if Camera.current:
		update_camera_rigging(delta)
		move_camera()

func _physics_process(delta):
	if Camera.current:
		move_camera()
		update_camera_rigging(0)

func update_camera_rigging(delta):
	if (player != null) and player.is_inside_tree():
		global_transform.origin = get_camera_target_position()
		if last_input_time < tolerance:
			move_camera_rig(dir)
		else:
			move_camera_rig(Vector2(0.0,0.0))
	last_input_time += delta


func _input(event):
	if event is InputEventMouseMotion and event.relative:
		dir = event.relative
	else:
		dir = controllerCameraSpeed * Vector2(Input.get_action_strength("camera_left") - Input.get_action_strength("camera_right"),Input.get_action_strength("camera_up") - Input.get_action_strength("camera_down"))
	last_input_time = 0


