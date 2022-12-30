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
	#start an empty world
	#get_parent().world.start()

func connected(id):
	Constants.set_mode(3)
	print("Connected with "+str(peer.get_unique_id()))
	Constants.id = peer.get_unique_id()


func player_created(player):
	if MultiplayerConstants.local_id != -1 and Constants.playerCamera.player != get_parent().world.players.get_player(MultiplayerConstants.local_id):
		get_parent().world.players.playerControllers.camera.player = get_parent().world.players.get_player(MultiplayerConstants.local_id)



func _process(delta):
	#this is terrible and needs to be adjusted
	pass
