extends Node3D


@onready var gateModel = $GateModel
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func activate():
	remove_child(gateModel)
	
func deactivate():
	print("Deactivated")
	add_child(gateModel)
