class_name AimAction extends ActionLeaf

@export var aim_target_name: String = "AimTarget"
func tick(actor: Node, blackboard: Blackboard) -> int:
	var aim_target_position = actor.get_node("AI/Utils/AimTargetPosition")
	var t =  blackboard.get_value(aim_target_name)
	if t == null:
		print("Target "+str(aim_target_name)+" not on Blackboard")
		return FAILURE
	aim_target_position.global_transform.origin = t.global_transform.origin
	actor.Aim()
	# if aim is good, return SUCCESS
	return RUNNING
