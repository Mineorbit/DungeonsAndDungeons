extends Node2D


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var currentMenu = null
@onready var canvasLayer = $MainMenuLayer

func _ready():
	Constants.set_mode(0)
	Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
	open_menu("Main")

func start_play(ip):
	Constants.remoteAddress = ip
	print("'"+str(Constants.remoteAddress)+"'")
	Bootstrap.start_play()



func open_menu(name):
	if currentMenu != null:
		currentMenu.queue_free()
	var pref = load("res://Prefabs/MainMenu/"+str(name)+".tscn")
	var instance = pref.instantiate()
	canvasLayer.add_child(instance)
	currentMenu = instance



func start_edit(name = null):
	Bootstrap.start_edit(name)
