extends Node3D

@onready var world = $World
# Called when the node enters the scene tree for the first time.
func _ready():
	pass
	

func add_player(id):
	world.players.spawn_player(id)

func add_player_controller(id = 0,peer_id = 0):
	var playercontroller = world.players.spawn_player_controller(id,peer_id)
