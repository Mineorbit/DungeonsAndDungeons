extends Spatial


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

class LevelObjectInstance:
	var x = 0
	var y = 0
	var z = 0
	var levelObjectData: LevelObjectData

var level
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

static func load_chunk():
	pass
	
func get_level_objects():
	var levelObjectInstances = []
	var x = global_transform.origin.x
	var y = global_transform.origin.y
	var z = global_transform.origin.z
	for i in range(8):
		for j in range(8):
			for k in range(8):
				var instance = LevelObjectInstance.new()
				
				var grid_position = level.gridMap.world_to_map(Vector3(x+i,y+j,z+k))
				if not level.gridMap.get_cell_item(grid_position.x,grid_position.y,grid_position.z) == -1:
					var levelObjectData = LevelObjectData.from_cell(level.gridMap.get_cell_item(grid_position.x,grid_position.y,grid_position.z),level.gridMap.get_cell_item_orientation(grid_position.x,grid_position.y,grid_position.z))
					instance.x = i
					instance.y = j
					instance.z = k
					instance.levelObjectData = levelObjectData
					levelObjectInstances.append(instance)
	for n in get_children():
		var instance = LevelObjectInstance.new()
		instance.x = n.translation.x
		instance.y = n.translation.y
		instance.z = n.translation.z
		instance.levelObjectData = n.levelObjectData
		levelObjectInstances.append(instance)
	return levelObjectInstances
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
