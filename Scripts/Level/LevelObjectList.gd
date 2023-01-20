extends Node3D


@export var display = false
# Called when the node enters the scene tree for the first time.
func _ready():
	pass
	var i = 0
	for levelObject in get_children():
		levelObject.transform.origin = Vector3(2*i + 1,0,0)
		i = i+1
		
	if display:
		i = 0
		for child in get_children():
			child.set_process(false)
			child.set_physics_process(false)
			child.scale = Constants.LevelObjectData[i].display_scale
			if child.has_method("getSpawnedEntity"):
				child.ready.connect(func():
					child.getSpawnedEntity().scale = Constants.LevelObjectData[i].display_scale
				)
			if child.name.begins_with("Tiled"):
				var first = true
				for subtile in child.get_children():
					if first:
						first = false
					else:
						subtile.hide()
			i = i+1
		Signals.selected_level_object_changed.connect(selection_changed)
		set_positions_cyclic(0)
	num_of_elems_on_screen = len(get_children()) + (len(get_children())%2)
	#print("NUM OF OBJECTS "+str(num_of_elems_on_screen))




var num_of_elems_on_screen
var selection = 0


func set_positions_cyclic(sel):
	selection = sel


func selection_changed(value):
	print("Selection changed to "+str(value))
	set_positions_cyclic(value)
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if display:
		for child in get_children():
			child.rotate(Vector3.UP,delta)
		var i = 0
		for levelObject in get_children():
			var new_pos = Vector3( ((i - selection + num_of_elems_on_screen/2 + num_of_elems_on_screen +1)%(num_of_elems_on_screen + 1))*4 - num_of_elems_on_screen*2,0,0)
			levelObject.transform.origin = (new_pos + levelObject.transform.origin)/2
			i = i+1

