extends Node3D
@onready var animtree: AnimationTree = $AnimationTree
var fsm
func _ready():
	fsm = animtree["parameters/fsm/playback"]

func switch():
	fsm.travel("Switch")

func switchback():
	fsm.travel("SwitchBack")
