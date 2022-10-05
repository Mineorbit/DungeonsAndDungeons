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

func despawn_player_controllers():
	for i in range(4):
		playerControllers.of(i).despawn()
		

func spawn_player_controllers():
	for i in range(4):
		playerControllers.add(players[i],i)	
	Constants.players = players
	playerControllers.of(0).spawn()
	set_current_player(0)


func spawn_players():
	print("Spawning Players")
	for i in range(4):
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

	# this is shit but it works
