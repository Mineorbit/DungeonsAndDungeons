extends MeshInstance3D


@export var face_granularity = 1

@export var grid_size = 10

var grid = []

func _ready():
	for i in range(grid_size):
		for j in range(grid_size):
			for k in range(grid_size):
				grid.append(false)

func compute_index(x,y,z):
	return 100*y+10*x+z

func add(x,y,z):
	var index = compute_index(x,y,z)
	if grid.size() > index:
		grid[index] = true
	build_mesh()

func remove(x,y,z):
	var index = compute_index(x,y,z)
	if grid.size() > index:
		grid[index] = false

func get_at(x,y,z):
	var index = compute_index(x,y,z)
	if index >= grid.size() or index < 0:
		return false
	return grid[index]

func add_top_face(st,pos):
	var normal = Vector3(0,1,0)
	st.set_normal(normal)
	st.add_vertex(pos+Vector3(0.5,0.5,-0.5))
	st.set_normal(normal)
	st.add_vertex(pos+Vector3(-0.5,0.5,0.5))
	st.set_normal(normal)
	st.add_vertex(pos+Vector3(-0.5,0.5,-0.5))
	
	st.set_normal(normal)
	st.add_vertex(pos+Vector3(-0.5,0.5,0.5))
	st.set_normal(normal)
	st.add_vertex(pos+Vector3(0.5,0.5,-0.5))
	st.set_normal(normal)
	st.add_vertex(pos+Vector3(0.5,0.5,0.5))

func add_bottom_face(st,pos):
	var normal = Vector3(0,-1,0)
	st.set_normal(normal)
	st.add_vertex(pos+Vector3(-0.5,-0.5,-0.5))
	st.set_normal(normal)
	st.add_vertex(pos+Vector3(-0.5,-0.5,0.5))
	st.set_normal(normal)
	st.add_vertex(pos+Vector3(0.5,-0.5,-0.5))
	
	st.set_normal(normal)
	st.add_vertex(pos+Vector3(0.5,-0.5,0.5))
	st.set_normal(normal)
	st.add_vertex(pos+Vector3(0.5,-0.5,-0.5))
	st.set_normal(normal)
	st.add_vertex(pos+Vector3(-0.5,-0.5,0.5))



func add_front_face(st,pos):
	var normal = Vector3(1,0,0)
	st.set_normal(normal)
	st.add_vertex(pos+Vector3(0.5,-0.5,-0.5))
	st.set_normal(normal)
	st.add_vertex(pos+Vector3(0.5,-0.5,0.5))
	st.set_normal(normal)
	st.add_vertex(pos+Vector3(0.5,0.5,-0.5))
	
	st.set_normal(normal)
	st.add_vertex(pos+Vector3(0.5,0.5,0.5))
	st.set_normal(normal)
	st.add_vertex(pos+Vector3(0.5,0.5,-0.5))
	st.set_normal(normal)
	st.add_vertex(pos+Vector3(0.5,-0.5,0.5))
	
	

func add_back_face(st,pos):
	var normal = Vector3(-1,0,0)
	st.set_normal(normal)
	st.add_vertex(pos+Vector3(-0.5,0.5,-0.5))
	st.set_normal(normal)
	st.add_vertex(pos+Vector3(-0.5,-0.5,0.5))
	st.set_normal(normal)
	st.add_vertex(pos+Vector3(-0.5,-0.5,-0.5))
	
	st.set_normal(normal)
	st.add_vertex(pos+Vector3(-0.5,-0.5,0.5))
	st.set_normal(normal)
	st.add_vertex(pos+Vector3(-0.5,0.5,-0.5))
	st.set_normal(normal)
	st.add_vertex(pos+Vector3(-0.5,0.5,0.5))

func build_mesh():
	print(grid)
	var st = SurfaceTool.new()
	st.begin(Mesh.PRIMITIVE_TRIANGLES)
	for i in range(grid_size):
		for j in range(grid_size):
			for k in range(grid_size):
				if get_at(i,j,k):
					var basepos = Vector3(i,j,k)
					if not get_at(i,j+1,k):
						add_top_face(st,basepos)
					if not get_at(i,j-1,k):
						add_bottom_face(st,basepos)
					if not get_at(i+1,j,k):
						add_front_face(st,basepos)
					if not get_at(i-1,j,k):
						add_back_face(st,basepos)
	
	#st.generate_normals(true)
	var resultmesh = st.commit()
	var mat = load("res://Assets/Materials/Floor.tres")
	resultmesh.surface_set_material(0,mat)
	mesh = resultmesh
	mesh = BevelEdges.CreateBevelEdgeMesh(resultmesh)
