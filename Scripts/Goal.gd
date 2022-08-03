extends Spatial


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


var numberOfPlayersNeeded = 1

var numberOfPlayersInside = 0
onready var enterArea: Area = $Area

signal game_won
func _ready():
	enterArea.connect("body_entered",self,"playerEntered")

func playerEntered(player):
	if player.name == "Player":
		numberOfPlayersInside = numberOfPlayersInside + 1
	if numberOfPlayersInside == numberOfPlayersNeeded:
		print("Game won!")

func playerleft(player):
	if player.name == "Player":
		numberOfPlayersInside = numberOfPlayersInside - 1
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
