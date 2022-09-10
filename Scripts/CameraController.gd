extends Node3D


var mouse_sensitivity := 0.005

func _ready() -> void:
	top_level = true



	
func move_camera(vec: Vector2) -> void:
		rotation.x -= vec.y * mouse_sensitivity
		rotation.x	 = clamp(rotation.x, -0.9, 0.3)
		
		rotation.y -= vec.x * mouse_sensitivity
		rotation.y = rotation.y
	

func _input(event):
	if event is InputEventMouseMotion and event.relative:
		move_camera(event.relative)


