extends Node3D


# Called when the node enters the scene tree for the first time.
func _ready():
	var level = load("res://Prefabs/Level.tscn").instantiate()
	add_child(level)
	level.setup_new()
	level.load("Test")

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
