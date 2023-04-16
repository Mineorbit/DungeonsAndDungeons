extends Node3D
class_name EntityModel

@onready var hitSound = $HitSound

@onready var anim_tree = $AnimationTree

@export var attachmentPoints: Array = []


var has_swim = false


var lastpos = Vector3.ZERO


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

var current_swim_blend = 0

func _physics_process(delta):
	var swim_blend = -2
	if not (get_parent() is Entity):
		return
	if get_parent().in_swim_area:
		swim_blend = 1
	if has_swim:
		current_swim_blend = clampf(current_swim_blend + delta*swim_blend,0,1)
		anim_tree["parameters/swim/blend_amount"] = current_swim_blend
	var last_speed_pos = lastpos
	var speed_pos = global_transform.origin
	yspeed = speed_pos.y - last_speed_pos.y
	last_speed_pos.y = 0
	speed_pos.y = 0
	speed = (speed_pos - last_speed_pos).length()
	lastpos = global_transform.origin
	if "parameters/swim_speed/blend_amount" in anim_tree:
		anim_tree["parameters/swim_speed/blend_amount"] = speed*16*get_parent().move_direction.length()
	anim_tree["parameters/speed/blend_amount"] = speed*8*get_parent().move_direction.length()
	
