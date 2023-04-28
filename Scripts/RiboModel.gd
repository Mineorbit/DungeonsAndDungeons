extends EntityModel

@onready var top_aim_throw = anim_tree["parameters/Top/throwfsm/playback"]
@onready var bot_aim_throw = anim_tree["parameters/Bot/throwfsm/playback"]

func set_local_aim_throw(state):
	top_aim_throw.travel(state)
	bot_aim_throw.travel(state)
