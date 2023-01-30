extends Node3D

var lobby

var selected_level
var selected_level_name

@onready var chunk_streamer_prefab = load("res://Prefabs/ChunkStreamer.tscn")
# Called when the node enters the scene tree for the first time.
func _ready():
	# only client should send back
	Constants.World.game_won.connect(start_lobby)
	ApiAccess.level_download_finished.connect(complete_start_round)

	# only if is on server change this in local mode
	# start_lobby()

@export var level_time: float = 0

var is_in_play = false

func start_lobby():
	is_in_play = false
	level_time = 0
	level_locked = false
	get_tree().paused = true
	remove_chunk_streamers()
	Constants.World.end()
	rpc("remove_level")
	# have to instantiate new for multiplayer to work, else server crashes on new lobby start
	lobby = load("res://Prefabs/Lobby.tscn").instantiate()
	add_child(lobby)
	Constants.World.players.set_start_positions()
	get_tree().paused = false


@rpc
func remove_level():
	Constants.World.end()

#this is called once the level was downloaded
func complete_start_round(levelname):
		
	print(str(Constants.id)+" Starting Level "+str(levelname))
	rpc("prepare_level")
	# maybe here await all players answers that they have created the level
	get_tree().paused = true
	# start world with level from downloads
	await Constants.World.start(selected_level_name,true,true)
	for i in range(4):
		add_chunk_streamer_for_player(i)
	remove_child(lobby)
	lobby = null
	#Constants.World.players.spawn()
	Constants.World.players.set_start_positions()
	
	get_tree().paused = false
	level_time = 0
	is_in_play = true

@rpc
func prepare_level():
	await Constants.World.prepare_level()
	Constants.World.level.level_object_added.connect(func(object):
		object.contained_level_object.ready.connect(func():
			print("Muting "+str(object.contained_level_object))
			object.contained_level_object.set_process(false)
			object.contained_level_object.set_physics_process(false))
		print("Muting "+str(object.contained_level_object))
		object.contained_level_object.set_process(false)
		object.contained_level_object.set_physics_process(false))
		

func _physics_process(delta):
	if is_in_play:
		level_time += delta

var level_locked = false


@rpc("any_peer")
func start_round(sel_lev,sel_lev_name):
	if multiplayer.get_remote_sender_id() != lobby.LevelSelectionScreen.owner_id:
		return 
	if level_locked:
		print("Level allready selected")
		return
	level_locked = true
	print("===Starting Round===")
	selected_level = sel_lev
	selected_level_name = sel_lev_name
	ApiAccess.download_level(selected_level)


func remove_chunk_streamers():
	if Constants.World.level == null:
		print("No Chunk Streamers to remove")
		return
	for streamer in Constants.World.ChunkStreamers.get_children():
		streamer.queue_free()

func spawn_player_hud():
	var playerhudpref = load("res://Prefabs/PlayerHUD.tscn")
	add_child(playerhudpref.instantiate())

func add_chunk_streamer_for_player(i):
	var servernetworking = $ServerNetworkManagement
	if MultiplayerConstants.local_id_to_id[i] == null:
		return
	var id = MultiplayerConstants.local_id_to_id[i]
	var new_chunk_streamer = chunk_streamer_prefab.instantiate()
	new_chunk_streamer.name = str(id)
	new_chunk_streamer.target = Constants.World.players.get_player(i)
	Constants.World.ChunkStreamers.add_child(new_chunk_streamer,true)

func add_player(id):
	var player = await Constants.World.players.spawn_player(id)
	player.on_entity_died.connect(func():
		respawn_player(id))

func respawn_player(id):
	print("Respawning Player "+str(id))
	var player = await Constants.World.players.spawn_player(id)
	player.on_entity_died.connect(func():
		respawn_player(id))

func remove_player(id):
	print("Removing Player "+str(id))
	Constants.World.players.despawn_player(id)


