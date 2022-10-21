extends Node2D


# Declare member variables here. Examples:
# var a = 2
# var b = "text"
func start_play(ip):
	Constants.remoteAddress = ip
	print("'"+str(Constants.remoteAddress)+"'")
	Bootstrap.start_play()





func start_edit(name = ""):
	Bootstrap.start_edit(name)
