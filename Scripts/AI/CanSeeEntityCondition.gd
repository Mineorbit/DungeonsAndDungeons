class_name CanSeePlayerCondition extends ConditionLeaf

var vision: NodePath = "VisionCheck"
@export var class_types: PackedStringArray
@export var viewDistance:float = 16
@export var blackboard_name = ""

func tick(actor: Node, blackboard: Blackboard) -> int:
	var vision = actor.get_node("AI/Utils/"+str(self.vision))
	var target
	if vision == null:
		return FAILURE

	if vision.is_colliding():
		var i = 0
		while i < min(3,vision.get_collision_count()):
			var t = vision.get_collider(i)
			if t == null:
				return FAILURE
			if t.get_type() in class_types:
				target = t
				break
			i = i + 1
	#print(target.get_property_list()["class_name"])
	if target == null:
		return FAILURE
	if blackboard_name.is_empty():
		blackboard_name = "vis"
	if actor.global_transform.origin.distance_to(target.global_position) < viewDistance:
		#print("Found Player: "+str(target))
		blackboard.set_value(blackboard_name,target)
		return SUCCESS
	return FAILURE
