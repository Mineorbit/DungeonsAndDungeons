extends ColorRect


@onready var levellistelement: Label = $Name
@onready var button: BaseButton = $Button
@onready var selection = $selection
@onready var thumbnailRect: TextureRect = $Thumbnail
# this needs to be a long int
var id:
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


func set_display_size(display_size):
	custom_minimum_size = 256*display_size
	levellistelement.reset_size()


func set_level_data(ldata):
	# this is necessary ater return to lobby because else it is null while the set_level_data method is called
	levellistelement = $Name
	level_name = ldata["name"]
	id = ldata["ulid"]
	if ldata.has("thumbnail"):
		thumbnailRect.texture = ldata["thumbnail"]


func on_selected():
	on_select.emit(id,self)
