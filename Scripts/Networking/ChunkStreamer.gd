extends Node3D

var target_player_network_id = 0
@export var target: Node3D = null
var loadedChunks = []
# Called when the node enters the scene tree for the first time.
func _ready():
	target_player_network_id = str(name).to_int()
	loadedChunks = []


@rpc
func stream_chunk(data):
	add_from_function(data)

var objects = []

func add_from_function(data):
	var base_position = data[0]
	data.erase(base_position)
	base_position *= 8
	for object in data:
		objects.append([base_position,object])

func _process(delta):
	if Constants.World.level != null:
		if objects.size() > 0:
			var o = objects[0]
			var base_position = o[0]
			var object = o[1]
			objects.remove_at(0)
			Constants.World.level.add_from_string(base_position,object)


func load_chunk(location):
	loadedChunks.append(location)
	var chunk = Constants.World.level.get_chunk_by_chunk_position(location)
	if chunk == null:
		return
	var chunk_instances = chunk.get_level_object_instances()
	var chunk_data = [location]
	for chunk_instance in chunk_instances:
		chunk_data.append(chunk_instance.serialize())
	rpc_id(target_player_network_id,"stream_chunk",chunk_data)
	#print("Sending to "+str(target_player_network_id))
	#rpc_id(target_player_network_id,"stream_chunk")



func test(position):
	if target_player_network_id == 0:
		return
	if Constants.World.level == null:
		return
	var currentChunk = Constants.World.level.get_chunk_position(position)
	if not loadedChunks.has(currentChunk):
		load_chunk(currentChunk)
	

func _physics_process(delta):
	if target != null:
		global_transform.origin = target.global_transform.origin
	for i in range(-1,2):
		for j in range(-1,2):
			for k in range(-1,2):
				test(global_transform.origin+(8*Vector3(i,j,k)))
