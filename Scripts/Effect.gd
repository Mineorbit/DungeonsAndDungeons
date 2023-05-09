extends Node3D

@onready var spike: GPUParticles3D = $Spike

func trigger():
	spike.restart()
