extends NavigationRegion3D

# Declare member variables here. Examples:
# var a = 2
# var b = "text"

class LevelObjectInstance:
	var x = 0
	var y = 0
	var z = 0
	var rotation = 0
	var levelObjectData: LevelObjectData
	var unique_instance_id = null
	var connectedObjects = []
	
	func serialize():
		var data = str(levelObjectData.levelObjectId)+"|"+str(x)+"|"+str(y)+"|"+str(z)+"|"+str(rotation)
		if unique_instance_id != null:
			data = data+"|"+str(unique_instance_id)+"|"+str(connectedObjects)
		return data



var level
@onready var levelGridMap = $GridMap
@onready var waterGridMap = $WaterGridMap
@onready var levelObjects = $LevelObjects
@onready var mesh: MeshInstance3D = $mesh

static func load_chunk():
	pass
	
var change_in_chunk = false

func set_tile_level_object(pos,index,is_water):
	var gridMap = levelGridMap
	if is_water:
		gridMap = waterGridMap
	var x = floor(pos.x - global_transform.origin.x)
	var y = floor(pos.y - global_transform.origin.y)
	var z = floor(pos.z - global_transform.origin.z)
	var localPos = Vector3(x,y,z)
	gridMap.set_cell_item(localPos,index)

# this should be done differently
func remove_tile_level_object(pos):
	print("Remove Object from grid at "+str(pos))
	print(get_tile_level_object(pos,levelGridMap))
	print(get_tile_level_object(pos,waterGridMap))
	var result = false 
	if get_tile_level_object(pos,levelGridMap) != -1:
		#set_tile_level_object(pos,-1,false)
		# update tile and neighbors (null is the -1 tile index i.e. empty)
		Constants.World.level.update_tiled_object(pos,null,levelGridMap)
		result = true
	if get_tile_level_object(pos,waterGridMap) != -1:
		#set_tile_level_object(pos,-1,true)
		Constants.World.level.update_tiled_object(pos,null,waterGridMap)
		result = true
	return result

func get_tile_level_object(pos,gridMap):
	var x = floor(pos.x - global_transform.origin.x)
	var y = floor(pos.y - global_transform.origin.y)
	var z = floor(pos.z - global_transform.origin.z)
	var localPos = Vector3(x,y,z)
	return gridMap.get_cell_item(localPos)

func get_tile_level_object_orient(pos,gridMap):
	var x = floor(pos.x - global_transform.origin.x)
	var y = floor(pos.y - global_transform.origin.y)
	var z = floor(pos.z - global_transform.origin.z)
	var localPos = Vector3(x,y,z)
	return gridMap.get_cell_item_orientation(localPos)

func update_navigation():
	#mesh.mesh = navmesh
	if change_in_chunk:
		bake_navigation_mesh()
		print("Baking Nav Mesh of "+str(self))
		await bake_finished
		print("Finished")
		change_in_chunk = false
		get_parent().changedChunks = get_parent().changedChunks - 1
		

func get_level_objects():
	return levelObjects.get_children()

func get_level_object_instance_of_cell(i,j,k,levelObjectInstances,gridMap):
	var instance = LevelObjectInstance.new()
	var offset = Vector3(i,j,k)
	var grid_position = level.get_grid_position(global_transform.origin+offset)
	if not get_tile_level_object(grid_position,gridMap) == -1:
		var levelObjectData = LevelObjectData.from_cell(get_tile_level_object(grid_position,gridMap),get_tile_level_object_orient(grid_position,gridMap))
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
				get_level_object_instance_of_cell(i,j,k,levelObjectInstances,levelGridMap)
	
	
	# get level objects of waterGridMap
	for i in range(8):
		for j in range(8):
			for k in range(8):
				get_level_object_instance_of_cell(i,j,k,levelObjectInstances,waterGridMap)
	
	for n in get_level_objects():
		var instance = LevelObjectInstance.new()
		levelObjectInstances.append(n.to_instance(instance))
	return levelObjectInstances
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
