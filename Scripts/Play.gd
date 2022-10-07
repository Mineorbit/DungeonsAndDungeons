extends Node3D

@onready var world = $World
# Called when the node enters the scene tree for the first time.
func _ready():
	pass
	

func add_player(id):
	world.players.spawn_player(id)
	var new_controller = world.players.spawn_player_controller(id)
	world.players.playerControllers.of(id).set_active(true)
	print(world.players.playerControllers.get_children())

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if Input.is_action_just_pressed("Connect"):
		print(world.players.playerControllers.get_children())
