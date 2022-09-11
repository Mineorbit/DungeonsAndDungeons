extends Node3D


var display = true
# Called when the node enters the scene tree for the first time.
func _ready():
	var i = 0
	for levelObject in get_children():
		levelObject.transform.origin = Vector3(4*i,0,0)
		i = i+1

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
