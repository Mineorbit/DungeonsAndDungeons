extends ShapeCast3D

func _ready():
	get_parent().on_entity_pickup.connect(Pickup)



var collected = []

func Pickup():
	print("Picking Up")
	var items_inside_range = get_items_in_area()
	#change left hand
	var left_avail = []
	var right_avail = []
	for item in items_inside_range:
		if item.attachment == 1:
			right_avail.append(item)
		else:
			left_avail.append(item)
	var any_items = items_inside_range.size() > 0
	change_left_hand(left_avail,any_items)
	change_right_hand(right_avail,any_items)

func change_left_hand(avail,any_items):
	if(get_parent().has_item(0) and (avail.size()>0 or not any_items)):
		get_parent().Dettach(get_parent().item[0])
	if avail.size() > 0:
		get_parent().Attach(avail[0])



func change_right_hand(avail,any_items):
	if(get_parent().has_item(1) and (avail.size()>0 or not any_items)):
		get_parent().Dettach(get_parent().item[1])
	if avail.size() > 0:
		get_parent().Attach(avail[0])


func get_items_in_area():
	var items_inside = []
	if is_colliding():
		for i in range(get_collision_count()):
			var collided_object = get_collider(i)
			if collided_object is ItemEntity and not (collided_object in items_inside):
				items_inside.append(collided_object)
	return items_inside
