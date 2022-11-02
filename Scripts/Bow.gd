extends ItemEntity

var arrowprefab
func _ready():
	super._ready()
	arrowprefab = load("res://Prefabs/LevelObjects/Entities/Arrow.tscn")

func OnAttach(new_owner):
	super.OnAttach(new_owner)

# eventuell bei boden kontakt oder so eigenen on_entity_melee_strike triggern
func Use():
	super.Use()
	var arrow: RigidBody3D = arrowprefab.instantiate()
	Constants.currentLevel.Entities.add_child(arrow)
	arrow.global_transform.origin = global_transform.origin + -0.5*transform.basis.z
	var strength = 25
	
	arrow.rotation = itemOwner.rotation - Vector3(-50,0,20)
	arrow.apply_impulse(-1*strength*global_transform.basis.z)
