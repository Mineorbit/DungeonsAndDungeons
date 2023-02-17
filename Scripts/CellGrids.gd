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

# this needs to be changed to get value from neighboring chunks
func get_at(gridpos):
	if gridpos.x < 0 or gridpos.y <0 or gridpos.z < 0 or gridpos.x >= grid_size or gridpos.y >= grid_size or gridpos.z >= grid_size:
		return - 1
	var i = get_grid_index(gridpos)
	if len(grid) <= i:
		return -1
	return grid[i]

func add_tiled_level_object(pos,levelObjectData, generate = false):
	if not levelObjectData.levelObjectId in gridmeshes:
		var grid_object: Node3D = get_tree().root.get_node("LevelObjects/LevelObjectList/Tiled"+levelObjectData.name).duplicate()
		add_child(grid_object)
		grid_object.transform.origin = Vector3(0,0,0)
		grid_object.levelObjectId = levelObjectData.levelObjectId
		print(levelObjectData)
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
	var grid_mesh = gridmeshes[oldid]
	grid[grid_index] = -1
	grid_mesh.generate()
	return true

func get_grid_position(v):
	return Vector3i(floor(v.x),floor(v.y),floor(v.z))

