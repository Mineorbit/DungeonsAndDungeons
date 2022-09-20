extends Node


var playercontroller_prefab
# Called when the node enters the scene tree for the first time.
func _ready():
	playercontroller_prefab.load("res://Prefabs/PlayerController.tscn")


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
