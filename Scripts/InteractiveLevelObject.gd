extends LevelObject


# Declare member variables here. Examples:
# var a = 2
# var b = "text"
var unique_instance_id = 0

signal activationSignal(state)
var connectedObjects = []


func attachSignals():
	print("Attaching to Signals of incoming to "+str(self.unique_instance_id)+" "+str(self.get_children()[1]))
	for object in connectedObjects:
		print("Attaching to "+str(object)+" "+str(Constants.interactiveLevelObjects[object].get_children()[1]))
		Constants.interactiveLevelObjects[object].activationSignal.connect(process)

func clearSignals():
	print("Clearing Signal of "+str(self.get_children()[1]))
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
	unique_instance_id = Constants.currentInteractive
	Constants.currentInteractive += 1
	Constants.interactiveLevelObjects[unique_instance_id] = self


func activate():
	if contained_level_object.has_method("activate"):
		contained_level_object.activate()
	list_connections()
	print("Activated "+str(self.get_children()[1]))
	activationSignal.emit(true)


func deactivate():
	if contained_level_object.has_method("deactivate"):
		contained_level_object.deactivate()
	activationSignal.emit(false)

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
