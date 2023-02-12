extends Node3D

@onready var gridmesh = $GridMesh
@onready var meshinstance: MeshInstance3D = $Cube
func _ready():
	#var new_mesh = BevelEdges.CreateBevelEdgeMesh(meshinstance.mesh)
	#meshinstance.mesh = new_mesh
	#print(meshinstance.mesh)
	pass
	#gridmesh.add(0,0,0)

func test():
	var rand = RandomNumberGenerator.new()
	var x = rand.randi_range(0,5)
	var y = rand.randi_range(0,5)
	var z = rand.randi_range(0,5)
	#print(str(x)+" "+str(y)+" "+str(z))
	gridmesh.add(x,y,z)
	ResourceSaver.save(gridmesh.mesh,"res://Editor/mesh.res")

func _physics_process(delta):
	if Input.is_action_just_pressed("Connect"):
		test()
