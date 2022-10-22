extends ScrollContainer

@onready var level_list_element_prefab = load("res://Prefabs/MainMenu/LevelList/LevelListElement.tscn")
@onready var grid: GridContainer = $Control/GridContainer
@onready var control: Control = $Control
var selected_level = null

signal on_selection(selected)
# Called when the node enters the scene tree for the first time.
func _ready():
	pass


func set_level_list(level_list):
	grid.columns = int(floor(sqrt(level_list.size())))
	for level_data in level_list:
		var level_list_element = level_list_element_prefab.instantiate()
		grid.add_child(level_list_element)
		level_list_element.set_level_data(level_data)
		level_list_element.on_select.connect(selected)
	#control.size = grid.size

var previous_selection

func selected(data,selected):
	if previous_selection != null:
		previous_selection.selection.hide()
	selected_level = data
	print("TEST "+str(data))
	selected.selection.show()
	previous_selection = selected
	on_selection.emit(selected)
