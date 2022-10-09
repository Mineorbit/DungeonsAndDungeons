extends Node3D

@onready var players = $Players
var level

# Called when the node enters the scene tree for the first time.
func start(levelpath = null,immediate = false):
	create_new_level()
	if levelpath != null:
		await level.load(levelpath,immediate)
		
	Constants.set_mode(2)
	await level.start()
	print(level.get_children())
	print(level.Entities.get_children())
	

func create_new_level():
	level = load("res://Prefabs/Level.tscn").instantiate()
	add_child(level)
	level.setup_new()
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
