extends ShapeCast3D

func _ready():
	get_parent().on_entity_pickup.connect(Pickup)

# Called when the node enters the scene tree for the first time.
func Pickup():
	print("Picking Up")
	if is_colliding():
		var collided_object = get_collider(0)
		if collided_object is ItemEntity:
			print("Can pickup: "+str(collided_object))
			get_parent().Attach(collided_object)
