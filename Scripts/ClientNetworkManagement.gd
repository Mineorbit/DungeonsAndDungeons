extends Node3D

var peer


# Called when the node enters the scene tree for the first time.
func _ready():
	peer = ENetMultiplayerPeer.new()
	#multiplayer.peer_connected.connect(self.create_player)
	
	#multiplayer.peer_disconnected.connect(self.destroy_player)
	peer.create_client(Constants.remoteAddress,13565)
	multiplayer.peer_connected.connect(connected)
	multiplayer.set_multiplayer_peer(peer)
	Constants.World.players.player_added.connect(player_created)
	MultiplayerConstants.on_local_id_set.connect(func(id):id_set = true)
	#start an empty world
	#get_parent().world.start()
	print("Connected signal to "+str(Constants.World))
	Constants.World.on_entity_spawned.connect(disable_local_computations)
	
	Constants.entity_control_function = func():
		print("Connected signal to "+str(Constants.World))
		Constants.World.on_entity_spawned.connect(disable_local_computations)
	Signals.on_new_world_created.connect(Constants.entity_control_function)


func disable_local_computations(entity):
	print(str(Constants.id)+" Muting Entity "+str(entity))
	entity.ready.connect(func():
		entity.set_physics_process(false)
		entity.set_process(false)
		)

var id_set = false
var player_exists = false


func connected(id):
	Constants.set_mode(3)
	print("Connected with "+str(peer.get_unique_id()))
	Constants.id = peer.get_unique_id()



func player_created(player):
	Constants.World.players.playerControllers.create_player_camera()
	# temporary
	player.playercontroller = null
	
	var playermodel = player.get_node("PlayerModel")
	
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

var camera_for_local_player_set = false

func setup_local_controls():
	if id_set and player_exists:
		if not camera_for_local_player_set:
			camera_for_local_player_set = true
			print(str(Constants.id)+" ID: "+str(MultiplayerConstants.local_id))
			Constants.World.players.playerControllers.camera.player = Constants.World.players.get_player(MultiplayerConstants.local_id)
			get_parent().spawn_player_hud()

func _process(delta):
	pass
