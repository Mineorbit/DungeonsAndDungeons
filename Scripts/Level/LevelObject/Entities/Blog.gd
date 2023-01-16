extends Enemy

@onready var vision: ShapeCast3D = $VisionCheck
var time_since_seen = 0

func _physics_process(delta):
	super._physics_process(delta)
	time_since_seen += delta
	if vision.is_colliding():
		target_entity = vision.get_collider(0)
		time_since_seen = 0
	if time_since_seen > 5:
		target_entity = null


