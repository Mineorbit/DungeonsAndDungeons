extends Spatial


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var chunkPrefab
onready var gridMap: GridMap = $levelobject_grid
# Called when the node enters the scene tree for the first time.
func _ready():
	chunkPrefab = load("res://Prefabs/Chunk.tscn")
	
	

export var level_name = "Test"

func setup_new():
	for i in range(-8,8):
		for j in range(-8,8):
			add(Constants.Default_Floor,Vector3(i,0,j))

func save():
	print("Saving Level "+level_name)
	var dir = Directory.new()
	dir.make_dir("user://level")
	dir.make_dir("user://level/"+level_name)
	var save_game = File.new()
	save_game.open("user://level/"+level_name+"/index.json", File.WRITE)
	save_game.store_line(level_name)
	save_game.close()
	print("There are "+str(chunks.size())+" Chunks")
	for c in chunks.keys():
		var chunk = chunks[c]
		var chunk_file = File.new()
		#var chunkstring = str(c.x) + "|"+str(c.y)+"|"+str(c.z)
		chunk_file.open("user://level/"+level_name+"/"+str(c), File.WRITE)
		var levelObjects = chunk.get_level_objects()
		for object in levelObjects:
			chunk_file.store_line(str(object.levelObjectData.levelObjectId)+"|"+str(object.x)+"|"+str(object.y)+"|"+str(object.z))
		chunk_file.close()
		print(levelObjects.size())

func load(path):
	pass

var chunks = {}

func get_chunk(position):
	var chunkPosition = get_chunk_position(position)

func get_chunk_position(position):
	return Vector3(int(floor(position.x/8)),int(floor(position.y/8)),int(floor(position.z/8)))
	

func add_chunk(position):
	var chunkPosition = get_chunk_position(position)
	var chunk = chunkPrefab.instance()
	chunk.level = self
	chunk.global_transform.origin = 8*chunkPosition
	chunks[chunkPosition] = chunk
	add_child(chunk)
	return chunk
	

func add(levelObjectData,position):
	var chunk = get_chunk(position)
	if(chunk == null):
		chunk = add_chunk(position)
	
	if(levelObjectData.tiled):
		var pos = gridMap.world_to_map(position)
		gridMap.set_cell_item(pos.x,pos.y,pos.z,levelObjectData.tileIndex)
	else:
		print("Other")
		


func remove(pos):
	var position = gridMap.world_to_map(pos)
	gridMap.set_cell_item(position.x,position.y,position.z,-1)


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
