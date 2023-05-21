class_name HasItemCondition extends ConditionLeaf

var vision: NodePath = "VisionCheck"
@export var slot = 0

func tick(actor: Node, blackboard: Blackboard) -> int:
	if actor.has_item(slot):
		return SUCCESS
	return FAILURE
