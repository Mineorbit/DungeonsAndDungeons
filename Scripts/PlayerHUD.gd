extends Node2D


@onready var playerhealthbar = $CanvasLayer/PlayerUI/Layout/MarginContainer/CanvasLayer/HealthBar
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


func _process(delta):
	if(Constants.players.size() > 0):
		playerhealthbar.value = Constants.players[0].health
