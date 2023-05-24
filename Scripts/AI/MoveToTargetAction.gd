class_name MoveToTargetAction extends ActionLeaf

@export var target: String = "GoToTargetPosition"
@export var goal_distance = 2
func tick(actor: Node, blackboard: Blackboard) -> int:
	var target_node = blackboard.get_value(target)
	print(target_node)
	if target_node == null:
		actor.go_to_target = false
		return FAILURE
	actor.go_to_target = true
	var target_pos = actor.get_node("AI/Utils/GoToTargetPosition")
	target_pos.global_transform.origin = target_node.global_transform.origin
	# this needs to be navigation based instead
	#actor.look_at(target_node.global_position)
	#actor.move_and_slide(actor.global_transform.basis.z * 10, Vector3.UP)

	if actor.global_transform.origin.distance_to(target_node.global_position) < goal_distance:
		actor.go_to_target = false
		return SUCCESS
	
	return RUNNING
