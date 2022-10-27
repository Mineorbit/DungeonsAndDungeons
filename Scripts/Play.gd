extends Node3D

@onready var world = $World
var lobby

@onready var chunk_streamer_prefab = load("res://Prefabs/ChunkStreamer.tscn")
# Called when the node enters the scene tree for the first time.
func _ready():
	pass
	Signals.game_won.connect(start_lobby)
	# only if is on server change this in local mode
	# start_lobby()
	
func start_lobby():
	get_tree().paused = true
	remove_chunk_streamers()
	world.end()
	lobby = load("res://Prefabs/Lobby.tscn").instantiate()
	add_child(lobby)
	world.players.spawn()
	get_tree().paused = false



func start_round():
	get_tree().paused = true
	world.start("Test",true)
	for i in range(4):
		add_chunk_streamer_for_player(i)
	#lobby.end()
	world.players.spawn()
	remove_child(lobby)
	get_tree().paused = false


func remove_chunk_streamers():
	if Constants.currentLevel == null:
		print("No Chunk Streamers to remove")
		return
	for streamer in Constants.currentLevel.ChunkStreamers.get_children():
		streamer.queue_free()

func add_chunk_streamer_for_player(i):
	var servernetworking = $ServerNetworkManagement
	if servernetworking.id_to_local_id[i] == null:
		return
	var id = servernetworking.id_to_local_id[i]
	var new_chunk_streamer = chunk_streamer_prefab.instantiate()
	new_chunk_streamer.name = str(id)
	new_chunk_streamer.target = world.players.get_player(i)
	Constants.currentLevel.ChunkStreamers.add_child(new_chunk_streamer)

func add_player(id):
	world.players.spawn_player(id)

func add_player_controller(id = 0,peer_id = 0):
	var playercontroller = world.players.spawn_player_controller(id,peer_id)

