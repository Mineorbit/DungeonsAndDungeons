extends Node3D


var playerpref
@onready var playerEntities = $PlayerEntities
@onready var playerControllers = $PlayerControllers
var player
# Called when the node enters the scene tree for the first time.
func _ready():
	playerpref = load("res://Prefabs/LevelObjects/Entities/Player.tscn")
	player = playerpref.instantiate()
	playerControllers.add(player)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass


func despawn_players():
	print("Despawning Players")
	player.global_transform.origin = Vector3(0.5,-5,0.5)
	await Constants.buffer()
	playerEntities.remove_child(player)
	playerControllers.of(player).despawn()
	
func spawn_players():
	player.global_transform.origin = Vector3(0.5,5,0.5)
	await Constants.buffer()
	player._velocity = Vector3.ZERO
	playerEntities.add_child(player)
	playerControllers.of(player).spawn()
