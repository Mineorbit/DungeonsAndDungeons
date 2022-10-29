extends Node


# Called when the node enters the scene tree for the first time.
func _ready(): 
	DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_FULLSCREEN)
	DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_WINDOWED)
	# this changes the view mode


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
