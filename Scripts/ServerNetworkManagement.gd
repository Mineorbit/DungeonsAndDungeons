extends Node3D


var id_to_local_id = [null,null,null,null]
var chunk_streamer_prefab = load("res://Prefabs/ChunkStreamer.tscn")

# Called when the node enters the scene tree for the first time.
func _ready():
	Constants.set_mode(2)
	var peer = ENetMultiplayerPeer.new()
	#multiplayer.peer_connected.connect(self.create_player)
	var servercamera = load("res://Prefabs/ServerCamera.tscn").instantiate()
	add_child(servercamera)
	servercamera.global_transform.origin = Vector3(0,2,12)
	#multiplayer.peer_disconnected.connect(self.destroy_player)
	peer.create_server(13565)
	Constants.id = peer.get_unique_id()
	print("Server has Unique ID "+str(Constants.id))
	multiplayer.set_multiplayer_peer(peer)
	multiplayer.peer_connected.connect(new_player)
	start_lobby()


var lobby


func start_lobby():
	lobby = load("res://Prefabs/Lobby.tscn").instantiate()
	get_parent().add_child(lobby)



func start_round():
	get_parent().world.start("Test",true)
	for i in range(4):
		add_chunk_streamer_for_player(i)



func add_chunk_streamer_for_player(i):
	if id_to_local_id[i] == null:
		return
	var id = id_to_local_id[i]
	var new_chunk_streamer = chunk_streamer_prefab.instantiate()
	new_chunk_streamer.name = str(id)
	new_chunk_streamer.target_player_network_id = id
	new_chunk_streamer.target = get_parent().world.players.get_player(i)
	Constants.currentLevel.ChunkStreamers.add_child(new_chunk_streamer)

func new_player(id):
	var i = 0
	while(id_to_local_id[i] != null):
		i = i + 1
	id_to_local_id[i] = id
	MultiplayerConstants.rpc_id(id,"set_local_id",i)
	print("New CONNECTION: "+str(id)+ " LOCAL ID: "+str(i))
	get_parent().add_player(i)
	get_parent().add_player_controller(i,id)
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
