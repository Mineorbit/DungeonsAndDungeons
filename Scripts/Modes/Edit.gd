extends Node

# Declare member variables here. Examples:
# var a = 2
# var b = "text"

@onready var builder = $Builder
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

var level_time = 0
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
	enter_edit_mode()
	Constants.World.game_won.connect(enter_edit_mode)


func next_player():
		current_player = (current_player + 1) % 4
		# can only look 4 times else there is no player
		var count = 0
		while Players.get_player(current_player) == null and count < 4:
			current_player = (current_player + 1) % 4
			count = count + 1
		Players.playerControllers.set_current_player(current_player)
		PlayerCamera.player = Players.get_player(current_player)


func prepare_edit(name):
	if Constants.World == null:
		Constants.World = get_node("World")
	if name == null:
		Constants.World.create_new_level()
	else:
		Constants.World.prepare_level()
		Constants.World.level.load(name,true)
	print("Generating Grids")
	var grid_thread = Thread.new()
	#grid_thread.start(func():
	#	Constants.World.level.generate_all_grids()
	#	)
	Constants.World.level.generate_all_grids()

var current_player = 0

func save_image():
	remove_child(builderhud)
	var timer = Timer.new()
	timer.timeout.connect(func():
		var image = get_viewport().get_texture().get_image()
		image.save_png(world.level.get_level_path()+"/thumbnail.png")
		add_child(builderhud)
		)
	timer.one_shot = true
	timer.wait_time = 0.125
	add_child(timer)
	timer.start()

func _process(delta) -> void:
	if (Constants.currentMode == 1) and Input.is_action_just_pressed("Save"):
		world.level.save()
		save_image()
	if not Constants.builder == null and not Constants.builder.editing and Input.is_action_just_pressed("Test"):
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
	if Constants.World.level != null:
		Constants.World.level.reset()
	add_child(builder)
	builder.start()
	if playerhud in get_children():
		remove_child(playerhud)
		remove_child(testhud)
	add_child(builderhud)
	#buildmusic.play()
	connections.show()

func enter_test_mode():
	print("Entered TestMode")
	if(Constants.currentMode == 2):
		print("Allready in TestMode")
		return
	Constants.set_mode(2)
	remove_child(builder)
	await Constants.World.level.start()
	Players.spawn_players()
	Players.spawn_player_controllers()
	if builderhud in get_children():
		remove_child(builderhud)
	add_child(playerhud)
	add_child(testhud)
	Players.set_start_positions()
	for player in Players.get_players():
		player.tree_exited.connect(func():
			if Constants.currentMode == 2 and player.id == current_player:
				next_player())
	Players.playerControllers.set_current_player(current_player)
	PlayerCamera.player = Players.get_player(current_player)
	#buildmusic.stop()
	connections.hide()
	

