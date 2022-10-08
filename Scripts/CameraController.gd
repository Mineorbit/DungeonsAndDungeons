extends Node3D


@onready var Camera = $Camera
var mouse_sensitivity := 0.005
@export var player: Node:
	get:
		return player
	set(value):
		if value == null:
			remove_child(Camera)
		else:
			add_child(Camera)	
			Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
			Camera.current = true	
		player = value

func _ready() -> void:
	player = null
	top_level = true
	Constants.playerCamera = self

func move_camera(vec: Vector2) -> void:
		rotation.x -= vec.y * mouse_sensitivity
		rotation.x	 = clamp(rotation.x, -0.9, 0.3)
		
		rotation.y -= vec.x * mouse_sensitivity
		rotation.y = rotation.y
		
	
func _process(delta):
	if(player != null):
		Camera.current = true	
		global_transform.origin = player.global_transform.origin + Vector3.UP*0.75


func _input(event):
	if event is InputEventMouseMotion and event.relative:
		move_camera(event.relative)


