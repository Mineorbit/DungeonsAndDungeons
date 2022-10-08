extends Node


var playercontroller_prefab
var playerControllers = [null,null,null,null]
# Called when the node enters the scene tree for the first time.
func _ready():
	playercontroller_prefab = load("res://Prefabs/PlayerController.tscn")


func add(playerEntity,id):
	var playercontroller = playercontroller_prefab.instantiate()
	
	playercontroller.name = str(id)
	add_child(playercontroller)
	playercontroller.player = playerEntity
	playerControllers[id] = playercontroller

func of(playerid):
	for node in get_children():
		if node.name == str(playerid):
			return node
	return playerControllers[playerid]
