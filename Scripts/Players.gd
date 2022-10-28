extends Node3D


var playerpref
@onready var playerEntities = $PlayerEntities
@onready var playerControllers = $PlayerControllers
# Called when the node enters the scene tree for the first time.
func _ready():
	playerpref = load("res://Prefabs/LevelObjects/Entities/Player.tscn")
	Constants.players = self
	


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass


signal player_spawned(local_id)

var number_of_players = 0

func spawn():
	for i in range(4):
		if get_player(i) != null:
			var base = Vector3.ZERO
			if Constants.currentLevel.player_spawns[i] != null:
				base = Constants.currentLevel.player_spawns[i].global_transform.origin
			else:
				base = Vector3(2*i,2,0)
			get_player(i).global_transform.origin = base
			print("Spawning at "+str(base))
			player_spawned.emit(i)

func set_current_player(number):
	Constants.currentPlayer = number
	for i in range(4):
		if get_player(i) != null:
			playerControllers.of(i).set_active(i == number)

func despawn_players():
	print("Despawning Players")
	for i in range(4):
		var player = get_player(i)
		if player == null:
			continue
		player.global_transform.origin = Vector3(0.5,-5,0.5)
		player.on_entity_despawn.emit()
		await Constants.buffer()
		number_of_players = number_of_players - 1
		playerEntities.remove_child(player)

func despawn_player_controllers():
	for i in range(4):
		if get_player(i) != null:
			playerControllers.of(i).despawn()
		

func spawn_player_controllers():
	for i in range(4):
		spawn_player_controller(i)
	playerControllers.of(0).spawn()
	set_current_player(0)


func spawn_player_controller(i,owner_id = 0):
	var p = get_player(i)
	playerControllers.add(p,i,owner_id)	
	p.playercontroller = playerControllers.of(i)
	p.playercontroller.player = p
	return p.playercontroller


func spawn_player(i):
		var player
		if get_player(i) == null:
			player = playerpref.instantiate()
			player.name = str(i)
			player.id = i
		else:
			player = get_player(i)
		playerEntities.add_child(player)
		player.global_transform.origin = Vector3(0.5 + i * 2,5,0.5)
		await Constants.buffer()
		player._velocity = Vector3.ZERO
		player_spawned.emit(i)
		number_of_players = number_of_players + 1
		player.start()

func get_player(i):
	for child in playerEntities.get_children():
		if child.name == str(i):
			return child
	return null

func spawn_players():
	print("Spawning Players")
	for i in range(4):
		spawn_player(i)

	# this is shit but it works
