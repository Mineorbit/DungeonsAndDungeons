extends Node3D


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
@onready var gridCursor = $GridCursorMesh
@onready var regularCursor = $MeshInstance
@onready var placeSound = $Place
@onready var displaceSound = $Displace
