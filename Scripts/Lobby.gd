extends Node3D


@onready var levellist = $LevelSelectionScreen/SubViewport/LevelList
@onready var refreshlist: Button = $LevelSelectionScreen/SubViewport/Refresh
# Called when the node enters the scene tree for the first time.
func _ready():
	pass
	if Constants.id == 1:
		refreshlist.pressed.connect(load_level_list)
	#levellist.set_level_list(["Test"])

func load_level_list():
		if Constants.id == 1:
			levellist.clear()
			levellist.enabled = true
			levellist.set_level_list(["1", "Test2","Test2","Test3","Test2","Test3","Test2","Test3","Test2","Test3"])
	

func _process(delta):
	pass
