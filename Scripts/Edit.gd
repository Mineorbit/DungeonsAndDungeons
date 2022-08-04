extends Node

# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var player = $Player
var builder = $Builder
var level
var playerpref
var builderpref
# Called when the node enters the scene tree for the first time.
func _ready():
	playerpref = load("res://Prefabs/LevelObjects/Entities/Player.tscn")
	builderpref = load("res://Prefabs/Builder.tscn")

	player = playerpref.instantiate()
	builder = builderpref.instantiate()
	builder.global_transform.origin = Vector3(0,5,0)
	create_new_level()
	enter_edit_mode()

func create_new_level():
	level = load("res://Prefabs/Level.tscn").instantiate()
	add_child(level)
	level.setup_new()




func _process(delta) -> void:
	if Input.is_action_just_pressed("Save"):
		level.save()
	if Input.is_action_just_pressed("Load"):
		level.load("Test")
	if Input.is_action_just_pressed("Test"):
		enter_test_mode()
	if Input.is_action_just_pressed("Edit"):
		enter_edit_mode()
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
func enter_edit_mode():
	if(Constants.currentMode == 1):
		return
	Constants.set_mode(1)
	if player in get_children():
		remove_child(player)
	add_child(builder)
	

func enter_test_mode():
	if(Constants.currentMode == 2):
		return
	Constants.set_mode(2)
	remove_child(builder)
	add_child(player)
	player.global_transform.origin = Vector3(0.5,5,0.5)
	
	
