extends Node3D
class_name LevelObject

# Declare member variables here. Examples:
# var a = 2
# var b = "text"
var contained_level_object

var levelObjectData: LevelObjectData
var uniqueLevelObjectId

@onready var construction_collision: StaticBody3D = $ConstructionCollision
# Called when the node enters the scene tree for the first time.
func _ready():
	preparing_Collision()
	on_mode_change()

func preparing_Collision():
	Constants.mode_changed.connect(on_mode_change)
	
func apply_construction_data():
	if construction_collision != null:
		construction_collision.global_translate(levelObjectData.construction_collision_offset)
		construction_collision.global_scale(levelObjectData.construction_collision_scale)
	else:
		print("There was no construcion collision object to modify")

func reset():
	if contained_level_object.has_method("reset"):
		contained_level_object.reset()

func start():
	if contained_level_object.has_method("start"):
		contained_level_object.start()

func on_remove():
	if contained_level_object.has_method("on_remove"):
		contained_level_object.on_remove()



func on_mode_change():
	if Constants.currentMode == 1:
		add_child(construction_collision)
	else:
		remove_child(construction_collision)

func to_instance(instance):
		instance.x = floor(transform.origin.x)
		instance.y = floor(transform.origin.y)
		instance.z = floor(transform.origin.z)
		var rot = int( round(global_transform.basis.get_euler().y /PI * 2) )
		rot = ( rot + 4) % 4
		instance.rotation = rot
		instance.levelObjectData = levelObjectData
		return instance

func _process(delta):
	pass
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
