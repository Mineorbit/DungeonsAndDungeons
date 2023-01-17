extends Node3D

@onready var CameraHoldingPoint = $CameraHoldingPoint
@onready var Camera = $Camera
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
			Camera.current = true
			player_to_follow_exists = true
			update_camera_rigging(0)
			player.on_entity_remove.connect(func():player_to_follow_exists = false)
			player.on_entity_despawn.connect(func():player_to_follow_exists = false)
			player.on_entity_aiming.connect(ChangeMovementState)

var player_to_follow_exists = false
var mouse_sensitivity := 0.005

func _ready() -> void:
	player = null
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
var tolerance: float = 0.04

@export var controllerCameraSpeed: float = 6

var dir: Vector2 = Vector2.ZERO
var target_position: Vector3 = Vector3.ZERO

func move_camera_rig(vec: Vector2) -> void:
		rotation.x -= vec.y * mouse_sensitivity
		rotation.x	 = clamp(rotation.x, -0.9, 0.3)
		
		rotation.y -= vec.x * mouse_sensitivity
		rotation.y = rotation.y


# interpolate camera position between current position and the target Position (Holding point)
func move_camera():
	var new_target_position = get_camera_target_position()
	if((new_target_position-target_position).length() < 0.00001):
		# return early as position has no changed
		return
	target_position = get_camera_target_position()
	Camera.global_transform.origin = CameraHoldingPoint.global_transform.origin
	Camera.look_at(target_position)


func get_camera_target_position():
	return player.global_transform.origin + Vector3.UP*0.75 + player.basis.x*offset

func _physics_process(delta):
	pass
	update_camera_rigging(delta)
	
func _process(delta):
	pass
	update_camera_rigging(0)

func update_camera_rigging(delta):
	if player_to_follow_exists and (player != null):
		move_camera()
		#Camera.look_at(player.global_transform.origin)
		global_transform.origin = get_camera_target_position()
		if last_input_time < tolerance:
			move_camera_rig(dir)
	last_input_time += delta


func _input(event):
	if event is InputEventMouseMotion and event.relative:
		dir = event.relative
	else:
		dir = controllerCameraSpeed * Vector2(Input.get_action_strength("camera_left") - Input.get_action_strength("camera_right"),Input.get_action_strength("camera_up") - Input.get_action_strength("camera_down"))
	last_input_time = 0

