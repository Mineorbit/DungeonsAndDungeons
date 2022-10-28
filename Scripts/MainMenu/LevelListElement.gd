extends ColorRect


@onready var levellistelement: RichTextLabel = $Name
@onready var button: BaseButton = $Button
@onready var selection = $selection


var id

func _ready():
	selection.hide()

signal on_select(data)


func set_level_data(ldata):
	print("Set LevelData "+str(ldata))
	levellistelement.text = ldata["name"]
	id = ldata["ulid"]


func on_selected():
	on_select.emit(id,self)
