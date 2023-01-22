extends Panel


func stop_property_edit():
	Signals.edited_interactive_level_object.emit(null)
