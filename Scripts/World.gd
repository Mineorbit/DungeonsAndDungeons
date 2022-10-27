extends Node3D

@onready var players = $Players
@onready var light = $DirectionalLight
var level


func _ready():
	light.hide()

# Called when the node enters the scene tree for the first time.
func start(levelpath = null,immediate = false):
	light.show()
	prepare_level()
	if levelpath != null:
		await level.load(levelpath,immediate)
	await level.start()
	

func end():
	light.hide()
	if level != null:
		level.clear()
		level.queue_free()


func prepare_level():
	level = load("res://Prefabs/Level.tscn").instantiate()
	add_child(level)
	

func create_new_level():
	prepare_level()
	level.setup_new()
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
