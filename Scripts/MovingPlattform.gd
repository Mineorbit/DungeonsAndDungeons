extends Spatial


# Declare member variables here. Examples:
# var a = 2
# var b = "text"
export(float) var strength = 0.125

onready var plattformBody: KinematicBody = $PlattformBody

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

var time = 0
func _physics_process(delta) -> void:
	time += delta
	plattformBody.move_and_collide(Vector3.FORWARD*strength*sin(wrapf(time + 0.1, -PI, PI)))

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
