extends Node3D

@onready var world = $World
# Called when the node enters the scene tree for the first time.
func _ready():
	pass
	

func add_player(id):
	world.players.spawn_player(id)

func add_player_controller(id = 0):
	world.players.spawn_player_controller(id)
	world.players.playerControllers.of(id).set_active(true)
	print("New Controller "+str(world.players.playerControllers.get_children()))
	return world.players.playerControllers.of(id)

var set = false
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if Constants.is_client:
		if (not set) and world.players.playerControllers.of(0) != null:
			set = true
			world.players.playerControllers.of(0).get_child(0).set_multiplayer_authority(Constants.id)
	if Input.is_action_just_pressed("Connect"):
		print(world.players.playerControllers.get_children())


