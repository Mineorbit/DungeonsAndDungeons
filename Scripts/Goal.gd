extends Node3D


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


var numberOfPlayersNeeded = 1

var numberOfPlayersInside = 0
var enterArea: Area3D
signal game_won

func reset():
	numberOfPlayersInside = 0
	
func _ready():
	enterArea = $Area
	enterArea.body_entered.connect(playerEntered)
	enterArea.body_exited.connect(playerLeft)
	
func playerEntered(player):
	print("ENTERED")
	print(numberOfPlayersInside)
	if player.name == "Player":
		numberOfPlayersInside = numberOfPlayersInside + 1
	if numberOfPlayersInside == numberOfPlayersNeeded:
		print("Game won!")
		Constants.game_won.emit()

func playerLeft(player):
	print("LEFT")
	print(numberOfPlayersInside)
	if player.name == "Player":
		numberOfPlayersInside = max(0,numberOfPlayersInside - 1)

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
