extends Node3D


var constructionprefab
var constructconnection
var connections = {}

# Called when the node enters the scene tree for the first time.
func _ready():
	Signals.connection_added.connect(new_connections)
	Signals.connection_removed.connect(remove_connections)
	Signals.level_loaded.connect(reset_connections)
	constructionprefab = load("res://Prefabs/Connection.tscn")
	constructconnection = constructionprefab.instantiate()

# build connection visualizations rom scratch
func reset_connections():
	clear_all_connections()
	for interactiveobject in Constants.World.level.get_interactive_objects():
		new_connections(interactiveobject.unique_instance_id,interactiveobject.connectedObjects)

func clear_all_connections():
	connections.clear()
	for x in get_children():
		remove_child(x)

func new_connections(b,list):
	for a in list:
		var x = constructionprefab.instantiate() 
		x.ready.connect(func():
			visualize_connection(x,a,b))
		add_child(x)
		
		


func visualize_connection(x,a,b):
		if (not Constants.World.level.interactiveLevelObjects.has(a)) or (not Constants.World.level.interactiveLevelObjects.has(b)):
			return
		var obja = Constants.World.level.interactiveLevelObjects[a]
		var objb = Constants.World.level.interactiveLevelObjects[b]
		x.point_a.global_transform.origin = obja.global_transform.origin + Vector3(0.5,0.5,0.5)
		x.point_b.global_transform.origin = objb.global_transform.origin + Vector3(0.5,0.5,0.5)
		if not connections.has(a):
			connections[a] = {}
		(connections[a])[b] = x

func remove_connections(a,list):
	for b in list:
		if connections.has(a) and connections[a].has(b):
			remove_child((connections[a])[b])
			(connections[a])[b].queue_free()
