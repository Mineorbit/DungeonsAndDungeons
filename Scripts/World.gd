extends Node3D

@onready var players: Node3D = $Players
@onready var light = $DirectionalLight
var level

signal game_won

signal on_entity_spawned(entity)

func _ready():
	Constants.World = self
	print(str(Constants.id)+" "+str(Constants.World))

# Called when the node enters the scene tree for the first time.
func start(levelpath = null,immediate = false,download_level = false):
	prepare_level()
	if levelpath != null:
		await level.load(levelpath,immediate,download_level)
	await level.start()
	

func end():
	if level != null:
		level.clear()
		level.queue_free()
		level = null


func prepare_level():
	level = load("res://Prefabs/Level.tscn").instantiate()
	add_child(level)
	

func create_new_level():
	prepare_level()
	level.setup_new()
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
