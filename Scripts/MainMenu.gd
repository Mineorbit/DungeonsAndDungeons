extends Node2D


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

# Called when the node enters the scene tree for the first time.
func _ready():
	pass


func open_play_menu():
	Bootstrap.start_play()


func _open_edit_menu():
	Bootstrap.start_edit()
