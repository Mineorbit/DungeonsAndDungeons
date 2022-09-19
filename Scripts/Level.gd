extends NavigationRegion3D


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var chunkPrefab
var levelObjectPrefab
var interactiveLevelObjectPrefab


var changedChunks = 0

# Called when the node enters the scene tree for the first time.
func _ready():
	Constants.currentLevel = self
	chunkPrefab = load("res://Prefabs/Chunk.tscn")
	levelObjectPrefab = load("res://Prefabs/LevelObject.tscn")
	interactiveLevelObjectPrefab = load("res://Prefabs/InteractiveLevelObject.tscn")

var level_name = "Test"

func setup_new():
	for i in range(-8,8):
		for j in range(-8,8):
			add(Constants.Default_Floor,Vector3(i,0,j))


func reset():
	print("=== Reseting Level ===")
	for chunk in chunks.values():
		for levelobject in chunk.get_level_objects():
			if levelobject.has_method("reset"):
				levelobject.reset()
			if levelobject.has_method("clearSignals"):
				levelobject.clearSignals()
			if levelobject.has_method("deactivate"):
				levelobject.deactivate()
				
	clear_entities()
	Constants.buffer()
	Constants.buffer()
	Constants.buffer()
	
	for entity in Constants.currentEntities.get_children():
		if entity.has_method("reset"):
			entity.reset()
		

var changes = true
#this is a start routine for a level
func start():
	print("===Starting Level===")
	reset()
	
	
	# This is to prevent instant level restart in edit mode, because player position does not get updated correctly
	Constants.buffer()
	Constants.buffer()
	Constants.buffer()
	Constants.buffer()
	Constants.buffer()
	Constants.buffer()
	
	for chunk in chunks.values():
		for object in chunk.levelObjects.get_children():
			if object.has_method("prepare_for_navmesh_build"):
				object.prepare_for_navmesh_build()
				
	
	for map in NavigationServer3D.get_maps():
		NavigationServer3D.map_set_edge_connection_margin(map,2)


	for chunk in chunks.values():
		if chunk.change_in_chunk:
			changedChunks = changedChunks + 1
			
	if changedChunks > 0:
		for chunk in chunks.values():
			chunk.update_navigation()
		await changedChunks == 0
	changedChunks = 0
	for map in NavigationServer3D.get_maps():
		NavigationServer3D.map_set_edge_connection_margin(map,2)
	

	for chunk in chunks.values():
		for object in chunk.levelObjects.get_children():
			if object.has_method("restore_after_navmesh_build"):
				object.restore_after_navmesh_build()
	
	
	for chunk in chunks.values():
		for object in chunk.levelObjects.get_children():
			if object.has_method("attachSignals"):
				object.attachSignals()
			if object.has_method("start"):
				object.start()
				
	for entity in Constants.currentEntities.get_children():
		if entity.has_method("start"):
			entity.start()

func get_interactive_objects():
	return Constants.interactiveLevelObjects.values()


func delete_level(level_name):
	
	var chunkdirectory = Directory.new()
	var chunkpath = "user://level/"+level_name+"/chunks/"
	if chunkdirectory.open(chunkpath) == OK:
		chunkdirectory.list_dir_begin()
		var file_name = chunkdirectory.get_next()
		while file_name != "":
			if chunkdirectory.current_is_dir():
				print("Found directory?")
			else:
				chunkdirectory.remove(file_name)
			file_name = chunkdirectory.get_next()
	
	chunkdirectory.remove("user://level/"+level_name+"/chunks/")
	

func save():
	print("Saving Level "+level_name)
	var dir = Directory.new()
	
	delete_level(level_name)
	dir.make_dir("user://level")
	dir.make_dir("user://level/"+level_name)
	dir.make_dir("user://level/"+level_name+"/chunks")
	var save_game = File.new()
	save_game.open("user://level/"+level_name+"/index.json", File.WRITE)
	save_game.store_line(level_name)
	save_game.close()
	for c in chunks.keys():
		var chunk = chunks[c]
		var chunk_file = File.new()
		chunk_file.open("user://level/"+level_name+"/chunks/"+str(c), File.WRITE)
		var levelObjects = chunk.get_level_object_instances()
		for object in levelObjects:
			chunk_file.store_line(object.serialize())
		chunk_file.close()


func clear_entities():
	print("Clearing Entities")
	for entity in Constants.currentEntities.get_children():
		print("Removing "+str(entity))
		if entity.has_method("remove"):
			entity.remove()



func clear():
	clear_entities()
	for chunk in chunks.keys():
		for child in chunks[chunk].get_children():
			child.queue_free()
		chunks[chunk].queue_free()
	Constants.interactiveLevelObjects.clear()
	for key in Constants.numberOfPlacedLevelObjects.keys():
		Constants.numberOfPlacedLevelObjects[key] = 0
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
				
				var x = coords[0].to_int()*8
				var y = coords[1].to_int()*8
				var z = coords[2].to_int()*8
				var base_position = Vector3(x,y,z)
				print("Loading Chunk at Position "+str(base_position))
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
	var id = lineData[0].to_int()
	var i = lineData[1].to_int()
	var j = lineData[2].to_int()
	var k = lineData[3].to_int()
	var instance_id = null
	var connectedInteractiveObjects = null
	if lineData.size() > 4:
		instance_id = lineData[4].to_int()
		connectedInteractiveObjects = []
		var commalist = lineData[5].split("[")[1].split("]")[0].split(",")
		for instanceid in commalist:
			if instanceid == "":
				continue
			connectedInteractiveObjects.append(instanceid.to_int())
	var levelObjectData = Constants.LevelObjectData[id]
	var pos = Vector3(i,j,k)
	add(levelObjectData, base_position+pos,instance_id,connectedInteractiveObjects)

var toAdd = []

func _process(delta):
	if toAdd.size() > 0:
		var result = toAdd[0]
		toAdd.remove_at(0)
		add_from_string(result[0],result[1])
	
var chunks = {}

func get_chunk(position):
	var chunkPosition = get_chunk_position(position)
	if chunks.has(chunkPosition):
		return chunks[chunkPosition]


func add_chunk(position):
	var chunkPosition = get_chunk_position(position)
	var chunk = chunkPrefab.instantiate()
	chunk.level = self
	chunks[chunkPosition] = chunk
	add_child(chunk)
	chunk.global_transform.origin = 8*chunkPosition
	return chunk

	

func add(levelObjectData: LevelObjectData, position, unique_instance_id = null, connectedObjects = []):
	if(levelObjectData.maximumNumber != -1):
		if(Constants.numberOfPlacedLevelObjects[levelObjectData.levelObjectId] == levelObjectData.maximumNumber):
			return
	Constants.numberOfPlacedLevelObjects[levelObjectData.levelObjectId] = Constants.numberOfPlacedLevelObjects[levelObjectData.levelObjectId] + 1
	var chunk = get_chunk(position)
	if(chunk == null):
		chunk = add_chunk(position)
	chunk.change_in_chunk = true
	changes = true
	var pos = get_grid_position(position)
	if(levelObjectData.tiled):
		chunk.set_tile_level_object(pos,levelObjectData.tileIndex)
	else:
		var new_level_object
		if levelObjectData.interactive:
			new_level_object = interactiveLevelObjectPrefab.instantiate()	
		else:
			new_level_object = levelObjectPrefab.instantiate()
		
		chunk.levelObjects.add_child(new_level_object)
		new_level_object.global_transform.origin = Vector3(pos.x,pos.y,pos.z)
		# assign new inner levelobject
		var level_object_dupe: Node3D = get_tree().root.get_node("LevelObjects/LevelObjectList/"+levelObjectData.name).duplicate()
		new_level_object.add_child(level_object_dupe)
		level_object_dupe.transform.origin = levelObjectData.offset
		if(unique_instance_id != null):
			new_level_object.unique_instance_id = unique_instance_id
			new_level_object.connectedObjects = connectedObjects
			Constants.connection_added.emit(unique_instance_id,connectedObjects)	
		if (new_level_object.has_method("sign_up")):
			new_level_object.sign_up()
		new_level_object.contained_level_object = level_object_dupe
		new_level_object.levelObjectData = levelObjectData
		if level_object_dupe.has_method("setup"):
			level_object_dupe.setup()


func get_chunk_position(startpos):
	var gridposition = get_grid_position(startpos)
	var chunkposition = Vector3(int(floor(float(gridposition.x)/8)),int(floor(float(gridposition.y)/8)),int(floor(float(gridposition.z)/8)))
	return chunkposition


func get_grid_position(pos):
	return Vector3(floor(pos.x),floor(pos.y),floor(pos.z))

func remove_by_object(objectToRemove):
	remove_level_object(objectToRemove)

func remove_by_position(pos: Vector3):
	var isRemoved = false
	var gridposition = get_grid_position(pos)
	var floatPosition = Vector3(gridposition.x,gridposition.y,gridposition.z)
	var chunk = get_chunk(gridposition)
	if chunk != null:
		for levelObject in chunk.get_level_objects():
			if (floatPosition - levelObject.global_transform.origin).length() < 0.5:
				remove_level_object(levelObject)
				isRemoved = true
				return isRemoved
		isRemoved = chunk.remove_tile_level_object(gridposition)
	return isRemoved

func remove_level_object(object):
	if object == null:
		return
	var chunk = get_chunk(object.global_transform.origin)
	chunk.change_in_chunk = true
	changes = true
	if object.has_method("on_remove"):
		object.on_remove()
	
	var removal_outgoing = []
	var removal_ingoing = []
	for second_object in get_interactive_objects():
		print("Checking "+str(second_object))
		if object.unique_instance_id in second_object.connectedObjects:
			removal_outgoing.append(second_object.unique_instance_id)
			second_object.connectedObjects.erase(object.unique_instance_id)
			
	removal_ingoing = object.connectedObjects
	
	
	print(removal_ingoing)
	print(removal_outgoing)
	for i in removal_ingoing:
		Constants.connection_removed.emit(i,[object.unique_instance_id])
		
	Constants.connection_removed.emit(object.unique_instance_id,removal_outgoing)
	
	
	Constants.interactiveLevelObjects.erase(object.unique_instance_id)
	chunk.levelObjects.remove_child(object)
	Constants.numberOfPlacedLevelObjects[object.levelObjectData.levelObjectId] = max(0,Constants.numberOfPlacedLevelObjects[object.levelObjectData.levelObjectId] - 1)

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
