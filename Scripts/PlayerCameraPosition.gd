extends Node3D

@onready var synchronizer = $MultiplayerSynchronizer
# Called when the node enters the scene tree for the first time.
func _ready():
	synchronizer.set_multiplayer_authority(str(name).to_int())

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if Constants.id != 1:
		global_transform.origin = Constants.playerCamera.global_transform.origin
