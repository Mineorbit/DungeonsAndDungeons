extends Node2D


@onready var playerhealthbar = $CanvasLayer/PlayerUI/Layout/MarginContainer/CanvasLayer/HealthBar

func _ready():
	Constants.player_hud = self

func updateHealth(health,playerid):
	if(playerid == Constants.currentPlayer):
		playerhealthbar.value = health
