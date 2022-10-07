extends Node3D

@onready var world = $World
# Called when the node enters the scene tree for the first time.
func _ready():
	pass
	

func add_player(id):
	world.players.spawn_player(0)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
