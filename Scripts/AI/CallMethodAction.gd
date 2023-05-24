class_name CallMethodAction extends ActionLeaf

@export var method: String = "Strike"
func tick(actor: Node, blackboard: Blackboard) -> int:
	actor.call(method)
	return SUCCESS
