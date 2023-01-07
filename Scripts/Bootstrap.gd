extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"
@export var is_server = false
var current_scene = null
# Called when the node enters the scene tree for the first time.
func _ready():
	pass


var started = false

func start(should_be_server):
	var dir_access = DirAccess.open("user://")
	dir_access.make_dir("level")
	dir_access.make_dir("level/localLevels")
	dir_access.make_dir("level/downloadLevels")
	started = true
	var server = should_be_server or is_server or OS.has_feature("Server") or "--server" in OS.get_cmdline_args()
	if server:
		print("===Starting Server===")
		start_server()
	else:
		print("===Starting Dungeons And Dungeons===")
		start_main_menu()
	

func start_main_menu():
		remove_child(current_scene)
		current_scene = load("res://Scenes/menu.tscn").instantiate()
		add_child(current_scene)

func _process(_delta):
	if not started:
		if not OS.is_debug_build():
			start(false)
		if Input.is_action_just_pressed("Client"):
			start(false)
		if Input.is_action_just_pressed("Server"):
			start(true)

func start_server():
	current_scene = load("res://Scenes/play.tscn").instantiate()
	add_child(current_scene)
	var server_management = load("res://Scenes/ServerNetworkManagement.tscn").instantiate()
	current_scene.add_child(server_management)


func start_play():
		remove_child(current_scene)
		current_scene = load("res://Scenes/play.tscn").instantiate()
		add_child(current_scene)
		var client_management = load("res://Scenes/ClientNetworkManagement.tscn").instantiate()
		current_scene.add_child(client_management)

func start_edit(levelname = null):
		remove_child(current_scene)
		current_scene = load("res://Scenes/edit.tscn").instantiate()
		add_child(current_scene)
		if levelname != null:
			current_scene.call_deferred("edit",levelname)
			#current_scene.edit(levelname)
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
