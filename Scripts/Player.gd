extends Entity


var id = 0
var itemLeft
var itemRight

var playercontroller
@onready var camera_offset = $CameraAnchor/CameraOffset


func UseLeft():
	if itemLeft != null:
		itemLeft.Use()


func StopUseLeft():
	if itemLeft != null:
		itemLeft.StopUse()

func UseRight():
	var shot = false
	if itemLeft != null:
		#check if item in right is bow
		if itemLeft.has_method("Shoot"):
			shot = true
			itemLeft.Shoot()
	if itemRight != null and not shot:
		itemRight.Use()


func StopUseRight():
	if itemRight != null:
		itemRight.StopUse()


func _ready():
	super._ready()
	on_entity_despawn.connect(DettachAllItems)
	on_entity_remove.connect(DettachAllItems)
	on_entity_aiming.connect(ChangeMovementState)
	on_entity_landed.connect(LandedOnGround)

func LandedOnGround(v):
	print("Landed on ground")
	if input_blocked:
		_velocity.x = 0
		_velocity.z = 0

func ChangeMovementState(aiming):
	input_blocked = aiming

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

@onready var leftHandAttachment = $PlayerModel/PlayerModelAttachment/PlayerModel/root/Skeleton3D/lefthand
@onready var rightHandAttachment = $PlayerModel/PlayerModelAttachment/PlayerModel/root/Skeleton3D/righthand

@rpc(any_peer, call_local)
func Attach(item):
	super.Attach(item)
	if item.hand:
		itemRight = item
		item.itemAttachmentPoint = rightHandAttachment
	else:
		itemLeft = item
		item.itemAttachmentPoint = leftHandAttachment


@rpc(any_peer, call_local)
func Dettach(item):
	super.Dettach(item)
	if item.hand:
		itemRight = null
	else:
		itemLeft = null
