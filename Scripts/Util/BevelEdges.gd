extends Node


func CreateBevelEdgeMesh(inputmesh):
	
	var workingmesh = ArrayMesh.new()
	if inputmesh is ArrayMesh:
		workingmesh = inputmesh
	else:
		workingmesh.add_surface_from_arrays(Mesh.PRIMITIVE_TRIANGLES, inputmesh.get_mesh_arrays())

	var mesh = ArrayMesh.new()
	var mdt = MeshDataTool.new()
	mdt.create_from_surface(workingmesh,0)
	
	# collect faces that need to be added
	for i in mdt.get_edge_count():
		pass
	
	var shift_dir = {}
	
	
	var facecenter = Vector3.ZERO
	for i in mdt.get_face_count():
		var k = i - i % 2
		
		for j in range(3):
			var id = mdt.get_face_vertex(i,j)
			var p = mdt.get_vertex(mdt.get_face_vertex(i,j))
			facecenter += p
			
		if i % 2 == 1:
			var center = facecenter/6
			for m in range(i-1,i+1):
				for n in range(3):
					var id = mdt.get_face_vertex(m,n)
					var p = mdt.get_vertex(mdt.get_face_vertex(m,n))
					if not id in shift_dir:
						print("Setting "+str(id))
						shift_dir[id] = 0.125*(center-p)
			facecenter = Vector3.ZERO
	
	for i in mdt.get_vertex_count():
		mdt.set_vertex(i,mdt.get_vertex(i)+shift_dir[i])
	
	
	mdt.commit_to_surface(mesh)
	return mesh
