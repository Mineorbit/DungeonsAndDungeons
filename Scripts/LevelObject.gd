extends Node3D
class_name LevelObject

# Declare member variables here. Examples:
# var a = 2
# var b = "text"
var contained_level_object

var levelObjectData: LevelObjectData
var uniqueLevelObjectId

var construction_collision
# Called when the node enters the scene tree for the first time.
func _ready():
	preparing_Collision()

func preparing_Collision():
	construction_collision = $ConstructionCollision
	Constants.mode_changed.connect(on_mode_change)
	


func reset():
	if contained_level_object.has_method("reset"):
		contained_level_object.reset()

func on_mode_change():
	print("Mode changed to "+str(Constants.currentMode))
	if Constants.currentMode == 1:
		add_child(construction_collision)
	else:
		print("Disabling "+str(construction_collision))
		remove_child(construction_collision)

func to_instance(instance):
		instance.x = floor(transform.origin.x)
		instance.y = floor(transform.origin.y)
		instance.z = floor(transform.origin.z)
		instance.levelObjectData = levelObjectData
		return instance

func _process(delta):
	pass
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
