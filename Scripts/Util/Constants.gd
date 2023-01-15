extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var currentMode = 0

var navmargin = 0.6

var World:
	set(value):
		World = value
		Signals.on_new_world_created.emit()

@export var deathplane = -8


# this function is stored by a client to disable the processing of all entities processes
var entity_control_function

#local id of the player currently controlled
var currentPlayer = 0

var LevelObjectData = {}


var levelObjects_initialized = false

var builder: Node3D

var builderPosition = Vector3.ZERO

var remoteAddress = "127.0.0.1"

var players

var id = 0

var player_hud

var player_colors = [Color.BLUE, Color.RED, Color.GREEN, Color.YELLOW]
var alt_player_colors = [Color.CORAL,Color.DARK_OLIVE_GREEN,Color.DARK_RED,Color.NAVY_BLUE]
var Default_Floor = load("res://Resources/LevelObjectData/Floor.tres")
# Called when the node enters the scene tree for the first time.


func buffer():
	await get_tree().physics_frame
	await get_tree().physics_frame
	await get_tree().physics_frame




func set_mode(new_mode):
	currentMode = new_mode
	Signals.mode_changed.emit()
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
