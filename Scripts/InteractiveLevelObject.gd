extends LevelObject


# Declare member variables here. Examples:
# var a = 2
# var b = "text"
var unique_instance_id = 0

signal activationSignal(state)
var connectedObjects = []



func to_instance(instance):
		instance.x = floor(transform.origin.x)
		instance.y = floor(transform.origin.y)
		instance.z = floor(transform.origin.z)
		instance.levelObjectData = levelObjectData
		instance.unique_instance_id = unique_instance_id
		instance.connectedObjects = connectedObjects
		return instance

func attachSignals():
	for object in connectedObjects:
		Constants.interactiveLevelObjects[object].activationSignal.connect(process)

func clearSignals():
	var connections = activationSignal.get_connections().duplicate()
	for conn in connections:
		activationSignal.disconnect(conn.callable)
		
func list_connections():
	var connections = activationSignal.get_connections().duplicate()
	for conn in connections:
		print(conn)
		
func process(activation):
	if activation:
		activate()
	else:
		deactivate()
# Called when the node enters the scene tree for the first time.
func _ready():
	preparing_Collision()
	unique_instance_id = Constants.currentInteractive
	Constants.currentInteractive += 1



func sign_up():
	Constants.interactiveLevelObjects[unique_instance_id] = self

#this should be extracted into another prefab
func preparing_Collision():
	construction_collision = $ConstructionCollision
	Constants.mode_changed.connect(on_mode_change)
	


func activate():
	if contained_level_object.has_method("activate"):
		contained_level_object.activate()
	activationSignal.emit(true)


func deactivate():
	if contained_level_object.has_method("deactivate"):
		contained_level_object.deactivate()
	activationSignal.emit(false)

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
