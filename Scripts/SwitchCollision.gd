extends AnimatableBody3D

var skeleton: Skeleton3D

func _ready():
	skeleton = get_parent().get_parent()

func _physics_process(delta):
	transform = skeleton.get_bone_global_pose(0)
