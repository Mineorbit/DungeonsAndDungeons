extends Node3D


@onready var doorModel: Node3D = $LockedDoorModel
@onready var nav = $Navigation
@onready var collision = $Collision

@export var is_open = false:
	set(val):
		if val:
			if doorModel != null:
				doorModel.hide()
				#doorModel.switch()
		else:
			if doorModel != null:
				doorModel.show()
				#doorModel.switchback()
		is_open = val

# Called when the node enters the scene tree for the first time.
func _ready():
	close()


func start():
	close()

func reset():
	close()


func open():
	if not is_open:
		add_child(nav)
		if collision.get_parent() != null:
			remove_child(collision)
		is_open = true
	

func close():
	if is_open:
		remove_child(nav)
		if collision.get_parent() == null:
			add_child(collision)
		is_open = false
	


func prepare_for_navmesh_build():
	print("Disabled before baking Nav Mesh")
	#open()
	
func restore_after_navmesh_build():
	print("Enabling before baking Nav Mesh")
	#close()
