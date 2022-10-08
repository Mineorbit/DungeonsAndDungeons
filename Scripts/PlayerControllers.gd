extends Node


var playercontroller_prefab
var playerControllers = [null,null,null,null]
# Called when the node enters the scene tree for the first time.
func _ready():
	playercontroller_prefab = load("res://Prefabs/PlayerController.tscn")


func add(playerEntity,id, owner_id = 0):
	var playercontroller = playercontroller_prefab.instantiate()
	playercontroller.player = playerEntity
	playerControllers[id] = playercontroller
	playercontroller.name = str(owner_id)
	add_child(playercontroller)

func of(playerid):
	for node in get_children():
		if node.name == str(playerid):
			return node
	return playerControllers[playerid]
