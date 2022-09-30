extends Node3D


var constructionprefab
var constructconnection
var connections = {}

# Called when the node enters the scene tree for the first time.
func _ready():
	constructionprefab = load("res://Prefabs/Connection.tscn")
	constructconnection = constructionprefab.instantiate()
	Signals.connection_added.connect(new_connections)
	Signals.connection_removed.connect(remove_connections)
	Signals.level_loaded.connect(reset_connections)

# build connection visualizations rom scratch
func reset_connections():
	print("Resetting Connections")
	clear_all_connections()
	for interactiveobject in Constants.currentLevel.get_interactive_objects():
		print(interactiveobject.contained_level_object)
		new_connections(interactiveobject.unique_instance_id,interactiveobject.connectedObjects)

func clear_all_connections():
	connections.clear()
	for x in get_children():
		remove_child(x)

func new_connections(b,list):
	for a in list:
		
		print("Adding "+str(a)+" -> " +str(b))
		var x = constructionprefab.instantiate() 
		add_child(x)
		if (not Constants.interactiveLevelObjects.has(a)) or (not Constants.interactiveLevelObjects.has(b)):
			continue
		var obja = Constants.interactiveLevelObjects[a]
		var objb = Constants.interactiveLevelObjects[b]
		
		x.point_a.global_transform.origin = obja.global_transform.origin + Vector3(0.5,0.5,0.5)
		x.point_b.global_transform.origin = objb.global_transform.origin + Vector3(0.5,0.5,0.5)
		if not connections.has(a):
			connections[a] = {}
		(connections[a])[b] = x


func remove_connections(a,list):
	print("Removing: "+str(a)+" -> "+str(list))
	for b in list:
		if connections.has(a) and connections[a].has(b):
			remove_child((connections[a])[b])
			(connections[a])[b].queue_free()
