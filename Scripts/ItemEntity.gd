extends Entity
class_name ItemEntity

var itemOwner

var isEquipped = false


func OnUse():
	pass

func OnAttach(new_owner):
	isEquipped = true
	itemOwner = new_owner

func OnDettach():
	isEquipped = true
	itemOwner = null
