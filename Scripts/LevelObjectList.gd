extends Node3D


@export var display = false
# Called when the node enters the scene tree for the first time.
func _ready():
	pass
	var i = 0
	for levelObject in get_children():
		levelObject.transform.origin = Vector3(2*i + 1,0,0)
		i = i+1

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if display:
		for child in get_children():
			child.rotate(Vector3.UP,delta)
