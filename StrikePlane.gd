extends Node3D

@export var flash = 0:
	set(val):
		print(val)
		mat.set_shader_parameter("percentage",val)
@export var mat: ShaderMaterial
var strike_plane: MeshInstance3D

func _ready():
	strike_plane = get_node("StrikePlane")
	mat = strike_plane.get_active_material(0)

