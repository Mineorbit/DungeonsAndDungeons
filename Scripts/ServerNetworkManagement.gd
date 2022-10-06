extends Node3D


var id_to_local_id = [null,null,null,null]

# Called when the node enters the scene tree for the first time.
func _ready():
	var peer = ENetMultiplayerPeer.new()
	var server_camera = load("res://Prefabs/Camera.tscn").instantiate()
	add_child(server_camera)
	server_camera.global_transform.origin = Vector3(0,6,0)
	#multiplayer.peer_connected.connect(self.create_player)
	
	#multiplayer.peer_disconnected.connect(self.destroy_player)
	peer.create_server(13565)
	multiplayer.set_multiplayer_peer(peer)
	multiplayer.peer_connected.connect(new_player)
	get_parent().world.start()


func new_player(id):
	var i = 0
	while(id_to_local_id[i] != null):
		i = i + 1
	id_to_local_id[i] = id
	
	print("New CONNECTION: "+str(id)+ " LOCAL ID: "+str(i))


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
