extends Node3D

@onready var LevelSelectionScreen = $LevelSelectionScreen
@onready var lobbyArea = $LobbyArea


func _ready():
	lobbyArea.body_entered.connect(updateOwner)
	lobbyArea.body_exited.connect(updateOwner)


func updateOwner(test):
	pass
	#LevelSelectionScreen.update_interface_owner()
