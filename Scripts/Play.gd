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
	
func start_lobby():
	print(str(Constants.id)+" Starting Lobby")
	get_tree().paused = true
	remove_chunk_streamers()
	Constants.World.end()
	# have to instantiate new for multiplayer to work, else server crashes on new lobby start
	lobby = load("res://Prefabs/Lobby.tscn").instantiate()
	add_child(lobby)
	Constants.World.players.set_start_positions()
	get_tree().paused = false



#this is called once the level was downloaded
func complete_start_round():
	get_tree().paused = true
	# start world with level from downloads
	Constants.World.start(selected_level_name,true,true)
	for i in range(4):
		add_chunk_streamer_for_player(i)
	remove_child(lobby)
	lobby = null
	#Constants.World.players.spawn()
	Constants.World.players.set_start_positions()
	get_tree().paused = false

@rpc(any_peer)
func start_round(sel_lev,sel_lev_name):
	if multiplayer.get_remote_sender_id() != lobby.LevelSelectionScreen.owner_id:
		return 
	print("===Starting Round===")
	selected_level = sel_lev
	selected_level_name = sel_lev_name
	
	ApiAccess.download_level(selected_level)

func remove_chunk_streamers():
	if Constants.World.level == null:
		print("No Chunk Streamers to remove")
		return
	for streamer in Constants.World.level.ChunkStreamers.get_children():
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
	Constants.World.level.ChunkStreamers.add_child(new_chunk_streamer)

func add_player(id):
	Constants.World.players.spawn_player(id)
	

func remove_player(id):
	Constants.World.players.despawn_player(id)

func add_player_controller(id = 0,peer_id = 0):
	var playercontroller = Constants.World.players.spawn_player_controller(id,peer_id)
	print(playercontroller)
	return playercontroller

