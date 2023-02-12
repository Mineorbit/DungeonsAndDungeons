extends Node

func close(a,b):
	return (a-b).length() < 0.1

func sort_by_clock(list,normal,normals):
	var center = Vector3.ZERO
	for l in list:
		center += l
	center = center/list.size()
	normal = normal.normalized()
	# pick start as first element
	var result = []
	var plane = Plane(normal,0)
	var d = list[0]-center
	d = plane.project(d)
	var d1 = d.normalized()
	var resultangles = []
	for i in range(0,list.size()):
		var pd = list[i] - center
		var pointdir = plane.project(pd)
		var d2 = pointdir.normalized()
		var dot = d1.dot(d2)
		var det = normal.dot(d1.cross(d2))
		#var det = d1.x*d2.y*normal.z + d2.x*normal.y*d1.z + normal.x*d1.y*d2.z - (d1.z*d2.y*normal.x + d2.z*normal.y*d1.x + normal.z*d1.y*d2.x)
		var resultangle = atan2(det,dot)
		resultangles.append([list[i],resultangle,i,normals[i]])
	resultangles.sort_custom(sort_rule)
	# rotate list until 0 is at 0
	while resultangles[0][2] != 0:
		var new_resultangles = resultangles.duplicate()
		for i in range(resultangles.size()):
			new_resultangles[(i-1)%resultangles.size()] = resultangles[i]
		resultangles = new_resultangles
	for x in resultangles:
		result.append([x[0],x[3]])
	return result

func sort_rule(a,b):
	if a[1] > b[1]:
		return true
	return false


var modify = true

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
	
	var bevelsize = 0.2
	
	var facecenter = Vector3.ZERO
	for i in mdt.get_face_count():
		var k = i - i % 2
		
		for j in range(3):
			var id = mdt.get_face_vertex(i,j)
			var p = mdt.get_vertex(id)
			facecenter += p
			
		if i % 2 == 1:
			var center = facecenter/6
			for m in range(i-1,i+1):
				for n in range(3):
					var id = mdt.get_face_vertex(m,n)
					var p = mdt.get_vertex(id)
					shift_dir[id] = bevelsize*(center-p)
			facecenter = Vector3.ZERO
	
	var n = mdt.get_edge_count()
	
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
			if d < 0.125 or d > -0.125:
				#  we need to check that the positions of edge vertices are "close"
				
				if close(a0,b0) and close(a1,b1):
					var collection = [edgea[0],edgea[1],edgeb[1],edgeb[0]]
					edge_verts.append(collection)
					pass
					# every edge has exactly one face, therefore we can just add face normals
					# since these two edges share points and are collinear, they must connect two faces
				if close(a0,b1) and close(a1,b0):
					var collection = [edgea[0],edgea[1],edgeb[1],edgeb[0]]
					edge_verts.append(collection)
					pass
					#var collection = [edgeb[0],edgeb[1],edgea[1],edgea[0]]
					# since these two edges share points and are collinear, they must connect two faces
					#edge_verts.append(collection)
					
				#if (close(a0,b0) or close(a0,b1)) and (close(a1,b0) or close(a1,b1)):
					
				#	var collection = [edgea[0],edgea[1],edgeb[0],edgeb[1]]
					# since these two edges share points and are collinear, they must connect two faces
				#	edge_verts.append(collection)
	# collect all vertices that are at same position
	var corner_verts = []
	var vertex_list = range(mdt.get_vertex_count())
	var corner_pos = {}
	for i in vertex_list:
		#print(hash(mdt.get_vertex(i)))
		var p = mdt.get_vertex(i)
		corner_pos[i] = p
		var added = false
		for corner in corner_verts:
			if (corner_pos[corner[0]] - p).length_squared() < 0.01:
				corner.append(i)
				added = true
		if not added:
			corner_verts.append([i])

	# update vertices to new positions
	#print("I: "+str(i)+" "+str(edgea)+" J: "+str(j)+" "+str(edgeb))
	for i in mdt.get_vertex_count():
		mdt.set_vertex(i,mdt.get_vertex(i)+shift_dir[i])
	
	var edges = []
	
	for e in edge_verts:
		#we copy over the normal
		edges.append([
			[mdt.get_vertex(e[0]),mdt.get_vertex_normal(e[0])],
			[mdt.get_vertex(e[1]),mdt.get_vertex_normal(e[1])],
			[mdt.get_vertex(e[2]),mdt.get_vertex_normal(e[2])],
			[mdt.get_vertex(e[3]),mdt.get_vertex_normal(e[3])]
			])
	
	
	var corners = []
	for c in corner_verts:
		var corner = []
		for x in c:
			corner.append([mdt.get_vertex(x),mdt.get_vertex_normal(x)])
		corners.append(corner)
	
	# translate both corner verts and edge_verts to their actual "new" positions
	mdt.commit_to_surface(mesh)
	
	var st = SurfaceTool.new()
	
	if modify:
		st.index()
		st.begin(Mesh.PRIMITIVE_TRIANGLES)
		var threshold = 0.5
		for e in edges:
					var vertlist = [e[0][0],e[1][0],e[2][0],e[3][0]]
					var normallist = [e[0][1],e[1][1],e[2][1],e[3][1]]
					# or rather add normals
					var facenormal = Vector3.ZERO
					for x in normallist:
						facenormal += x
					facenormal = facenormal/normallist.size()
					facenormal = facenormal.normalized()
					var r = sort_by_clock(vertlist,facenormal,normallist)
					for i in range(0,r.size()-2):
						st.set_normal(r[0][1])
						st.add_vertex(r[0][0])
						for j in range(1,3):
							st.set_normal(r[j+i][1])
							st.add_vertex(r[j+i][0])
							#st.add_triangle_fan(resultvertlist,null,null,null,resultnormallist)
		
		
		for c in corners:
			var vertlist = []
			var normallist = []
			
			for x in c:
				vertlist.append(x[0])
				normallist.append(x[1])
			
			var normal = Vector3.ZERO
			#print("===")
			#print(c)
			
			for x in normallist:
				normal += x
			normal = normal/normallist.size()
			normal =  normal.normalized()
			
			# todo: order according to normal and center point of corner
			var r = sort_by_clock(vertlist,normal,normallist)
			for i in range(0,r.size()-2):
				st.set_normal(r[0][1])
				st.add_vertex(r[0][0])
				for j in range(1,3):
					st.set_normal(r[j+i][1])
					st.add_vertex(r[j+i][0])

	for i in mesh.get_surface_count():
		st.index()
		st.append_from(mesh,i,Transform3D.IDENTITY)
	#st.generate_normals()
	var resultmesh = st.commit()
	return resultmesh

