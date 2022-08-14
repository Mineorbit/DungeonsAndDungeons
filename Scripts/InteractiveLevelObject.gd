extends LevelObject


# Declare member variables here. Examples:
# var a = 2
# var b = "text"
var unique_instance_id = 0

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


func activate():
	if contained_level_object.has_method("activate"):
		contained_level_object.activate()


func deactivate():
	if contained_level_object.has_method("deactivate"):
		contained_level_object.deactivate()

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
