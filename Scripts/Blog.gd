extends Enemy


@onready var vision: ShapeCast3D = $VisionCheck


func _physics_process(delta):
	super._physics_process(delta)
	if vision.is_colliding():
		target_entity = vision.get_collider(0)
