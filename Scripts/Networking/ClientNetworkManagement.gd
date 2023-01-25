extends Node3D

var peer


# Called when the node enters the scene tree for the first time.
func _ready():
	peer = ENetMultiplayerPeer.new()
	#multiplayer.peer_connected.connect(self.create_player)
	
	#multiplayer.peer_disconnected.connect(self.destroy_player)
	peer.create_client(Constants.remoteAddress,13565)
	multiplayer.peer_connected.connect(connected)
	multiplayer.peer_disconnected.connect(disconnected)
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


func create_player_camera():
	var camera = load("res://Prefabs/PlayerCamera.tscn").instantiate()
	Constants.localPlayerCamera = camera
	add_child(camera)


func disable_local_computations(entity):
	print(str(Constants.id)+" Muting Entity "+str(entity))
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

func player_created(player):
	print(str(Constants.id)+" Player Created")
	# temporary
	player.playercontroller = null
	
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
		print(str(Constants.id)+" ID: "+str(MultiplayerConstants.local_id))
		PlayerCamera.player = Constants.World.players.get_player(MultiplayerConstants.local_id)
		get_parent().spawn_player_hud()

func _process(delta):
	pass
