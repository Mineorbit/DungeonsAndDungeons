extends MeshInstance3D


@export var face_granularity = 1

@export var grid_size = 8

var grid: PackedByteArray = []

func _ready():
	for i in range(grid_size):
		for j in range(grid_size):
			for k in range(grid_size):
				grid.append(true)

func compute_index(x,y,z):
	return 64*x+8*y+z

func add(x,y,z):
	var index = compute_index(x,y,z)
	print(index)
	if grid.size() > index:
		grid[index] = true
	build_mesh()

func remove(x,y,z):
	var index = compute_index(x,y,z)
	if grid.size() > index:
		grid[index] = false

func get_at(x,y,z):
	var index = compute_index(x,y,z)
	return grid[index]

func add_top_face(st,pos):
	st.set_normal(Vector3(0,-1,0))
	st.add_vertex(pos+Vector3(-0.5,0.5,-0.5))
	st.set_normal(Vector3(0,-1,0))
	st.add_vertex(pos+Vector3(-0.5,0.5,0.5))
	st.set_normal(Vector3(0,-1,0))
	st.add_vertex(pos+Vector3(0.5,0.5,-0.5))
	
	st.set_normal(Vector3(0,-1,0))
	st.add_vertex(pos+Vector3(-0.5,0.5,0.5))
	st.set_normal(Vector3(0,-1,0))
	st.add_vertex(pos+Vector3(0.5,0.5,-0.5))
	st.set_normal(Vector3(0,-1,0))
	st.add_vertex(pos+Vector3(0.5,0.5,0.5))

func build_mesh():
	var st = SurfaceTool.new()
	st.begin(Mesh.PRIMITIVE_TRIANGLES)
	for i in range(grid_size):
		for j in range(grid_size):
			for k in range(grid_size):
				if get_at(i,j,k):
					print("LOL")
					var basepos = Vector3(i,j,k)
					add_top_face(st,basepos)
	var resultmesh = st.commit()
	var mat = load("res://Assets/Materials/Floor.tres")
	resultmesh.surface_set_material(0,mat)
	mesh = resultmesh
	#mesh = BevelEdges.CreateBevelEdgeMesh(resultmesh)
