extends RigidBody3D

@onready var bombArea = $BombArea


func explode():
	for body in bombArea.get_overlapping_bodies():
		body.Hit(25,self)
	despawn()

func despawn():
	queue_free()

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
