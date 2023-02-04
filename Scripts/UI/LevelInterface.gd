extends SubViewport


@onready var levellist = $LevelList

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
func _ready():
	setupInterfaceMaterial()


func setupInterfaceMaterial():
	var newmaterial = StandardMaterial3D.new()
	newmaterial.albedo_texture = get_texture()
	get_parent().material_override = newmaterial
	levellist.set_display_size(2)
	
