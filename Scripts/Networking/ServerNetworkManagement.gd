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
	Constants.World.players.player_added.connect(on_player_added)
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
	
	
	var player = await get_parent().add_player(i)
	var newplayercontroller = add_player_controller(i,id)
	newplayercontroller.ready.connect(func():
		newplayercontroller.set_process(false)
		newplayercontroller.set_physics_process(false)
	)
	newplayercontroller.get_node("PlayerCamera/CameraTarget").prepare_camera_target(player)
	newplayercontroller.set_process(false)
	newplayercontroller.set_physics_process(false)
	#MultiplayerConstants.player_cameras[i] = campos
	# spawn player camera position

func add_player_controller(id = 0,peer_id = 0):
	var playercontroller = Constants.World.players.spawn_player_controller(id,peer_id)
	return playercontroller


func on_player_added(player):
	var playercontroller = Constants.World.players.playerControllers.playerControllers[int(str(player.name))]
	# this only matters if player respawns
	if playercontroller != null:
		player.playercontroller = playercontroller
		player.playercontroller.player = player
		player.playercontroller.get_node("PlayerCamera/CameraTarget").prepare_camera_target(player)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if Input.is_action_just_pressed("Connect"):
		pass
		#Constants.World.players.set_start_positions()
