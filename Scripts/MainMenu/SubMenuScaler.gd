extends GridContainer


# Called when the node enters the scene tree for the first time.
func _ready():
	get_tree().get_root().size_changed.connect(rescale)
	rescale()
	#custom_minimum_size = get_parent().size
		#child.size = size

@onready var levellist = $VSplitContainer/LevelList

func rescale():
	for child in get_children():
		child.custom_minimum_size = get_parent().size
	print("AHOI: "+str(get_parent().size.y * 0.5))
	levellist.custom_minimum_size.y = get_parent().size.y * 0.95
