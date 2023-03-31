extends MeshInstance3D

func _ready():
	var vertices = PackedVector3Array()
	vertices.append(Vector3(1,0,0))
	vertices.append(Vector3(0,1,0))
	vertices.append(Vector3(0,0,1))
	var arrays = []
	arrays.resize(Mesh.ARRAY_MAX)
	arrays[Mesh.ARRAY_VERTEX] = vertices
	#arrays[Mesh.ARRAY_NORMAL] = vertices
	var m = ArrayMesh.new()
	m.add_surface_from_arrays(Mesh.PRIMITIVE_TRIANGLES,arrays)
	mesh = m

func _process(delta):
	rotate(Vector3.UP,delta)
