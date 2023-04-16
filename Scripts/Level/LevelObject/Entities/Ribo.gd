extends Enemy


var spear

@onready var vision: ShapeCast3D = $VisionCheck
var time_since_seen = 0

@export var aim_range = 4

func _ready():
	super._ready()
	spear = load("res://Prefabs/LevelObjects/Entities/Items/Spear.tscn").instantiate()
	Constants.World.level.spawn_entity(spear)
	Attach(spear)


var try_throw = false

var throw_timer = 0

func _physics_process(delta):
	super._physics_process(delta)
	time_since_seen += delta
	if vision.is_colliding():
		target_entity = vision.get_collider(0)
		time_since_seen = 0
	if time_since_seen > 5:
		target_entity = null
	if target_entity != null and distance_to_target() > aim_range and has_item(0):
		follow_target = false
		if not try_throw:
			throw_timer = 0
		try_throw = true
	else:
		follow_target = true
		try_throw = false
	if try_throw:
		aim_at(target,delta)
		throw_timer += delta
		print(throw_timer)
	if try_throw and throw_timer > 2:
		throw()
		try_throw = false


func throw():
	if has_item(0):
		Dettach(spear)
		spear.global_transform.origin = global_transform.origin + global_transform.basis.x*0.5

func aim_at(target,delta):
	var target_dir = target.global_transform.origin - global_transform.origin
	target_dir = Vector2(target_dir.x,target_dir.z)
	look_into_direction(target_dir)
