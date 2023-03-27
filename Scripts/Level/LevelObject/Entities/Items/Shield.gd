extends ItemEntity



@export var aimTime: float = 0.5
func _ready():
	super._ready()

func OnAttach(new_owner):
	super.OnAttach(new_owner)


func OnDettach():
	itemOwner.on_entity_aiming.emit(false)
	super.OnDettach()
	in_use = false


# eventuell bei boden kontakt oder so eigenen on_entity_melee_strike triggern



func Use():
	super.Use()
	itemOwner.on_entity_can_shoot.emit(false)
	itemOwner.on_entity_aiming.emit(true)


func StopUse():
	super.StopUse()
	in_use = false
#	itemOwner.global_rotation = start_rot
	#itemOwner.target_rot = Quaternion(Vector3.UP,PI/2)
