extends SubViewport


@onready var sprite: Sprite2D = $Sprite2D
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func _input(event):
	if event is InputEventMouseButton or event is InputEventMouseMotion:
		sprite.transform.origin = event.position
