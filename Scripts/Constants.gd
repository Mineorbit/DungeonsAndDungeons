extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var LevelObjectData = {}

var levelObjects_initialized = false

onready var Default_Floor: LevelObjectData = load("res://Resources/LevelObjectData/Floor.tres")
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
