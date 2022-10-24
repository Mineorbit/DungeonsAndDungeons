extends MeshInstance3D

@onready var camera: Camera3D = $Camera3D
@onready var enterArea: Area3D = $Area3D
# Called when the node enters the scene tree for the first time.
func _ready():
	enterArea.body_entered.connect(player_entered)
	enterArea.body_exited.connect(player_left)


func player_entered(player):
	print(str(Constants.id)+"Entered capture area "+str(player.get_parent()))
	camera.current = true
func player_left(player):
	print(str(Constants.id)+"Entered capture area "+str(player.get_parent()))
	camera.current = false

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
