extends SubViewport


@onready var levellist = $LevelList


func _ready():
	setupInterfaceMaterial()


func setupInterfaceMaterial():
	var newmaterial = StandardMaterial3D.new()
	newmaterial.albedo_texture = get_texture()
	get_parent().material_override = newmaterial
	levellist.set_display_size(2)
	
