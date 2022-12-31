extends Node

var playercontroller_prefab
var playerControllers = [null,null,null,null]

# this variable is only set to the existing PlayerCamera if we are on a client
var camera
var playercamera_prefab
# Called when the node enters the scene tree for the first time.
func _ready():
	create_player_camera()
	if Constants.id != 1:
		camera = get_node("../PlayerCamera")
	playercontroller_prefab = load("res://Prefabs/PlayerController.tscn")

func create_player_camera():
	if camera != null:
		return
	print("Creating Camera")
	playercamera_prefab = load("res://Prefabs/PlayerCamera.tscn")
	# if this is a client create a PlayerCamera
	if Constants.id != 1:
		camera = playercamera_prefab.instantiate()
		get_parent().add_child(camera)

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


func set_current_player(number):
	Constants.currentPlayer = number
	for i in range(4):
		if get_parent().get_player(i) != null:
			of(i).set_active(i == number)
	
	if get_parent().get_player(number) != null:
		of(number).get_player_camera().player = get_parent().get_player(number)
