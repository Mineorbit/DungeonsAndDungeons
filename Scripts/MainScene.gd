extends Node3D

@onready var tavernlight: SpotLight3D = $TavernLight

var t = 0

func _process(delta):
	t += delta
	var energy = 4 + sin(0.5*t)*1
	tavernlight.light_energy = energy
