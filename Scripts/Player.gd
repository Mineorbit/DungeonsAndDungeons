extends Entity


var id = 0

func start():
	super.start()
	Signals.playerHealthChanged.emit(id,health)

func Hit(damage,hitting_entity):
	super.Hit(damage,hitting_entity)
	Signals.playerHealthChanged.emit(id,health)