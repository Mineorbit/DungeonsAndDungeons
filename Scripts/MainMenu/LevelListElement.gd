extends ColorRect


@onready var levellistelement: Label = $Name
@onready var button: BaseButton = $Button
@onready var selection = $selection
@onready var thumbnailRect: TextureRect = $Thumbnail
@onready var description: Label = $Description
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
	set_display_size()

signal on_select(data)


func set_display_size():
	var ds = get_parent().get_parent().get_parent().displaysize
	var s = Vector2(ds,ds)
	custom_minimum_size = 256*s
	var labelstyle = LabelSettings.new()
	labelstyle.font_size = s.x*18
	
	description.label_settings = labelstyle
	levellistelement.label_settings = labelstyle
	levellistelement.reset_size()


func set_level_data(ldata):
	# this is necessary ater return to lobby because else it is null while the set_level_data method is called
	levellistelement = $Name
	description = $Description
	level_name = ldata["name"]
	id = ldata["ulid"]
	if ldata.has("thumbnail"):
		thumbnailRect.texture = ldata["thumbnail"]
	if ldata.has("description"):
		description.text = ldata["description"]

func on_selected():
	on_select.emit(id,self)
