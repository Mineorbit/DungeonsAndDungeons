extends Node3D

@export var spawnedEntity = "Enemies/Blog"

var prefab
var currentSpawned
# Called when the node enters the scene tree for the first time.
func _ready():
	print("Starting Spawner at"+str(global_transform.origin))
	prefab = load("res://Prefabs/LevelObjects/Entities/"+spawnedEntity+".tscn")

func setup():
	spawnEntity()
	
func start():
	spawnEntity()

func reset():
	if currentSpawned != null:
		set_spawn_pos()
	else:
		spawnEntity()

func set_spawn_pos():
	currentSpawned.global_transform.origin = self.global_transform.origin
	print("Setting position to "+str(currentSpawned.global_transform.origin))

func spawnEntity():
	print("Spawning Entity at "+str(self.global_transform.origin))
	if currentSpawned != null:
		currentSpawned.remove()
	currentSpawned = prefab.instantiate()
	Constants.currentEntities.add_child(currentSpawned)
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
