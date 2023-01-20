extends NavigationRegion3D


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var chunkPrefab
var levelObjectPrefab
var interactiveLevelObjectPrefab
@onready var Entities = $Entities
@onready var ChunkStreamers = $ChunkStreamers


var player_spawns = [null,null,null,null]

var player_goal = null

var numberOfPlacedLevelObjects = {}


var level_name = "Test"

var changedChunks = 0

# Called when the node enters the scene tree for the first time.
func _ready():
	chunkPrefab = load("res://Prefabs/Chunk.tscn")
	levelObjectPrefab = load("res://Prefabs/LevelObject.tscn")
	interactiveLevelObjectPrefab = load("res://Prefabs/InteractiveLevelObject.tscn")
	Entities.child_entered_tree.connect(
		func(entity):
			Constants.World.on_entity_spawned.emit(entity)
	)

func setup_new():
	for i in range(-8,8):
		for j in range(-8,8):
			add(Constants.Default_Floor,Vector3(i,0,j))


func reset():
	print("=== Reseting Level ===")
	clear_entities()
	resetting_level_objects()


func resetting_level_objects():
	for chunk in chunks.values():
		for levelobject in chunk.get_level_objects():
			if levelobject.has_method("reset"):
				levelobject.reset()
			if levelobject.has_method("clearSignals"):
				levelobject.clearSignals()
			if levelobject.has_method("deactivate"):
				levelobject.deactivate()
	# there may be new entities
	for entity in Entities.get_children():
		if entity.has_method("reset"):
			entity.reset()

func build_navigation():
	for chunk in chunks.values():
		for object in chunk.levelObjects.get_children():
			if object.has_method("prepare_for_navmesh_build"):
				object.prepare_for_navmesh_build()
		
	for map in NavigationServer3D.get_maps():
		NavigationServer3D.map_set_edge_connection_margin(map,2)
	for chunk in chunks.values():
		if chunk.change_in_chunk:
			changedChunks = changedChunks + 1
	
	print("Building Nav Mesh")
	# in base method
	await build_navigation_per_chunk()
	#await bake_navigation_mesh(false)
	
	print("Finished Building Nav Mesh")
	for map in NavigationServer3D.get_maps():
		NavigationServer3D.map_set_edge_connection_margin(map,2)
	for chunk in chunks.values():
		for object in chunk.levelObjects.get_children():
			if object.has_method("restore_after_navmesh_build"):
				object.restore_after_navmesh_build()				
	
	
func build_navigation_per_chunk():
	if changedChunks > 0:
		for chunk in chunks.values():
			await chunk.update_navigation()
	changedChunks = 0


var changes = true
#this is a start routine for a level
func start():
	print("=== Starting Level ===")
	await build_navigation()
	for chunk in chunks.values():
		for object in chunk.levelObjects.get_children():
			if object.has_method("attachSignals"):
				object.attachSignals()
			if object.has_method("start"):
				object.start()
				
	for entity in Entities.get_children():
		if entity.has_method("start"):
			entity.start()

var interactiveLevelObjects = {}

func get_interactive_objects():
	return interactiveLevelObjects.values()


func get_interactive_object(unique_id):
	return null

var entity_id = 0
func spawn_entity(entity):
	print("Spawning Entity "+str(entity)+" at "+str(entity.global_transform.origin)+" in World "+str(Constants.World))
	entity.name = str(entity_id)
	entity_id = entity_id + 1
	Constants.World.level.Entities.add_child(entity)


func delete_level(level_name):
	var level_folder = DirAccess.open("user://level/localLevels")
	if not level_folder.dir_exists(level_name):
		return
	var chunkpath = "user://level/localLevels/"+level_name+"/chunks/"
	var chunkdirectory = DirAccess.open(chunkpath)
	if true:
		chunkdirectory.list_dir_begin()
		var file_name = chunkdirectory.get_next()
		while file_name != "":
			if chunkdirectory.current_is_dir():
				print("Found directory?")
			else:
				chunkdirectory.remove(file_name)
			file_name = chunkdirectory.get_next()
	
	chunkdirectory.remove("user://level/localLevels/"+level_name+"/chunks/")
	

func save():
	print("Saving Level "+level_name)
	delete_level(level_name)
	
	DirAccess.make_dir_absolute("user://level/localLevels")
	DirAccess.make_dir_absolute("user://level/localLevels/"+level_name)
	DirAccess.make_dir_absolute("user://level/localLevels/"+level_name+"/chunks")
	var save_game = FileAccess.open("user://level/localLevels/"+level_name+"/index.json", FileAccess.WRITE)
	save_game.store_line(level_name)
	for c in chunks.keys():
		var chunk = chunks[c]
		var chunk_file = FileAccess.open("user://level/localLevels/"+level_name+"/chunks/"+str(c), FileAccess.WRITE)
		var levelObjects = chunk.get_level_object_instances()
		for object in levelObjects:
			chunk_file.store_line(object.serialize())


func clear_entities():
	print("Clearing Entities")
	for entity in Entities.get_children():
		print("Removing "+str(entity))
		if entity.has_method("remove"):
			entity.remove()



func clear():
	clear_entities()
	for chunk in chunks.keys():
		for child in chunks[chunk].get_children():
			child.queue_free()
		chunks[chunk].queue_free()
	interactiveLevelObjects.clear()
	for key in numberOfPlacedLevelObjects.keys():
		numberOfPlacedLevelObjects[key] = 0
	chunks.clear()

var started_loading = false
func load(level_name, immediate = false, download_level = false):
	self.level_name = level_name
	started_loading = true
	var leveltype = "localLevels"
	if download_level:
		leveltype = "downloadLevels"
	var path = "user://level/"+str(leveltype)+"/"+level_name
	var dir = DirAccess.open(path+"/chunks")
	print("Loading Level at "+str(path))
	# eventuell probleme
	if  true:
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
				var chunkpath = path+"/chunks/"+file_name
				if not FileAccess.file_exists(chunkpath):
					continue
				var file = FileAccess.open(chunkpath, FileAccess.READ)
				var line = file.get_line()
				while line != "":
					if immediate:
						add_from_string(base_position,line)
					else:
						toAdd.append([base_position,line])
					line = file.get_line()
				#close file access is automatically done
			file_name = dir.get_next()
		if immediate:	
			print("Loaded level")
			Signals.level_loaded.emit()
	else:
		print("An error occurred when trying to access the path.")

var toAdd = []

func add_from_string(base_position,line):
	var lineData = line.split("|")
	var id = lineData[0].to_int()
	var i = lineData[1].to_int()
	var j = lineData[2].to_int()
	var k = lineData[3].to_int()
	var r = lineData[4].to_int()
	var instance_id = null
	var connectedInteractiveObjects = null
	if lineData.size() > 5:
		instance_id = lineData[5].to_int()
		connectedInteractiveObjects = []
		var commalist = lineData[6].split("[")[1].split("]")[0].split(",")
		for instanceid in commalist:
			if instanceid == "":
				continue
			connectedInteractiveObjects.append(instanceid.to_int())
	var levelObjectData = Constants.LevelObjectData[id]
	var pos = Vector3(i,j,k)
	add(levelObjectData, base_position+pos,r,instance_id,connectedInteractiveObjects)


func _process(delta):
	if toAdd.size() > 0:
		var result = toAdd[0]
		toAdd.remove_at(0)
		add_from_string(result[0],result[1])
	if toAdd.size() == 0 and started_loading:
		started_loading = false
		print("Level loaded")
		Signals.level_loaded.emit()
	
var chunks = {}

func get_chunk(position):
	var chunkPosition = get_chunk_position(position)
	return get_chunk_by_chunk_position(chunkPosition)

func get_chunk_by_chunk_position(chunkPosition):
	if chunks.has(chunkPosition):
		return chunks[chunkPosition]
	return null


func add_chunk(position):
	var chunkPosition = get_chunk_position(position)
	var chunk = chunkPrefab.instantiate()
	chunk.name = str(chunkPosition)
	chunk.level = self
	chunks[chunkPosition] = chunk
	add_child(chunk)
	chunk.global_transform.origin = 8*chunkPosition
	return chunk

var free_unique_instance_id = 0
func get_neighbor(i,j,k,pos):
				var local_pos = pos + Vector3(i,j,k)
				var local_chunk = get_chunk(local_pos)
				if local_chunk == null:
					return null
#				return LevelObjectData.from_cell(local_chunk.get_tile_level_object(local_pos,local_gridmap),local_chunk.get_tile_level_object_orient(local_pos,local_gridmap))
				var resulta = LevelObjectData.from_cell(local_chunk.get_tile_level_object(local_pos,local_chunk.waterGridMap))
				var resultb = LevelObjectData.from_cell(local_chunk.get_tile_level_object(local_pos,local_chunk.levelGridMap))
				if not resulta == null:
					return resulta
				return resultb



# update local tile indices, whose LevelObjectData are allready set correctly with new center levelobject
# NOTE: Deleting is handled seperately in the Chunk script
# NOTE: every tile index has exactly one LevelObjectData type
func update_tiled_object(pos,levelObjectData,gridMap):
		# empty field updates dont need to be handled (this is done by the delete method in the Chunk script) 
	
	var chunk = get_chunk(pos)
	var new_index = -1
	var last_index = chunk.get_tile_level_object(pos,gridMap)
	var rot = 0
	if not levelObjectData == null:
	# default, all sides visible
		var localIndex = 0
		
		#here we pick the local tile index and rotation based on what neighbors there are
		# TODO Compute tileIndex by neighboring tile indices of levelobjects with same levelobjectid
		if get_neighbor(0,1,0,pos) != null and get_neighbor(0,-1,0,pos) != null and get_neighbor(1,0,0,pos) != null and get_neighbor(-1,0,0,pos) != null and get_neighbor(0,0,1,pos) != null and get_neighbor(0,0,-1,pos) != null:
			localIndex = 1
		
		
		# sides
		#up CORRECT
		elif get_neighbor(0,1,0,pos) == null and not get_neighbor(0,-1,0,pos) == null and not get_neighbor(1,0,0,pos) == null and not get_neighbor(-1,0,0,pos) == null and not get_neighbor(0,0,1,pos) == null and not get_neighbor(0,0,-1,pos) == null:
			localIndex = 2
			rot = 1

		#down CORRECT
		elif get_neighbor(0,-1,0,pos) == null and not get_neighbor(0,1,0,pos) == null and not get_neighbor(1,0,0,pos) == null and not get_neighbor(-1,0,0,pos) == null and not get_neighbor(0,0,1,pos) == null and not get_neighbor(0,0,-1,pos) == null:
			localIndex = 2
			rot = 3
			
		
		
		#front CORRECT
		elif get_neighbor(0,0,1,pos) == null and not get_neighbor(0,0,-1,pos) == null and not get_neighbor(0,1,0,pos) == null and not get_neighbor(0,-1,0,pos) == null and not get_neighbor(1,0,0,pos) == null and not get_neighbor(-1,0,0,pos) == null:
			localIndex = 2
			rot = 22
		
		
		#back CORRECT
		elif get_neighbor(0,0,-1,pos) == null and not get_neighbor(0,0,1,pos) == null and not get_neighbor(0,1,0,pos) == null and not get_neighbor(0,-1,0,pos) == null and not get_neighbor(1,0,0,pos) == null and not get_neighbor(-1,0,0,pos) == null:
			localIndex = 2
			rot = 16
		
		
		
		#left CORRECT
		elif get_neighbor(-1,0,0,pos) == null and not get_neighbor(1,0,0,pos) == null and not get_neighbor(0,1,0,pos) == null and not get_neighbor(0,-1,0,pos) == null and not get_neighbor(0,0,1,pos) == null and not get_neighbor(0,0,-1,pos) == null:
			localIndex = 2
			rot = 10
			
		
		#right CORRECT
		elif get_neighbor(1,0,0,pos) == null and not get_neighbor(-1,0,0,pos) == null and not get_neighbor(0,1,0,pos) == null and not get_neighbor(0,-1,0,pos) == null and not get_neighbor(0,0,1,pos) == null and not get_neighbor(0,0,-1,pos) == null:
			localIndex = 2
			rot = 0
		
		
		#edges
		#right CORRECT
		elif get_neighbor(1,0,0,pos) == null and not get_neighbor(-1,0,0,pos) == null and get_neighbor(0,1,0,pos) == null and not get_neighbor(0,-1,0,pos) == null and not get_neighbor(0,0,1,pos) == null and not get_neighbor(0,0,-1,pos) == null:
			localIndex = 6
			rot = 17
		
		
		
		
		
		
		
		#print(localIndex)
	#print(str(levelObjectData)+" "+str(levelObjectData.tileIndex.size()))
		if levelObjectData.tileIndex.size() <= localIndex:
			localIndex = 0
		new_index = levelObjectData.tileIndex[localIndex]
	# if the tile value has changed
	if not new_index == last_index:
		chunk.set_tile_level_object(pos,new_index,gridMap,rot)
		updateNeighbors(pos)

func updateNeighbors(pos):
		for i in range(-1,2):
			for j in range(-1,2):
				for k in range(-1,2):
					var local_pos = pos + Vector3(i,j,k)
					var local_chunk = get_chunk(local_pos)
					if local_chunk == null:
						continue
					var levelObjectData = get_neighbor(i,j,k,pos)
					if levelObjectData == Constants.Water:
						update_tiled_object(local_pos,levelObjectData,local_chunk.waterGridMap)
					else:
						update_tiled_object(local_pos,levelObjectData,local_chunk.levelGridMap)

func add(levelObjectData: LevelObjectData, position,rotation = 0, unique_instance_id = null, connectedObjects = []):
	
	# this sets the table of maximum numbers at the start
	if not numberOfPlacedLevelObjects.has(levelObjectData.levelObjectId):
		numberOfPlacedLevelObjects[levelObjectData.levelObjectId] = 0
	var is_interactive = false
	if(levelObjectData.maximumNumber != -1):
		if(numberOfPlacedLevelObjects[levelObjectData.levelObjectId] == levelObjectData.maximumNumber):
			return
	numberOfPlacedLevelObjects[levelObjectData.levelObjectId] = numberOfPlacedLevelObjects[levelObjectData.levelObjectId] + 1
	var chunk = get_chunk(position)
	if(chunk == null):
		chunk = add_chunk(position)
	chunk.change_in_chunk = true
	changes = true
	var pos = get_grid_position(position)
	if(levelObjectData.tiled):
		var local_gridmap = chunk.levelGridMap
		
		if Constants.Water == levelObjectData:
			local_gridmap = chunk.waterGridMap
		
		update_tiled_object(pos,levelObjectData,local_gridmap)
	else:
		var new_level_object
		if levelObjectData.interactive:
			new_level_object = interactiveLevelObjectPrefab.instantiate()	
			is_interactive = true
		else:
			new_level_object = levelObjectPrefab.instantiate()
		
		chunk.levelObjects.add_child(new_level_object)
		new_level_object.global_transform.origin = Vector3(pos.x,pos.y,pos.z)
		var construction_collision = new_level_object.get_child(0)
		# assign new inner levelobject
		var level_object_dupe: Node3D = get_tree().root.get_node("LevelObjects/LevelObjectList/"+levelObjectData.name).duplicate()
		new_level_object.add_child(level_object_dupe)
		level_object_dupe.transform.origin = levelObjectData.offset
		# has actual instance id already?
		if(unique_instance_id != null):
			new_level_object.unique_instance_id = unique_instance_id
			free_unique_instance_id = max(free_unique_instance_id, unique_instance_id + 1)
			new_level_object.connectedObjects = connectedObjects
			Signals.connection_added.emit(unique_instance_id,connectedObjects)
		elif is_interactive:
			new_level_object.unique_instance_id = free_unique_instance_id
			free_unique_instance_id = free_unique_instance_id + 1
		if (new_level_object.has_method("sign_up")):
			new_level_object.sign_up()
		new_level_object.contained_level_object = level_object_dupe
		new_level_object.levelObjectData = levelObjectData
		# in future this should only be done in edit mode
		
		
		var translation = Vector3.ZERO
		if rotation == 1:
			translation = Vector3(-1,0,0)
		if rotation == 2:
			translation = Vector3(-1,0,-1)
		if rotation == 3:
			translation = Vector3(0,0,-1)
		
		
		new_level_object.rotate(Vector3.UP,rotation*PI/2)
		level_object_dupe.transform = level_object_dupe.transform.translated_local(translation)
		construction_collision.transform = construction_collision.transform.translated_local(translation)
		
		new_level_object.apply_construction_data()
		new_level_object.on_mode_change()
		construction_collision.rotate(Vector3.UP,rotation*PI/2)
		# correct internal positions:
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
	chunk.levelObjects.remove_child(object)
	numberOfPlacedLevelObjects[object.levelObjectData.levelObjectId] = max(0,numberOfPlacedLevelObjects[object.levelObjectData.levelObjectId] - 1)

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
