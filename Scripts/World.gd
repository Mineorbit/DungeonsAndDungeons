extends Node3D

@onready var players = $Players
var level

# Called when the node enters the scene tree for the first time.
func start(levelpath = null,immediate = false):
	prepare_level()
	if levelpath != null:
		await level.load(levelpath,immediate)
	await level.start()
	

func prepare_level():
	level = load("res://Prefabs/Level.tscn").instantiate()
	add_child(level)
	

func create_new_level():
	prepare_level()
	level.setup_new()
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
