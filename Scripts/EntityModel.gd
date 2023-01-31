extends Node3D
class_name EntityModel

@onready var hitSound = $HitSound


func _ready():
	print("TEST "+str(self))
	if get_parent() is Entity:
		get_parent().on_entity_hit.connect(entity_hit)

var rng = RandomNumberGenerator.new()
func entity_hit():
	var random_num = rng.randf_range(0.75, 1.25)
	if hitSound != null:
		hitSound.pitch_scale = random_num
		hitSound.play()
