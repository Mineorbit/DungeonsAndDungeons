extends KinematicBody


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


var level: Spatial = null
onready var cursor = $CursorArm/Cursor
onready var removalRay = $CursorArm/Cursor/RayCast
onready var gridCursorMesh = $CursorArm/Cursor/GridCursorMesh
export var mouse_sensitivity := 0.05

export var move_speed = 4

var closestIndex = 0
func _ready() -> void:
		level = $"../Level"
		set_as_toplevel(true)
		Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)




func move_camera(vec: Vector2) -> void:
		rotation_degrees.x -= vec.y * mouse_sensitivity
		rotation_degrees.x	 = clamp(rotation_degrees.x, -90, 30)
		
		rotation_degrees.y -= vec.x * mouse_sensitivity
		rotation_degrees.y = wrapf(rotation_degrees.y, 0, 360)
		

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
	move_and_slide(move_direction,Vector3.UP)
	#colliding = removalRay.is_colliding()

var selection = 0

func _process(delta) -> void:
	if Input.is_action_just_pressed("Place"):
		print("Adding Block "+str(cursor.global_transform.origin))
		level.add(Constants.LevelObjectData[selection],cursor.global_transform.origin)
	if Input.is_action_just_pressed("Displace"):
		print("Trying to Remove Block")
		if colliding:
			var aim = cursor.get_global_transform().basis
			var forward = -aim.z
			var position_to_remove = cursor.global_transform.origin  - forward
			level.remove_by_position(position_to_remove)
			if removalRay.is_colliding():
				var result = removalRay.get_collider()
				if result.name == "ConstructionCollision":
					var level_object = result.get_parent()
					level.remove_by_object(level_object)
	gridCursorMesh.global_transform.origin = Vector3(0.5,0,0.5) + level.gridMap.world_to_map(cursor.global_transform.origin)		
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
