extends Enemy


var spear

@onready var vision: ShapeCast3D = $VisionCheck
@onready var target_point = $AI/Utils/AimTargetPosition

@export var aim_range = 4

var aiming = false

var follow_target


func _ready():
	super._ready()
	spear = load("res://Prefabs/LevelObjects/Entities/Items/Spear.tscn").instantiate()
	Constants.World.level.spawn_entity(spear)
	print("LOL "+str(item))
	Attach(spear)


func _physics_process(delta):
	super._physics_process(delta)


func Strike():
	print("Striking")
	if has_item(0):
		item[0].Use()

func Throw():
	print("Throwing")
	if has_item(0) and (item[0] is Spear):
		if aiming:
			aiming = false
			print("Has thrown")
			item[0].Use(1,target_point.global_transform.origin)


func Aim():
	if not aiming:
		aiming = true
		on_entity_throw_aiming.emit()
	if aiming:
		var target_dir = target_point.global_transform.origin - global_transform.origin
		target_dir = Vector2(target_dir.x,target_dir.z)
		look_into_direction(target_dir)
