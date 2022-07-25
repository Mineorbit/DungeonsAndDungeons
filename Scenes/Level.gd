extends Spatial


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


onready var gridMap: GridMap = $levelobject_grid
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


func add(position):
	var pos = gridMap.world_to_map(position)
	print(pos)
	gridMap.set_cell_item(pos.x,pos.y,pos.z,0)
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
