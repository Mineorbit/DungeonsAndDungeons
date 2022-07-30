extends Spatial


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var chunkPrefab
onready var gridMap: GridMap = $levelobject_grid
# Called when the node enters the scene tree for the first time.
func _ready():
	chunkPrefab = load("res://Prefabs/Chunk.tscn")
	

func setup_new():
	for i in range(-8,8,2):
		for j in range(-8,8,2):
			add(Constants.Default_Floor,Vector3(i,0,j))

func save(path):
	pass

func load(path):
	pass

func get_chunk(position):
	var chunkPosition = Vector3(position.x*floor(position.x/8),position.y*floor(position.y/8),position.z*floor(position.z/8))
	# here need to read from some dictionary
	

func add_chunk(position):
	var chunkPosition = Vector3(position.x*floor(position.x/8),position.y*floor(position.y/8),position.z*floor(position.z/8))
	var chunk = chunkPrefab.instance()
	chunk.global_transform.origin = chunkPosition
	add_child(chunk)
	return chunk
	

func add(levelObjectData,position):
	var chunk = get_chunk(position)
	if(chunk != null):
		chunk = add_chunk(position)
	
	if(levelObjectData.tiled):
		var pos = gridMap.world_to_map(position)
		gridMap.set_cell_item(pos.x,pos.y,pos.z,levelObjectData.tileIndex)
	else:
		print("Other")
		
var new = true
var tiles_to_remove = []
func _process(delta):
	
	if(tiles_to_remove.size() > 0):
		var value = tiles_to_remove[0] 
		print("Removing "+str(value))
		tiles_to_remove.remove(0)
		var pos = value
		gridMap.set_cell_item(int(pos.x),int(pos.y),int(pos.z),-1)
		
func remove(position):
	var pos = gridMap.world_to_map(position)
	tiles_to_remove.append(pos)

func remove_from_grid(pos):
	gridMap.set_cell_item(pos.x,pos.y,pos.z,-1)
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
