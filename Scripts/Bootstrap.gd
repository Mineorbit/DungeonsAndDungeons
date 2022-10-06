extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"
@export var is_server = false
var current_scene = null
# Called when the node enters the scene tree for the first time.
func _ready():
	var server = is_server or OS.has_feature("Server") or "--server" in OS.get_cmdline_args()
	if server:
		print("===Starting Server===")
		start_server()
	else:
		print("===Starting Dungeons And Dungeons===")
		remove_child(current_scene)
		current_scene = load("res://Scenes/menu.tscn").instantiate()
		add_child(current_scene)

func start_server():
	current_scene = load("res://Scenes/play.tscn").instantiate()
	add_child(current_scene)
	var server_management = load("res://Scenes/ServerNetworkManagement.tscn").instantiate()
	current_scene.add_child(server_management)

func start_play():
		remove_child(current_scene)
		current_scene = load("res://Scenes/test.tscn").instantiate()
		add_child(current_scene)
func start_edit():
		remove_child(current_scene)
		current_scene = load("res://Scenes/edit.tscn").instantiate()
		add_child(current_scene)
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
