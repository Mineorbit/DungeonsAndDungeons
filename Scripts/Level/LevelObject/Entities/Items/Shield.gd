extends ItemEntity



@export var aimTime: float = 0.5
func _ready():
	super._ready()

func OnAttach(new_owner):
	super.OnAttach(new_owner)


func OnDettach():
	super.OnDettach()
	in_use = false


# eventuell bei boden kontakt oder so eigenen on_entity_melee_strike triggern



func Use():
	super.Use()


func StopUse():
	super.StopUse()
	in_use = false
#	itemOwner.global_rotation = start_rot
	#itemOwner.target_rot = Quaternion(Vector3.UP,PI/2)
