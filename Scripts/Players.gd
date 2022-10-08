extends Node3D


var playerpref
@onready var playerEntities = $PlayerEntities
@onready var playerControllers = $PlayerControllers
var players = [null,null,null,null]
# Called when the node enters the scene tree for the first time.
func _ready():
	playerpref = load("res://Prefabs/LevelObjects/Entities/Player.tscn")


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass


func set_current_player(number):
	print(number)
	Constants.currentPlayer = number
	for i in range(4):
		if players[i] != null:
			playerControllers.of(i).set_active(i == number)

func despawn_players():
	print("Despawning Players")
	for i in range(4):
		var player = players[i]
		if player == null:
			continue
		player.global_transform.origin = Vector3(0.5,-5,0.5)
		player.on_entity_despawn.emit()
		await Constants.buffer()
		playerEntities.remove_child(player)
	Constants.players = players

func despawn_player_controllers():
	for i in range(4):
		if players[i] != null:
			playerControllers.of(i).despawn()
		

func spawn_player_controllers():
	for i in range(4):
		spawn_player_controller(i)
	playerControllers.of(0).spawn()
	set_current_player(0)


func spawn_player_controller(i):
	playerControllers.add(players[i],i)	
	players[i].playercontroller = playerControllers.of(i)


func spawn_player(i):
		var player
		if players[i] == null:
			player = playerpref.instantiate()
			player.id = i
			players[i] = player
		else:
			player = players[i]
		playerEntities.add_child(player)
		player.global_transform.origin = Vector3(0.5 + i * 2,5,0.5)
		await Constants.buffer()
		player._velocity = Vector3.ZERO
		player.start()
		Constants.players = players

func spawn_players():
	print("Spawning Players")
	for i in range(4):
		spawn_player(i)

	# this is shit but it works
