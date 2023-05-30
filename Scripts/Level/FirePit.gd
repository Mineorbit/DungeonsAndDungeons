extends Node3D

@onready var fire: Node3D = $Flammable/Fire

@export var var_burning: bool = true:
	set(value):
		var_burning = value
		set_fire()


func set_fire():
	if var_burning:
		$Flammable.add_child(fire)
	else:
		print("Removing Fire")
		$Flammable.remove_child(fire)

func _ready():
	set_fire()

func start():
	set_fire()

func reset():
	set_fire()
