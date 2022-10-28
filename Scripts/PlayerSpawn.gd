extends Node3D

@export var local_id: int

@onready var mesh: MeshInstance3D = $Mesh
# Called when the node enters the scene tree for the first time.
func _ready():
	var material = StandardMaterial3D.new()
	material.albedo_color = Constants.player_colors[local_id]
	mesh.set_surface_override_material(0,material)

func start():
	if Constants.currentLevel != null:
		Constants.currentLevel.player_spawns[local_id] = self


func on_remove():
	if Constants.currentLevel != null:
		Constants.currentLevel.player_spawns[local_id] = null
		print("Removing Spawn Entry of "+str(local_id))

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
