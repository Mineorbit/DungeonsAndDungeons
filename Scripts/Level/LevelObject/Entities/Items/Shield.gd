extends ItemEntity
class_name Shield


@export var aimTime: float = 0.5
func _ready():
	super._ready()

func OnAttach(new_owner):
	super.OnAttach(new_owner)


func OnDettach():
	super.OnDettach()
	in_use = false


# eventuell bei boden kontakt oder so eigenen on_entity_melee_strike triggern

func damage_modifier(direction):
	var hit_direction = direction.normalized()
	if in_use and hit_direction.dot(itemOwner.global_transform.basis.z) > 0:
		return 0
	return 1 

func Use():
	super.Use()


func StopUse():
	super.StopUse()
	in_use = false
#	itemOwner.global_rotation = start_rot
	#itemOwner.target_rot = Quaternion(Vector3.UP,PI/2)
