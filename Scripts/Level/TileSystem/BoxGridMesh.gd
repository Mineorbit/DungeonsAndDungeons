
extends MeshInstance3D


var levelObjectId


var generating = false

var previewmesh

@export var surfacematerial: Material

@onready var gridmesh = $Grid

func _ready():
	previewmesh = get_node("0")
	if get_parent().get("display") == null:
		previewmesh.hide()
		remove_child(previewmesh)

var neighbors = [Vector3(0,1,0),
				Vector3(0,-1,0)]

@onready var collisionshape = $Collision

var building = false

func rebuild_mesh():
	building = true
	var surfaceTool = SurfaceTool.new()
	
	for n in collisionshape.get_children():
		collisionshape.remove_child(n)
		n.queue_free()
	surfaceTool.begin(Mesh.PRIMITIVE_TRIANGLES)
	for x in range(8):
		for y in range(8):
			for z in range(8):
				var p = global_transform.origin + Vector3(x,y,z)
				if get_parent().get_at(p) == levelObjectId:
					var box_col = CollisionShape3D.new()
					box_col.shape = BoxShape3D.new()
					box_col.position = Vector3(x+0.5,y+0.5,z+0.5)
					collisionshape.add_child(box_col)
					#top face
					if get_parent().get_at(p+Vector3(0,1,0)) != levelObjectId:
						surfaceTool.set_normal(Vector3(0,1,0))
						surfaceTool.add_vertex(Vector3(x+1,y+1,z+1))
						surfaceTool.set_normal(Vector3(0,1,0))
						surfaceTool.add_vertex(Vector3(x,y+1,z+1))
						surfaceTool.set_normal(Vector3(0,1,0))
						surfaceTool.add_vertex(Vector3(x,y+1,z))
						surfaceTool.set_normal(Vector3(0,1,0))
						surfaceTool.add_vertex(Vector3(x,y+1,z))
						surfaceTool.set_normal(Vector3(0,1,0))
						surfaceTool.add_vertex(Vector3(x+1,y+1,z))
						surfaceTool.set_normal(Vector3(0,1,0))
						surfaceTool.add_vertex(Vector3(x+1,y+1,z+1))
					
					#right face
					
					if get_parent().get_at(p+Vector3(1,0,0)) != levelObjectId:
						surfaceTool.set_normal(Vector3(1,0,0))
						surfaceTool.add_vertex(Vector3(x+1,y+1,z+1))
						surfaceTool.set_normal(Vector3(1,0,0))
						surfaceTool.add_vertex(Vector3(x+1,y+1,z))
						surfaceTool.set_normal(Vector3(1,0,0))
						surfaceTool.add_vertex(Vector3(x+1,y,z))
						surfaceTool.set_normal(Vector3(1,0,0))
						surfaceTool.add_vertex(Vector3(x+1,y,z))
						surfaceTool.set_normal(Vector3(1,0,0))
						surfaceTool.add_vertex(Vector3(x+1,y,z+1))
						surfaceTool.set_normal(Vector3(1,0,0))
						surfaceTool.add_vertex(Vector3(x+1,y+1,z+1))
					
					#left face
					
					if get_parent().get_at(p+Vector3(-1,0,0)) != levelObjectId:
						surfaceTool.set_normal(Vector3(-1,0,0))
						surfaceTool.add_vertex(Vector3(x,y,z))
						surfaceTool.set_normal(Vector3(-1,0,0))
						surfaceTool.add_vertex(Vector3(x,y+1,z))
						surfaceTool.set_normal(Vector3(-1,0,0))
						surfaceTool.add_vertex(Vector3(x,y+1,z+1))
						surfaceTool.set_normal(Vector3(-1,0,0))
						surfaceTool.add_vertex(Vector3(x,y+1,z+1))
						surfaceTool.set_normal(Vector3(-1,0,0))
						surfaceTool.add_vertex(Vector3(x,y,z+1))
						surfaceTool.set_normal(Vector3(-1,0,0))
						surfaceTool.add_vertex(Vector3(x,y,z))
					
					#front face
					
					if get_parent().get_at(p+Vector3(0,0,1)) != levelObjectId:
						surfaceTool.set_normal(Vector3(0,0,1))
						surfaceTool.add_vertex(Vector3(x,y,z+1))
						surfaceTool.set_normal(Vector3(0,0,1))
						surfaceTool.add_vertex(Vector3(x,y+1,z+1))
						surfaceTool.set_normal(Vector3(0,0,1))
						surfaceTool.add_vertex(Vector3(x+1,y+1,z+1))
						surfaceTool.set_normal(Vector3(0,0,1))
						surfaceTool.add_vertex(Vector3(x+1,y+1,z+1))
						surfaceTool.set_normal(Vector3(0,0,1))
						surfaceTool.add_vertex(Vector3(x+1,y,z+1))
						surfaceTool.set_normal(Vector3(0,0,1))
						surfaceTool.add_vertex(Vector3(x,y,z+1))
					
					# back face 
					
					
					if get_parent().get_at(p+Vector3(0,0,-1)) != levelObjectId:
						surfaceTool.set_normal(Vector3(0,0,-1))
						surfaceTool.add_vertex(Vector3(x+1,y+1,z))
						surfaceTool.set_normal(Vector3(0,0,-1))
						surfaceTool.add_vertex(Vector3(x,y+1,z))
						surfaceTool.set_normal(Vector3(0,0,-1))
						surfaceTool.add_vertex(Vector3(x,y,z))
						surfaceTool.set_normal(Vector3(0,0,-1))
						surfaceTool.add_vertex(Vector3(x,y,z))
						surfaceTool.set_normal(Vector3(0,0,-1))
						surfaceTool.add_vertex(Vector3(x+1,y,z))
						surfaceTool.set_normal(Vector3(0,0,-1))
						surfaceTool.add_vertex(Vector3(x+1,y+1,z))
					
					#bottom face
					if get_parent().get_at(p+Vector3(0,-1,0)) != levelObjectId:
						surfaceTool.set_normal(Vector3(0,-1,0))
						surfaceTool.add_vertex(Vector3(x,y,z))
						surfaceTool.set_normal(Vector3(0,-1,0))
						surfaceTool.add_vertex(Vector3(x,y,z+1))
						surfaceTool.set_normal(Vector3(0,-1,0))
						surfaceTool.add_vertex(Vector3(x+1,y,z+1))
						surfaceTool.set_normal(Vector3(0,-1,0))
						surfaceTool.add_vertex(Vector3(x+1,y,z+1))
						surfaceTool.set_normal(Vector3(0,-1,0))
						surfaceTool.add_vertex(Vector3(x+1,y,z))
						surfaceTool.set_normal(Vector3(0,-1,0))
						surfaceTool.add_vertex(Vector3(x,y,z))
	surfaceTool.generate_normals()
	surfaceTool.generate_tangents()
	var mesh = surfaceTool.commit()
	gridmesh.mesh = mesh
	gridmesh.set_surface_override_material(0,surfacematerial)
	
	building = false
	#collisionshape.shape = mesh.create_trimesh_shape()
# regenerate mesh at position where stuff changed / in worst case the location can be ignored
func generate():
	rebuild_mesh()

func queue_generate(p):
	generate()
