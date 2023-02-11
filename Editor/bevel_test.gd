extends Node3D

@onready var gridmesh = $GridMesh
@onready var meshinstance: MeshInstance3D = $Cube
func _ready():
	#var new_mesh = BevelEdges.CreateBevelEdgeMesh(meshinstance.mesh)
	#meshinstance.mesh = new_mesh
	#print(meshinstance.mesh)
	for i in range(8):
		for j in range(8):
			pass
			gridmesh.add(i,0,j)
	#gridmesh.add(0,0,0)
	gridmesh.build()
	ResourceSaver.save(gridmesh.mesh,"res://Editor/mesh.res")
