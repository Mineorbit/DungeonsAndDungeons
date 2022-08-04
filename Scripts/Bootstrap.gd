extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var current_scene = null
# Called when the node enters the scene tree for the first time.
func _ready():
	if OS.has_feature("Server") or "--server" in OS.get_cmdline_args():
		print("Starting Server")
	else:
		print("Starting Dungeons And Dungeons")
		remove_child(current_scene)
		current_scene = load("res://Scenes/menu.tscn").instantiate()
		add_child(current_scene)

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
