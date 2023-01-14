extends Node3D



@onready var activationArea: Area3D = $InteractionArea
@onready var levermodel = $LeverModel
# Called when the node enters the scene tree for the first time.
func _ready():
	activationArea.body_entered.connect(process_entered)
	pass # Replace with function body.


var is_active = false


func process_entered(_x):
	if get_parent().has_method("activate"):
		if is_active:
			deactivate()
			get_parent().deactivate_connected()
		else:
			activate()
			get_parent().activate_connected()

@rpc
func activate():
	is_active = true
	levermodel.switch()

@rpc
func deactivate():
	is_active = false
	levermodel.switchback()

