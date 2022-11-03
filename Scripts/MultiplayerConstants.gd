extends Node


var local_id = -1
var local_id_to_id = [null,null,null,null]

var player_cameras = [null,null,null,null]

@rpc
func set_local_id(id):
	local_id = id
	
func get_first_connected():
	var i = 0
	while i < local_id_to_id.size() and local_id_to_id[i] == null:
		i = i + 1
	if local_id_to_id.size() == i:
		# in this case we need to give the server the role
		return 1
	return local_id_to_id[i]
