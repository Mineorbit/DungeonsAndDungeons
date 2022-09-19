extends Node

# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var player = $Player
var builder = $Builder
var level
var playerpref
var builderpref
var playerhudpref
var builderhudpref
@onready var connections = $Connections

var builderhud
var playerhud

# Called when the node enters the scene tree for the first time.
func _ready():
	playerpref = load("res://Prefabs/LevelObjects/Entities/Player.tscn")
	builderpref = load("res://Prefabs/Builder.tscn")
	playerhudpref = load("res://Prefabs/PlayerHUD.tscn")
	builderhudpref = load("res://Prefabs/BuilderHUD.tscn")
	player = playerpref.instantiate()
	builder = builderpref.instantiate()
	playerhud = playerhudpref.instantiate()
	builderhud = builderhudpref.instantiate()
	builder.global_transform.origin = Vector3(0,5,0)
	create_new_level()
	enter_edit_mode()
	Constants.game_won.connect(despawn_players)
	Constants.game_won.connect(enter_edit_mode)
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)

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
	print("Entered EditMode")
	if(Constants.currentMode == 1):
		return
	Constants.set_mode(1)
	if player in get_children():
		despawn_players()
	level.reset()
	add_child(builder)
	if playerhud in get_children():
		remove_child(playerhud)
	add_child(builderhud)
	connections.show()
	

func enter_test_mode():
	print("Entered TestMode")
	if(Constants.currentMode == 2):
		return
	Constants.set_mode(2)
	remove_child(builder)
	await level.start()
	spawn_players()
	if builderhud in get_children():
		remove_child(builderhud)
	add_child(playerhud)
	connections.hide()
	
func despawn_players():
	player.global_transform.origin = Vector3(0.5,-5,0.5)
	await Constants.buffer()
	remove_child(player)
	
func spawn_players():
	player.global_transform.origin = Vector3(0.5,5,0.5)
	await Constants.buffer()
	player._velocity = Vector3.ZERO
	add_child(player)
