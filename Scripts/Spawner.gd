extends Node3D

@export var spawnedEntity = "Enemies/Blog"

var prefab
var currentSpawned
# Called when the node enters the scene tree for the first time.
func _ready():
	print("Starting Spawner")
	prefab = load("res://Prefabs/LevelObjects/Entities/"+spawnedEntity+".tscn")
	spawnEntity()
	
func start():
	spawnEntity()

func spawnEntity():
	print("Spawning Entity")
	currentSpawned = prefab.instantiate()
	if Constants.currentLevel != null:
		Constants.currentLevel.entities.add_child(currentSpawned)
	else:
		self.add_child(currentSpawned)
	currentSpawned.global_transform.origin = self.global_transform.origin

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
