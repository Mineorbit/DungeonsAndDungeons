extends Spatial


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var chunkPrefab
var levelObjectPrefab
onready var gridMap: GridMap = $levelobject_grid
# Called when the node enters the scene tree for the first time.
func _ready():
	chunkPrefab = load("res://Prefabs/Chunk.tscn")
	levelObjectPrefab = load("res://Prefabs/LevelObject.tscn")
	

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
	dir.make_dir("user://level/"+level_name+"/chunks")
	var save_game = File.new()
	save_game.open("user://level/"+level_name+"/index.json", File.WRITE)
	save_game.store_line(level_name)
	save_game.close()
	print("There are "+str(chunks.size())+" Chunks")
	for c in chunks.keys():
		var chunk = chunks[c]
		var chunk_file = File.new()
		#var chunkstring = str(c.x) + "|"+str(c.y)+"|"+str(c.z)
		chunk_file.open("user://level/"+level_name+"/chunks/"+str(c), File.WRITE)
		var levelObjects = chunk.get_level_objects()
		for object in levelObjects:
			chunk_file.store_line(str(object.levelObjectData.levelObjectId)+"|"+str(object.x)+"|"+str(object.y)+"|"+str(object.z))
		chunk_file.close()

func clear():
	gridMap.clear()
	for chunk in chunks.keys():
		for child in chunks[chunk].get_children():
			child.queue_free()
		chunks[chunk].queue_free()
	chunks.clear()

func load(level_name):
	clear()
	var path = "user://level/"+level_name
	var dir = Directory.new()
	if dir.open(path+"/chunks") == OK:
		dir.list_dir_begin()
		var file_name = dir.get_next()
		while file_name != "":
			if dir.current_is_dir():
				pass
			else:
				var chunkplace = file_name.substr(1,-1)
				chunkplace = chunkplace.trim_suffix(")")
				var coords = chunkplace.split(", ")
				var x = int(coords[0])*8
				var y = int(coords[1])*8
				var z = int(coords[2])*8
				var base_position = Vector3(x,y,z)
				var file = File.new()
				file.open(path+"/chunks/"+file_name, File.READ)
				var line = file.get_line()
				while line != "":
					if immediate:
						add_from_string(base_position,line)
					else:
						toAdd.append([base_position,line])
					line = file.get_line()
				file.close()
			file_name = dir.get_next()
		print("Loaded level")
	else:
		print("An error occurred when trying to access the path.")

var immediate = false

func add_from_string(base_position,line):
	var lineData = line.split("|")
	var id = int(lineData[0])
	var i = int(lineData[1])
	var j = int(lineData[2])
	var k = int(lineData[3])
	var levelObjectData = Constants.LevelObjectData[id]
	var pos = Vector3(i,j,k)
	add(levelObjectData, base_position+pos)

var toAdd = []

func _process(delta):
	if toAdd.size() > 0:
		var result = toAdd[0]
		toAdd.remove(0)
		add_from_string(result[0],result[1])
	
var chunks = {}

func get_chunk(position):
	var chunkPosition = get_chunk_position(position)
	if chunks.has(chunkPosition):
		return chunks[chunkPosition]

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
	
	var pos = gridMap.world_to_map(position)
	if(levelObjectData.tiled):
		gridMap.set_cell_item(pos.x,pos.y,pos.z,levelObjectData.tileIndex)
	else:
		var new_level_object = levelObjectPrefab.instance()
		chunk.add_child(new_level_object)
		new_level_object.global_transform.origin = pos
		var level_object_dupe: Spatial = get_tree().root.get_node("LevelObjects/"+levelObjectData.name).duplicate()
		new_level_object.add_child(level_object_dupe)
		new_level_object.levelObjectData = levelObjectData
		level_object_dupe.translation = Vector3.ZERO


func remove(pos):
	var position = gridMap.world_to_map(pos)
	gridMap.set_cell_item(position.x,position.y,position.z,-1)
	var chunk = get_chunk(position)
	for levelObject in chunk.get_children():
		if (position - levelObject.global_transform.origin).length() < 0.5:
			levelObject.queue_free()
			return


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
