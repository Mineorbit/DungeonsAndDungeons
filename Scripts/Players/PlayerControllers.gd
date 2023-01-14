extends Node

var playercontroller_prefab
var playerControllers = [null,null,null,null]


var playercamera_prefab
# Called when the node enters the scene tree for the first time.
func _ready():
	playercontroller_prefab = load("res://Prefabs/PlayerController.tscn")


func add(playerEntity,id, owner_id = 0):
	var playercontroller = playercontroller_prefab.instantiate()
	playercontroller.player = playerEntity
	playerControllers[id] = playercontroller
	playercontroller.name = str(owner_id)
	add_child(playercontroller)



func set_current_player(number):
	Constants.currentPlayer = number
	for i in range(4):
		if get_parent().get_player(i) != null:
			playerControllers[i].set_active(i == number)
	
	if get_parent().get_player(number) != null:
		print(playerControllers[number])
