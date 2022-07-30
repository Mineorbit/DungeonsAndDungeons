extends KinematicBody


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


var level: Spatial = null
onready var cursor = $CursorArm/Cursor
export var removalRays = []
var removalRayObjects = []
onready var test1 = $MeshInstance
onready var test2 = $MeshInstance2
onready var test3 = $MeshInstance3
onready var rayCollection = $CursorArm/Cursor/RayCollection
export var mouse_sensitivity := 0.05

export var move_speed = 4

var closestIndex = 0
func _ready() -> void:
		level = $"../Level"
		set_as_toplevel(true)
		Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
		for ray in removalRays:
			removalRayObjects.append(get_node(ray))
			
		rayCollection.set_as_toplevel(true)




func move_camera(vec: Vector2) -> void:
		rotation_degrees.x -= vec.y * mouse_sensitivity
		rotation_degrees.x	 = clamp(rotation_degrees.x, -90, 30)
		
		rotation_degrees.y -= vec.x * mouse_sensitivity
		rotation_degrees.y = wrapf(rotation_degrees.y, 0, 360)
		

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
	var i = 0	
	
	
	for ray in removalRayObjects:
			
		var distance = ( ray.get_collision_point() - cursor.global_transform.origin).length()
		if(distance < smallest_dist):
			smallest_dist = distance
			closestIndex = i
		i = i+1


func report_rays():
	print("==========")
	for i in range(6):
		if removalRayObjects[i].is_colliding():
			print(str(i) +" "+str(removalRayObjects[i].get_collision_point()))
			
	print(closestIndex)
	test1.global_transform.origin = removalRayObjects[0].get_collision_point()
	test2.global_transform.origin = removalRayObjects[1].get_collision_point()
	test3.global_transform.origin = removalRayObjects[2].get_collision_point()
	
func _process(delta) -> void:
	rayCollection.global_transform.origin = cursor.global_transform.origin
	
	if Input.is_action_just_pressed("Place"):
		print("Adding Block "+str(cursor.global_transform.origin))
		level.add(Constants.Default_Floor,cursor.global_transform.origin)
	if Input.is_action_just_pressed("Displace"):
		print("Trying to Remove Block")
		
		var direction = removalRayObjects[closestIndex].cast_to
		direction.z *= -1
		direction.x *= -1
		var position_to_remove = cursor.global_transform.origin + direction
		level.remove(position_to_remove)
	
func _input(event):
	if event is InputEventMouseMotion and event.relative:

		move_camera(event.relative)
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
