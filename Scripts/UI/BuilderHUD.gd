extends Node2D
@onready var editpanel = $CanvasLayer/EditPanel
@onready var text:TextEdit = $CanvasLayer/EditPanel/text


var editing_level_object: Node3D

func _ready():
	Signals.edited_interactive_level_object.connect(func (object):
		if object == null:
			editpanel.hide()
			return
		editing_level_object = object.contained_level_object
		print(object.contained_level_object.get_property_list())
		editpanel.show()
		)
	editpanel.hide()

func text_changed():
	editing_level_object.set("sign_text",text.text)
