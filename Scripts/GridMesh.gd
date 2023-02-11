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

func add(x,y,z, with_rebuild = false):
	var index = compute_index(x,y,z)
	if grid.size() > index:
		grid[index] = true
	if with_rebuild:
		build()

func remove(x,y,z, with_rebuild = false):
	var index = compute_index(x,y,z)
	if grid.size() > index:
		grid[index] = false
	if with_rebuild:
		build()

func get_at(x,y,z):
	var index = compute_index(x,y,z)
	if index >= grid.size() or index < 0:
		return false
	return grid[index]



func add_top_face(st,pos):
	st.index()
	var planemesh = PlaneMesh.new()
	planemesh.size = Vector2(1,1)
	var t = Transform3D.IDENTITY
	t = t.translated_local(pos+Vector3(0,0.5,0))
	st.append_from(planemesh,0,t)

func add_bottom_face(st,pos):
	st.index()
	var planemesh = PlaneMesh.new()
	planemesh.size = Vector2(1,1)
	var t = Transform3D.IDENTITY
	t = t.translated_local(pos+Vector3(0,-0.5,0))
	t = t.rotated_local(Vector3(1,0,0),PI)
	st.append_from(planemesh,0,t)




func add_front_face(st,pos):
	st.index()
	var planemesh = PlaneMesh.new()
	planemesh.size = Vector2(1,1)
	var t = Transform3D.IDENTITY
	t = t.translated_local(pos+Vector3(0.5,0,0))
	t = t.rotated_local(Vector3(0,0,1),-PI/2)
	st.append_from(planemesh,0,t)
	
	

func add_back_face(st,pos):
	st.index()
	var planemesh = PlaneMesh.new()
	planemesh.size = Vector2(1,1)
	var t = Transform3D.IDENTITY
	t = t.translated_local(pos+Vector3(-0.5,0,0))
	t = t.rotated_local(Vector3(0,0,1),PI/2)
	st.append_from(planemesh,0,t)



func add_left_face(st,pos):
	st.index()
	var planemesh = PlaneMesh.new()
	planemesh.size = Vector2(1,1)
	var t = Transform3D.IDENTITY
	t = t.translated_local(pos+Vector3(0,0,-0.5))
	t = t.rotated_local(Vector3(1,0,0),-PI/2)
	st.append_from(planemesh,0,t)


func add_right_face(st: SurfaceTool,pos):
	var normal = Vector3(0,0,1)
	st.index()
	var planemesh = PlaneMesh.new()
	planemesh.size = Vector2(1,1)
	var t = Transform3D.IDENTITY
	t = t.translated_local(pos+Vector3(0,0,0.5))
	t = t.rotated_local(Vector3(1,0,0),PI/2)
	st.append_from(planemesh,0,t)
	
func build():
	var st = SurfaceTool.new()
	st.begin(Mesh.PRIMITIVE_TRIANGLE_STRIP)
	for i in range(grid_size):
		for j in range(grid_size):
			for k in range(grid_size):
				if get_at(i,j,k):
					var basepos = Vector3(i,j,k)
					if not get_at(i,j+1,k):
						pass
						add_top_face(st,basepos)
					if not get_at(i,j-1,k):
						pass
						add_bottom_face(st,basepos)
					if not get_at(i+1,j,k):
						pass
						add_front_face(st,basepos)
					if not get_at(i-1,j,k):
						pass
						add_back_face(st,basepos)
					if not get_at(i,j,k-1):
						pass
						add_left_face(st,basepos)
					if not get_at(i,j,k+1):
						pass
						add_right_face(st,basepos)
	#st.generate_normals()
	var resultmesh = st.commit()
	var mat = load("res://Assets/Materials/Floor.tres")
	resultmesh.surface_set_material(0,mat)
	mesh = resultmesh
	mesh = BevelEdges.CreateBevelEdgeMesh(resultmesh)
