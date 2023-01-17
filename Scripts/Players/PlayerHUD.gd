extends Node2D


@onready var playerhealthbar = $CanvasLayer/PlayerUI/Layout/MarginContainer/CanvasLayer/HealthBar
@onready var time = $CanvasLayer/Time
func _ready():
	Constants.player_hud = self

func updateHealth(health,playerid):
	if(playerid == Constants.currentPlayer):
		playerhealthbar.value = health

func _process(delta):
	var total_seconds = get_parent().level_time
	# this code is from https://godotengine.org/qa/130614/formatting-a-timer
	var seconds:float = fmod(total_seconds , 60.0)
	var minutes:int   =  int(total_seconds / 60.0) % 60
	var hours:  int   =  int(total_seconds / 3600.0)
	var hhmmss_string:String = "%02d:%02d:%05.2f" % [hours, minutes, seconds]
	if total_seconds > 0.01:
		time.text = hhmmss_string
	else:
		time.text = ""
