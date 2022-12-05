extends ItemEntity


var bombprefab


func _ready():
	super._ready()
	bombprefab = load("res://Prefabs/LevelObjects/Entities/Bomb.tscn")

func OnAttach(new_owner):
	super.OnAttach(new_owner)


func OnDettach():
	itemOwner.on_entity_aiming.emit(false)
	super.OnDettach()
	in_use = false


func Use():
	super.Use()
	Throw()







func Throw():
	if not in_use:
		return
	var bomb: RigidBody3D = bombprefab.instantiate()
	Constants.currentLevel.Entities.add_child(bomb)
	bomb.global_transform.origin = global_transform.origin + -0.5*transform.basis.z
	var strength = 15
	#arrow.gravity_scale = 0
	# this needs to be generalized for item carrying enemies
	var new_rot = itemOwner.playercontroller.get_player_camera().rotation
	bomb.rotation = new_rot
	bomb.apply_impulse(strength*(-1*itemOwner.playercontroller.get_player_camera().transform.basis.z+Vector3.UP*0.25))


