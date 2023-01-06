extends Node3D

@onready var synchronizer = $MultiplayerSynchronizer
# Called when the node enters the scene tree for the first time.
func _ready():
	set_multiplayer_authority(str(name).to_int())
	synchronizer.set_multiplayer_authority(str(name).to_int())

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if Constants.id != 1:
		#need to get location data of actual camera not just PlayerCamera Node
		if Constants.World.players.playerControllers.camera != null:
			global_transform.origin = Constants.World.players.playerControllers.camera.Camera.global_transform.origin
			global_rotation = Constants.World.players.playerControllers.camera.Camera.global_rotation
