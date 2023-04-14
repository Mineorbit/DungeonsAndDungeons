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


@export var SwordStrikeTime = 0.45
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
var Water = load("res://Resources/LevelObjectData/Water.tres")
# Called when the node enters the scene tree for the first time.


var PlayerCamera


func buffer():
	await get_tree().physics_frame
	await get_tree().physics_frame
	await get_tree().physics_frame


var t = 0
var water_speed = 0.125
var materials = []


func _ready():
	pass
	#var mesh_library = load("res://Resources/grid.tres")
	#for index in Constants.Water.tileIndex:
	#	if mesh_library.get_item_mesh(index) != null:
	#		var mat = mesh_library.get_item_mesh(index).surface_get_material(0)
	#		materials.append(mat)

func _process(delta):
	t += delta
	for mat in materials:
		mat.set_shader_parameter("triplanar_offset",Vector3(water_speed*t,water_speed*t,water_speed*t))



func set_mode(new_mode):
	currentMode = new_mode
	Signals.mode_changed.emit()
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
