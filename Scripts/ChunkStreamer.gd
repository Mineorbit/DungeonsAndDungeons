extends Node3D


@export var target_player_network_id = 0
@export var target = null
var loadedChunks = []
# Called when the node enters the scene tree for the first time.
func _ready():
	loadedChunks = []


@rpc
func stream_chunk(data):
	var base_position = data[0]
	print("Loading new Chunk "+str(base_position)+" for "+str(target_player_network_id))
	data.erase(base_position)
	base_position *= 8
	print(base_position)
	for object in data:
		Constants.currentLevel.add_from_string(base_position,object)
	


func load_chunk(location):
	loadedChunks.append(location)
	var chunk = Constants.currentLevel.get_chunk_by_chunk_position(location)
	if chunk == null:
		print("There was no Chunk at "+str(location))
		return
	var chunk_instances = chunk.get_level_object_instances()
	var chunk_data = [location]
	for chunk_instance in chunk_instances:
		chunk_data.append(chunk_instance.serialize())
	rpc_id(target_player_network_id,"stream_chunk",chunk_data)



func test(position):
	if target_player_network_id == 0:
		return
	var currentChunk = Constants.currentLevel.get_chunk_position(position)
	if not loadedChunks.has(currentChunk):
		load_chunk(currentChunk)
	

func _physics_process(delta):
	if target != null:
		global_transform.origin = target.global_transform.origin
	
	for i in range(-1,1):
		for j in range(-1,1):
			for k in range(-1,1):
				test(global_transform.origin+(8*Vector3(i,j,k)))
