extends Node3D




var target_position = Vector3.ZERO
var target_offset = Vector3.ZERO
var player
@export var offset = 0

func prepare_camera_target(p):
	print("Preparing Camera Target")
	if player != p:
		player = p
		print("Preparing Camera Target2 "+str(player))
		player.on_entity_aiming.connect(change_aiming_state)

func _enter_tree():
	set_multiplayer_authority(1)

func change_aiming_state(s):
	offset = s
	print(str(Constants.id)+" "+str(s))

var s = 0.35

var t = 0.9
@export var target_distance = 1.5

func get_camera_target_position():
	var aim_offset = Vector3.ZERO
	if player != null:
		aim_offset = target_distance*Vector3(1,0,0)*int(offset)
	#print(aim_offset)
	target_offset = t*target_offset + (1-t)*aim_offset
	return Vector3.UP*0.75 + target_offset


func move_camera_target():
	target_position = s*target_position + (1-s)*get_camera_target_position()
	transform.origin = target_position


func _physics_process(delta):
	move_camera_target()

func _process(delta):
	move_camera_target()
