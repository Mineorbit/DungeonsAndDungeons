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
@onready var gridMap = $GridMap
@onready var levelObjects = $LevelObjects
@onready var mesh: MeshInstance3D = $mesh
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

static func load_chunk():
	pass
	
var change_in_chunk = false

func set_tile_level_object(pos,index):
	var x = floor(pos.x - global_transform.origin.x)
	var y = floor(pos.y - global_transform.origin.y)
	var z = floor(pos.z - global_transform.origin.z)
	var localPos = Vector3(x,y,z)
	gridMap.set_cell_item(localPos,index)

func remove_tile_level_object(pos):
	if get_tile_level_object(pos) != -1:
		set_tile_level_object(pos,-1)
		return true
	return false

func get_tile_level_object(pos):
	var x = floor(pos.x - global_transform.origin.x)
	var y = floor(pos.y - global_transform.origin.y)
	var z = floor(pos.z - global_transform.origin.z)
	var localPos = Vector3(x,y,z)
	return gridMap.get_cell_item(localPos)

func get_tile_level_object_orient(pos):
	var x = floor(pos.x - global_transform.origin.x)
	var y = floor(pos.y - global_transform.origin.y)
	var z = floor(pos.z - global_transform.origin.z)
	var localPos = Vector3(x,y,z)
	return gridMap.get_cell_item_orientation(localPos)

func update_navigation():
	#mesh.mesh = navmesh
	if change_in_chunk:
		navmesh = NavigationMesh.new()
		navmesh.agent_radius = Constants.navmargin
		navmesh.agent_height = 1
		navmesh.agent_max_climb = 0
		#navmesh.geometry_parsed_geometry_type = 3
		
		bake_navigation_mesh()
		print("Baking Nav Mesh of "+str(self))
		await bake_finished
		print("Finished")
		change_in_chunk = false
		get_parent().changedChunks = get_parent().changedChunks - 1
		

func get_level_objects():
	return levelObjects.get_children()

func get_level_object_instances():
	var levelObjectInstances = []
	for i in range(8):
		for j in range(8):
			for k in range(8):
				var instance = LevelObjectInstance.new()
				var offset = Vector3(i,j,k)
				var grid_position = level.get_grid_position(global_transform.origin+offset)
				if not get_tile_level_object(grid_position) == -1:
					var levelObjectData = LevelObjectData.from_cell(get_tile_level_object(grid_position),get_tile_level_object_orient(grid_position))
					instance.x = floor(grid_position.x - global_transform.origin.x)
					instance.y = floor(grid_position.y - global_transform.origin.y)
					instance.z = floor(grid_position.z - global_transform.origin.z)
					instance.levelObjectData = levelObjectData
					levelObjectInstances.append(instance)
	for n in get_level_objects():
		print("TEST: "+str(n))
		var instance = LevelObjectInstance.new()
		levelObjectInstances.append(n.to_instance(instance))
	return levelObjectInstances
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
