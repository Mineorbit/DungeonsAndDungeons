extends ItemEntity


func OnAttach(new_owner):
	super.OnAttach(new_owner)

# eventuell bei boden kontakt oder so eigenen on_entity_melee_strike triggern
func Use():
	super.Use()
	itemOwner.on_entity_melee_strike.emit(15)
	
