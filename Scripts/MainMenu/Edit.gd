extends Control

@onready var tabmenu: TabContainer = $TabContainer
@onready var nameText: LineEdit = $TabContainer/NewLevel/VBoxContainer/LevelName
@onready var level_list = $TabContainer/EditLevel/LevelList
@onready var start_edit_button: Button = $TabContainer/EditLevel/CenterContainer/HBoxContainer/Edit
@onready var delete_button: Button = $TabContainer/EditLevel/CenterContainer/HBoxContainer/Delete
@onready var upload_button: Button = $TabContainer/EditLevel/CenterContainer/HBoxContainer/Upload

@onready var remote_level_list = $TabContainer/DownloadLevel/VBoxContainer/LevelList
@onready var download_button: Button = $TabContainer/DownloadLevel/VBoxContainer/Download

# Called when the node enters the scene tree for the first time.
func _ready():
	tabmenu.set_tab_hidden ( 2,true )
	var levels = load_level_list()
	level_list.set_level_list(levels)
	level_list.on_selection.connect(func (x): 
		start_edit_button.modulate = Color.WHITE
		delete_button.modulate = Color.WHITE
		upload_button.modulate = Color.WHITE
		)
	remote_level_list.on_selection.connect(func (x): 
		download_button.modulate = Color.WHITE
		)
	if levels.size() == 0:
		pass
		#open new level tab immediately
	ApiAccess.levels_fetched.connect(load_remote_level_list)
	ApiAccess.fetch_level_list()

func load_remote_level_list(list):
	remote_level_list.set_level_list(list)


func load_level_list():
	var local_levels = DirAccess.open("user://level/localLevels/").get_directories()
	var levels = []
	for l in local_levels:
		var leveldata = {"name":l,"ulid":l}
		print(l)
		if FileAccess.file_exists("user://level/localLevels/"+l+"/thumbnail.png"):
			print("File exists")
			var image = Image.load_from_file("user://level/localLevels/"+l+"/thumbnail.png")
			print(image.get_size())
			print(image.get_format())
			print("user://level/localLevels/"+l+"/thumbnail.png")
			var imt = ImageTexture.create_from_image(image)
			print("Test: "+str(imt.get_size()))
			Constants.levelThumbnails.append(imt)
			leveldata["thumbnail"] = Constants.levelThumbnails.size() - 1
		levels.append(leveldata)
	return levels

func on_download_level():
	var level = remote_level_list.selected_level
	print(remote_level_list.selected_level)
	ApiAccess.download_level(remote_level_list.selected_level,true)


func start_new_level():
	Bootstrap.start_edit(null)
	Constants.World.level.level_name = nameText.text
	Constants.World.level.save()

func start_edit_level():
	if level_list.selected_level == null:
		return
	print("Starting to edit Level: "+str(level_list.selected_level))
	Bootstrap.start_edit(level_list.selected_level)


func _on_delete_level():
	for file in DirAccess.open("user://level/localLevels/"+str(level_list.selected_level)).get_files():
			DirAccess.remove_absolute("user://level/localLevels/"+str(level_list.selected_level)+"/"+str(file))
	for file in DirAccess.open("user://level/localLevels/"+str(level_list.selected_level)+"/chunks").get_files():
			DirAccess.remove_absolute("user://level/localLevels/"+str(level_list.selected_level)+"/chunks/"+str(file))
	
	DirAccess.remove_absolute("user://level/localLevels/"+str(level_list.selected_level)+"/chunks")
	DirAccess.remove_absolute("user://level/localLevels/"+str(level_list.selected_level))
	level_list.set_level_list(load_level_list())

@onready var uploadlevelname: TextEdit = $TabContainer/UploadLevel/VBoxContainer/LevelName
@onready var uploadLabel: Label = $TabContainer/UploadLevel/VBoxContainer/CenterContainer/Label

func _on_upload_level():
	tabmenu.current_tab = 2
	uploadlevelname.text = str(level_list.selected_level)
	uploadLabel.text = "Upload Level: "+str(level_list.selected_level)
	
func upload_level():
	print("Uploading Level")
	DirAccess.rename_absolute("user://level/localLevels/"+str(level_list.selected_level),"user://level/localLevels/"+uploadlevelname.text)
	ApiAccess.upload_level(uploadlevelname.text,uploadlevelname.text)
	DirAccess.rename_absolute("user://level/localLevels/"+uploadlevelname.text,"user://level/localLevels/"+str(level_list.selected_level))
	
