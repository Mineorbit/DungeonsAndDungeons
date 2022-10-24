extends Control

@onready var subMenu: ScrollContainer = $AspectRatioContainer2/SubMenu
@onready var nameText: TextEdit = $AspectRatioContainer2/SubMenu/GridContainer/ColorRect/MarginContainer/TextEdit
@onready var level_list = $AspectRatioContainer2/SubMenu/GridContainer/VSplitContainer/LevelList
@onready var start_edit_button: Button = $AspectRatioContainer2/SubMenu/GridContainer/VSplitContainer/AspectRatioContainer3/Edit
# Called when the node enters the scene tree for the first time.
func _ready():
	var local_levels = DirAccess.open("user://level/").get_directories()
	level_list.set_level_list(local_levels)
	level_list.on_selection.connect(func (x): start_edit_button.modulate = Color.WHITE)



@export var selectedSubmenu = 0

func _process(delta):
	subMenu.scroll_horizontal = selectedSubmenu*(628) * 0.2 + subMenu.scroll_horizontal * 0.8
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

func _on_edit_level():
	selectedSubmenu = 0
