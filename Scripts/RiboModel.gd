extends HumanoidModel

var top_aim_throw
var bot_aim_throw

func _ready():
	super._ready()
	top_aim_throw = anim_tree["parameters/Top/throwfsm/playback"]
	bot_aim_throw = anim_tree["parameters/Bot/throwfsm/playback"]


var target_throw_blend = 0
var current_throw_blend = 0



#there should be a cleaner approach (change blend amount afer animation is finished)
var k = 0.9
func set_local_aim_throw(state):
	if(state == "Aim"):
		target_throw_blend= 1
		k = 0.9
	else:
		target_throw_blend = 0
		k = 0.99
	top_aim_throw.travel(state)
	bot_aim_throw.travel(state)



func _physics_process(delta):
	super._physics_process(delta)
	current_throw_blend = k*current_throw_blend+(1-k)*target_throw_blend
	anim_tree["parameters/Top/Throw/blend_amount"] = current_throw_blend
	anim_tree["parameters/Bot/Throw/blend_amount"] = current_throw_blend
