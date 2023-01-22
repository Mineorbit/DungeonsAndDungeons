extends Node2D
@onready var editpanel = $CanvasLayer/EditPanel

func _ready():
	Signals.edited_interactive_level_object.connect(func (object):
		print(object)
		if object == null:
			editpanel.hide()
		else:
			editpanel.show()
		)
	editpanel.hide()
