extends Node


var settings: Settings
# Called when the node enters the scene tree for the first time.
func _ready():
	if DirAccess.open("user://").file_exists("settings.tres"):
		print("Settings loaded")
		settings = ResourceLoader.load("user://settings.tres")
	else:
		print("Settings saved")
		settings = Settings.new()
		ResourceSaver.save(settings,"user://settings.tres")
	#DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_FULLSCREEN)
	#DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_WINDOWED)
	# this changes the view mode
	
	# may only prepare api for usage if settings are loaded
	ApiAccess.prepare_api()

