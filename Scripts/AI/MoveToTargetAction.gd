class_name MoveToTargetAction extends ActionLeaf

var target: NodePath = "GoToTargetPosition"

func _init(target: NodePath):
	self.target = target

func tick(actor: Node, blackboard: Blackboard) -> int:
	var target_node = blackboard.get_node(self.target)
	if target_node == null:
		return FAILURE

	actor.look_at(target_node.global_position)
	actor.move_and_slide(actor.global_transform.basis.z * 10, Vector3.UP)

	if actor.global_transform.origin.distance_to(target_node.global_position) < 2:
		return SUCCESS

	return RUNNING

func interrupt(actor: Node, blackboard: Blackboard) -> void:
	actor.stop()
