extends Node3D

@onready var anim_tree = $AnimationTree

@onready var playerSkeleton: Skeleton3D = $root/Skeleton3D
@onready var runTrail = $RunTrail
var aimfsm: AnimationNodeStateMachinePlayback
var verticalfsm: AnimationNodeStateMachinePlayback
var lastpos
# Called when the node enters the scene tree for the first time.
func _ready():
	lastpos = global_transform.origin
	get_parent().on_entity_landed.connect(player_landed)
	get_parent().on_entity_melee_strike.connect(player_striking)
	get_parent().on_entity_aiming.connect(player_aiming)
	get_parent().on_entity_shoot.connect(player_shot)
	get_parent().on_entity_can_shoot.connect(can_shoot)
	get_parent().on_entity_jump.connect(player_jump)
	aimfsm = anim_tree["parameters/aimingstatemachine/playback"]
	verticalfsm = anim_tree["parameters/verticalstatemachine/playback"]

var speed = 0
var yspeed = 0
var lastyspeed = 0
var landblend = 0

func player_aiming(is_aiming):
	var v = 0
	if is_aiming:
		v = 1
		update_aim_state_machine("Aim")
	else:
		update_aim_state_machine("Stop")
	anim_tree["parameters/aim/blend_amount"] = v


func player_shot():
	update_aim_state_machine("Release")

# this will be used once there is a shooting cool down
func can_shoot(can_shootnow):
	if can_shootnow:
		update_aim_state_machine("Aim")


func player_striking(v):
	anim_tree["parameters/strike/request"] = AnimationNodeOneShot.ONE_SHOT_REQUEST_FIRE


func player_jump():
	update_vertical_state_machine("Jump")

var animate_local = true

func player_landed(blend):
	landblend = min(1,-blend/35)
	update_vertical_state_machine("Fall")
	anim_tree["parameters/landidle/blend_amount"] = landblend
	anim_tree["parameters/land/request"] = AnimationNodeOneShot.ONE_SHOT_REQUEST_FIRE



func left_hand():
	return playerSkeleton.find_bone("hand.l")

func right_hand():
	return playerSkeleton.find_bone("hand.r")

var aimtargetstate = ""
var verticaltargetstate = ""

func update_aim_state_machine(state):
	print("Want to go to "+str(state))
	if aimtargetstate != state:
		aimfsm.travel(state)
		aimtargetstate = state
		rpc("update_aim_state_machine_remote",state)


func update_vertical_state_machine(state):
	#print(str(verticalfsm.get_current_node())+"->"+str(state))
	if verticaltargetstate != state:
		verticalfsm.travel(state)
		#print("Result: "+str(verticalfsm.get_current_node()))
		verticaltargetstate = state
		rpc("update_vertical_state_machine_remote",state)


@rpc
func update_aim_state_machine_remote(state):
	aimfsm.travel(state)

@rpc
func update_vertical_state_machine_remote(state):
	verticalfsm.travel(state)


func _physics_process(delta):
	var last_speed_pos = lastpos
	var speed_pos = global_transform.origin
	yspeed = speed_pos.y - last_speed_pos.y
	last_speed_pos.y = 0
	speed_pos.y = 0
	speed = (speed_pos - last_speed_pos).length()
	lastpos = global_transform.origin
	
	anim_tree["parameters/speed/blend_amount"] = speed*8*get_parent().move_direction.length()
	# cound back down landblend
	landblend = max(0,landblend-0.4*delta)
	anim_tree["parameters/landidle/blend_amount"] = landblend
	# travel darf nur einmal aufgerufen werden, transitions werden quasi gebuffered
	if yspeed<0:
	# and (verticalfsm.get_current_node() in ["Stop","Jump"]):
		update_vertical_state_machine("Fall")
	lastyspeed = yspeed
	var v = 0
	if get_parent().is_on_floor():
		v = 0
	else:
		v = 1
	runTrail.emitting = get_parent().is_on_floor() and speed > 0.05
	
	anim_tree["parameters/vertical/blend_amount"] = v
