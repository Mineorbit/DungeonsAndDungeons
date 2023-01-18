extends Node3D


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

@onready var LevelObjectList = $LevelObjectList

func load_levelobject_list():
	var resourcedir = DirAccess.open("res://Resources/LevelObjectData")
	print(resourcedir.get_files())
	for resource_name in resourcedir.get_files():
		var path = "res://Resources/LevelObjectData/"+resource_name
		if path.ends_with(".remap"):
			path = path.get_basename().get_basename()+".tres"
		if ResourceLoader.exists(path):
			var newres = load(path)
			Constants.LevelObjectData[newres.levelObjectId] = newres
			print("Loaded LevelObjectData for "+str(newres))
		else:
			print("There is no ResourceLoader for "+str(path))
		#ResourceLoader.load()"res://Resources/LevelObjectData/"+levelobjectname+".tres"
	Constants.levelObjects_initialized = true

# Called when the node enters the scene tree for the first time.
func _ready():
	load_levelobject_list()
	hide()

func setup_resources():
	var meshlibrary = MeshLibrary.new()
	var unique_id = 0
	var meshlibrary_id = 0
	for levelobject in LevelObjectList.get_children():
		var is_tiled = false
		var levelobjectname = str(levelobject.name)
		if "Tiled" in levelobjectname:
			levelobjectname = levelobjectname.trim_prefix("Tiled")
			is_tiled = true
		var path = "res://Resources/LevelObjectData/"+levelobjectname+".tres"
		var new_res: LevelObjectData = LevelObjectData.new()
		if ResourceLoader.exists(path):
			new_res = load(path)
		else:
			new_res = LevelObjectData.new()
		if new_res == null:
			return
		new_res.levelObjectId = unique_id
		new_res.name = levelobjectname
		new_res.tiled = is_tiled
		Constants.LevelObjectData[unique_id] = new_res
		unique_id = unique_id + 1
		#Constants.numberOfPlacedLevelObjects[new_res.levelObjectId] = 0
		if is_tiled:
			# allways build Tile Index from scratch
			new_res.tileIndex = []
			for child in levelobject.get_children():
				# this should be replaced with an array
				new_res.tileIndex.append(meshlibrary_id)
				meshlibrary.create_item(meshlibrary_id)
				meshlibrary.set_item_mesh(meshlibrary_id,child.get_mesh())
				var new_transform = child.transform
				new_transform.origin = Vector3.ZERO
				meshlibrary.set_item_mesh_transform(meshlibrary_id,new_transform)
				meshlibrary.set_item_shapes(meshlibrary_id,[child.get_child(0).get_child(0).shape,child.get_child(0).get_child(0).transform])
				meshlibrary_id = meshlibrary_id + 1
			
		ResourceSaver.save(new_res, path)
	ResourceSaver.save(meshlibrary,"res://Resources/grid.tres")
