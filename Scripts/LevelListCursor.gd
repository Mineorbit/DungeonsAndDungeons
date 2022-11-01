extends Sprite2D


@onready var synchronizer = $MultiplayerSynchronizer
var levellist
func _ready():
	synchronizer.set_multiplayer_authority(str(name).to_int())
	set_multiplayer_authority(str(name).to_int())
	levellist = get_parent().get_parent().get_parent().levellist


func _input(event):
	if name != str(Constants.id):
		return
	if event is InputEventMouseMotion:
		transform.origin = event.position
		rpc_id(1,"move",event.relative,event.pos)
	elif event is InputEventMouseButton:
		rpc_id(1,"click",event.button_index,event.pressed)


@rpc(any_peer)
func click(code,pressed):
	if Constants.id != 1:
		return
	var event = InputEventMouseButton.new()
	event.button_index = code
	event.pressed = pressed
	event.position = position
	#Input.parse_input_event(event)
	get_parent().get_parent().get_parent()._input(event)


@rpc(any_peer,unreliable_ordered)
func move(direction,pos):
	if Constants.id != 1:
		return
	var event = InputEventMouseMotion.new()
	event.relative = direction
	event.position = pos
	get_parent().get_parent().get_parent()._input(event)
	#Input.parse_input_event(event)
	#get_parent().get_parent().push_input(event,true)
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
