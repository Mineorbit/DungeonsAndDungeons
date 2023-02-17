extends MeshInstance3D


var levelObjectId

func generate():
	for x in get_children():
		x.generate()
