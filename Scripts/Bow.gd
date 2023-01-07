extends ItemEntity

var arrowprefab

@onready var timer = $ShootCooldownTimer

@export var aimTime: float = 0.5
func _ready():
	super._ready()
	arrowprefab = load("res://Prefabs/LevelObjects/Entities/Arrow.tscn")

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
	timer.start(aimTime)


func StopUse():
	super.StopUse()
	in_use = false
#	itemOwner.global_rotation = start_rot
	#itemOwner.target_rot = Quaternion(Vector3.UP,PI/2)
	itemOwner.on_entity_aiming.emit(false)


func rot_player():
	if in_use:
		var new_rot = itemOwner.playercontroller.get_player_camera().rotation
		new_rot.x = 0
		itemOwner.target_rot = Quaternion.from_euler(new_rot)


func _process(delta):
	super._process(delta)
	#rot_player()


func _physics_process(delta):
	super._physics_process(delta)
	rot_player()


func Shoot():
	if not in_use:
		return
	if not itemOwner.can_shoot:
		return
	
	itemOwner.on_entity_shoot.emit()
	var arrow: RigidBody3D = arrowprefab.instantiate()
	Constants.World.level.spawn_entity(arrow)
	arrow.global_transform.origin = global_transform.origin + -0.5*transform.basis.z
	var strength = 25
	#arrow.gravity_scale = 0
	# this needs to be generalized for item carrying enemies
	var new_rot = itemOwner.playercontroller.get_player_camera().rotation
	arrow.rotation = new_rot
	arrow.apply_impulse(strength*(-1*itemOwner.playercontroller.get_player_camera().transform.basis.z+Vector3.UP*0.25))
	
	
	itemOwner.on_entity_can_shoot.emit(false)
	timer.start(aimTime)




func on_shoot_timeout():
	if itemOwner != null:
		itemOwner.on_entity_can_shoot.emit(true)
