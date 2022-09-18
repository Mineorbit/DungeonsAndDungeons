extends Node3D


@onready var gateModel = $GateModel
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func activate():
	open()
	
func deactivate():
	close()

func open():
	remove_child(gateModel)

func close():
	add_child(gateModel)
	

func prepare_for_navmesh_build():
	print("Disabled before baking Nav Mesh")
	open()
	
func restore_after_navmesh_build():
	print("Enabling before baking Nav Mesh")
	close()
