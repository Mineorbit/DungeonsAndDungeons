extends Node3D

# this maps a unique levelobject id to a gridmesh
var gridmeshes = {}

var grid = []

func get_at(gridpos):
	return -1

func add_tiled_level_object(pos,levelObjectData):
	if not levelObjectData.name in gridmeshes:
		print(levelObjectData.name)
		var grid_object: Node3D = get_tree().root.get_node("LevelObjects/LevelObjectList/Tiled"+levelObjectData.name).duplicate()
		add_child(grid_object)
		gridmeshes[levelObjectData.name] = grid_object
		
func remove_tiled_level_object(pos):
	pass

func get_grid_pos(x,y,z):
	return Vector3i(floor(x),floor(y),floor(z))
