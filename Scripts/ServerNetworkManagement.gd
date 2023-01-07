extends Node3D



# Called when the node enters the scene tree for the first time.
func _ready():
	Constants.set_mode(2)
	var peer = ENetMultiplayerPeer.new()
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
	print("LOL2 "+str(MultiplayerConstants.local_id_to_id))
	MultiplayerConstants.rpc_id(id,"set_local_id",i)
	get_parent().add_player(i)
	var newplayercontroller = get_parent().add_player_controller(i,id)
	newplayercontroller.is_active = false
	var campos = load("res://Prefabs/PlayerCameraPosition.tscn").instantiate()
	campos.name = str(id)
	get_parent().add_child(campos)
	MultiplayerConstants.player_cameras[i] = campos
	# spawn player camera position
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if Input.is_action_just_pressed("Connect"):
		var player0 = Constants.World.players.playerEntities.get_node("0")
		player0.remove_child(player0.get_node("MultiplayerSynchronizer"))
		Constants.World.players.set_start_positions()
