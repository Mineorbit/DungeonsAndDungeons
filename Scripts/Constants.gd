extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var currentMode = 0

var navmargin = 0.6

var currentLevel

@export var deathplane = -8

var currentInteractive = 0

var currentPlayer = 0

var LevelObjectData = {}


var interactiveLevelObjects = {}

var levelObjects_initialized = false


var builderPosition = Vector3.ZERO

var playerCamera

var remoteAddress = "127.0.0.1"

var players

var id = 0

var player_colors = [Color.BLUE, Color.RED, Color.GREEN, Color.YELLOW]

var Default_Floor = load("res://Resources/LevelObjectData/Floor.tres")
# Called when the node enters the scene tree for the first time.



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
