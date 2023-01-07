extends Control

@onready var ipText: TextEdit = $CenterContainer/HBoxContainer/CenterContainer/MarginContainer/VBoxContainer/IP
# Called when the node enters the scene tree for the first time.
func _ready():
	pass


func open_play_menu():
	Constants.remoteAddress = ipText.text
	Bootstrap.start_play()


func _open_edit_menu():
	get_parent().get_parent().open_menu("Edit")
