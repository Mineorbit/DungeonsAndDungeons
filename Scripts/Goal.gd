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
func playerEntered(player):
	if player.name == "Player":
		numberOfPlayersInside = numberOfPlayersInside + 1
	if numberOfPlayersInside == numberOfPlayersNeeded:
		print("Game won!")
		Constants.game_won.emit()

func playerleft(player):
	if player.name == "Player":
		numberOfPlayersInside = numberOfPlayersInside - 1
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
