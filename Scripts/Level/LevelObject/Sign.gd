extends Node3D
@onready var text = $Text
@export var sign_text: String = "BottomText":
	set(value):
		sign_text = value
		if text != null:
			if value == null:
				sign_text = "BottomText"
			text.text = sign_text
