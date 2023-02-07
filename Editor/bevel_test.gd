extends Node3D

@onready var meshinstance: MeshInstance3D = $Cube
func _ready():
	var new_mesh = BevelEdges.CreateBevelEdgeMesh(meshinstance.mesh)
	meshinstance.mesh = new_mesh
	print(meshinstance.mesh)
	BevelEdges.sort_by_clock([Vector3(1,0,0),Vector3(0,0,1),Vector3(1,0,1),Vector3(-1,0,1)],Vector3(0,1,0))
