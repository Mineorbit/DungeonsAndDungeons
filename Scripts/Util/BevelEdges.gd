extends Node

func close(a,b):
	return (a-b).length() < 0.1

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
						shift_dir[id] = 0.125*(center-p)
			facecenter = Vector3.ZERO
	
	var n = mdt.get_edge_count()
	
	var corner_verts = []
	var edge_verts = []
	for i in n:
		# two edges that share corners need to be connected
		var edgea = [mdt.get_edge_vertex(i,0),mdt.get_edge_vertex(i,1)]
		var a0 = mdt.get_vertex(edgea[0]) 
		var a1 = mdt.get_vertex(edgea[1]) 
		var dira = a0 - a1
		
		for j in range(i+1,n):
			var edgeb = [mdt.get_edge_vertex(j,0),mdt.get_edge_vertex(j,1)]
			var b0 = mdt.get_vertex(edgeb[0])
			var b1 = mdt.get_vertex(edgeb[1])
			var dirb = b0 - b1
			var d = dira.dot(dirb)
			var collection = [edgea[0],edgea[1],edgeb[0],edgeb[1]]
			if d < 0.125 or d > -0.125:
				#  we need to check that the positions of edge vertices are "close"
				if (close(a0,b0) or close(a0,b1)) and (close(a1,b0) or close(a1,b1)):
					# since these two edges share points and are collinear, they must connect two faces
					edge_verts.append(collection)
			
	print("TEST")
	print(edge_verts)
	#print("I: "+str(i)+" "+str(edgea)+" J: "+str(j)+" "+str(edgeb))
	for i in mdt.get_vertex_count():
		mdt.set_vertex(i,mdt.get_vertex(i)+shift_dir[i])
	
	var edges = []
	
	for e in edge_verts:
		edges.append([mdt.get_vertex(e[0]),mdt.get_vertex(e[1]),mdt.get_vertex(e[2]),mdt.get_vertex(e[3])])
	
	
	# translate both corner verts and edge_verts to their actual "new" positions
	mdt.commit_to_surface(mesh)
	return mesh
