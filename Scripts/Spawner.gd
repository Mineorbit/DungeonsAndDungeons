extends Node3D

@export var spawnedEntity = "Enemies/Blog"

var prefab
var currentSpawned
@onready var text = $Text
# Called when the node enters the scene tree for the first time.
func _ready():
	show_text()
	print("Starting Spawner at "+str(global_transform.origin))
	prefab = load("res://Prefabs/LevelObjects/Entities/"+spawnedEntity+".tscn")

func setup():
	show_text()
	spawnEntity()
	
func start():
	show_text()
	spawnEntity()

func reset():
	print("Resetting Spawner")
	show_text()
	if currentSpawned != null:
		set_spawn_pos()
	else:
		spawnEntity()

func set_spawn_pos():
	currentSpawned.global_transform.origin = self.global_transform.origin + Vector3(0,1,0)
	print("Setting position to "+str(currentSpawned.global_transform.origin))

func show_text():
	print("Text showing?")
	if Constants.currentMode > 1:
		remove_child(text)
	else:
		add_child(text)
	

func spawnEntity():
	print("HALLO "+str(Constants.currentMode))
	if Constants.currentMode > 2:
		return
	print("Spawning Entity at "+str(self.global_transform.origin))
	if currentSpawned != null:
		currentSpawned.remove()
	currentSpawned = prefab.instantiate()
	Constants.currentLevel.Entities.add_child(currentSpawned)
	Constants.buffer()
	set_spawn_pos()
	currentSpawned.on_entity_remove.connect(clear_after_remove)

func on_remove():
	despawnEntity()

func despawnEntity():
	if currentSpawned != null:
		currentSpawned.remove()

func clear_after_remove():
	currentSpawned = null
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
