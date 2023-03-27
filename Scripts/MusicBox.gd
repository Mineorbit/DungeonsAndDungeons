extends Node3D

@export var var_music: String = "FunkyBach"
@onready var model = $Model
@onready var audio: AudioStreamPlayer = $Audio
func _ready():
	Signals.mode_changed.connect(show_music)

func reset():
	audio.stop()

func start():
	audio.stream = load("res://Resources/Music/"+var_music+".mp3")
	audio.play(0)

func show_music():
	if Constants.currentMode > 1:
			model.hide()
	else:
			model.show()
