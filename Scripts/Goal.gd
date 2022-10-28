extends Node3D


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


var numberOfPlayersNeeded = 1

var numberOfPlayersInside = 0
@onready var enterArea: Area3D = $Area

func reset():
	numberOfPlayersInside = 0
	numberOfPlayersNeeded = Constants.players.number_of_players
	
func _ready():
	enterArea.body_entered.connect(playerEntered)
	enterArea.body_exited.connect(playerLeft)
	
func playerEntered(player):
	if Constants.currentMode != 2:
		return
	print("Entered")
	print("Needed: "+str(numberOfPlayersNeeded))
	numberOfPlayersInside = numberOfPlayersInside + 1
	if numberOfPlayersInside == numberOfPlayersNeeded:
		print("Game won!")
		Signals.game_won.emit()

func playerLeft(player):
	if Constants.currentMode == 2:
		return
	if player.name == "Player":
		numberOfPlayersInside = max(0,numberOfPlayersInside - 1)

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
