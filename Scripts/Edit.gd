extends Node

# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var builder = $Builder
var level
var builderpref
var playerhudpref
var builderhudpref
@onready var Players = $World/Players
@onready var connections = $Connections
@onready var world = $World
var builderhud
var playerhud

# Called when the node enters the scene tree for the first time.
func _ready():
	builderpref = load("res://Prefabs/Builder.tscn")
	playerhudpref = load("res://Prefabs/PlayerHUD.tscn")
	builderhudpref = load("res://Prefabs/BuilderHUD.tscn")
	builder = builderpref.instantiate()
	playerhud = playerhudpref.instantiate()
	builderhud = builderhudpref.instantiate()
	builder.global_transform.origin = Vector3(0,5,0)
	world.create_new_level()
	enter_edit_mode()
	Signals.game_won.connect(Players.despawn_players)
	Signals.game_won.connect(enter_edit_mode)




func edit(name):
	world.level.load(name)


func _process(delta) -> void:
	if Input.is_action_just_pressed("Save"):
		world.level.save()
	if Input.is_action_just_pressed("Load"):
		world.level.load("Test")
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
	Players.despawn_players()
	Players.despawn_player_controllers()
	world.level.reset()
	add_child(builder)
	Constants.playerCamera.player = null
	builder.start()
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
	await world.level.start()
	Players.spawn_players()
	Players.spawn_player_controllers()
	if builderhud in get_children():
		remove_child(builderhud)
	add_child(playerhud)
	Constants.playerCamera.player = Players.get_player(0)
	connections.hide()
	
