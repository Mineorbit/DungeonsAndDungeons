extends Node3D

@onready var point_a = $A
@onready var point_b = $B
@onready var wire = $Wire
@onready var wiremodel = $Wire/WireModel
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

var dir = Vector3.ZERO
var dirAB = Vector3.ZERO
var scaling = 0
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	#project targetpoint onto plane of AB direction on centerpoint
	dirAB = (point_a.global_transform.origin - point_b.global_transform.origin)
	dirAB = dirAB.normalized()
	var wirecenter = (point_a.global_transform.origin + point_b.global_transform.origin)/2
	var targetpoint = Plane(dirAB,0).project(Constants.builderPosition)+wirecenter
	wire.global_transform.origin = wirecenter
	
	dir = Constants.builderPosition-wire.global_transform.origin
	dir = dir.normalized()
	scaling = (point_a.global_transform.origin - point_b.global_transform.origin).length()
	dirAB = dirAB.normalized()
	dir = dir.cross(dirAB)
	wire.look_at(targetpoint,dir)
	wire.scale = Vector3(scaling/2,0.03,0.03)
