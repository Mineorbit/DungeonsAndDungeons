extends ShapeCast3D

func _ready():
	get_parent().on_entity_pickup.connect(Pickup)



var collected = []
# Called when the node enters the scene tree for the first time.
func Pickup():
	print("Picking Up")
	var to_remove = []
	if collected.size() > 0:
		for object in collected:
			to_remove.append(object)
			#get_parent().Dettach(object)
			get_parent().rpc("Dettach",object)
	if is_colliding():
		var collided_object = get_collider(0)
		if collided_object is ItemEntity and not (collided_object in collected):
			collected.append(collided_object)
			#get_parent().Attach(collided_object)
			get_parent().rpc("Dettach",collided_object)
	for object in to_remove:
		collected.erase(object)
