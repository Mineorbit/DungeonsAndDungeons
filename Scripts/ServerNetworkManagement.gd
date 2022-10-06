extends Node3D


# Called when the node enters the scene tree for the first time.
func _ready():
	var peer = ENetMultiplayerPeer.new()
	#multiplayer.peer_connected.connect(self.create_player)
	
	#multiplayer.peer_disconnected.connect(self.destroy_player)
	peer.create_server(13565)
	multiplayer.set_multiplayer_peer(peer)




# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
