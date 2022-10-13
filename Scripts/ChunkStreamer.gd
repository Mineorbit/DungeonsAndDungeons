extends Node3D


@export var target_player_network_id = 0
@export var target = null
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


@rpc
func stream_chunk(data):
	pass

var loadedChunks = []

func _physics_process(delta):
	if target_player_network_id == 0:
		return
	if target != null:
		global_transform.origin = target.global_transform.origin
	var currentChunk = Constants.currentLevel.get_chunk_position(global_transform.origin)
	if not loadedChunks.has(currentChunk):
		print("Loading new Chunk "+str(currentChunk)+" for "+str(target_player_network_id))
		loadedChunks.append(currentChunk)
		rpc_id(target_player_network_id,"stream_chunk",null)
