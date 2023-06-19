extends Node3D


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


@onready var goalAudio: AudioStreamPlayer3D = $ambient

var numberOfPlayersInside = 0
@onready var enterArea: Area3D = $Area


var can_enter = true

func reset():
	numberOfPlayersInside = 0
	can_enter = true
	if Constants.currentMode > 1:
		goalAudio.play()
	else:
		goalAudio.stop()

func start():
	Constants.World.level.player_goal = self
	if Constants.currentMode > 1:
		goalAudio.play()
	else:
		goalAudio.stop()

func number_of_players_needed():
	return Constants.World.players.number_of_players()

func _ready():
	enterArea.body_entered.connect(playerEntered)
	enterArea.body_exited.connect(playerLeft)
	
func playerEntered(_player):
	if Constants.currentMode == 3:
		return
	if not can_enter:
		return
	numberOfPlayersInside = numberOfPlayersInside + 1
	print(str(numberOfPlayersInside)+"/"+str(number_of_players_needed())+" Players reached the Goal")
	if numberOfPlayersInside == number_of_players_needed():
		can_enter = false
		print("Game won!")
		Constants.World.game_won.emit()

func playerLeft(player):
	if Constants.currentMode == 3:
		return
	if not can_enter:
		return
	numberOfPlayersInside = max(0,numberOfPlayersInside - 1)

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
