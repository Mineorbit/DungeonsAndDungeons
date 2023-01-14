extends Node

# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var builder = $Builder
var level
var builderpref
var playerhudpref
var testhudpref
var builderhudpref
@onready var Players = $World/Players
@onready var connections = $Connections
@onready var world = $World
var builderhud
var playerhud
var testhud


@onready var buildmusic = $buildmusic

# Called when the node enters the scene tree for the first time.
func _ready():
	builderpref = load("res://Prefabs/Builder.tscn")
	playerhudpref = load("res://Prefabs/PlayerHUD.tscn")
	builderhudpref = load("res://Prefabs/BuilderHUD.tscn")
	testhudpref = load("res://Prefabs/TestHUD.tscn")
	builder = builderpref.instantiate()
	playerhud = playerhudpref.instantiate()
	builderhud = builderhudpref.instantiate()
	testhud = testhudpref.instantiate()
	builder.global_transform.origin = Vector3(0,5,0)
	world.create_new_level()
	enter_edit_mode()
	world.game_won.connect(enter_edit_mode)
	Constants.World.players.player_removed.connect(func(player):
		if player.id == current_player:
			next_player())


func next_player():
		current_player = (current_player + 1) % 4
		while Players.get_player(current_player) == null:
			current_player = (current_player + 1) % 4
		Players.playerControllers.set_current_player(current_player)
		PlayerCamera.player = Players.get_player(current_player)


func edit(name):
	world.level.load(name)

var current_player = 0


func _process(delta) -> void:
	if Input.is_action_just_pressed("Save"):
		world.level.save()
	if Input.is_action_just_pressed("Test"):
		enter_test_mode()
	if Input.is_action_just_pressed("Edit"):
		enter_edit_mode()
	if Input.is_action_just_pressed("SwitchPlayer") and Constants.currentMode == 2:
		next_player()
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
func enter_edit_mode():
	print("Entered EditMode")
	if(Constants.currentMode == 1):
		print("Allready in EditMode")
		return
	Constants.set_mode(1)
	Players.despawn_players()
	Players.despawn_player_controllers()
	world.level.reset()
	add_child(builder)
	builder.start()
	if playerhud in get_children():
		remove_child(playerhud)
		remove_child(testhud)
	add_child(builderhud)
	buildmusic.play()
	connections.show()

func enter_test_mode():
	print("Entered TestMode")
	if(Constants.currentMode == 2):
		print("Allready in TestMode")
		return
	Constants.set_mode(2)
	remove_child(builder)
	await world.level.start()
	Players.spawn_players()
	Players.spawn_player_controllers()
	if builderhud in get_children():
		remove_child(builderhud)
	add_child(playerhud)
	add_child(testhud)
	Players.set_start_positions()
	Players.playerControllers.set_current_player(current_player)
	PlayerCamera.player = Players.get_player(current_player)
	buildmusic.stop()
	connections.hide()
	

