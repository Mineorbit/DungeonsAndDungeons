extends Resource
class_name LevelObjectData

# Declare member variables here. Examples:
# var a = 2
# var b = "text"

@export var name = "Default Level Object"
@export var tiled = false
@export var interactive = false
var gridScale = 2
# -1 if this is not a tiled object
@export var tileIndex = -1
@export var levelObjectId = -1
@export var maximumNumber = -1
@export var offset = Vector3.ZERO
@export var construction_collision_offset = Vector3.ZERO
@export var construction_collision_scale = Vector3(1,1,1)
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func to_cell():
	return tileIndex

static func from_cell(cellTileIndex,_cellTileOrientation):
	for levelObject in Constants.LevelObjectData.values():
		if levelObject.tiled and cellTileIndex == levelObject.tileIndex:
			return levelObject
	return Constants.Default_Floor
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
