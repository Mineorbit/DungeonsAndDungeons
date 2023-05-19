extends Node

var player
var is_active = false

@export var input_direction = Vector3.ZERO
@onready var playercamera = $PlayerCamera
@onready var playercameraposition = $PlayerCamera/Camera
@onready var synchronizer = $MultiplayerSynchronizer
# Called when the node enters the scene tree for the first time.
func _ready():
	setup_playercontroller_networking()
	playercamera.player = player

func setup_playercontroller_networking():
	# if this is not in multiplayer, do not authority
	if Constants.id == 0:
		return
	synchronizer.set_multiplayer_authority(str(name).to_int(),false)
	set_multiplayer_authority(str(name).to_int(),false)
	playercamera.get_node("Camera/MultiplayerSynchronizer").set_multiplayer_authority(str(name).to_int(),false)

func get_player_camera_position():
	return playercameraposition

func set_active(active):
	is_active = active
	if active:
		playercamera = $PlayerCamera
		if playercamera.is_inside_tree():
			playercamera.activate()
		else:
			playercamera.ready.connect(playercamera.activate)




func report_network():
	print("Me: "+str(Constants.id))
	print("RPC called by: ", multiplayer.get_remote_sender_id())
	print("Authority: "+str(get_multiplayer_authority()))

@rpc
func Jump():
	if player != null:
		player.jump()

func JumpAction():
	if Constants.id == 0:
		Jump()
	else:
		#only to server
		rpc_id(1,"Jump") 


@rpc
func UseLeft():
	if player != null:
		player.UseLeft()


@rpc
func StopUseLeft():
	if player != null:
		player.StopUseLeft()	





func UseLeftAction():
	if Constants.id == 0:
		UseLeft()
	else:
		rpc_id(1,"UseLeft") 


func StopUseLeftAction():
	if Constants.id == 0:
		StopUseLeft()
	else:
		rpc_id(1,"StopUseLeft") 





@rpc
func UseRight():
	if player != null:
		player.UseRight()


@rpc
func StopUseRight():
	if player != null:
		player.StopUseRight()


func UseRightAction():
	if Constants.id == 0:
		#if right item is bow:
		#StartAimingCamera()
		UseRight()
	else:
		rpc_id(1,"UseRight") 


func StopUseRightAction():
	if Constants.id == 0:
		#StopAimingCamera()
		StopUseRight()
	else:
		rpc_id(1,"StopUseRight") 



@rpc
func Pickup():
	print(str(name))
	player.on_entity_pickup.emit()
	
func PickupAction():
	if Constants.id == 0:
		Pickup()
	else:
		rpc_id(1,"Pickup")

func check():
	if is_active:
		return true
	if Constants.id != 0:
		if not multiplayer.is_server():
			return (synchronizer.is_multiplayer_authority())
	return false


func get_player_camera():
	return playercamera


func _physics_process(delta):
	if check():
		input_direction = Vector3.ZERO
		input_direction.x = Input.get_action_strength("right") - Input.get_action_strength("left")
		input_direction.z = Input.get_action_strength("back") - Input.get_action_strength("forward")
		if get_player_camera() != null:
			input_direction = input_direction.rotated(Vector3.UP, get_player_camera().rotation.y).normalized()
		synchronizer.input_direction = input_direction
		if Input.is_action_just_pressed("LeftUse"):
			UseLeftAction()
		if Input.is_action_just_released("LeftUse"):
			StopUseLeftAction()
		if Input.is_action_just_pressed("RightUse"):
			UseRightAction()
		if Input.is_action_just_released("RightUse"):
			StopUseRightAction()
		if Input.is_action_just_pressed("jump"):
			JumpAction()
		if Input.is_action_just_pressed("Pickup"):
			PickupAction()
