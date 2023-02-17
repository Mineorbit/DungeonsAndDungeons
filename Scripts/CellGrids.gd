extends Node3D

# this maps a unique levelobject id to a gridmesh
var gridmeshes = {}

var grid: PackedInt32Array = []
var grid_size = 8

func _ready():
	for i in range(grid_size):
		for j in range(grid_size):
			for k in range(grid_size):
				grid.append(-1)

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
	var grid_mesh = gridmeshes[levelObjectData.levelObjectId]
	var gridpos = get_grid_position(pos)
	var grid_index = get_grid_index(gridpos)
	grid[grid_index] = levelObjectData.levelObjectId
	if generate:
		grid_mesh.generate()

func get_grid_index(grid_position):
	return grid_size*grid_size*grid_position.y+grid_size*grid_position.x+grid_position.z

func remove_tiled_level_object(pos):
	var grid_index = get_grid_index(get_grid_position(pos))
	var oldid = grid[grid_index]
	if oldid == -1:
		return false
	print(pos)
	var grid_mesh = gridmeshes[oldid]
	grid[grid_index] = -1
	grid_mesh.generate()
	return true


