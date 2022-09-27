extends Node2D


@onready var playerhealthbar = $CanvasLayer/PlayerUI/Layout/MarginContainer/CanvasLayer/HealthBar
# Called when the node enters the scene tree for the first time.
func _ready():
	Signals.currentPlayerHealthChanged.connect(updateHealth)


func updateHealth(health):
	playerhealthbar.value = health
	
