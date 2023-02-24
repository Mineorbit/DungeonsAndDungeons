extends Node3D

@onready var gridmesh = $GridMesh
@onready var meshinstance: MeshInstance3D = $Cube
func _ready():
	#var new_mesh = BevelEdges.CreateBevelEdgeMesh(meshinstance.mesh)
	#meshinstance.mesh = new_mesh
	#print(meshinstance.mesh)
	#gridmesh.add(0,0,0)
	pass

func test():
	var rand = RandomNumberGenerator.new()
	#gridmesh.add(rand.randi_range(0,5),rand.randi_range(0,5),rand.randi_range(0,5))
	for i in range(8):
		for j in range(8):
			for k in range(8):
				gridmesh.add(i,j,k)
	gridmesh.build()
	ResourceSaver.save(gridmesh.mesh,"res://Editor/mesh.res")

func _physics_process(delta):
	if Input.is_action_just_pressed("Connect"):
		test()
