extends ScrollContainer

@onready var level_list_element_prefab = load("res://Prefabs/MainMenu/LevelList/LevelListElement.tscn")
@onready var grid: GridContainer = $Control/GridContainer
@onready var control: Control = $Control
var selected_level = null

signal on_selection(selected)
# Called when the node enters the scene tree for the first time.
func _ready():
	grid.child_entered_tree.connect(update_column_tiling)
	grid.child_exiting_tree.connect(update_column_tiling)

var displaysize = 1


func update_column_tiling():
	print("TEST")
	grid.columns = int(floor(sqrt(grid.get_children().size())))

func set_display_size(size):
	displaysize = 2
	scale =Vector2(displaysize,displaysize)

func set_level_list(level_list):
	for child in grid.get_children():
		child.queue_free()
	for level_data in level_list:
		var level_list_element = level_list_element_prefab.instantiate()
		# this must be unique per level
		level_list_element.name = str(level_data)
		grid.add_child(level_list_element)
		level_list_element.set_level_data(level_data)
		level_list_element.on_select.connect(selected)
	#control.size = grid.size

var previous_selection

func selected(data,selected):
	if previous_selection != null:
		previous_selection.selection.hide()
	selected_level = data
	selected.selection.show()
	previous_selection = selected
	on_selection.emit(selected)
