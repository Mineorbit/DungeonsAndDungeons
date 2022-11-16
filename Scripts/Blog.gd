extends Enemy


@onready var vision: ShapeCast3D = $VisionCheck
@onready var animation_player = $BlogModel/AnimationPlayer
@onready var model_animation_tree: AnimationTree = $AnimationTree
func _ready():
	super._ready()
	model_animation_tree.anim_player = "./BlogModel/AnimationPlayer"
	model_animation_tree.active = true


func _physics_process(delta):
	super._physics_process(delta)
	if vision.is_colliding():
		target_entity = vision.get_collider(0)
