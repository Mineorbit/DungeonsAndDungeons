class_name CanSeePlayerCondition extends ConditionLeaf

var vision: NodePath = "VisionCheck"
@export var class_type: String
@export var viewDistance:float = 16
func _init():
	pass

func tick(actor: Node, blackboard: Blackboard) -> int:
	var vision = blackboard.get_node(self.vision)
	var target
	if vision == null:
		return FAILURE

	if vision.is_colliding():
		target = vision.get_collider(0)
	print(target.get_property_list()["class_name"])
	if actor.global_transform.origin.distance_to(target.global_position) < viewDistance and target.get_property_list()["class_name"] == class_type:
		blackboard.set_value("seen"+str(class_type),target)
		return SUCCESS
	return FAILURE
