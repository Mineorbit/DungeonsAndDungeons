extends Node


var local_id = -1
var local_id_to_id = [null,null,null,null]

@rpc
func set_local_id(id):
	local_id = id
