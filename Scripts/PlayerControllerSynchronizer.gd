extends MultiplayerSynchronizer

@export var input_direction: Vector3:
	set(val):
		if is_multiplayer_authority():
			input_direction = val
		else:
			get_parent().input_direction = val
