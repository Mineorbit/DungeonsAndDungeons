extends Spatial


export var mouse_sensitivity := 0.05

func _ready() -> void:
		set_as_toplevel(true)
		Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)



	
func move_camera(vec: Vector2) -> void:
		rotation_degrees.x -= vec.y * mouse_sensitivity
		rotation_degrees.x	 = clamp(rotation_degrees.x, -90, 30)
		
		rotation_degrees.y -= vec.x * mouse_sensitivity
		rotation_degrees.y = wrapf(rotation_degrees.y, 0, 360)
	

func _input(event):
	if event is InputEventMouseMotion and event.relative:
		move_camera(event.relative)


