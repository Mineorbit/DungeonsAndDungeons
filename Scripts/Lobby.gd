extends Node3D


@onready var levellist = $LevelSelectionScreen/SubViewport/LevelList
# Called when the node enters the scene tree for the first time.
func _ready():
	levellist.set_display_size(2)
	levellist.set_level_list(["1", "Test2","Test2","Test3","Test2","Test3","Test2","Test3","Test2","Test3"])

	#levellist.set_level_list(["Test"])

