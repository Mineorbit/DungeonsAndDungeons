extends Node3D


@onready var gateModel = $GateModel
@onready var nav = $Navigation
# Called when the node enters the scene tree for the first time.
func _ready():
	close()

func activate():
	open()
	
func deactivate():
	close()

func open():
	add_child(nav)
	remove_child(gateModel)

func close():
	remove_child(nav)
	add_child(gateModel)
	

func prepare_for_navmesh_build():
	print("Disabled before baking Nav Mesh")
	#open()
	
func restore_after_navmesh_build():
	print("Enabling before baking Nav Mesh")
	#close()
