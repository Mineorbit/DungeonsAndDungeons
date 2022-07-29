extends Spatial


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

onready var gridMap: GridMap = $levelobject_grid
# Called when the node enters the scene tree for the first time.
func _ready():
	pass

func setup_new():
	for i in range(-8,8,2):
		for j in range(-8,8,2):
			add(Constants.Default_Floor,Vector3(i,0,j))

func save():
	pass

func load(path):
	pass

func get_chunk(position):
	pass

func add(levelObjectData,position):
	if(levelObjectData.tiled):
		var pos = gridMap.world_to_map(position)
		gridMap.set_cell_item(pos.x,pos.y,pos.z,levelObjectData.tileIndex)
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
