extends Node

var player
var camera
@export var input_direction = Vector3.ZERO
@onready var synchronizer = $MultiplayerSynchronizer
# Called when the node enters the scene tree for the first time.
func _ready():
	synchronizer.set_multiplayer_authority(str(name).to_int())
	camera = load("res://Prefabs/PlayerCamera.tscn").instantiate()

var spawned = false


func spawn():
	spawned = true
	add_child(camera)
	camera.player = player
	
var is_active = true
func set_active(active):
	print("Setting "+str(active))
	is_active = active

func despawn():
	spawned = false
	remove_child(camera)
	camera.player = player

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta):
	if is_active and synchronizer.is_multiplayer_authority():
		input_direction = Vector3.ZERO
		input_direction.x = Input.get_action_strength("right") - Input.get_action_strength("left")
		input_direction.z = Input.get_action_strength("back") - Input.get_action_strength("forward")
		input_direction = input_direction.rotated(Vector3.UP, camera.rotation.y).normalized()
		synchronizer.input_direction = input_direction
	#print(str(self)+" "+str(input_direction))
	#if Input.is_action_just_pressed("LeftUse"):
	#	player.UseLeft()
		
	#player.should_jump = Input.is_action_just_pressed("jump")
		
	#if Input.is_action_just_pressed("Pickup"):
	#	player.on_entity_pickup.emit()
		
		
