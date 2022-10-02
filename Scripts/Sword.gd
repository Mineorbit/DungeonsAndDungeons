extends ItemEntity



# eventuell bei boden kontakt oder so eigenen on_entity_melee_strike triggern

func OnUse():
	super.OnUse()
	itemOwner.on_entity_melee_strike.emit(15)
