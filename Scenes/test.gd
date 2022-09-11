extends NavigationRegion3D

@onready var blog = $Blog
# Called when the node enters the scene tree for the first time.
func _ready():
	blog.started = true


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	blog.target.global_transform.origin = blog.global_transform.origin
