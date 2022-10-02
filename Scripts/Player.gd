extends Entity


var id = 0

func UseLeft():
	itemLeft.Use()
	
func UseRight():
	pass


func start():
	super.start()
	Signals.playerHealthChanged.emit(id,health)

func Hit(damage,hitting_entity):
	super.Hit(damage,hitting_entity)
	Signals.playerHealthChanged.emit(id,health)

var itemLeft
var itemRight

func Attach(item):
	super.Attach(item)
	itemLeft = item
