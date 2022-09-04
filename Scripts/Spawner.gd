extends Node3D

@export var spawnedEntity = "Enemies/Blog"

var prefab
var currentSpawned
# Called when the node enters the scene tree for the first time.
func _ready():
	pass
func start():
	print("Starting Spawner")
	prefab = load("res://Prefabs/LevelObjects/Entities/"+spawnedEntity+".tscn")
	currentSpawned = prefab.instantiate()
	if Constants.currentLevel != null:
		Constants.currentLevel.get_child(3).add_child(currentSpawned)
	else:
		self.add_child(currentSpawned)
	currentSpawned.global_transform.origin = self.global_transform.origin

		
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
