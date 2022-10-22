extends Control

@onready var subMenu: ScrollContainer = $AspectRatioContainer2/SubMenu
@onready var nameText: TextEdit = $AspectRatioContainer2/SubMenu/GridContainer/ColorRect/MarginContainer/TextEdit
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


@export var selectedSubmenu = 0

func _process(delta):
	subMenu.scroll_horizontal = selectedSubmenu*(628) * 0.2 + subMenu.scroll_horizontal * 0.8
	#subMenu.scroll_horizontal = selectedSubmenu*(628)

func _on_new_level():
	selectedSubmenu = 1

func start_new_level():
	get_parent().get_parent().get_parent().start_edit()
	Constants.currentLevel.name = nameText.text


func _on_edit_level():
	selectedSubmenu = 0
