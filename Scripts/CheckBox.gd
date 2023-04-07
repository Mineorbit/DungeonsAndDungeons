extends Control

var level_selection_screen

func _ready():
	level_selection_screen = get_parent().get_parent().get_parent().get_parent()
	print(level_selection_screen)
	set_checkbox_owner(level_selection_screen.owner_id)
	level_selection_screen.owner_changed.connect(set_checkbox_owner)

func set_checkbox_owner(id):
	get_node("MultiplayerSynchronizer").set_multiplayer_authority(id)
