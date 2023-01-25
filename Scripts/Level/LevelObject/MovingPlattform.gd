extends Node3D

var plattform
@export var var_plattform_distance: float = 8
@export var var_plattform_time: float = 2
@onready var model = $MovingPlattformModel
var p = 0
func _ready():
	show_plattform()

func start():
	p = 0
	plattform = load("res://Prefabs/LevelObjects/Entities/PlattformEntity.tscn").instantiate()
	plattform.start_pos = global_transform.origin
	plattform.end_pos = global_transform.origin + basis.x*var_plattform_distance
	plattform.plattform_distance = var_plattform_distance
	plattform.plattform_time = var_plattform_time
	Constants.World.level.spawn_entity(plattform)
	show_plattform()

func reset():
	show_plattform()

func show_plattform():
	if Constants.currentMode > 1:
			model.hide()
	else:
			model.show()
	
