extends KinematicBody


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


var level: Spatial = null
onready var cursor = $CursorArm/Cursor


export var mouse_sensitivity := 0.05

export var move_speed = 4
func _ready() -> void:
		level = $"../Level"
		set_as_toplevel(true)
		Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)




func move_camera(vec: Vector2) -> void:
		rotation_degrees.x -= vec.y * mouse_sensitivity
		rotation_degrees.x	 = clamp(rotation_degrees.x, -90, 30)
		
		rotation_degrees.y -= vec.x * mouse_sensitivity
		rotation_degrees.y = wrapf(rotation_degrees.y, 0, 360)
		


func _physics_process(delta: float) -> void:
	var aim = get_global_transform().basis
	var forward = -aim.z
	var right = aim.x
	var up = aim.y
	var move_direction = Vector3.ZERO
	move_direction += right*(Input.get_action_strength("right") - Input.get_action_strength("left"))
	move_direction += -forward*(Input.get_action_strength("back") - Input.get_action_strength("forward"))
	move_direction += up*(Input.get_action_strength("up") - Input.get_action_strength("down"))
	move_direction *= move_speed
	move_and_slide(move_direction,Vector3.UP)

func _process(delta) -> void:
	if Input.is_action_just_pressed("Place"):
		level.add(cursor.global_transform.origin)
	
func _input(event):
	if event is InputEventMouseMotion and event.relative:

		move_camera(event.relative)
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
