extends Sprite2D


@onready var synchronizer = $MultiplayerSynchronizer
var levellist

func _ready():
	levellist = get_parent().get_parent().get_parent().levellist

func _enter_tree():
	get_node("MultiplayerSynchronizer").set_multiplayer_authority(str(name).to_int())
	set_multiplayer_authority(str(name).to_int())

func _input(event):
	if name != str(Constants.id):
		return
	if event is InputEventMouseMotion:
		transform.origin = event.position
	elif event is InputEventMouseButton:
		rpc_id(get_parent().get_parent().get_parent().owner_id,"click",event.button_index,event.pressed)



@rpc("any_peer")
func click(code,pressed):
	if Constants.id != get_parent().get_parent().get_parent().owner_id:
		return
	var event = InputEventMouseButton.new()
	event.button_index = code+32
	event.pressed = pressed
	event.position = position
	Input.parse_input_event(event)


@rpc("any_peer","unreliable_ordered")
func move(direction,pos):
	if Constants.id != get_parent().get_parent().get_parent().owner_id:
		return
	var event = InputEventMouseMotion.new()
	event.relative = direction
	event.pressure = 1
	event.position = Vector2(-1,-1)
	Input.parse_input_event(event)
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
