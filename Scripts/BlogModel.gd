extends Node3D

var lastpos = Vector3.ZERO
var yspeed
var speed
@onready var anim_tree = $AnimationTree


func StrikeAnimation(damage):
	anim_tree["parameters/strike/active"] = true

func _physics_process(delta):
	var last_speed_pos = lastpos
	var speed_pos = global_transform.origin
	yspeed = speed_pos.y - last_speed_pos.y
	last_speed_pos.y = 0
	speed_pos.y = 0
	speed = (speed_pos - last_speed_pos).length()
	lastpos = global_transform.origin
	anim_tree["parameters/speed/blend_amount"] = speed*8

