extends Node3D

@onready var fire: Node3D = $Flammable/Fire

@export var var_burning: bool = true:
	set(value):
		var_burning = value
		set_fire()


func set_fire():
	$Flammable.remove_child($Flammable/Fire)
	if var_burning:
		$Flammable.add_child(fire)
	else:
		$Flammable.remove_child(fire)

func _ready():
	set_fire()

func start():
	set_fire()

func reset():
	set_fire()


func on_fire_started(node):
	if Constants.currentMode > 1:
		if get_parent() is InteractiveLevelObject:
			get_parent().activate_connected()

func on_fire_stopped(node):
	if Constants.currentMode > 1:
		if get_parent() is InteractiveLevelObject:
			get_parent().deactivate_connected()
