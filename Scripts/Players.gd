extends Node3D


@onready var playerEntities = $PlayerEntities
@onready var playerControllers = $PlayerControllers

signal player_added(player)
signal player_spawned(local_id)

var number_of_players = 0
var playerpref


# Called when the node enters the scene tree for the first time.
func _ready():
	playerpref = load("res://Prefabs/LevelObjects/Entities/Player.tscn")
	Constants.players = self
	playerEntities.child_entered_tree.connect(new_player_added)

func new_player_added(node):
	player_added.emit(node)


func spawn():
	for i in range(4):
		if get_player(i) != null:
			var base = Vector3.ZERO
			if Constants.World.level.player_spawns[i] != null:
				base = Constants.World.level.player_spawns[i].global_transform.origin
			else:
				base = Vector3(2*i,8,0)
			base = Vector3(2*i,8,0)
			get_player(i).global_transform.origin = base
			get_player(i)._velocity = Vector3.ZERO
			print("Spawning at "+str(base))
			player_spawned.emit(i)


func despawn_players():
	print("Despawning Players")
	for i in range(4):
		despawn_player(i)


func spawn_players():
	print("Spawning Players")
	for i in range(4):
		spawn_player(i)


func despawn_player_controllers():
	for i in range(4):
		if get_player(i) != null:
			playerControllers.of(i).despawn()


func spawn_player_controllers():
	for i in range(4):
		spawn_player_controller(i)
	playerControllers.of(0).spawn()


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


func despawn_player(i):
	var player = get_player(i)
	if player == null:
		return
	player.global_transform.origin = Vector3(0.5,-5,0.5)
	player.on_entity_despawn.emit()
	await Constants.buffer()
	number_of_players = number_of_players - 1
	playerEntities.remove_child(player)


func get_player(i):
	for child in playerEntities.get_children():
		if child.name == str(i):
			return child
	return null
