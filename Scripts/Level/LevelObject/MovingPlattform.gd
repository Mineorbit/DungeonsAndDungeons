extends Node3D

var start_pos = Vector3.ZERO
@onready var plattform = $Plattform
@export var var_plattform_distance: float = 8
@export var var_plattform_time: float = 2
var p = 0
func _ready():
	p = 0
	plattform.transform.origin = start_pos

func start():
	p = 0
	plattform.transform.origin = start_pos


func reset():
	p = 0
	plattform.transform.origin = start_pos

var phase = 1
func _physics_process(delta):
	if(Constants.currentMode == 2):
		var a = start_pos
		var b = start_pos + basis.x*var_plattform_distance
		var time = max(var_plattform_time,2)
		var speed = var_plattform_distance/(2*time)
		if p > 1:
			phase = -1
		elif p < 0:
			phase = 1
			
		p += phase*delta*speed
		plattform.transform.origin = p*b + (1-p)*a
