extends Node

var player
var is_active = false

@export var input_direction = Vector3.ZERO
@onready var synchronizer = $MultiplayerSynchronizer
# Called when the node enters the scene tree for the first time.
func _ready():
	setup_playercontroller_networking()

func setup_playercontroller_networking():
	# if this is not in multiplayer, do not authority
	if Constants.id == 0:
		return
	synchronizer.set_multiplayer_authority(str(name).to_int())
	set_multiplayer_authority(str(name).to_int())
	
	
func set_active(active):
	is_active = active


func report_network():
	print("Me: "+str(Constants.id))
	print("RPC called by: ", multiplayer.get_remote_sender_id())
	print("Authority: "+str(get_multiplayer_authority()))

@rpc
func Jump():
	if player != null:
		player.jump()

func JumpAction():
	if player != null:
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
	if player != null:
		UseLeft()
	else:
		rpc_id(1,"UseLeft") 


func StopUseLeftAction():
	if player != null:
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
	if player != null:
		#if right item is bow:
		#StartAimingCamera()
		
		UseRight()
	else:
		rpc_id(1,"UseRight") 


func StopUseRightAction():
	if player != null:
		#StopAimingCamera()
		StopUseRight()
	else:
		rpc_id(1,"StopUseRight") 



@rpc
func Pickup():
	player.on_entity_pickup.emit()
	
func PickupAction():
	if player != null:
		Pickup()
	else:
		rpc_id(1,"Pickup") 	

func check():
	if Constants.id != 0:
		return (synchronizer.is_multiplayer_authority())
	return false


func get_player_camera():
	if Constants.id == 1:
		return MultiplayerConstants.player_cameras[str(name).to_int()]
	else:
		return PlayerCamera


func _physics_process(delta):
	if is_active or check():
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
