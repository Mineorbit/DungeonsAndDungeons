extends MeshInstance3D

@onready var anim_tree = $AnimationTree

var lastpos
# Called when the node enters the scene tree for the first time.
func _ready():
	lastpos = global_transform.origin
	get_parent().on_entity_landed.connect(player_landed)
	get_parent().on_entity_melee_strike.connect(player_striking)
	get_parent().on_entity_aiming.connect(player_aiming)
	pass # Replace with function body.

var speed = 0
var yspeed = 0
var lastyspeed = 0
var landblend = 0
func player_aiming(is_aiming):
	var v = 0
	if is_aiming:
		v = 1
	anim_tree["parameters/aim/blend_amount"] = v
	

func player_striking(v):
	anim_tree["parameters/strike/active"] = true

func player_landed(blend):
	landblend = min(1,-blend/35)
	print("===")
	anim_tree["parameters/landidle/blend_amount"] = landblend
	anim_tree["parameters/land/active"] = true


func _physics_process(delta):
	var last_speed_pos = lastpos
	var speed_pos = global_transform.origin
	yspeed = speed_pos.y - last_speed_pos.y
	last_speed_pos.y = 0
	speed_pos.y = 0
	speed = (speed_pos - last_speed_pos).length()
	lastpos = global_transform.origin
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	anim_tree["parameters/speed/blend_amount"] = speed*8
	var fallblend = min(1,max(0,-8*yspeed))
	lastyspeed = yspeed
	anim_tree["parameters/fall/blend_amount"] = fallblend
	# cound back down landblend
	landblend = max(0,landblend-0.4*delta)
	anim_tree["parameters/landidle/blend_amount"] = landblend
	
