extends Entity


var id = 0
var itemLeft
var itemRight

var playercontroller
@onready var camera_offset = $CameraAnchor/CameraOffset


func UseLeft():
	var shot = false
	if itemRight != null:
		#check if item in right is bow
		if itemRight.has_method("Shoot"):
			shot = true
			itemRight.Shoot()
	if itemLeft != null and not shot:
		itemLeft.Use()


func StopUseLeft():
	if itemLeft != null:
		itemLeft.StopUse()

func UseRight():
	if itemRight != null:
		itemRight.Use()


func StopUseRight():
	if itemRight != null:
		itemRight.StopUse()


func _ready():
	super._ready()
	on_entity_despawn.connect(DettachAllItems)
	on_entity_remove.connect(DettachAllItems)
	on_entity_aiming.connect(ChangeMovementState)

func ChangeMovementState(aiming):
	allowed_to_move = not aiming

func DettachAllItems():
	print("Dettaching all Items")
	if itemLeft != null:
		Dettach(itemLeft)
	if itemRight != null:
		Dettach(itemRight)

func start():
	super.start()
	Signals.playerHealthChanged.emit(id,health)

func Hit(damage,hitting_entity,direction = null):
	super.Hit(damage,hitting_entity,direction)
	Signals.playerHealthChanged.emit(id,health)


func _physics_process(delta):
	if playercontroller != null:
		move_direction.x = playercontroller.input_direction.x
		move_direction.z = playercontroller.input_direction.z
	super._physics_process(delta)


@rpc(any_peer, call_local)
func Attach(item):
	super.Attach(item)
	if item.hand:
		itemRight = item
	else:
		itemLeft = item


@rpc(any_peer, call_local)
func Dettach(item):
	super.Dettach(item)
	if item.hand:
		itemRight = null
	else:
		itemLeft = null
