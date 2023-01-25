extends Entity

var start_pos
var end_pos

var plattform_distance: float = 8
var plattform_time: float = 2
var p = 0
var phase = 1

func _physics_process(delta):
	if(Constants.currentMode == 2):
		var a = start_pos
		var b = end_pos
		var time = max(plattform_distance,2)
		var speed = plattform_distance/(2*plattform_time)
		if p > 1:
			phase = -1
		elif p < 0:
			phase = 1
		p += phase*delta*speed
		transform.origin = p*b + (1-p)*a
