extends Resource
class_name LevelObjectData

# Declare member variables here. Examples:
# var a = 2
# var b = "text"

@export var name: String = "Default Level Object"
@export var tiled: bool = false
@export var interactive: bool = false
var gridScale: int = 2
# -1 if this is not a tiled object
@export var tileIndex: PackedInt32Array = []
@export var levelObjectId: int = -1
@export var maximumNumber: int = -1
@export var offset: Vector3 = Vector3.ZERO
@export var construction_collision_offset: Vector3 = Vector3.ZERO
@export var construction_collision_scale: Vector3 = Vector3(1,1,1)

@export var display_scale: Vector3 = Vector3(1,1,1)
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func to_cell():
	return tileIndex

static func from_cell(cellTileIndex,_cellTileOrientation):
	for levelObject in Constants.LevelObjectData.values():
		if levelObject.tiled and (cellTileIndex in levelObject.tileIndex):
			return levelObject
	# if there is no match return null
	return null
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
