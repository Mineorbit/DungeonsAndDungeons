extends CanvasLayer

var target = true

@export var transition_time:float = 0.5


var t = 1

@onready var panel: Panel = $Panel

signal opened


func open():
	if target:
		return
	target = true
	t = 0
	var timer = Timer.new()
	timer.one_shot = true
	add_child(timer)
	timer.start(transition_time)
	return timer


func close():
	if not target:
		return
	target = false
	t = 1


func _process(delta):
	var dir = -1
	if target:
		dir = 1
	if t > 0:
		visible = true
	else:
		visible = false
	var velocity = (dir*delta)/transition_time
	var last_t = t
	t = clamp(t+velocity,0,1)
	if last_t <= 0.99 and t > 0.99:
		opened.emit()
	panel.modulate.a = t
	
