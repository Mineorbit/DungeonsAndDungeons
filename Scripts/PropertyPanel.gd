extends MarginContainer

var editing_level_object
var property_name
@onready var property_name_label:Label = $VBoxContainer/PropertyName
@onready var property_value:TextEdit = $VBoxContainer/PropertyValue

func setup(level_object,prop_name):
	property_name = prop_name
	editing_level_object = level_object
	var prop_label = prop_name
	prop_label = prop_label.trim_prefix("var_")
	prop_label = prop_label.capitalize()
	property_name_label.text = prop_label
	property_value.text = str(level_object.get(prop_name))

func text_changed():
	print("Setting value "+str(property_value.text))
	if property_value.text in ["false","true"]:
		if property_value.text == "false":
			print("SET "+str(editing_level_object))
			editing_level_object.set(property_name,false)
		else:
			editing_level_object.set(property_name,true)
	else:
		editing_level_object.set(property_name,property_value.text)
