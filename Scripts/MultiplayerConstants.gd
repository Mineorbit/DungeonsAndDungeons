extends Node


signal on_local_id_set(id)

var local_id = -1:
	get:
		return local_id
	set(value):
		local_id = value
		on_local_id_set.emit(local_id)


@export var local_id_to_id = [null,null,null,null]

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


func get_local_id(id):
	var i = 0
	while (i < local_id_to_id.size() and local_id_to_id[i] != id):
		i = i + 1
	return i
