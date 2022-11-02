extends Sprite2D


var levellist


func _input(event):
	if event is InputEventMouseMotion:
		if name == str(Constants.id):
			transform.origin = event.position

	if not should_send():
		return
	if event is InputEventMouseMotion:
		rpc_id(1,"move",event.relative,event.position)
	elif event is InputEventMouseButton:
		rpc_id(1,"click",event.button_index,event.pressed)

func should_send():
	return name != str(Constants.id)


@rpc(any_peer)
func click(code,pressed):
	if Constants.id != 1:
		return
	var event = InputEventMouseButton.new()
	event.button_index = code
	event.pressed = pressed
	event.position = position
	Input.parse_input_event(event)
	#get_parent().get_parent().get_parent()._input(event)


@rpc(any_peer,unreliable_ordered)
func move(direction,pos):
	if Constants.id != 1:
		return
	var event = InputEventMouseMotion.new()
	event.relative = direction
	event.position = pos
	Input.parse_input_event(event)
	#get_parent().get_parent().get_parent()._input(event)
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
