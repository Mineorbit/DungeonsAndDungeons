extends Camera3D

var cursor
var collisionRay
var gridCursorMesh
var mouse_sensitivity := 0.005

var edit
var move_speed = 4

var rot_x = 0
var rot_y = 0
var colliding = true
var smallest_dist = 1
var selection = 0
var start_object
var closestIndex = 0

func _ready():
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)

func move_camera(vec: Vector2) -> void:
	rot_x += -vec.x * mouse_sensitivity
	rot_y += -vec.y * mouse_sensitivity
	rot_y = clamp(rot_y, -1.5,1.5)
	transform.basis = Basis() # reset rotation
	rotate_object_local(Vector3(0, 1, 0), rot_x) # first rotate in Y
	rotate_object_local(Vector3(1, 0, 0), rot_y)


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
	global_transform.origin += 0.05 * move_direction

func _input(event):
	if event is InputEventMouseMotion and event.relative:
		move_camera(event.relative*0.5)
