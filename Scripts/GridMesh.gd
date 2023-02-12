extends MeshInstance3D


@export var face_granularity = 1

@export var grid_size = 6

var grid = []
var st

var resultmesh: ArrayMesh
func _ready():
	resultmesh = ArrayMesh.new()
	st = SurfaceTool.new()
	planemesh = PlaneMesh.new()
	planemesh.size = Vector2(1,1)
	for i in range(grid_size):
		for j in range(grid_size):
			for k in range(grid_size):
				grid.append(false)

func compute_index(x,y,z):
	return grid_size*grid_size*y+grid_size*x+z

var last_changed = Vector3i(0,0,0)

func add(x,y,z, with_rebuild = false):
	var index = compute_index(x,y,z)
	if grid.size() > index:
		grid[index] = true
	last_changed = Vector3i(x,y,z)
	build()

func remove(x,y,z, with_rebuild = false):
	var index = compute_index(x,y,z)
	if grid.size() > index:
		grid[index] = false
	last_changed = Vector3i(x,y,z)
	build()

func get_at(x,y,z):
	var index = compute_index(x,y,z)
	if x < 0 or x >= grid_size or y < 0 or y >= grid_size or z < 0 or z >= grid_size:
		return false
	return grid[index]



func add_top_face(st,pos,surface):
	var t = Transform3D.IDENTITY
	t = t.translated_local(pos+Vector3(0,0.5,0))
	st.append_from(planemesh,0,t)

func add_bottom_face(st,pos,surface):
	var t = Transform3D.IDENTITY
	t = t.translated_local(pos+Vector3(0,-0.5,0))
	t = t.rotated_local(Vector3(1,0,0),PI)
	st.append_from(planemesh,0,t)




func add_front_face(st,pos,surface):
	var t = Transform3D.IDENTITY
	t = t.translated_local(pos+Vector3(0.5,0,0))
	t = t.rotated_local(Vector3(0,0,1),-PI/2)
	st.append_from(planemesh,0,t)
	
	

func add_back_face(st,pos,surface):
	var t = Transform3D.IDENTITY
	t = t.translated_local(pos+Vector3(-0.5,0,0))
	t = t.rotated_local(Vector3(0,0,1),PI/2)
	st.append_from(planemesh,0,t)



func add_left_face(st,pos,surface):
	var t = Transform3D.IDENTITY
	t = t.translated_local(pos+Vector3(0,0,-0.5))
	t = t.rotated_local(Vector3(1,0,0),-PI/2)
	st.append_from(planemesh,0,t)


func add_right_face(st: SurfaceTool,pos,surface):
	var t = Transform3D.IDENTITY
	t = t.translated_local(pos+Vector3(0,0,0.5))
	t = t.rotated_local(Vector3(1,0,0),PI/2)
	st.append_from(planemesh,0,t)

var planemesh

var surface_dict = {}

func build(complete = true):
	if complete:
		#start fresh
		resultmesh = ArrayMesh.new()
		for i in range(grid_size):
			for j in range(grid_size):
				for k in range(grid_size):
					if get_at(i,j,k):
						st.clear()
						st.begin(Mesh.PRIMITIVE_TRIANGLE_STRIP)
						var basepos = Vector3(i,j,k)
						var surface = 0
						if not get_at(i,j+1,k):
							pass
							add_top_face(st,basepos,surface)
						if not get_at(i,j-1,k):
							pass
							add_bottom_face(st,basepos,surface)
						if not get_at(i+1,j,k):
							pass
							add_front_face(st,basepos,surface)
						if not get_at(i-1,j,k):
							pass
							add_back_face(st,basepos,surface)
						if not get_at(i,j,k-1):
							pass
							add_left_face(st,basepos,surface)
						if not get_at(i,j,k+1):
							pass
							add_right_face(st,basepos,surface)
						var mesh = st.commit()
						var index = compute_index(i,j,k)
						surface_dict[index] = resultmesh.get_surface_count()
						resultmesh.add_surface_from_arrays(Mesh.PRIMITIVE_TRIANGLES,mesh.surface_get_arrays(0))
	else:
		# remove faces in neighborhood of last changed
		for i in range(-1,2):
			for j in range(-1,2):
				for k in range(-1,2):
					var basepos = last_changed + Vector3i(i,j,k)
					var index = compute_index(basepos.x,basepos.y,basepos.z)
					if index in surface_dict:
					#currently we cannot remove surfaces, this will be added in later godot versions again
					#	resultmesh.surface_remove(surface_dict[index])
						surface_dict.erase(index)
					if get_at(basepos.x,basepos.y,basepos.z):
						st.clear()
						st.begin(Mesh.PRIMITIVE_TRIANGLE_STRIP)
						var surface = 0
						basepos = Vector3(basepos)
						if not get_at(basepos.x,basepos.y+1,basepos.z):
							pass
							add_top_face(st,basepos,surface)
						if not get_at(basepos.x,basepos.y-1,basepos.z):
							pass
							add_bottom_face(st,basepos,surface)
						if not get_at(basepos.x+1,basepos.y,basepos.z):
							pass
							add_front_face(st,basepos,surface)
						if not get_at(basepos.x-1,basepos.y,basepos.z):
							pass
							add_back_face(st,basepos,surface)
						if not get_at(basepos.x,basepos.y,basepos.z-1):
							pass
							add_left_face(st,basepos,surface)
						if not get_at(basepos.x,basepos.y,basepos.z+1):
							pass
							add_right_face(st,basepos,surface)
						var mesh = st.commit()
						print(mesh)
						surface_dict[index] = resultmesh.get_surface_count()
						resultmesh.add_surface_from_arrays(Mesh.PRIMITIVE_TRIANGLES,mesh.surface_get_arrays(0))

	#resultmesh = BevelEdges.CreateBevelEdgeMesh(resultmesh)
	mesh = resultmesh
