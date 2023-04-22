extends Skeleton3D


# Called when the node enters the scene tree for the first time.
func _ready():
	
	# cannot simultaneously start physical bone sim and animate physical bones
	physical_bones_start_simulation(["band_top.r","band_top.l"])
	animate_physical_bones = true
	#physical_bones_start_simulation()


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	#force_update_all_bone_transforms ( )
	pass
