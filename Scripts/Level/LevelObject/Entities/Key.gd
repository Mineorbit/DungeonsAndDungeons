extends ItemEntity
class_name Key


@onready var keyholeholder: Area3D = $KeyholeDetector

func Use():
	super.Use()
	if keyholeholder.has_overlapping_bodies():
		var locked_object = keyholeholder.get_overlapping_bodies()[0].get_parent()
		locked_object.open()
		itemOwner.Dettach(self)
		remove()
