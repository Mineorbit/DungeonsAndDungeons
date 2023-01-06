extends Node3D


# Declare member variables here. Examples:
# var a = 2
# var b = "text"



var numberOfPlayersInside = 0
@onready var enterArea: Area3D = $Area



func reset():
	print("Resetting Goal")
	numberOfPlayersInside = 0
	can_enter = true

var can_enter = true

func number_of_players_needed():
	return Constants.World.players.number_of_players()

func _ready():
	if Constants.World != null:
		Constants.World.level.player_goal = self
	enterArea.body_entered.connect(playerEntered)
	enterArea.body_exited.connect(playerLeft)
	
func playerEntered(_player):
	if not can_enter:
		return
	numberOfPlayersInside = numberOfPlayersInside + 1
	print(str(numberOfPlayersInside)+"/"+str(number_of_players_needed()))
	if numberOfPlayersInside == number_of_players_needed():
		can_enter = false
		print("Game won!")
		Constants.World.game_won.emit()

func playerLeft(player):
	if not can_enter:
		return
	numberOfPlayersInside = max(0,numberOfPlayersInside - 1)

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
