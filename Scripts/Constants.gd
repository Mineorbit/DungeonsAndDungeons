extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var currentMode = 0

var LevelObjectData = {}

var levelObjects_initialized = false

onready var Default_Floor: LevelObjectData = load("res://Resources/LevelObjectData/Floor.tres")
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


signal mode_changed
func set_mode(new_mode):
	currentMode = new_mode
	emit_signal("mode_changed")
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
