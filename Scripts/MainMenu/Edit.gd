extends Control

@onready var subMenu: ScrollContainer = $SubMenu
@onready var nameText: TextEdit = $SubMenu/GridContainer/ColorRect/MarginContainer/TextEdit
@onready var level_list = $SubMenu/GridContainer/VSplitContainer/LevelList
@onready var start_edit_button: Button = $SubMenu/GridContainer/VSplitContainer/AspectRatioContainer3/HBoxContainer/Edit
@onready var delete_button: Button = $SubMenu/GridContainer/VSplitContainer/AspectRatioContainer3/HBoxContainer/Delete

# Called when the node enters the scene tree for the first time.
func _ready():
	level_list.set_level_list(load_level_list())
	level_list.on_selection.connect(func (x): 
		start_edit_button.modulate = Color.WHITE
		delete_button.modulate = Color.WHITE
		)


func load_level_list():
	var local_levels = DirAccess.open("user://level/localLevels/").get_directories()
	var levels = []
	for l in local_levels:
		levels.append({"name":l,"ulid":l})
	return levels

@export var selectedSubmenu = 0

func _process(delta):
	#the plus 32 is because of the scroll bar
	subMenu.scroll_horizontal = selectedSubmenu*(size.x+32) * 0.2 + subMenu.scroll_horizontal * 0.8
	#subMenu.scroll_horizontal = selectedSubmenu*(628)

func _on_new_level():
	selectedSubmenu = 1

func start_new_level():
	get_parent().get_parent().get_parent().start_edit()
	Constants.currentLevel.level_name = nameText.text
	Constants.currentLevel.save()

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

func _on_edit_level():
	selectedSubmenu = 0
