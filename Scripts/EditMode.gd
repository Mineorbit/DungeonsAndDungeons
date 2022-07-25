extends Spatial


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

onready var player = $Player
onready var builder = $Builder
var playerpref
var builderpref
# Called when the node enters the scene tree for the first time.
func _ready():
	playerpref = load("res://Prefabs/LevelObjects/Entities/Player.tscn")
	builderpref = load("res://Prefabs/Builder.tscn")
	enter_edit_mode()

var mode = -1 # 0 = edit 1 = test


func _process(delta) -> void:
	if Input.is_action_just_pressed("Test"):
		enter_test_mode()
	if Input.is_action_just_pressed("Edit"):
		enter_edit_mode()
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
func enter_edit_mode():
	if(mode == 0):
		return
	mode = 0
	remove_child(player)
	builder = builderpref.instance()
	add_child(builder)
	builder.global_transform.origin = Vector3(0,5,0)
	

func enter_test_mode():
	if(mode == 1):
		return
	mode = 1
	remove_child(builder)
	player = playerpref.instance()
	add_child(player)
	player.global_transform.origin = Vector3(0,5,0)
	
	
