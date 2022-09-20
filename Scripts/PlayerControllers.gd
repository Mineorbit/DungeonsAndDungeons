extends Node


var playercontroller_prefab
var playerController
# Called when the node enters the scene tree for the first time.
func _ready():
	playercontroller_prefab = load("res://Prefabs/PlayerController.tscn")


func add(playerEntity):
	var playercontroller = playercontroller_prefab.instantiate()
	add_child(playercontroller)
	playercontroller.player = playerEntity
	playerController = playercontroller

func of(player):
	return playerController
