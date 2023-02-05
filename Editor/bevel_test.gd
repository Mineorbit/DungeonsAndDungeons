extends Node3D

@onready var meshinstance: MeshInstance3D = $Cube
func _ready():
	var new_mesh = BevelEdges.CreateBevelEdgeMesh(meshinstance.mesh)
	meshinstance.mesh = new_mesh
	print(meshinstance.mesh)

