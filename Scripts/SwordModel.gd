extends Node3D

@export var flash: float = 0
var mat
@onready var strikeplane: MeshInstance3D = $StrikePlane/StrikePlane
func _ready():
	mat = strikeplane.get_active_material(0).duplicate()
	strikeplane.set_surface_override_material(0,mat)

func _process(delta):
	if mat != null:
		mat.set_shader_parameter("percentage",flash)
