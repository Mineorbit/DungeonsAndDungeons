extends Node3D

var effect = {}

func _ready():
	var hiteffect = load("res://Prefabs/Effects/HitEffect.tscn").instantiate()
	add_child(hiteffect)
	effect["Hit"] = hiteffect

func spawn(t,pos):
	var e = effect[t]
	print(pos)
	e.global_transform.origin = pos
	e.trigger()
