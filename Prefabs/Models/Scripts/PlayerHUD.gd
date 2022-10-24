extends Node2D


@onready var playerhealthbar = $CanvasLayer/PlayerUI/Layout/MarginContainer/CanvasLayer/HealthBar
# Called when the node enters the scene tree for the first time.
func _ready():
	Signals.playerHealthChanged.connect(updateHealth)


func updateHealth(playerid,health):
	if(playerid == Constants.currentPlayer):
		playerhealthbar.value = health
	
