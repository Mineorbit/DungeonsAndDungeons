extends Node3D

# Called when the node enters the scene tree for the first time.

var owner_id = 0



func _enter_tree():
	owner_id = str(name).to_int()
	set_multiplayer_authority(str(name).to_int())
	get_node("MultiplayerSynchronizer").set_multiplayer_authority(str(name).to_int())
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if Constants.id == owner_id:
			global_transform.origin = PlayerCamera.Camera.global_transform.origin
			global_rotation = PlayerCamera.Camera.global_rotation
