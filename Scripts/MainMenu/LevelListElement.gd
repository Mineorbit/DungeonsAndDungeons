extends ColorRect


@onready var levellistelement: RichTextLabel = $Name
@onready var button: BaseButton = $Button
@onready var selection = $selection
var data

func _ready():
	selection.hide()

signal on_select(data)


func set_level_data(level_data):
	data = level_data
	levellistelement.text = level_data["name"]


func on_selected():
	on_select.emit(data["id"],self)
