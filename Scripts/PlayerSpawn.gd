extends Node3D

@export var local_id: int

# Called when the node enters the scene tree for the first time.
func _ready():
	if Constants.currentLevel != null:
		Constants.currentLevel.player_spawns[local_id] = self


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
