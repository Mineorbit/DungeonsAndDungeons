extends Sprite2D


@onready var synchronizer = $MultiplayerSynchronizer

func _ready():
	synchronizer.set_multiplayer_authority(str(name).to_int())
	set_multiplayer_authority(str(name).to_int())


func _input(event):
	if name != str(Constants.id):
		return
	if event is InputEventMouseMotion:
		print(Constants.id)
		transform.origin = event.position

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
