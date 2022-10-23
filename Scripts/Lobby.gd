extends Node3D


@onready var levellist = $LevelSelectionScreen/SubViewport/LevelList
# Called when the node enters the scene tree for the first time.
func _ready():
	pass
	levellist.set_display_size(2)
	levellist.set_level_list(["Test"])


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
