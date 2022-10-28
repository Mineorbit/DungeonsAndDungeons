extends Node3D

@onready var LevelSelectionScreen = $LevelSelectionScreen
@onready var levellist = $LevelSelectionScreen/SubViewport/LevelList
@onready var refreshlist: Button = $LevelSelectionScreen/SubViewport/Refresh
@onready var checkboxes = $LevelSelectionScreen/SubViewport/ReadyCalls
# Called when the node enters the scene tree for the first time.
func _ready():
	pass
	if Constants.id == 1:
		refreshlist.pressed.connect(load_level_list)
	get_parent().world.players.player_spawned.connect(add_checkbox)
	#levellist.set_level_list(["Test"])

func add_checkbox(local_id):
	print("Added Checkbox")
	var checkbox_pref = load("res://Prefabs/MainMenu/ReadyCall.tscn")
	var checkbox: Button = checkbox_pref.instantiate()
	checkbox.name = str(local_id)
	checkbox.scale = Vector2(4,4)
	checkbox.pressed.connect(start_round)
	checkboxes.add_child(checkbox)

func start_round():
	var can_start = check_ready()
	get_parent().start_round()

func end():
	LevelSelectionScreen.rpc("end")

func check_ready():
	for child in checkboxes.get_children():
		if not child.button_pressed:
			return false
	return true

func load_level_list():
		if Constants.id == 1:
			levellist.clear()
			levellist.enabled = true
			# fetch levellist
			levellist.set_level_list([])
	

func _process(delta):
	pass
