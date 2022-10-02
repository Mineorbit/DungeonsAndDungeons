extends Entity


var id = 0
@onready var camera_offset = $CameraAnchor/CameraOffset
	
func UseLeft():
	if itemLeft != null:
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

func Dettach(item):
	super.Dettach(item)
	itemLeft = null
