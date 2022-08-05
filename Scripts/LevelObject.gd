extends Node3D
class_name LevelObject

# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var levelObjectData: LevelObjectData
var uniqueLevelObjectId

var construction_collision
# Called when the node enters the scene tree for the first time.
func _ready():
	construction_collision = $ConstructionCollision
	Constants.mode_changed.connect(on_mode_change)
	pass # Replace with function body.

func on_mode_change():
	if Constants.currentMode == 1:
		add_child(construction_collision)
	else:
		remove_child(construction_collision)

func to_instance(instance):
		instance.x = transform.origin.x
		instance.y = transform.origin.y
		instance.z = transform.origin.z
		instance.levelObjectData = levelObjectData
		return instance

func _process(delta):
	pass
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
