extends Node3D


@onready var activationArea: Area3D = $Node3D/Area3D
# Called when the node enters the scene tree for the first time.
func _ready():
	activationArea.body_entered.connect(process_entered)
	pass # Replace with function body.

func process_entered(x):
	get_parent().activate()

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
	
func activate():
	print("Activated Lever")
