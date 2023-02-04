extends Node3D
class_name EntityModel

@onready var hitSound = $HitSound


@onready var anim_tree = $AnimationTree

var has_swim = false

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
	if get_parent().in_swim_area:
		swim_blend = 1
	if has_swim:
		anim_tree["parameters/swim/blend_amount"] = swim_blend
