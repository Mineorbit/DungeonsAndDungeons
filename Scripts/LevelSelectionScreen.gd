extends MeshInstance3D

@onready var enterArea: Area3D = $EnterArea
@onready var camera: Camera3D = $LevelListCamera
@onready var selectionArea: Area3D = $SelectionArea

var levellist
var interface: Viewport
var cursors
var refreshlist
var checkboxes
# Called when the node enters the scene tree for the first time.
func _ready():
	enterArea.body_entered.connect(player_entered)
	enterArea.body_exited.connect(player_left)
	spawnLevelInterface()


#change to local player inside

func spawnLevelInterface():
	if Constants.id != 1:
		get_parent().get_parent().world.players.player_spawned.connect(add_checkbox)
		print("spawning interface")
		var prefab = load("res://Prefabs/LevelInterface.tscn")
		interface = prefab.instantiate()
		
		add_child(interface)
		#this should not be hardcoded
		levellist = interface.get_child(1)
		cursors = interface.get_child(2)
		refreshlist = interface.get_child(3)
		checkboxes = interface.get_child(4)
		refreshlist.pressed.connect(refresh_level_list)
		ApiAccess.levels_fetched.connect(load_level_list)
		refresh_level_list()
		var newmaterial = StandardMaterial3D.new()
		newmaterial.albedo_texture = interface.get_texture()
		material_override = newmaterial







func player_entered(player):
	if Constants.playerCamera.player == player:
		camera.current = true
		Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
		local_player_inside = true
		create_cursor()
	if 1 == Constants.id:
		camera.current = true


var cursor

func create_cursor():
	var cursorprefab = load("res://Prefabs/MainMenu/LevelList/LevelListCursor.tscn")
	cursor = cursorprefab.instantiate()
	cursor.name = str(Constants.id)
	cursors.add_child(cursor)
	#player_cursors[player.name] = cursor
	

func player_left(player):
	if Constants.playerCamera.player == player:
		camera.current = false
		Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
		local_player_inside = false
		cursor.queue_free()

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
	if Constants.id == 1:
		levellist.clear()
		ApiAccess.fetch_level_list()

func load_level_list(list):
		if Constants.id == 1:
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
	if Constants.id == 1:
		if interface != null:
			interface.push_input(event, true)
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
				interface.push_input(event, false)



