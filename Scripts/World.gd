extends Node3D

@onready var players: Node3D = $Players
@onready var light = $DirectionalLight
@onready var ChunkStreamers = $ChunkStreamers
var level

signal game_won

signal on_entity_spawned(entity)

func _ready():
	Constants.World = self
	print(str(Constants.id)+" "+str(Constants.World))

# Called when the node enters the scene tree for the first time.
func start(levelpath = null,immediate = false,download_level = false,start_now = true):
	prepare_level()
	if levelpath != null:
		await level.load(levelpath,immediate,download_level)
		#level.generate_all_grids()
	# we need a signal fo the level being loaded
	if start_now:
		await level.start()
	

func end():
	if level != null:
		level.clear()
		level.queue_free()
		level = null


func prepare_level():
	#print_stack()
	level = load("res://Prefabs/Level.tscn").instantiate()
	add_child(level)
	

func create_new_level():
	prepare_level()
	level.setup_new()
	level.generate_all_grids()

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
