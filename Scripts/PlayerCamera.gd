extends Node3D

@onready var CameraHoldingPoint = $CameraHoldingPoint
@onready var Camera: Camera3D = $Camera
@onready var TargetPoint:Node3D = $TargetPoint


@export var player: Node:
	get:
		return player
	set(value):
		player = value
		if value == null:
			Camera.current = false
			player_to_follow_exists = false
			Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
		else:
			Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
			print("Camera now follows "+str(player))
			Camera.current = true
			player_to_follow_exists = true
			player.on_entity_remove.connect(func():player_to_follow_exists = false)
			player.on_entity_despawn.connect(func():player_to_follow_exists = false)
			player.on_entity_aiming.connect(ChangeMovementState)

var player_to_follow_exists = false
var mouse_sensitivity := 0.005

func _ready() -> void:
	player = null
	Camera.set_as_top_level(true)
	set_as_top_level(true)

var offset = 0

func ChangeMovementState(aiming):
	if aiming:
		offset = 1
	else:
		offset = 0
	# this changes the relative position of the camera, instead need to change relative position of PlayerCamera, i.e., self
	#Camera.top_level = true


var last_input_time = 0
#should be higher than inputrate framerate
var tolerance = 0.04

func move_camera_rig(vec: Vector2) -> void:
		rotation.x -= vec.y * mouse_sensitivity
		rotation.x	 = clamp(rotation.x, -0.9, 0.3)
		
		rotation.y -= vec.x * mouse_sensitivity
		rotation.y = rotation.y


var target_position = Vector3.ZERO
# interpolate camera position between current position and the target Position (Holding point)
func move_camera():
	if player != null:
		Camera.global_transform.origin = 0.5*(Camera.global_transform.origin+CameraHoldingPoint.global_transform.origin)
		target_position = 0.25*target_position+0.75*get_camera_target_position()
		Camera.look_at(target_position)

func get_camera_target_position():
	return player.global_transform.origin + Vector3.UP*0.75 + player.basis.x*offset

func _process(delta):
	#Camera.global_transform.origin = ( TargetPoint.global_transform.origin + Camera.global_transform.origin)*0.5
	#move_camera(dir)
	if player_to_follow_exists and (player != null):
		move_camera()
		#Camera.look_at(player.global_transform.origin)
		global_transform.origin = get_camera_target_position()
		if last_input_time < tolerance:
			move_camera_rig(dir)
	last_input_time += delta


@export var controllerCameraSpeed = 6

var dir = Vector2.ZERO

func _input(event):
	if event is InputEventMouseMotion and event.relative:
		dir = event.relative
	else:
		dir = controllerCameraSpeed * Vector2(Input.get_action_strength("camera_left") - Input.get_action_strength("camera_right"),Input.get_action_strength("camera_up") - Input.get_action_strength("camera_down"))
	last_input_time = 0


