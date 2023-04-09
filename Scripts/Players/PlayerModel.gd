extends EntityModel


@onready var playerSkeleton: Skeleton3D = $root/Skeleton3D
@onready var runTrail = $RunTrail
@onready var face: MeshInstance3D = $root/Skeleton3D/Face

var aimfsm: AnimationNodeStateMachinePlayback
var verticalfsm: AnimationNodeStateMachinePlayback
var shieldfsm: AnimationNodeStateMachinePlayback
var mouth: ShaderMaterial
var eyes: ShaderMaterial
var strikeTimer
# Called when the node enters the scene tree for the first time.
func _ready():
	super._ready()
	strikeTimer = Timer.new()
	add_child(strikeTimer)
	
	strikeTimer.timeout.connect(stop_strike)
	lastpos = global_transform.origin
	get_parent().on_entity_landed.connect(player_landed)
	get_parent().on_entity_melee_strike.connect(player_striking)
	get_parent().on_entity_aiming.connect(player_aiming)
	get_parent().on_entity_using_shield.connect(player_shield)
	get_parent().on_entity_shoot.connect(player_shot)
	get_parent().on_entity_can_shoot.connect(can_shoot)
	get_parent().on_entity_jump.connect(player_jump)
	aimfsm = anim_tree["parameters/aimingstatemachine/playback"]
	verticalfsm = anim_tree["parameters/verticalstatemachine/playback"]
	shieldfsm = anim_tree["parameters/shieldstatemachine/playback"]
	mouth = face.mesh.surface_get_material(0).duplicate(true)
	eyes = face.mesh.surface_get_material(1).duplicate(true)
	face.set_surface_override_material(0,mouth)
	face.set_surface_override_material(1,eyes)
	

func player_aiming(is_aiming):
	var v = 0
	if is_aiming:
		v = 1
		update_aim_state_machine("Aim")
	else:
		update_aim_state_machine("Stop")
	anim_tree["parameters/aim/add_amount"] = v



func player_shield(is_aiming):
	var v = 0
	if is_aiming:
		v = 1
		update_shield_state_machine("Raise")
	else:
		update_shield_state_machine("Lower")
	#anim_tree["parameters/ShieldBlock/add_amount"] = v


func entity_hit():
	super.entity_hit()
	print(str(Constants.id)+" HIT on "+str(self))
	mouth.set_shader_parameter("character",1)
	eyes.set_shader_parameter("character",1)

func player_shot():
	update_aim_state_machine("Release")

# this will be used once there is a shooting cool down
func can_shoot(can_shootnow):
	if can_shootnow:
		update_aim_state_machine("Aim")


func stop_strike():
	anim_tree["parameters/Strike/add_amount"] = 0
	

func player_striking(v):
	print(v)
	anim_tree["parameters/Strike/add_amount"] = 1
	anim_tree["parameters/StrikeStart/seek_request"] = 0
	strikeTimer.start(0.35)


func player_jump():
	update_vertical_state_machine("Jump")

var animate_local = true

func player_landed(blend):
	landblend = min(1,-blend/35)
	update_vertical_state_machine("Fall")
	mouth.set_shader_parameter("character",0)
	eyes.set_shader_parameter("character",0)
	anim_tree["parameters/landidle/blend_amount"] = landblend
	anim_tree["parameters/land/request"] = AnimationNodeOneShot.ONE_SHOT_REQUEST_FIRE



func left_hand():
	return playerSkeleton.find_bone("hand.l")

func right_hand():
	return playerSkeleton.find_bone("hand.r")

var aimtargetstate = ""
var verticaltargetstate = ""
var shieldtargetstate = ""

func update_aim_state_machine(state):
	if aimtargetstate != state:
		aimfsm.travel(state)
		aimtargetstate = state
		rpc("update_aim_state_machine_remote",state)



func update_shield_state_machine(state):
	if shieldtargetstate != state:
		shieldfsm.travel(state)
		shieldtargetstate = state
		rpc("update_shield_state_machine_remote",state)


func update_vertical_state_machine(state):
	#print(str(verticalfsm.get_current_node())+"->"+str(state))
	if verticaltargetstate != state:
		verticalfsm.travel(state)
		#print("Result: "+str(verticalfsm.get_current_node()))
		verticaltargetstate = state
		rpc("update_vertical_state_machine_remote",state)

@rpc
func update_shield_state_machine_remote(state):
	shieldfsm.travel(state)

@rpc
func update_aim_state_machine_remote(state):
	aimfsm.travel(state)

@rpc
func update_vertical_state_machine_remote(state):
	verticalfsm.travel(state)


func _physics_process(delta):
	super._physics_process(delta)
	anim_tree["parameters/speed/blend_amount"] = speed*8*get_parent().move_direction.length()
	anim_tree["parameters/strikespeed/blend_amount"] = speed*10*get_parent().move_direction.length()
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
