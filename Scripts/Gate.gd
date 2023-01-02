extends Node3D


@onready var gateModel: Node3D = $GateModel
@onready var nav = $Navigation
@onready var collision = $Collision
@export var is_open = false:	
	set(val):
		if val:
			if gateModel.get_parent() != null:
				remove_child(gateModel)
		else:
			if gateModel != null and gateModel.get_parent() == null:
				add_child(gateModel)
		is_open = val

# Called when the node enters the scene tree for the first time.
func _ready():
	close()

@rpc
func activate():
	open()
	
@rpc
func deactivate():
	close()

func open():
	add_child(nav)
	if collision.get_parent() != null:
		remove_child(collision)
	is_open = true
	

func close():
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
