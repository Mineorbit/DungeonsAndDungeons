extends Node3D

var activated = 0

@onready var andgatemodel = $AndGateModel

func _ready():
	Signals.mode_changed.connect(on_mode_change_and_gate)


func on_mode_change_and_gate():
	if Constants.currentMode == 1:
		andgatemodel.show()
	else:
		andgatemodel.hide()

@rpc
func activate():
	
	activated = min(get_parent().connectedObjects.size(),activated + 1)
	print(activated)
	if get_parent().connectedObjects.size() == activated:
		get_parent().activate_connected()

@rpc
func deactivate():
	activated = max(0,activated - 1)
	print(activated)
	if get_parent().connectedObjects.size() > activated:
		get_parent().deactivate_connected()
