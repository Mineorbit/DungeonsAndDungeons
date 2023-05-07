extends EntityModel


func _ready():
	super._ready()

func StrikeAnimation(damage):
	anim_tree["parameters/strike/request"] = AnimationNodeOneShot.ONE_SHOT_REQUEST_FIRE

func _physics_process(delta):
	super._physics_process(delta)
	anim_tree["parameters/speed/blend_amount"] = speed/delta*0.5

@onready var strikeSound: AudioStreamPlayer3D = $StrikeSound


func animation_started(anim_name):
	if anim_name == "Strike":
		strikeSound.play()
