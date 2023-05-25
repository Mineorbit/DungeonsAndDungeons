class_name HasItemCondition extends ConditionLeaf

@export var any: bool = false
@export var item_types: PackedStringArray
@export var slot = 0

func tick(actor: Node, blackboard: Blackboard) -> int:
	if actor.has_item(slot):
		if any:
			return SUCCESS
		if actor.item[slot].get_type() in item_types:
			return SUCCESS
	return FAILURE