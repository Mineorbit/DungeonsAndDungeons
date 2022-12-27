extends RigidBody3D

@onready var bombArea = $BombArea
@onready var explosionParticles: GPUParticles3D = $ExplosionParticles

func explode():
	for body in bombArea.get_overlapping_bodies():
		body.Hit(25,self)
	#this is a hot fix
	var part = load("res://Prefabs/Particles/ExplosionParticles.tscn").instantiate()
	part.emitting = true
	part.global_transform.origin = global_transform.origin
	Constants.add_child(part)
	despawn()

func despawn():
	queue_free()

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass