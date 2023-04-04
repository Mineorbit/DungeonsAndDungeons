extends ScrollContainer

@onready var level_list_element_prefab = load("res://Prefabs/MainMenu/LevelList/LevelListElement.tscn")
@onready var grid: GridContainer = $Control/GridContainer
@onready var control: Control = $Control
@export var enabled = true
@export var font_size = 24


var selected_level = null
var selected_level_name = null

signal on_selection(selected)
# Called when the node enters the scene tree for the first time.
func _ready():
	grid.child_entered_tree.connect(update_column_tiling)
	grid.child_exiting_tree.connect(update_column_tiling)

var displaysize = 1


func update_column_tiling(test):
	grid.columns = int(floor(sqrt(grid.get_children().size())))

func set_display_size(custom_size):
	displaysize = custom_size

signal added_element(element)

func set_level_list(level_list):
	clear()
	var num = 0
	for level in level_list:
		var level_list_element: ColorRect = level_list_element_prefab.instantiate()
		# this must be unique per level
		level_list_element.name = str(num)
		grid.add_child(level_list_element)
		level_list_element.on_select.connect(selected)
		level_list_element.set_level_data(level)
		print(str(Constants.id)+" spawning Level List element")
		added_element.emit(level_list_element)
		num = num + 1
	#control.size = grid.size

var previous_selection

func clear():
	for child in grid.get_children():
		child.queue_free()

func selected(identifier,sel):
	if not enabled:
		return
	if previous_selection != null:
		previous_selection.selection.hide()
	selected_level = identifier
	selected_level_name = sel.level_name
	sel.selection.show()
	previous_selection = sel
	on_selection.emit(sel)



func move(dir):
	if is_dragging():
			scroll_horizontal += -dir.x
			scroll_vertical += -dir.y

var dragging = false

func is_dragging():
	return enabled and (dragging)


func _input(event):
	if event is InputEventMouseButton:
		dragging = (event.button_index == 1 and event.pressed)
	if event is InputEventMouseMotion:
		move(event.relative)
