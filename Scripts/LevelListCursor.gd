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
		rpc("move",event.relative)
	elif event is InputEventMouseButton:
		rpc("click",event.button_index,event.pressed)


@rpc(any_peer)
func click(code,pressed):
	var event = InputEventMouseButton.new()
	event.button_index = code
	event.pressed = pressed
	event.position = position
	Input.parse_input_event(event)


@rpc(any_peer,unreliable_ordered)
func move(direction):
	var event = InputEventMouseMotion.new()
	event.relative = direction
	
	Input.parse_input_event(event)
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
