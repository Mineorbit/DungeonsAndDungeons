extends MeshInstance3D


@export var face_granularity = 1

@export var grid_size = 8

var grid: PackedByteArray = []

func _ready():
	for i in range(grid_size):
		for j in range(grid_size):
			for k in range(grid_size):
				grid.append(false)

func add(x,y,z):
	var index = 64*x+8*y+z
	print(index)
	if grid.size() > index:
		grid[index] = true

func remove(x,y,z):
	var index = 64*x+8*y+z
	if grid.size() > index:
		grid[index] = false
