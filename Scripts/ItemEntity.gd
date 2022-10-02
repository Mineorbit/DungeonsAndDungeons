extends Entity
class_name ItemEntity

var itemOwner

var isEquipped = false


func Use():
	pass


func OnAttach(new_owner):
	_velocity = Vector3.ZERO
	started = false
	isEquipped = true
	itemOwner = new_owner

func OnDettach():
	_velocity = itemOwner._velocity
	started = true
	isEquipped = true
	itemOwner = null
