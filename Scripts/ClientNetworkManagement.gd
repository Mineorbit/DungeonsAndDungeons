extends Node3D


# Called when the node enters the scene tree for the first time.
func _ready():
	var peer = ENetMultiplayerPeer.new()
	#multiplayer.peer_connected.connect(self.create_player)
	
	#multiplayer.peer_disconnected.connect(self.destroy_player)
	peer.create_client("127.0.0.1",13565)
	multiplayer.peer_connected.connect(connected)
	multiplayer.set_multiplayer_peer(peer)

func connected(id):
	print("Connected with "+str(id))

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
