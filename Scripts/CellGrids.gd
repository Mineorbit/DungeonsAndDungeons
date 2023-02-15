extends Node3D

# this maps a unique levelobject id to a gridmesh
var gridmeshes = {}

var grid = []

func get_at(gridpos):
	return -1


func get_grid_pos(x,y,z):
	return Vector3i(floor(x),floor(y),floor(z))
