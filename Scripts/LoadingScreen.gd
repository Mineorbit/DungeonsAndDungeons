extends CanvasLayer

var is_open = true

func open():
	if is_open:
		return
	is_open = true

func close():
	if not is_open:
		return
	is_open = false
