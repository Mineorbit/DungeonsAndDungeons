extends NavigationRegion3D

# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var level
@onready var cellGrids = $CellGrids
@onready var levelObjects = $LevelObjects


var change_in_chunk = false

class LevelObjectInstance:
	var x = 0
	var y = 0
	var z = 0
	var rotation = 0
	var levelObjectData: LevelObjectData
	var unique_instance_id = null
	var connectedObjects = []
	var properties = null
	
	func serialize():
		var data = str(levelObjectData.levelObjectId)+"|"+str(x)+"|"+str(y)+"|"+str(z)+"|"+str(rotation)
		data = data+"|"+JSON.stringify(properties)
		if unique_instance_id != null:
			data = data+"|"+str(unique_instance_id)+"|"+str(connectedObjects)
		return data




func update_navigation():
	#mesh.mesh = navmesh
	if change_in_chunk:
		bake_navigation_mesh()
		print("Baking Nav Mesh of "+str(self))
		await bake_finished
		print("Finished")
		change_in_chunk = false
		get_parent().changedChunks = get_parent().changedChunks - 1


func generate_grid():
	for grid in cellGrids.get_children():
		grid.generate()


func add_tiled_level_object(pos,levelObjectData, generate = false):
	cellGrids.add_tiled_level_object(pos - global_transform.origin,levelObjectData, generate)
	change_in_chunk = true

func remove_tiled_level_object(pos):
	cellGrids.remove_tiled_level_object(pos - global_transform.origin)
	change_in_chunk = true


func get_level_objects():
	return levelObjects.get_children()

func get_level_object_instance_of_cell(i,j,k,levelObjectInstances):
	var instance = LevelObjectInstance.new()
	var offset = Vector3(i,j,k)
	var grid_position = level.get_grid_position(global_transform.origin+offset)
	if not cellGrids.get_at(offset) == -1:
		var levelObjectData = LevelObjectData.new()
		#var levelObjectData = LevelObjectData.from_cell(get_tile_level_object(grid_position,gridMap),get_tile_level_object_orient(grid_position,gridMap))
		instance.x = floor(grid_position.x - global_transform.origin.x)
		instance.y = floor(grid_position.y - global_transform.origin.y)
		instance.z = floor(grid_position.z - global_transform.origin.z)
		#does instance.rotation need to be set here?
		instance.levelObjectData = levelObjectData
		levelObjectInstances.append(instance)


func get_level_object_instances():
	var levelObjectInstances = []
	# get level objects of levelGridMap
	
	for i in range(8):
		for j in range(8):
			for k in range(8):
				get_level_object_instance_of_cell(i,j,k,levelObjectInstances)
	
	for n in get_level_objects():
		var instance = LevelObjectInstance.new()
		levelObjectInstances.append(n.to_instance(instance))
	return levelObjectInstances
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
