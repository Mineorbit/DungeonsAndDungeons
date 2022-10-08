extends Node

var player
var is_active = false
var spawned = false

@export var input_direction = Vector3.ZERO
@onready var synchronizer = $MultiplayerSynchronizer
# Called when the node enters the scene tree for the first time.
func _ready():
	synchronizer.set_multiplayer_authority(str(name).to_int())
	set_multiplayer_authority(str(name).to_int())


func spawn():
	spawned = true
	
func set_active(active):
	is_active = active

func despawn():
	spawned = false

@rpc
func Jump():
	print("Me: "+str(Constants.id))
	print("RPC called by: ", multiplayer.get_remote_sender_id())
	print("Authority: "+str(get_multiplayer_authority()))
	player.jump()

func JumpAction():
	if player != null:
		Jump()
	else:
		rpc("Jump") 
		
	


@rpc
func UseLeft():
	player.UseLeft()


func UseLeftAction():
	if player != null:
		UseLeft()
	else:
		rpc("UseLeft") 
	

func UseRight():
	pass
	
func Pickup():
	player.on_entity_pickup.emit()
	
	
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta):
	if is_active or (synchronizer.is_multiplayer_authority()):
		input_direction = Vector3.ZERO
		input_direction.x = Input.get_action_strength("right") - Input.get_action_strength("left")
		input_direction.z = Input.get_action_strength("back") - Input.get_action_strength("forward")
		input_direction = input_direction.rotated(Vector3.UP, Constants.playerCamera.rotation.y).normalized()
		synchronizer.input_direction = input_direction
	#print(str(self)+" "+str(input_direction))
		if Input.is_action_just_pressed("LeftUse"):
			UseLeftAction()
		if Input.is_action_just_pressed("jump"):
			JumpAction()
		if Input.is_action_just_pressed("Pickup"):
			Pickup()
