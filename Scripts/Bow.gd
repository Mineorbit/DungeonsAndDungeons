extends ItemEntity

var arrowprefab


func _ready():
	super._ready()
	arrowprefab = load("res://Prefabs/LevelObjects/Entities/Arrow.tscn")

func OnAttach(new_owner):
	super.OnAttach(new_owner)


func OnDettach():
	super.OnDettach()


# eventuell bei boden kontakt oder so eigenen on_entity_melee_strike triggern



func Use():
	super.Use()
	itemOwner.on_entity_aiming.emit(true)


func StopUse():
	super.StopUse()
	in_use = false
#	itemOwner.global_rotation = start_rot
	itemOwner.rotate(Vector3.UP,PI/2)
	itemOwner.on_entity_aiming.emit(false)


func rot_player():
	if in_use:
		var new_rot = itemOwner.playercontroller.get_player_camera().rotation
		new_rot.x = 0
		itemOwner.rotation = new_rot

func _process(delta):
	super._process(delta)
	rot_player()


func _physics_process(delta):
	super._physics_process(delta)
	rot_player()

func Shoot():
	if not in_use:
		return
	var arrow: RigidBody3D = arrowprefab.instantiate()
	Constants.currentLevel.Entities.add_child(arrow)
	arrow.global_transform.origin = global_transform.origin + -0.5*transform.basis.z
	var strength = 25
	#arrow.gravity_scale = 0
	# this needs to be generalized for item carrying enemies
	var new_rot = itemOwner.playercontroller.get_player_camera().rotation
	arrow.rotation = new_rot
	arrow.apply_impulse(strength*(-1*itemOwner.playercontroller.get_player_camera().transform.basis.z+Vector3.UP*0.25))


