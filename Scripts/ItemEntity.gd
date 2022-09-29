extends Entity
class_name ItemEntity

var itemOwner

func OnAttach(new_owner):
	itemOwner = new_owner

func OnDettach():
	itemOwner = null
