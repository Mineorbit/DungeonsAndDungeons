extends Node3D
# this maps a unique levelobject id to a gridmesh
var gridmeshes = {}

var grid: PackedInt32Array = []
var grid_size = 8

class LevelObjectInstance:
	var x = 0
	var y = 0
	var z = 0
	var levelObjectData: LevelObjectData
	func serialize():
		var data = str(levelObjectData.levelObjectId)+"|"+str(x)+"|"+str(y)+"|"+str(z)
		return data



func _ready():
	for i in range(grid_size):
		for j in range(grid_size):
			for k in range(grid_size):
				grid.append(-1)


func get_instances():
	var instances = []
	for i in range(grid_size):
		for j in range(grid_size):
			for k in range(grid_size):
				var p = get_grid_position(Vector3(i,j,k))
				var index = get_grid_index(p)
				var levelObjectId = grid[index]
				if levelObjectId == -1:
					continue
				var levelObjectData = Constants.LevelObjectData[levelObjectId]
				var instance = LevelObjectInstance.new()
				instance.x = i
				instance.y = j
				instance.z = k
				instance.levelObjectData = levelObjectData
				instances.append(instance)
	return instances

func get_at(world_position):
	#print(world_position)
	#print(world_position)
	var chunk = Constants.World.level.get_chunk(world_position)
	if not chunk == null:
		#print(chunk.cellGrids)
		var local_pos = world_position - chunk.global_transform.origin
		#print(local_pos)
		#print(local_pos)
		#print_stack()
		var x = get_grid_position(local_pos)
		#print(x)
		return chunk.cellGrids.grid[get_grid_index(x)]
	return -1

func get_grid_position(v):
	return Vector3(floor(v.x),floor(v.y),floor(v.z))


func add_tiled_level_object(pos,levelObjectData, generate = false):
	if not levelObjectData.levelObjectId in gridmeshes:
		var grid_object: Node3D = get_tree().root.get_node("LevelObjects/LevelObjectList/Tiled"+levelObjectData.name).duplicate()
		grid_object.transform.origin = Vector3(0,0,0)
		add_child(grid_object)
		grid_object.levelObjectId = levelObjectData.levelObjectId
		gridmeshes[levelObjectData.levelObjectId] = grid_object
		print("Added "+str(grid_object)+" "+str(levelObjectData.levelObjectId))
	var grid_mesh = gridmeshes[levelObjectData.levelObjectId]
	var gridpos = get_grid_position(pos)
	var grid_index = get_grid_index(gridpos)
	grid[grid_index] = levelObjectData.levelObjectId
	if generate:
		#get_parent().generate_grid()
		start_generate_at(global_transform.origin+gridpos,levelObjectData.levelObjectId)


func start_generate_at(pos,ulid):
	for i in range(-1,2):
		for j in range(-1,2):
			for k in range(-1,2):
				var n = Vector3(i,j,k)
				var p = pos + n
				var chunk: Node3D = Constants.World.level.get_chunk(p)
				if chunk == null:
					print("There is no chunk at "+str(p))
					chunk = Constants.World.level.add_chunk(p)
					#chunks.append(chunk)
				if not chunk.is_inside_tree():
					chunk.ready.connect(func():
						chunk.cellGrids.start_generate_cell(p,ulid)
						)
				chunk.cellGrids.start_generate_cell(p,ulid)

func start_generate_cell(pos,ulid):
	if ulid in gridmeshes:
		print("GEN AT "+str(pos))
		gridmeshes[ulid].queue_generate(pos)
	else:
		print("There is no Grid for "+str(pos)+" in "+str(self))

func get_grid_index(grid_position):
	return grid_size*grid_size*grid_position.y+grid_size*grid_position.x+grid_position.z

func remove_tiled_level_object(pos):
	var grid_pos = get_grid_position(pos)
	var grid_index = get_grid_index(grid_pos)
	var oldid = grid[grid_index]
	if oldid == -1:
		return false
	var grid_mesh = gridmeshes[oldid]
	grid[grid_index] = -1
	start_generate_at(global_transform.origin+grid_pos,oldid)
	return true


