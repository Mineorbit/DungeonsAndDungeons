extends CanvasLayer

@onready var menu = $Menu
var open = false
# Called when the node enters the scene tree for the first time.
func _ready():
	menu.hide()


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if Input.is_action_just_pressed("PauseMenu"):
		if not open:
			pause()
		else:
			unpause()

var last_mouse_mode

func pause():
	last_mouse_mode = Input.get_mouse_mode()
	Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
	get_tree().paused = true
	menu.show()
	open = true

func unpause():
	Input.set_mouse_mode(last_mouse_mode)
	get_tree().paused = false
	menu.hide()
	open = false
	
	
