extends Panel




@onready var properties = $Properties
var editing_level_object: Node3D

func _ready():
	Signals.edited_level_object.connect(func (object):
		if object == null:
			hide()
			return
		editing_level_object = object.contained_level_object
		var prefab = load("res://Prefabs/Property.tscn")

		for prop in object.contained_level_object.get_property_list():
			var prop_name = prop["name"]
			if prop_name.begins_with("var"):
				var propertypanel = prefab.instantiate()
				propertypanel.ready.connect(func():
					propertypanel.setup(object.contained_level_object,prop_name)
				)
				properties.add_child(propertypanel)
		show()
		)
	hide()


func stop_property_edit():
	for prop in properties.get_children():
		prop.queue_free()
	Signals.edited_level_object.emit(null)
