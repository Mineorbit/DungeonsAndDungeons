extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


# Called when the node enters the scene tree for the first time.
func _ready():
	if OS.has_feature("Server") or "--server" in OS.get_cmdline_args():
		print("Starting Server")
	else:
		print("Starting Dungeons And Dungeons")
		get_tree().change_scene("res://Scenes/menu.tscn")
		

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
