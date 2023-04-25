extends Node3D
class_name EntityModel

@onready var hitSound = $HitSound

@onready var anim_tree = $AnimationTree

@export var attachmentPoints: Array = []

@export var animation_multiplier = 1


var has_swim = false


var lastpos = Vector3.ZERO


var speed = 0
var yspeed = 0
var lastyspeed = 0
var landblend = 0

func _ready():
	has_swim = "parameters/Top/swim/blend_amount" in anim_tree
	if get_parent() is Entity:
		get_parent().on_entity_hit.connect(entity_hit)


var rng = RandomNumberGenerator.new()
func entity_hit():
	var random_num = rng.randf_range(0.75, 1.25)
	if hitSound != null:
		hitSound.pitch_scale = random_num
		hitSound.play()

var current_swim_blend = 0

var current_v = 0


func _physics_process(delta):
	var swim_blend = -2
	if not (get_parent() is Entity):
		return
	if get_parent().in_swim_area:
		swim_blend = 1
	if has_swim:
		current_swim_blend = clampf(current_swim_blend + delta*swim_blend,0,1)
		anim_tree["parameters/Top/swim/blend_amount"] = current_swim_blend
		anim_tree["parameters/Bot/swim/blend_amount"] = current_swim_blend
	var last_speed_pos = lastpos
	var speed_pos = global_transform.origin
	yspeed = speed_pos.y - last_speed_pos.y
	last_speed_pos.y = 0
	speed_pos.y = 0
	speed = (speed_pos - last_speed_pos).length()
	lastpos = global_transform.origin
	if "parameters/Top/swim_speed/blend_amount" in anim_tree:
		anim_tree["parameters/Top/swim_speed/blend_amount"] = speed*16*get_parent()._velocity.length()
		anim_tree["parameters/Bot/swim_speed/blend_amount"] = speed*16*get_parent()._velocity.length()
	
	anim_tree["parameters/Top/speed/blend_amount"] = animation_multiplier*speed*get_parent()._velocity.length()
	anim_tree["parameters/Bot/speed/blend_amount"] = animation_multiplier*speed*get_parent()._velocity.length()
	
	
	var v = 0
	if get_parent().is_on_floor():
		v = 0
	else:
		v = sign(yspeed)
	var d = (v - current_v)
	current_v = min(max(-1,current_v+8*delta*sign(d)),1)
	if ("parameters/Bot/vertical/blend_amount" in anim_tree):
		anim_tree["parameters/Bot/vertical/blend_amount"] = current_v
		anim_tree["parameters/Top/vertical/blend_amount"] = current_v
