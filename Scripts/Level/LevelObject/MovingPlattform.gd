extends Node3D

var start_pos = Vector3.ZERO
@onready var plattform = $Plattform
@export var plattformDistance: float = 8

var t = 0
func _ready():
	plattform.transform.origin = start_pos

func start():
	t = 0
	plattform.transform.origin = start_pos


func reset():
	plattform.transform.origin = start_pos


func _physics_process(delta):
	if(Constants.currentMode == 2):
		t += delta
		var a = start_pos
		var b = start_pos + basis.x*plattformDistance
		var p = cos(t)*0.5 + 0.5
		plattform.transform.origin = (1-p)*b + p*a
