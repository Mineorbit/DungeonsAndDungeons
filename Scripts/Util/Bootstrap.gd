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
	if started:
		return
	var dir_access = DirAccess.open("user://")
	dir_access.make_dir("level")
	dir_access.make_dir("level/localLevels")
	dir_access.make_dir("level/downloadLevels")
	started = true
	var server = should_be_server or is_server or OS.has_feature("Server") or "--server" in OS.get_cmdline_args()
	print(OS.get_cmdline_args())
	if server:
		print("===Starting Server===")
		start_server()
	else:
		print("===Starting Dungeons And Dungeons===")
		start_main_menu()
	

func start_main_menu():
	var t = LoadingScreen.open() 
	if t == null:
		
		remove_child(current_scene)
		current_scene = load("res://Scenes/menu.tscn").instantiate()
		add_child(current_scene)
		LoadingScreen.close()
		return
		
	t.timeout.connect(
		func():
		remove_child(current_scene)
		current_scene = load("res://Scenes/menu.tscn").instantiate()
		add_child(current_scene)
		LoadingScreen.close()
	)

func _process(_delta):
	if not started:
		if not OS.is_debug_build():
			start(false)
		elif Input.is_action_just_pressed("Client"):
			start(false)
		elif Input.is_action_just_pressed("Server") or "--server" in OS.get_cmdline_args():
			start(true)

func start_server():
	LoadingScreen.close()
	current_scene = load("res://Scenes/play.tscn").instantiate()
	add_child(current_scene)
	var server_management = load("res://Scenes/ServerNetworkManagement.tscn").instantiate()
	current_scene.add_child(server_management)


func start_play():
		LoadingScreen.open()
		remove_child(current_scene)
		current_scene = load("res://Scenes/play.tscn").instantiate()
		add_child(current_scene)
		var client_management = load("res://Scenes/ClientNetworkManagement.tscn").instantiate()
		current_scene.add_child(client_management)
		LoadingScreen.close()

func start_edit(levelname = null, new_level = false):
	
		
	LoadingScreen.open().timeout.connect(
		func():
			print("Starting Edit")
			var t = Thread.new()
			t.start(func():
				remove_child(current_scene)
				current_scene = load("res://Scenes/edit.tscn").instantiate()
			# prepare edit
				add_child(current_scene)
				current_scene.prepare_edit(null)
				if new_level:
					Constants.World.level.level_name = levelname
					Constants.World.level.save()
				else:
					current_scene.prepare_edit(levelname)
				LoadingScreen.close()
			)
	)
		#t.start(func():
		#)
			#current_scene.edit(levelname)
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
