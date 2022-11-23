extends Node


# Called when the node enters the scene tree for the first time.
func _ready():
	get_parent().added_element.connect(new_list_element)
	get_parent().get_node("Control/GridContainer").child_entered_tree.connect(new_list_element)

var ownerid = 0

func new_list_element(element):
	print("Changing Element "+str(element))
	element.get_node("MultiplayerSynchronizer").set_multiplayer_authority(ownerid)

func set_auth(id:int):
	print("["+str(Constants.id)+"] Setting owner of LevelList to "+str(id))
	for child in get_children():
		child.set_multiplayer_authority(id)
	ownerid = id
	for element in get_parent().get_node("Control/GridContainer").get_children():
		print("Changing Element "+str(element))
		element.get_node("MultiplayerSynchronizer").set_multiplayer_authority(ownerid)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
