extends NavigationRegion3D

# Declare member variables here. Examples:
# var a = 2
# var b = "text"

class LevelObjectInstance:
	var x = 0
	var y = 0
	var z = 0
	var levelObjectData: LevelObjectData
	var unique_instance_id = null
	var connectedObjects = []
	func serialize():
		var data = str(levelObjectData.levelObjectId)+"|"+str(x)+"|"+str(y)+"|"+str(z)
		if unique_instance_id != null:
			data = data+"|"+str(unique_instance_id)+"|"+str(connectedObjects)
		return data



var level
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

static func load_chunk():
	pass
	
var change_in_chunk = false

func update_navigation():	
	if change_in_chunk:
		bake_navigation_mesh()
		print("Baking Nav Mesh of "+str(self))
		await bake_finished
		print("Finished")
		change_in_chunk = false


func get_level_objects():
	var levelObjectInstances = []
	for i in range(8):
		for j in range(8):
			for k in range(8):
				var instance = LevelObjectInstance.new()
				var offset = Vector3(i,j,k)
				var grid_position = level.gridMap.world_to_map(global_transform.origin+offset)
				if not level.gridMap.get_cell_item(grid_position) == -1:
					var levelObjectData = LevelObjectData.from_cell(level.gridMap.get_cell_item(grid_position),level.gridMap.get_cell_item_orientation(grid_position))
					instance.x = floor(grid_position.x - global_transform.origin.x)
					instance.y = floor(grid_position.y - global_transform.origin.y)
					instance.z = floor(grid_position.z - global_transform.origin.z)
					instance.levelObjectData = levelObjectData
					levelObjectInstances.append(instance)
	for n in get_children():
		var instance = LevelObjectInstance.new()
		levelObjectInstances.append(n.to_instance(instance))
	return levelObjectInstances
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
