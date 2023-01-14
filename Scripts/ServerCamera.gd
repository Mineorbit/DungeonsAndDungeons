extends Camera3D




# Declare member variables here. Examples:
# var a = 2
# var b = "text"


var velocity
var move_speed = 2
var mouse_sensitivity = 0.025
var closestIndex = 0

func start():
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)


	

var rot_x = 0
var rot_y = 0

func move_camera(vec: Vector2) -> void:
	rot_x += -vec.x * mouse_sensitivity
	rot_y += -vec.y * mouse_sensitivity
	rot_y = clamp(rot_y, -1.5,1.5)
	transform.basis = Basis() # reset rotation
	rotate_object_local(Vector3(0, 1, 0), rot_x) # first rotate in Y
	rotate_object_local(Vector3(1, 0, 0), rot_y)


var colliding = true
var smallest_dist = 1
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
	smallest_dist = 200
	velocity = move_direction
	global_transform.origin += velocity
	#colliding = removalRay.is_colliding()



func _input(event):
	if event is InputEventMouseMotion and event.relative:
		move_camera(event.relative)
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
