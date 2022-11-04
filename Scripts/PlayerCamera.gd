extends Node3D


@onready var Camera: Camera3D = $Camera
@onready var TargetPoint:Node3D = $TargetPoint


@export var player: Node:
	get:
		return player
	set(value):
		player = value
		if value == null:
			remove_child(Camera)
			player_to_follow_exists = false
			Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
		else:
			add_child(Camera)	
			Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
			print("Capturing")
			Camera.current = true
			player_to_follow_exists = true
			player.on_entity_remove.connect(func():player_to_follow_exists = false)
			player.on_entity_despawn.connect(func():player_to_follow_exists = false)

var player_to_follow_exists = false
var mouse_sensitivity := 0.005

func _ready() -> void:
	player = null
	top_level = true
	Constants.playerCamera = self


	#Camera.top_level = true

func move_camera(vec: Vector2) -> void:
		rotation.x -= vec.y * mouse_sensitivity
		rotation.x	 = clamp(rotation.x, -0.9, 0.3)
		
		rotation.y -= vec.x * mouse_sensitivity
		rotation.y = rotation.y
		
	
func _process(delta):
	#Camera.global_transform.origin = ( TargetPoint.global_transform.origin + Camera.global_transform.origin)*0.5
	#move_camera(dir)
	if player_to_follow_exists and (player != null):
		#Camera.look_at(player.global_transform.origin)
		global_transform.origin = player.global_transform.origin + Vector3.UP*0.75


@export var controllerCameraSpeed = 6

var dir = Vector2.ZERO

func _input(event):
	if event is InputEventMouseMotion and event.relative:
		dir = event.relative
	else:
		dir = controllerCameraSpeed * Vector2(Input.get_action_strength("camera_left") - Input.get_action_strength("camera_right"),Input.get_action_strength("camera_up") - Input.get_action_strength("camera_down"))
	move_camera(dir)


