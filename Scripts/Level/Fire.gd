extends Node3D


@export var timer: Timer
@onready var fire_prefab = load("res://Prefabs/Elements/Fire.tscn")


var burningObject

func _ready():
	burningObject = get_parent().get_parent()
	if burningObject is Entity:
		if not(burningObject.material == LevelObjectData.LevelObjectMaterial.Wood):
			timer.start()
	else:
		if burningObject.get_parent() is LevelObject and burningObject.get_parent().levelObjectData != null  and not(burningObject.get_parent().levelObjectData.material == LevelObjectData.LevelObjectMaterial.Wood):
			#print(burningObject.get_parent().material)
			timer.start()

func ignite(area:Node3D):
	if not area.has_node("Fire"):
		var fire:Node3D = fire_prefab.instantiate()
		area.add_child(fire)
		fire.name = "Fire"
		fire.transform.origin = Vector3(0,0,0)


func on_flammable_entered(area):
	ignite(area)

var count = 45
var start_count = 45

func _physics_process(delta):
	tick_damage()


func tick_damage():
	if count > 0:
		count = count - 1
	else:
		count = start_count
		if burningObject != null and burningObject is Entity:
			burningObject.Hit(5,self,null,false)


func extinguish():
	queue_free()


func on_water_entered(body):
	print("Entered Water "+str(body))
	extinguish()
