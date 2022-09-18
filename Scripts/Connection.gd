extends Node3D

@onready var point_a = $A
@onready var point_b = $B
@onready var wire = $Wire

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

var dir = Vector3.ZERO
var dirAB = Vector3.ZERO
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	wire.global_transform.origin = (point_a.global_transform.origin + point_b.global_transform.origin)/2
	
	dir = Constants.builderPosition-wire.global_transform.origin
	dir = dir.normalized()
	dirAB = (point_a.global_transform.origin - point_b.global_transform.origin).normalized()
	dir = dir.cross(dirAB)
	wire.look_at(Constants.builderPosition,dir)
