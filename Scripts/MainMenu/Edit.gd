extends Control


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


func _on_new_level():
	get_parent().get_parent().start_edit()
