extends Node3D
@onready var animtree: AnimationTree = $AnimationTree
var fsm
func _ready():
	fsm = animtree["parameters/openfsm/playback"]

func open():
	fsm.travel("Switch")

func close():
	fsm.travel("SwitchBack")
