extends ColorRect


@onready var levellistelement: RichTextLabel = $Name
@onready var button: BaseButton = $Button
@onready var selection = $selection

# this needs to be a long int
var id: int:
	get:
		return id
	set(value):
		id = value
var level_name: String:
	get:
		return level_name
	set(value):
		levellistelement.text = value
		level_name = value

func _ready():
	selection.hide()

signal on_select(data)


func set_level_data(ldata):
	# this is necessary ater return to lobby because else it is null while the set_level_data method is called
	levellistelement = $Name
	print("Set LevelData "+str(ldata))
	level_name = ldata["name"]
	id = ldata["ulid"]


func on_selected():
	on_select.emit(id,self)
