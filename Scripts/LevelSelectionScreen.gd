extends MeshInstance3D

@onready var enterArea: Area3D = $EnterArea
@onready var camera: Camera3D = $LevelListCamera
@onready var selectionArea: Area3D = $SelectionArea
@onready var interface = $Interface
@onready var checkboxes = $Interface/CheckBoxes
@onready var levellist = $Interface/LevelList
@onready var refreshButton: Button = $Interface/Refresh
@onready var cursors = $Interface/Cursors

#original owner is server
var owner_id = 1
#change to local player inside
var has_sent = false



var player_cursors = {}

var localcursor = null
var local_player_inside = false
# Called when the node enters the scene tree for the first time.
func _ready():
	has_sent = false
	enterArea.body_entered.connect(player_entered)
	enterArea.body_exited.connect(player_left)
	multiplayer.peer_connected.connect(update_interface_player_connect)
	multiplayer.peer_disconnected.connect(update_interface_player_disconnect)

func update_interface_player_connect(id):
	update_interface_owner()
	# interface has to be owned by one player because else Input.parse_action does not work.
	

func update_interface_player_disconnect(id):
	if Constants.id == 1:
		if owner_id == id:
			update_interface_owner()

# allways inform all clients who the owner is
func update_interface_owner():
	# only server assigns the owner
	if Constants.id != 1:
		return
	var new_owner_id = MultiplayerConstants.get_first_connected()
	print("Update interface owner on all clients")
	owner_id = new_owner_id
	#trigger update on other clients
	rpc("set_interface_owner",owner_id)
	print("["+str(Constants.id)+"] Changing Interface Owner to "+str(new_owner_id))
	interface.get_node("LevelList/LevelListNetworking").set_auth(owner_id)


#only called by server

@rpc(any_peer)
func set_interface_owner(id):
	print("["+str(Constants.id)+"] Received Interface Owner to "+str(id))
	owner_id = id
	interface.get_node("LevelList/LevelListNetworking").set_auth(owner_id)
	interface.get_node("CursorSpawner").set_multiplayer_authority(id)
	interface.get_node("CheckboxSpawner").set_multiplayer_authority(id)
	if owner_id == Constants.id:
		ApiAccess.levels_fetched.connect(load_level_list)
		refreshButton.pressed.connect(refresh_level_list)
		refresh_level_list()




func player_entered(player):
	# server does not have to continue
	if Constants.id == 1:
		return
	if Constants.World.players.playerControllers.camera.player == player:
		camera.current = true
		Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
		local_player_inside = true
	if Constants.id == owner_id:
		add_checkbox(player.name)
		camera.current = true
		create_cursor(player)


func create_cursor(player):
	var cursorprefab = load("res://Prefabs/MainMenu/LevelList/LevelListCursor.tscn")
	var cursor = cursorprefab.instantiate()
	cursor.name = str(MultiplayerConstants.local_id_to_id[player.id])
	cursors.add_child(cursor)

func remove_checkbox(player):
	checkboxes.remove_child(checkboxes.get_child(0))

func remove_cursor(player):
	var cursor = cursors.get_node(str(MultiplayerConstants.local_id_to_id[player.id]))
	cursors.remove_child(cursor)

func player_left(player):	
	if 1 == Constants.id:
		camera.current = false
		localcursor = null
		local_player_inside = false
		player_cursors[player.name] = null
	elif Constants.World.players.playerControllers.camera.player == player:
		camera.current = false
		Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
		local_player_inside = false
	if owner_id == Constants.id:
		remove_cursor(player)
		remove_checkbox(player)


func add_checkbox(local_id):
	if Constants.id != owner_id:
		return
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
	if can_start:
		get_parent().get_parent().rpc_id(1,"start_round",levellist.selected_level,levellist.selected_level_name)



func refresh_level_list():
	levellist.clear()
	ApiAccess.fetch_level_list()

func load_level_list(list):
	levellist.clear()
	levellist.enabled = true
	levellist.set_level_list(list)


@rpc
func end():
	camera.current = false
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	local_player_inside = false



func _input(event):
	var local_coord = true
	if local_player_inside:
		if interface != null:
			if event is InputEventMouseButton:
				interface.push_input(event, local_coord)
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
				interface.push_input(event, local_coord)




