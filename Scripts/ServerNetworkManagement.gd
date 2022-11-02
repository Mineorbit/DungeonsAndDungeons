extends Node3D



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
	multiplayer.peer_connected.connect(player_connected)
	multiplayer.peer_disconnected.connect(player_disconnected)
	get_parent().start_lobby()




func player_disconnected(id):
	var i = 0
	while MultiplayerConstants.local_id_to_id[i] != id:
		i = i + 1
	MultiplayerConstants.local_id_to_id[i] = null
	get_parent().remove_player(i)

func player_connected(id):
	var i = 0
	while(MultiplayerConstants.local_id_to_id[i] != null):
		i = i + 1
	MultiplayerConstants.local_id_to_id[i] = id
	MultiplayerConstants.rpc_id(id,"set_local_id",i)
	print("New CONNECTION: "+str(id)+ " LOCAL ID: "+str(i))
	get_parent().add_player(i)
	get_parent().add_player_controller(i,id)
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
