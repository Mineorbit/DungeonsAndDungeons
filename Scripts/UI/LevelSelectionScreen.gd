extends MeshInstance3D

@onready var enterArea: Area3D = $EnterArea
@onready var camera: Camera3D = $LevelListCamera
@onready var selectionArea: Area3D = $SelectionArea
@onready var interface = $Interface
@onready var checkboxes = $Interface/CheckboxPanel/CheckBoxes
@onready var levellist = $Interface/LevelList
@onready var refreshButton: Button = $Interface/Refresh
@onready var cursors = $Interface/Cursors

#original owner is server
var owner_id = 1
#change to local player inside
var has_sent = false

signal owner_changed(owner_id)

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
	update_interface_owner()

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
	owner_id = new_owner_id
	#trigger update on other clients
	set_auth_on_objects(owner_id)
	rpc("set_interface_owner",owner_id)
	print("["+str(Constants.id)+"] Changing Interface Owner to "+str(new_owner_id))


func set_auth_on_objects(id):
	interface.get_node("LevelList/LevelListNetworking").set_auth(id)
	interface.get_node("CursorSpawner").set_multiplayer_authority(id)
	interface.get_node("CheckboxSpawner").set_multiplayer_authority(id)
	owner_changed.emit(id)
#only called by server

var initialize_list = false

@rpc("any_peer")
func set_interface_owner(id):
	if owner_id == id:
		return
	print(str(Constants.id)+" received Owner ID")
	owner_id = id
	set_auth_on_objects(owner_id)
	if owner_id == Constants.id:
		if not initialize_list:
			initialize_list = true
			ApiAccess.levels_fetched.connect(load_level_list)
			refresh_level_list()



func add_cursor(player):
	var cursorprefab = load("res://Prefabs/MainMenu/LevelList/LevelListCursor.tscn")
	var cursor = cursorprefab.instantiate()
	cursor.name = str(MultiplayerConstants.local_id_to_id[player.id])
	cursors.add_child(cursor)

func remove_cursor(player):
	var cursor = cursors.get_node(str(MultiplayerConstants.local_id_to_id[player.id]))
	cursors.remove_child(cursor)


var checkbox_list = {}

func add_checkbox(player):
	if Constants.id != owner_id:
		return
	
	var local_id = player.name
	var checkbox_pref = load("res://Prefabs/MainMenu/CheckBox.tscn")
	var checkbox = checkbox_pref.instantiate()
	checkbox.size = Vector2(0.125,0.125)
	checkbox.scale = Vector2(0.125,0.125)
	checkbox.name = str(local_id)
	checkbox.get_node("CheckBox").pressed.connect(start_round)
	checkboxes.add_child(checkbox)
	checkbox_list[local_id] = checkbox

func remove_checkbox(player):
	var local_id = player.name
	if local_id in checkbox_list:
		checkboxes.remove_child(checkbox_list[local_id])



func player_entered(player):
	# server does not have to continue
	if Constants.World.players.get_player(MultiplayerConstants.local_id) == player:
		camera.current = true
		Input.set_mouse_mode(Input.MOUSE_MODE_CONFINED_HIDDEN)
		#Input.set_mouse
		local_player_inside = true
	if Constants.id == owner_id:
		add_checkbox(player)
		add_cursor(player)

func player_left(player):
	if 1 == Constants.id:
		camera.current = false
		localcursor = null
		local_player_inside = false
		player_cursors[player.name] = null
	elif Constants.World.players.get_player(MultiplayerConstants.local_id) == player:
		camera.current = false
		Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
		Constants.World.players.get_player(MultiplayerConstants.local_id).playercontroller.playercamera.activate()
		local_player_inside = false
	if owner_id == Constants.id:
		remove_cursor(player)
		remove_checkbox(player)




func check_ready():
	if levellist.selected_level == null:
		return false
	for child in checkboxes.get_children():
		if not child.get_node("CheckBox").button_pressed:
			return false
	return true


func start_round():
	var can_start = check_ready()
	if can_start:
		get_parent().get_parent().rpc_id(1,"start_round",levellist.selected_level,levellist.selected_level_name)



func refresh_level_list():
	print(str(owner_id)+" "+str(Constants.id)+" requested Level List")
	if Constants.id != owner_id:
		pass
		return
	#levellist.clear()
	ApiAccess.fetch_level_list()

func load_level_list(list):
	print("List: "+str(list))
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
	#pass_inputs_to_interface(event)
	if event is InputEventMouseButton or event is InputEventMouseMotion:
		var is_foreign = false
		if event is InputEventMouseButton:
			if event.button_index > 32:
				is_foreign = true
				event.button_index = event.button_index - 32
		if event is InputEventMouseMotion:
			if event.pressure > 0.5:
				is_foreign = true
				event.pressure = 0
		if local_player_inside or is_foreign:
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
				if not is_foreign:
					event.position.x = rel_pos.x * interface.size.x
					event.position.y = rel_pos.y * interface.size.y 
				local_coord = false
				#print(str(is_foreign)+" "+str(event))
			if local_player_inside or is_foreign:
				interface.push_input(event, local_coord)




