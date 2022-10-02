extends Entity
class_name ItemEntity

var itemOwner

var isEquipped = false


func Use():
	pass


func OnAttach(new_owner):
	started = false
	isEquipped = true
	itemOwner = new_owner

func OnDettach():
	started = true
	isEquipped = true
	itemOwner = null
