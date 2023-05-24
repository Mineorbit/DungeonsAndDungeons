class_name CallMethodAction extends ActionLeaf

@export var method: String = "Strike"
func tick(actor: Node, blackboard: Blackboard) -> int:
	print("Trying to call "+str(method))
	actor.call(method)
	return SUCCESS
