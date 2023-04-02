extends CanvasLayer

var is_open = true

var transition_time = 2

var t = 1

@onready var panel: Panel = $Panel


func open():
	if is_open:
		return
	is_open = true
	t = 0


func close():
	if not is_open:
		return
	is_open = false
	t = 1


func _process(delta):
	var dir = -1
	if is_open:
		dir = 1
	if t > 0:
		visible = true
	else:
		visible = false
	var velocity = (dir*delta)/transition_time
	t = clamp(t+velocity,0,1)
	panel.modulate.a = t
	
