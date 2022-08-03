extends Spatial


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
onready var gridCursor = $GridCursorMesh
onready var regularCursor = $MeshInstance

func _process(delta):
	var new_pos = Vector3(floor(regularCursor.global_transform.origin.x),floor(regularCursor.global_transform.origin.y),floor(regularCursor.global_transform.origin.z))
	#new_pos += Vector3(0.5,0.5,0.5)
	#gridCursor.global_transform.origin = new_pos
