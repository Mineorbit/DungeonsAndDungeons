extends ShapeCast3D

func _ready():
	get_parent().on_entity_pickup.connect(Pickup)



var collected = []

func Pickup():
	var items_inside_range = get_items_in_area()
	#change left hand
	var avail = []
	print(get_parent())
	for i in range(get_parent().item_slots):
		avail.append([])
	for item in items_inside_range:
		avail[item.attachment].append(item)
	var any_items = items_inside_range.size() > 0
	for i in range(get_parent().item_slots):
		change_slot(avail[i],any_items,i)

func change_slot(avail,any_items,slot):
	if(get_parent().has_item(slot) and (avail.size()>0 or not any_items)):
		get_parent().Dettach(get_parent().item[slot])
	if avail.size() > 0:
		get_parent().Attach(avail[0])



func get_items_in_area():
	var items_inside = []
	if is_colliding():
		for i in range(get_collision_count()):
			var collided_object = get_collider(i)
			if collided_object is ItemEntity and not (collided_object in items_inside) and not(collided_object.isEquipped):
				items_inside.append(collided_object)
	return items_inside
