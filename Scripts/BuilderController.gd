extends CharacterBody3D


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


var level = null
var cursor = $CursorArm/Cursor
var removalRay = $CursorArm/Cursor/RayCast
var gridCursorMesh = $CursorArm/Cursor/GridCursorMesh
var mouse_sensitivity := 0.005

var move_speed = 4

var closestIndex = 0
func _ready() -> void:
	cursor = $CursorArm/Cursor
	removalRay = $CursorArm/Cursor/RayCast
	gridCursorMesh = $CursorArm/Cursor/GridCursorMesh
	level = $"../Level"
	top_level = true
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
	move_and_slide()
	#colliding = removalRay.is_colliding()

var selection = 0

func _process(delta) -> void:
	print(cursor.get_global_transform().origin)
	if Input.is_action_just_pressed("Place"):
		level.add(Constants.LevelObjectData[selection],cursor.get_global_transform().origin)
	if Input.is_action_just_pressed("Displace"):
		print("Trying to Remove Block")
		if colliding:
			var aim = cursor.get_global_transform().basis
			var forward = -aim.z
			var position_to_remove = cursor.global_transform.origin  - forward
			var isRemoved = level.remove_by_position(position_to_remove)
			if not isRemoved and removalRay.is_colliding():
				var result = removalRay.get_collider()
				if result.name == "ConstructionCollision":
					var level_object = result.get_parent()
					level.remove_by_object(level_object)
	#gridCursorMesh.global_transform.origin = Vector3(0.5,0,0.5) + level.gridMap.world_to_map(cursor.global_transform.origin)		
	if Input.is_action_just_pressed("SelectLeft"):
		selection = (selection - 1 + Constants.LevelObjectData.size()) %(Constants.LevelObjectData.size())
	if Input.is_action_just_pressed("SelectRight"):
		selection = (selection + 1) %(Constants.LevelObjectData.size())
	
	
func _input(event):
	if event is InputEventMouseMotion and event.relative:
		move_camera(event.relative)
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
