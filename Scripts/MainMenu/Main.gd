extends Control

@onready var ipText: TextEdit = $CenterContainer/HBoxContainer/CenterContainer/VBoxContainer/IP
# Called when the node enters the scene tree for the first time.
func _ready():
	pass


func open_play_menu():
	Constants.remoteAddress = ipText.text
	print("'"+str(Constants.remoteAddress)+"'")
	Bootstrap.start_play()





func _open_edit_menu():
	Bootstrap.start_edit("test")
