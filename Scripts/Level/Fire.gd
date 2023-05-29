extends Node3D


@onready var fire_prefab = load("res://Prefabs/Elements/Fire.tscn")




func ignite(area:Node3D):
	if not area.has_node("Fire"):
		var fire:Node3D = fire_prefab.instantiate()
		area.add_child(fire)
		fire.name = "Fire"
		fire.transform.origin = Vector3(0,0,0)


func on_flammable_entered(area):
	ignite(area)
