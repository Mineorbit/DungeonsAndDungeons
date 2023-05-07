extends HumanoidModel


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
	anim_tree["parameters/Top/strikestart/seek_request"] = 128
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
	aimfsm = anim_tree["parameters/Top/aimingstatemachine/playback"]
	#verticalfsm = anim_tree["parameters/verticalstatemachine/playback"]
	shieldfsm = anim_tree["parameters/Top/shieldstatemachine/playback"]
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
		pass
		update_aim_state_machine("End")
	anim_tree["parameters/Top/aim/blend_amount"] = v


func update_aim_state_machine(state):
	if aimtargetstate != state:
		aimfsm.travel(state)
		aimtargetstate = state
		print(str(aimfsm)+" "+str(state)+" "+str(Constants.id))
		rpc("update_aim_state_machine_remote",state)

func player_shot():
	update_aim_state_machine("Release")

# this will be used once there is a shooting cool down
func can_shoot(can_shootnow):
	if can_shootnow:
		update_aim_state_machine("Aim")


@rpc
func update_aim_state_machine_remote(state):
	print(str(aimfsm)+" "+str(state)+" "+str(Constants.id))
	aimfsm.travel(state)


func player_shield(is_aiming):
	var v = 0
	#print("Updating "+str(is_aiming))
	if is_aiming:
		v = 1
		update_shield_state_machine("Raise")
	else:
		update_shield_state_machine("Lower")
	use_blend = v
	#anim_tree["parameters/ShieldBlock/add_amount"] = v


func entity_hit():
	super.entity_hit()
	print(str(Constants.id)+" HIT on "+str(self))
	mouth.set_shader_parameter("character",1)
	eyes.set_shader_parameter("character",1)


var use_blend = 0

var blend_target = 0

func stop_strike():
	blend_target = 0
	

func player_striking(v):
	blend_target = 1
	anim_tree["parameters/Top/strikestart/seek_request"] = 0
	strikeTimer.start(Constants.SwordStrikeTime)


func player_jump():
	update_vertical_state_machine("Jump")

var animate_local = true

func player_landed(blend):
	landblend = min(1,-blend/35)
	update_vertical_state_machine("Fall")
	mouth.set_shader_parameter("character",0)
	eyes.set_shader_parameter("character",0)
	#anim_tree["parameters/landidle/blend_amount"] = landblend
	#anim_tree["parameters/land/request"] = AnimationNodeOneShot.ONE_SHOT_REQUEST_FIRE



func left_hand():
	return playerSkeleton.find_bone("hand.l")

func right_hand():
	return playerSkeleton.find_bone("hand.r")

var aimtargetstate = ""
var verticaltargetstate = ""
var shieldtargetstate = ""




func update_shield_state_machine(state):
	if shieldtargetstate != state:
		shieldfsm.travel(state)
		shieldtargetstate = state
		rpc("update_shield_state_machine_remote",state)





func update_vertical_state_machine(state):
	#print(str(verticalfsm.get_current_node())+"->"+str(state))
	if verticaltargetstate != state:
		#verticalfsm.travel(state)
		#print("Result: "+str(verticalfsm.get_current_node()))
		verticaltargetstate = state
		rpc("update_vertical_state_machine_remote",state)

@rpc
func update_shield_state_machine_remote(state):
	shieldfsm.travel(state)


@rpc
func update_vertical_state_machine_remote(state):
	pass
	#verticalfsm.travel(state)


func _physics_process(delta):
	super._physics_process(delta)
	use_blend = 0.5*(use_blend + blend_target)
	anim_tree["parameters/Top/Use/blend_amount"] = (get_parent().items_in_use > 0)
	#anim_tree["parameters/strikespeed/blend_amount"] = speed*10*get_parent().move_direction.length()
	# cound back down landblend
	#anim_tree["parameters/landidle/blend_amount"] = landblend
	
	
	# travel darf nur einmal aufgerufen werden, transitions werden quasi gebuffered
	if yspeed<0:
		pass
	# and (verticalfsm.get_current_node() in ["Stop","Jump"]):
	#	update_vertical_state_machine("Fall")

	runTrail.emitting = get_parent().is_on_floor() and speed > 0.05
