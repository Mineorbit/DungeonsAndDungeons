extends Node2D


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func start_play():
	get_tree().change_scene("res://Scenes/test.tscn")

func start_edit():
	get_tree().change_scene("res://Scenes/edit.tscn")


func open_play_menu():
	start_play()




func _open_edit_menu():
	start_edit()
