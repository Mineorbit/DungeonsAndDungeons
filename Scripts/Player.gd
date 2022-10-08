extends Entity


var id = 0
var itemLeft
var itemRight

var playercontroller
@onready var camera_offset = $CameraAnchor/CameraOffset
	
func UseLeft():
	if itemLeft != null:
		itemLeft.Use()
	
func UseRight():
	pass

func _ready():
	super._ready()
	on_entity_despawn.connect(DettachAllItems)
	on_entity_remove.connect(DettachAllItems)

func DettachAllItems():
	print("Dettaching all Items")
	if itemLeft != null:
		Dettach(itemLeft)
	if itemRight != null:
		Dettach(itemRight)

func start():
	super.start()
	Signals.playerHealthChanged.emit(id,health)

func Hit(damage,hitting_entity):
	super.Hit(damage,hitting_entity)
	Signals.playerHealthChanged.emit(id,health)


func _physics_process(delta):
	if playercontroller != null:
		move_direction.x = playercontroller.input_direction.x
		move_direction.z = playercontroller.input_direction.z
	super._physics_process(delta)

func Attach(item):
	super.Attach(item)
	itemLeft = item

func Dettach(item):
	super.Dettach(item)
	itemLeft = null
