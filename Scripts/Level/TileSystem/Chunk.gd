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

signal gridGenerated(pos)

func generate_grid():
	var cells_todo = 0
	for grid in cellGrids.get_children():
		cells_todo = cells_todo + 1
	print("TODO: "+str(cells_todo))
	for grid in cellGrids.get_children():
		#print("Doing "+str(grid))
		var t = Thread.new()
		t.start(func():
			grid.generate()
			print("Done with grid")
			cells_todo = cells_todo - 1
			if cells_todo == 0:
				call_deferred("chunkDone")
		)

func chunkDone():
	print("Chunk Done")
	gridGenerated.emit(position)


func add_tiled_level_object(pos,levelObjectData, generate = false):
	cellGrids.add_tiled_level_object(pos - global_transform.origin,levelObjectData, generate)
	change_in_chunk = true

func remove_tiled_level_object(pos):
	cellGrids.remove_tiled_level_object(pos - global_transform.origin)
	change_in_chunk = true


func get_level_objects():
	return levelObjects.get_children()



func get_level_object_instances():
	var levelObjectInstances = []
	# get level objects of levelGridMap
	
	levelObjectInstances += cellGrids.get_instances()
	
	for n in get_level_objects():
		var instance = LevelObjectInstance.new()
		levelObjectInstances.append(n.to_instance(instance))
	return levelObjectInstances
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
