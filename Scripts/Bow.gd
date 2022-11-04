extends ItemEntity

var arrowprefab
func _ready():
	super._ready()
	arrowprefab = load("res://Prefabs/LevelObjects/Entities/Arrow.tscn")

func OnAttach(new_owner):
	super.OnAttach(new_owner)

# eventuell bei boden kontakt oder so eigenen on_entity_melee_strike triggern
var in_use = false
func Use():
	in_use = true
	super.Use()

func _process(delta):
	if in_use:
		var new_rot = itemOwner.playercontroller.get_player_camera().rotation
		new_rot.x = 0
		itemOwner.rotation = new_rot

func StopUse():
	super.StopUse()
	in_use = false
	var arrow: RigidBody3D = arrowprefab.instantiate()
	Constants.currentLevel.Entities.add_child(arrow)
	arrow.global_transform.origin = global_transform.origin + -0.5*transform.basis.z
	var strength = 25
	#arrow.gravity_scale = 0
	# this needs to be generalized for item carrying enemies
	var new_rot = itemOwner.playercontroller.get_player_camera().rotation
	arrow.rotation = new_rot
	arrow.apply_impulse(strength*(-1*itemOwner.playercontroller.get_player_camera().transform.basis.z+Vector3.UP*0.25))

