extends Node3D

@export var spawnedEntity = "Enemies/Blog"

var prefab
var currentSpawned: Node3D
@onready var text = $Text
# Called when the node enters the scene tree for the first time.
func _ready():
	show_text()
	prefab = load("res://Prefabs/LevelObjects/Entities/"+spawnedEntity+".tscn")

func setup():
	show_text()
	
func start():
	print("Spawning now")
	show_text()
	spawnEntity()

func reset():
	print("Resetting Spawner")
	show_text()

func set_spawn_pos():
	currentSpawned.global_transform.origin = self.global_transform.origin + Vector3(0,1,0)
	print("Setting position to "+str(currentSpawned.global_transform.origin))

func show_text():
	if Constants.currentMode > 1:
		if text.get_parent() != null:
			remove_child(text)
	else:
		if text.get_parent() == null:
			add_child(text)
	

func spawnEntity():
	if Constants.currentMode > 2:
		return
	currentSpawned = prefab.instantiate()
	currentSpawned.rotate_y(deg_to_rad(global_rotation_degrees.y))
	set_spawn_pos()
	Constants.World.level.spawn_entity(currentSpawned)
	currentSpawned.on_entity_remove.connect(clear_after_remove)

func on_remove():
	despawnEntity()

func despawnEntity():
	if currentSpawned != null:
		currentSpawned.remove()

func clear_after_remove():
	currentSpawned = null
