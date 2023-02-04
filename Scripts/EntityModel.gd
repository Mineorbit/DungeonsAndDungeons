extends Node3D
class_name EntityModel

@onready var hitSound = $HitSound


@onready var anim_tree = $AnimationTree

var has_swim = false


var lastpos


var speed = 0
var yspeed = 0
var lastyspeed = 0
var landblend = 0

func _ready():
	has_swim = "parameters/swim/blend_amount" in anim_tree
	if get_parent() is Entity:
		get_parent().on_entity_hit.connect(entity_hit)

var rng = RandomNumberGenerator.new()
func entity_hit():
	var random_num = rng.randf_range(0.75, 1.25)
	if hitSound != null:
		hitSound.pitch_scale = random_num
		hitSound.play()


func _physics_process(delta):
	var swim_blend = 0
	print(get_parent())
	if not (get_parent() is Entity):
		return
	if get_parent().in_swim_area:
		swim_blend = 1
	if has_swim:
		anim_tree["parameters/swim/blend_amount"] = swim_blend
	var last_speed_pos = lastpos
	var speed_pos = global_transform.origin
	yspeed = speed_pos.y - last_speed_pos.y
	last_speed_pos.y = 0
	speed_pos.y = 0
	speed = (speed_pos - last_speed_pos).length()
	lastpos = global_transform.origin
	anim_tree["parameters/swim_speed/blend_amount"] = speed*16*get_parent().move_direction.length()
	anim_tree["parameters/speed/blend_amount"] = speed*8*get_parent().move_direction.length()
	
