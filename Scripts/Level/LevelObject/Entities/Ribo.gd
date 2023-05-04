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
	if vision.is_colliding() and target_entity == null:
		target_entity = vision.get_collider(0)
		time_since_seen = 0
	if time_since_seen > 5:
		target_entity = null
	if target_entity != null and distance_to_target() > aim_range and has_item(0):
		follow_target = false
		if not try_throw:
			throw_timer = 0
		on_entity_throw_aiming.emit()
		try_throw = true
	else:
		follow_target = true
		try_throw = false
	if try_throw:
		aim_at(target,delta)
		throw_timer += delta
	if try_throw and throw_timer > 2:
		throw()
		try_throw = false
	if not has_item(0) and not spear.in_throw:
		target_entity = spear
		#print((spear.global_transform.origin - global_transform.origin).length())
		if (spear.global_transform.origin - global_transform.origin).length()  < 1.75:
			Attach(spear)
			target_entity = null


func throw():
	if has_item(0):
		spear.Use(1,target_entity.global_transform.origin)

func aim_at(target,delta):
	var target_dir = target.global_transform.origin - global_transform.origin
	target_dir = Vector2(target_dir.x,target_dir.z)
	look_into_direction(target_dir)
