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
	Attach(spear)


func _physics_process(delta):
	super._physics_process(delta)
	if aiming:
		aim_at()

func start_aiming():
	aiming = true
	on_entity_throw_aiming.emit()

func throw():
	if has_item(0):
		aiming = false
		spear.Use(1,target_point.global_transform.origin)

func aim_at():
	var target_dir = target_point.global_transform.origin - global_transform.origin
	target_dir = Vector2(target_dir.x,target_dir.z)
	look_into_direction(target_dir)
