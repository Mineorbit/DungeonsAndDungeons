extends MeshInstance3D

@onready var anim_tree = $AnimationTree

var lastpos
# Called when the node enters the scene tree for the first time.
func _ready():
	lastpos = global_transform.origin
	pass # Replace with function body.

var speed = 0
var yspeed = 0

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
	anim_tree["parameters/fall/blend_amount"] = fallblend
	
