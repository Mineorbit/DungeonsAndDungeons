extends Node3D

var peer: MultiplayerPeer


# Called when the node enters the scene tree for the first time.
func _ready():
	
	Constants.World.players.player_added.connect(player_created)
	Constants.World.on_entity_spawned.connect(disable_local_computations)
	MultiplayerConstants.on_local_id_set.connect(func(id):id_set = true)
	tree_exiting.connect(close_connection)
	peer = ENetMultiplayerPeer.new()
	peer.create_client(Constants.remoteAddress,13565)
	multiplayer.peer_connected.connect(connected)
	multiplayer.peer_disconnected.connect(disconnected)
	multiplayer.set_multiplayer_peer(peer)
	Constants.World.players.playerControllers.child_entered_tree.connect(playercontroller_created)

func close_connection():
	Constants.id = 0
	peer.close()



func disable_local_computations(entity):
	entity.ready.connect(func():
		entity.set_physics_process(false)
		entity.set_process(false)
		if entity.model != null:
			entity.model.set_physics_process(false)
			entity.model.set_process(false)
		)

var id_set = false
var player_exists = false


func connected(id):
	Constants.set_mode(3)
	print("Connected with "+str(peer.get_unique_id()))
	Constants.id = peer.get_unique_id()

func disconnected(id):
	print("Disconnected: "+str(id))


func playercontroller_created(playercontroller):
	if str(playercontroller.name).to_int() == Constants.id:
		playercontroller.player = Constants.World.players.get_player(MultiplayerConstants.local_id)
	playercontroller.set_active(str(playercontroller.name).to_int() == Constants.id)

func player_created(player):
	print(str(Constants.id)+" Player Created")
	
	var playermodel = player.get_node("Model")
	
	
	# need to cancel on ready
	player.ready.connect(
		func():
			player.set_physics_process(false)
			player.set_process(false)
	)
	playermodel.ready.connect(
		func():
			playermodel.set_physics_process(false)
			playermodel.set_process(false)
	)
	
	player_exists = true
	setup_local_controls()


func setup_local_controls():
	if id_set and player_exists:
		#Constants.PlayerCamera.player = Constants.World.players.get_player(MultiplayerConstants.local_id)
		get_parent().spawn_player_hud()
