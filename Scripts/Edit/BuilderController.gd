extends CharacterBody3D


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


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
@onready var extracursor = $ExtraCursor
@onready var camera = $BuilderCamera
signal on_levelobject_placed
signal on_levelobject_displaced
func start():
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	camera.current = true

func _ready() -> void:
	Constants.builder = self
	cursor = $CursorArm/Cursor
	collisionRay = $CursorArm/Cursor/RayCast
	gridCursorMesh = $CursorArm/Cursor/GridCursorMesh
	top_level = true
	edit = get_parent()
	Signals.edited_level_object.connect(
		func(levelobject):
			if levelobject == null:
				Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
				editing = false
			else:
				Input.set_mouse_mode(Input.MOUSE_MODE_CONFINED)
				editing = true
	)
	on_levelobject_placed.connect(func():
		cursor.placeSound.play())
	on_levelobject_displaced.connect(func():
		cursor.displaceSound.play())
	


func move_camera(vec: Vector2) -> void:
	rot_x += -vec.x * mouse_sensitivity
	rot_y += -vec.y * mouse_sensitivity
	rot_y = clamp(rot_y, -1.5,1.5)
	transform.basis = Basis() # reset rotation
	rotate_object_local(Vector3(0, 1, 0), rot_x) # first rotate in Y
	rotate_object_local(Vector3(1, 0, 0), rot_y)


func _physics_process(delta: float) -> void:
	if editing:
		return
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


func get_collided_level_object():
	if collisionRay.is_colliding():
		var result = collisionRay.get_collider()
		if result.name == "ConstructionCollision":
			var level_object = result.get_parent()
			return level_object



func connect_interactive_objects(a, b):
	if a == null or b == null or a == b:
		return
	# check or cycle
	if a is InteractiveLevelObject and b is InteractiveLevelObject:
		if not a.unique_instance_id in b.connectedObjects:
			b.connectedObjects.append(a.unique_instance_id)
			Signals.connection_added.emit(b.unique_instance_id,[a.unique_instance_id])
		else:
			print("These Objects are allready connected")
	else:
		print("Cannot connect these objects as they are not Interactive")


var selected_rotation = 0

var editing = false
func _process(delta) -> void:
	
	extracursor.global_transform.origin = Vector3(floor(cursor.global_transform.origin.x),floor(cursor.global_transform.origin.y),floor(cursor.global_transform.origin.z))
	if Input.is_action_just_pressed("EditProperties"):
		if not editing:
			var to_edit = get_collided_level_object()
			if to_edit != null and to_edit is LevelObject:
				for prop in to_edit.contained_level_object.get_property_list():
					if prop["name"].begins_with("var"):
						Signals.edited_level_object.emit(to_edit)
						break
	if editing:
		return
	Constants.builderPosition = global_transform.origin
	if Input.is_action_just_pressed("RotateRight"):
		selected_rotation = (selected_rotation + 1)%4
	if Input.is_action_just_pressed("Place"):
		Constants.World.level.add(Constants.LevelObjectData[selection],cursor.get_global_transform().origin,selected_rotation,null,[],null,true)
		on_levelobject_placed.emit()
	if Input.is_action_just_pressed("Displace"):
		if colliding:
			var aim = cursor.get_global_transform().basis
			var forward = -aim.z
			var position_to_remove = cursor.global_transform.origin - forward
			var isRemoved = Constants.World.level.remove_by_position(position_to_remove)
			if isRemoved:
				on_levelobject_displaced.emit()
			else:
				if get_collided_level_object() != null:
					on_levelobject_displaced.emit()
					Constants.World.level.remove_by_object(get_collided_level_object())
	if Input.is_action_just_pressed("Connect"):
		start_object = get_collided_level_object()
		if start_object != null:
			is_connecting = true
			edit.connections.add_child(edit.connections.constructconnection)
			edit.connections.constructconnection.point_a.global_transform.origin = start_object.global_transform.origin + Vector3(0.5,0.5,0.5)
	
	if is_connecting:
		edit.connections.constructconnection.point_b.global_transform.origin = cursor.global_transform.origin
	if Input.is_action_just_released("Connect"):
		connect_interactive_objects(start_object,get_collided_level_object())
		start_object = null
		edit.connections.remove_child(edit.connections.constructconnection)
	if Input.is_action_just_pressed("SelectLeft"):
		selection = (selection - 1 + Constants.LevelObjectData.size()) %(Constants.LevelObjectData.size())
		Signals.selected_level_object_changed.emit(selection)
	if Input.is_action_just_pressed("SelectRight"):
		selection = (selection + 1) %(Constants.LevelObjectData.size())
		Signals.selected_level_object_changed.emit(selection)

var is_connecting = false
func _input(event):
	if event is InputEventMouseMotion and event.relative and not editing:
		move_camera(event.relative)
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass

