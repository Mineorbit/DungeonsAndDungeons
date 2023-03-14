extends TabContainer


func set_main_volume(value):
	AudioServer.set_bus_volume_db(AudioServer.get_bus_index("Master"), log(0.0125*value))
