extends ItemEntity
class_name Shield


@export var aimTime: float = 0.5
func _ready():
	super._ready()

func OnAttach(new_owner):
	super.OnAttach(new_owner)


func get_type():
	return "Shield"


func OnDettach():
	itemOwner.on_entity_using_shield.emit(false)
	super.OnDettach()


# eventuell bei boden kontakt oder so eigenen on_entity_melee_strike triggern

func damage_modifier(direction):
	var hit_direction = -direction.normalized()
	var forward_direction = itemOwner.global_transform.basis.x
	var relation = hit_direction.dot(forward_direction)
	if in_use and relation > 0:
		return 0
	return 1 

func Use():
	super.Use()
	in_use = true
	itemOwner.on_entity_using_shield.emit(true)


func StopUse():
	super.StopUse()
	in_use = false
	itemOwner.on_entity_using_shield.emit(false)
#	itemOwner.global_rotation = start_rot
	#itemOwner.target_rot = Quaternion(Vector3.UP,PI/2)
