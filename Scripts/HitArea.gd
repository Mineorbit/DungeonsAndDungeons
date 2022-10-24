extends ShapeCast3D


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


func Strike(damage,striking_object = null):
	if is_colliding():
		var collided_object = get_collider(0)
		print(collided_object)
		# check if collided has method hit/ is entity
		if collided_object.has_method("Hit"):
			collided_object.Hit(damage,striking_object)
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
