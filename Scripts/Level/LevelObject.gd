extends Node3D
class_name LevelObject

# Declare member variables here. Examples:
# var a = 2
# var b = "text"
var contained_level_object

var levelObjectData: LevelObjectData
var uniqueLevelObjectId

@export var material: LevelObjectData.LevelObjectMaterial = LevelObjectData.LevelObjectMaterial.Default

@onready var construction_collision: StaticBody3D = $ConstructionCollision
# Called when the node enters the scene tree for the first time.
func _ready():
	Signals.mode_changed.connect(on_mode_change)

	
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


func encode_properties():
	var result_table = {}
	var property_added = false
	for prop in contained_level_object.get_property_list():
			var prop_name = prop["name"]
			if prop_name.begins_with("var"):
				result_table[prop_name] = contained_level_object.get(prop_name)
				property_added = true
	if not property_added:
		return null
	return result_table

func decode_properties(properties):
	if properties != null:
		for prop in properties:
			contained_level_object.set(prop,properties[prop])

func on_mode_change():
	if Constants.currentMode == 1:
		pass
		add_child(construction_collision)
	else:
		pass
		remove_child(construction_collision)

func to_instance(instance):
		instance.x = floor(transform.origin.x)
		instance.y = floor(transform.origin.y)
		instance.z = floor(transform.origin.z)
		var rot = int( round(global_transform.basis.get_euler().y /PI * 2) )
		rot = ( rot + 4) % 4
		instance.rotation = rot
		instance.levelObjectData = levelObjectData
		instance.properties = encode_properties()
		return instance

func _process(delta):
	pass
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
