class_name CanSeePlayerCondition extends ConditionLeaf

var vision: NodePath = "VisionCheck"
var target: NodePath = "GoToTargetPosition"
@export var viewDistance:float = 16
func _init():
	pass

func tick(actor: Node, blackboard: Blackboard) -> int:
	var vision = blackboard.get_node(self.vision)
	var target = blackboard.get_node(self.target)
	if vision == null:
		return FAILURE

	if vision.is_colliding():
		target = vision.get_collider(0)
	if actor.global_transform.origin.distance_to(target.global_position) < viewDistance and target is Player:
		return SUCCESS
	return FAILURE
