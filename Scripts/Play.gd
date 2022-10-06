extends Node3D

@onready var Players = $Players
# Called when the node enters the scene tree for the first time.
func _ready():
	pass


func start():
	var level = load("res://Prefabs/Level.tscn").instantiate()
	add_child(level)
	level.setup_new()
	level.immediate = true
	await level.load("Test")
	Constants.set_mode(2)
	await level.start()
	Players.spawn_players()
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
