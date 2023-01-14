extends Node3D


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

@onready var LevelObjectList = $LevelObjectList

func load_levelobject_list():
	var resourcedir = DirAccess.open("res://Resources/LevelObjectData")
	for resource in resourcedir.get_files():
		var newres = ResourceLoader.load("res://Resources/LevelObjectData/"+resource)
		print(str(newres.levelObjectId)+" "+str(newres.name))
		Constants.LevelObjectData[newres.levelObjectId] = newres
		#ResourceLoader.load()"res://Resources/LevelObjectData/"+levelobjectname+".tres"
	Constants.levelObjects_initialized = true

# Called when the node enters the scene tree for the first time.
func _ready():
	load_levelobject_list()
	get_parent().remove_child(self)
	hide()

func setup_resources():
	if Constants.levelObjects_initialized:
		return
	Constants.levelObjects_initialized = true
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
		var new_res: LevelObjectData
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
		ResourceSaver.save(new_res, path)
		#Constants.numberOfPlacedLevelObjects[new_res.levelObjectId] = 0
		if is_tiled:
			for child in levelobject.get_children():
				meshlibrary.create_item(meshlibrary_id)
				meshlibrary.set_item_mesh(meshlibrary_id,child.get_mesh())
				var new_transform = child.transform
				new_transform.origin = Vector3.ZERO
				meshlibrary.set_item_mesh_transform(meshlibrary_id,new_transform)
				meshlibrary.set_item_shapes(meshlibrary_id,[child.get_child(0).get_child(0).shape,child.get_child(0).get_child(0).transform])
				meshlibrary_id = meshlibrary_id + 1
	
	ResourceSaver.save(meshlibrary,"res://Resources/grid.tres")
