extends MeshInstance3D

@onready var enterArea: Area3D = $EnterArea
@onready var camera: Camera3D = $LevelListCamera
@onready var selectionArea: Area3D = $SelectionArea
@onready var interface = $Interface
@onready var checkboxes = $Interface/CheckBoxes
@onready var levellist = $Interface/LevelList
@onready var refreshButton: Button = $Interface/Refresh

var owner_id = 0
#change to local player inside
var has_sent = false

# Called when the node enters the scene tree for the first time.
func _ready():
	enterArea.body_entered.connect(player_entered)
	enterArea.body_exited.connect(player_left)
	multiplayer.peer_connected.connect(first_player_own_interface)
	#if Constants.id == 1:
	#	get_parent().get_parent().world.players.player_spawned.connect(add_checkbox)

func first_player_own_interface(id):
	if Constants.id == 1:
		if not has_sent:
			has_sent = true
			owner_id = id
		rpc("set_spawner_owner",owner_id)
		interface.get_node("LevelList/LevelListNetworking").set_auth(owner_id)

#only called by server

@rpc
func set_spawner_owner(id):
	owner_id = id
	interface.get_node("LevelList/LevelListNetworking").set_auth(owner_id)
	if owner_id == Constants.id:
		pass
		ApiAccess.levels_fetched.connect(load_level_list)
		refreshButton.pressed.connect(refresh_level_list)


func spawnLevelInterface(id):
	pass
		#
		#refresh_level_list()
		
		
		#this should not be hardcoded
		#refreshlist.pressed.connect(refresh_level_list)






func player_entered(player):
	if Constants.playerCamera.player == player:
		camera.current = true
		Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
		local_player_inside = true
		#create_cursor()
	if 1 == Constants.id:
		camera.current = true


var cursor

func create_cursor():
	var cursorprefab = load("res://Prefabs/MainMenu/LevelList/LevelListCursor.tscn")
	cursor = cursorprefab.instantiate()
	cursor.name = str(Constants.id)
	#cursors.add_child(cursor)
	#player_cursors[player.name] = cursor


func player_left(player):
	if Constants.playerCamera.player == player:
		camera.current = false
		Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
		local_player_inside = false
		#cursor.queue_free()


	if 1 == Constants.id:
		camera.current = false
		localcursor = null
		local_player_inside = false
		player_cursors[player.name] = null


func add_checkbox(local_id):
	print("Added Checkbox")
	var checkbox_pref = load("res://Prefabs/MainMenu/ReadyCall.tscn")
	var checkbox: Button = checkbox_pref.instantiate()
	checkbox.name = str(local_id)
	checkbox.scale = Vector2(4,4)
	checkbox.pressed.connect(start_round)
	checkboxes.add_child(checkbox)


func check_ready():
	if levellist.selected_level == null:
		print("No Level was selected")
		return false
	for child in checkboxes.get_children():
		if not child.button_pressed:
			return false
	return true


func start_round():
	var can_start = check_ready()
	print("Can start: "+str(can_start))
	if can_start:
		get_parent().get_parent().start_round()



func refresh_level_list():
		levellist.clear()
		ApiAccess.fetch_level_list()

func load_level_list(list):
			levellist.clear()
			levellist.enabled = true
			levellist.set_level_list(list)



var player_cursors = {}

var localcursor = null
var local_player_inside = false



@rpc
func end():
	camera.current = false
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	local_player_inside = false

func _input(event):
	var local_coord = true
	if local_player_inside:
		if event is InputEventMouseButton or event is InputEventMouseMotion:
			var relative_pos = Vector2(event.position.x/get_viewport().size.x,event.position.y/get_viewport().size.y)
			var from = camera.project_ray_origin(event.position)
			var to = from + camera.project_ray_normal(event.position) * 100
			var raydir = PhysicsRayQueryParameters3D.new()
			raydir.from = from
			raydir.to = to
			raydir.collide_with_areas = true
			raydir.collide_with_bodies = false
			raydir.collision_mask = selectionArea.collision_mask
			
			var result = get_world_3d().direct_space_state.intersect_ray(raydir)
			if result.size() > 0:
				var pos = selectionArea.to_local(result.position)
				var rel_pos = Vector2(pos.x/0.9, pos.y/1.7)
				rel_pos = (rel_pos + Vector2(1,1))/2
				rel_pos.x = clamp(1-rel_pos.x,0,1)
				rel_pos.y = clamp(1-rel_pos.y,0,1)
				event.position.x = rel_pos.x * interface.size.x
				event.position.y = rel_pos.y * interface.size.y 
				local_coord = false

	if owner_id == Constants.id:
		if interface != null:
			interface.push_input(event, local_coord)



