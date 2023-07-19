extends EntityModel
class_name HumanoidModel

@export var speed_blend_cap: float = 1

var p

func _ready():
	p = get_parent()
	has_swim = "parameters/Top/swim/blend_amount" in anim_tree
	if get_parent() is Entity:
		get_parent().on_entity_hit.connect(entity_hit)
		get_parent().on_entity_throw_aiming.connect(on_throw_aiming)
		get_parent().on_entity_throw.connect(on_throw)


func entity_hit():
	var random_num = rng.randf_range(0.75, 1.25)
	if hitSound != null:
		hitSound.pitch_scale = random_num
		hitSound.play()


@onready var walkSound = $WalkSound
var walking = false
func _physics_process(delta):
	super._physics_process(delta)
	if has_swim:
		anim_tree["parameters/Top/swim/blend_amount"] = current_swim_blend
		anim_tree["parameters/Bot/swim/blend_amount"] = current_swim_blend
	if "parameters/Top/swim_speed/blend_amount" in anim_tree:
		anim_tree["parameters/Top/swim_speed/blend_amount"] = speed*16*get_parent()._velocity.length()
		anim_tree["parameters/Bot/swim_speed/blend_amount"] = speed*16*get_parent()._velocity.length()
	if p is Entity:
		var blend_speed = clampf(animation_multiplier*speed*get_parent()._velocity.length(),0,speed_blend_cap)
		anim_tree["parameters/Top/speed/blend_amount"] = blend_speed
		anim_tree["parameters/Bot/speed/blend_amount"] = blend_speed
		if blend_speed > 0.1 and get_parent().last_floor:
			if(walking == false):
				walking = true
				walkSound.play()
		else:
			if(walking == true):
				walking = false
				walkSound.stop()
	if ("parameters/Bot/vertical/blend_amount" in anim_tree):
		anim_tree["parameters/Bot/vertical/blend_amount"] = current_v
		anim_tree["parameters/Top/vertical/blend_amount"] = current_v



func on_throw_aiming():
	update_aim_throw_state_machine("Aim")
	
func on_throw():
	update_aim_throw_state_machine("Throw")

func set_local_aim_throw(state):
	pass


func update_aim_throw_state_machine(state):
	if aimthrowtargetstate != state:
		set_local_aim_throw(state)
		aimthrowtargetstate = state
		rpc("update_aim_throw_state_machine_remote",state)


@rpc
func update_aim_throw_state_machine_remote(state):
	set_local_aim_throw(state)
