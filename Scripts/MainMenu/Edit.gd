extends Control

@onready var nameText: TextEdit = $TabContainer/NewLevel/VBoxContainer/MarginContainer/LevelName
@onready var level_list = $TabContainer/EditLevel/LevelList
@onready var start_edit_button: Button = $TabContainer/EditLevel/CenterContainer/HBoxContainer/Edit
@onready var delete_button: Button = $TabContainer/EditLevel/CenterContainer/HBoxContainer/Delete

# Called when the node enters the scene tree for the first time.
func _ready():
	var levels = load_level_list()
	level_list.set_level_list(levels)
	level_list.on_selection.connect(func (x): 
		pass
		start_edit_button.modulate = Color.WHITE
		delete_button.modulate = Color.WHITE
		)
	if levels.size() == 0:
		pass
		#open new level tab immediately


func load_level_list():
	var local_levels = DirAccess.open("user://level/localLevels/").get_directories()
	var levels = []
	for l in local_levels:
		levels.append({"name":l,"ulid":l})
	return levels



func start_new_level():
	get_parent().get_parent().get_parent().start_edit()
	Constants.World.level.level_name = nameText.text
	Constants.World.level.save()

func start_edit_level():
	if level_list.selected_level == null:
		return
	print("Starting to edit Level: "+str(level_list.selected_level))
	get_parent().get_parent().get_parent().start_edit(level_list.selected_level)

func _on_delete_level():
	for file in DirAccess.open("user://level/localLevels/"+str(level_list.selected_level)).get_files():
			DirAccess.remove_absolute("user://level/localLevels/"+str(level_list.selected_level)+"/"+str(file))
	for file in DirAccess.open("user://level/localLevels/"+str(level_list.selected_level)+"/chunks").get_files():
			DirAccess.remove_absolute("user://level/localLevels/"+str(level_list.selected_level)+"/chunks/"+str(file))
	
	DirAccess.remove_absolute("user://level/localLevels/"+str(level_list.selected_level)+"/chunks")
	DirAccess.remove_absolute("user://level/localLevels/"+str(level_list.selected_level))
	level_list.set_level_list(load_level_list())
