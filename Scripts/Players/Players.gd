extends Node3D


@onready var playerEntities = $PlayerEntities
@onready var playerControllers = $PlayerControllers

signal player_added(player)
signal player_removed(player)
signal player_spawned(local_id)

var playerpref


# Called when the node enters the scene tree for the first time.
func _ready():
	playerpref = load("res://Prefabs/LevelObjects/Entities/Player.tscn")
	Constants.players = self
	playerEntities.child_entered_tree.connect(func(player):
		player_added.emit(player))
	playerEntities.child_exiting_tree.connect(func(player):
		player_removed.emit(player))


func set_start_positions():
	for i in range(4):
		set_start_position(i)

func set_start_position(i):
		if get_player(i) != null:
			var base = Vector3.ZERO
			#get_player(i).set_physics_process(false)
			if Constants.World.level != null and Constants.World.level.player_spawns[i] != null:
				base = Constants.World.level.player_spawns[i].global_transform.origin
			else:
				base = Vector3(2*i,2,0)
			print(base)
			get_player(i).set_position(base)
			get_player(i)._velocity = Vector3.ZERO
			player_spawned.emit(i)
	

func despawn_players():
	for i in range(4):
		despawn_player(i)


func spawn_players():
	for i in range(4):
		spawn_player(i)


func despawn_player_controllers():
	for i in range(4):
		if get_player(i) != null:
			playerControllers.playerControllers[i].queue_free()
			playerControllers.playerControllers[i] = null


func spawn_player_controllers():
	for i in range(4):
		spawn_player_controller(i)


func spawn_player_controller(i,owner_id = 0):
	var p = get_player(i)
	playerControllers.add(p,i,owner_id)
	var newplayercontroller = playerControllers.playerControllers[i]
	p.playercontroller = newplayercontroller
	p.playercontroller.player = p
	print("Spawned playercontroller "+str(newplayercontroller))
	return newplayercontroller


func spawn_player(i):
		if playerpref == null:
			playerpref = load("res://Prefabs/LevelObjects/Entities/Player.tscn")
		var player
		if get_player(i) == null:
			player = playerpref.instantiate()
			player.name = str(i)
			player.id = i
		else:
			player = get_player(i)
		playerEntities.add_child(player)
		set_start_position(i)
		await Constants.buffer()
		player._velocity = Vector3.ZERO
		player_spawned.emit(i)
		player.start()
		return player


func despawn_player(i):
	var player = get_player(i)
	if player == null:
		return
	player.global_transform.origin = Vector3(0.5,-5,0.5)
	player.on_entity_despawn.emit()
	print("Despawning Player "+str(player))
	playerEntities.remove_child(player)


func number_of_players():
	return playerEntities.get_child_count()

func get_player(i):
	for child in playerEntities.get_children():
		if child.name == str(i):
			return child
	print("Player not found: "+str(i))
	return null

