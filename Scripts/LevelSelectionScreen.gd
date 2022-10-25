extends MeshInstance3D

@onready var camera: Camera3D = $Camera3D
@onready var enterArea: Area3D = $Area3D
@onready var levellist = $SubViewport/LevelList
@onready var subViewport: Viewport = $SubViewport
# Called when the node enters the scene tree for the first time.
func _ready():
	enterArea.body_entered.connect(player_entered)
	enterArea.body_exited.connect(player_left)

#change to local player inside
var players_inside = 0

func player_entered(player):
	print(str(Constants.id)+"Entered capture area "+str(player.get_parent()))
	camera.current = true
	Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
	players_inside = players_inside + 1
func player_left(player):
	print(str(Constants.id)+"Entered capture area "+str(player.get_parent()))
	camera.current = false
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	players_inside = players_inside - 1

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	#needed for dragging
	print(Input.is_action_pressed("Drag"))
	levellist.dragging = Input.is_action_pressed("Drag")

func _input(event):
	if players_inside > 0:
		subViewport.push_input(event, true)
