extends Node3D


var constructionprefab
var constructconnection
var connections = {}

# Called when the node enters the scene tree for the first time.
func _ready():
	constructionprefab = load("res://Prefabs/Connection.tscn")
	constructconnection = constructionprefab.instantiate()
	Constants.connection_added.connect(new_connections)
	Constants.connection_removed.connect(remove_connections)

func new_connections(b,list):
	for a in list:
		
		print("Adding "+str(a)+" -> " +str(b))
		var x = constructionprefab.instantiate() 
		add_child(x)
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
		remove_child((connections[a])[b])
		(connections[a])[b].queue_free()
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
