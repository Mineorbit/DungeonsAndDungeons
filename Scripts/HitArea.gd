extends ShapeCast3D




func Strike(damage,striking_object = null):
	if is_colliding():
		var collided_object = get_collider(0)
		# check if collided has method hit/ is entity
		if collided_object.has_method("Hit"):
			collided_object.Hit(damage,striking_object)
