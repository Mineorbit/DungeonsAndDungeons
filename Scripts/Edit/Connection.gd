extends Node3D

@onready var point_a = $A
@onready var point_b = $B
@onready var wire = $Wire
@onready var closest = $Closest
@onready var wiremodel: MeshInstance3D = $Wire/WireModel
var mat
var mesh
var array
var arr_mesh
# Called when the node enters the scene tree for the first time.
func _ready():
	mat = StandardMaterial3D.new()
	mat.shading_mode = BaseMaterial3D.SHADING_MODE_UNSHADED
	mat.albedo_color = Color.GREEN
	
	pass # Replace with function body.

var lasta = Vector3.ZERO
var lastb = Vector3.ZERO
var a = Vector3.ZERO
var b = Vector3.ZERO
func update_mesh():
	a = (a + point_a.global_transform.origin)*0.5
	b = (b + point_b.global_transform.origin)*0.5
	
	if not ((a-lasta).length() > 0 or (b-lastb).length() > 0):
		return
	lasta = a
	lastb = b
	# this cannot be factored out
	arr_mesh = ArrayMesh.new()
	mesh = PlaneMesh.new()
	array = mesh.get_mesh_arrays()
	# this should instead be the cross product of the orthogonal of the camera position and normal and the ab direction
	var n = (b-a).normalized()
	var p = Constants.builderPosition
	var ortho = (p-a)-((p-a).dot(n))*n
	ortho = ortho.normalized()
	var offset = n.cross(ortho)
	var scaleing = 0.03
	wiremodel.global_transform.origin = a
	var planenormal = Vector3.UP.cross(b-a)
	# determine side of plane through a with vectors ab and up
	var factor = -1
	
	# these are the coordinates of all for corners of the connection mesh
	array[0][0] =  (b-a)+factor*scaleing* offset
	array[0][1] = (b-a)-factor*scaleing* offset
	array[0][2] = factor*scaleing* offset
	array[0][3] = - factor*scaleing* offset
	
	closest.global_transform.origin = a+offset
	arr_mesh.add_surface_from_arrays(Mesh.PRIMITIVE_TRIANGLE_STRIP,array)
	wiremodel.mesh = arr_mesh
	arr_mesh.surface_set_material(0,mat)


func _physics_process(delta):
	update_mesh()


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	update_mesh()
