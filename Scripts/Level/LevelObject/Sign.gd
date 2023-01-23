extends Node3D
@onready var text = $Text
var test
@export var var_sign_text: String = "BottomText":
	set(value):
		var_sign_text = value
		if text != null:
			if value == null:
				var_sign_text = "BottomText"
			text.text = var_sign_text
