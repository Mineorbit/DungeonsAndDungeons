class_name CallMethodAction extends ActionLeaf

@export var method: String = "Strike"

func tick(actor: Node, blackboard: Blackboard) -> int:
	print("Trying to call "+str(method))
	if actor.has_method(method):
		actor.call(method)
		return SUCCESS
	return FAILURE
