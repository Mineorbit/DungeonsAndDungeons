extends Node3D

@onready var LevelSelectionScreen = $LevelSelectionScreen
@onready var levellist = $LevelSelectionScreen/SubViewport/LevelList
@onready var refreshlist: Button = $LevelSelectionScreen/SubViewport/Refresh
@onready var checkboxes = $LevelSelectionScreen/SubViewport/ReadyCalls
# Called when the node enters the scene tree for the first time.
func _ready():
	if Constants.id == 1:
		refreshlist.pressed.connect(refresh_level_list)
		ApiAccess.levels_fetched.connect(load_level_list)
		get_parent().world.players.player_spawned.connect(add_checkbox)
		refresh_level_list()

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
	print("Can start: "+str(can_start))
	if can_start:
		get_parent().start_round()

func check_ready():
	if levellist.selected_level == null:
		print("No Level was selected")
		return false
	for child in checkboxes.get_children():
		if not child.button_pressed:
			return false
	return true

func end():
	LevelSelectionScreen.rpc("end")



func refresh_level_list():
	if Constants.id == 1:
		levellist.clear()
		ApiAccess.fetch_level_list()

func load_level_list(list):
		if Constants.id == 1:
			levellist.clear()
			levellist.enabled = true
			levellist.set_level_list(list)
	

func _process(delta):
	pass
