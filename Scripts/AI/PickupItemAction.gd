class_name PickupItemAction extends ActionLeaf


func tick(actor: Node, blackboard: Blackboard) -> int:
	actor.on_entity_pickup.emit()
	return SUCCESS
