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
	var unique_id = 0
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
		ResourceSaver.save(new_res, path)
