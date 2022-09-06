extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var currentMode = 0

var currentLevel

var currentEntities

var currentInteractive = 0

var players = []

var LevelObjectData = {}
var numberOfPlacedLevelObjects = {}

var interactiveLevelObjects = {}

var levelObjects_initialized = false

signal game_won

var Default_Floor = load("res://Resources/LevelObjectData/Floor.tres")
# Called when the node enters the scene tree for the first time.
func _ready():
	currentEntities = load("res://Prefabs/Entities.tscn").instantiate()
	add_child(currentEntities)


func buffer():
	await get_tree().physics_frame
	await get_tree().physics_frame
	await get_tree().physics_frame

signal mode_changed
func set_mode(new_mode):
	currentMode = new_mode
	emit_signal("mode_changed")
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
