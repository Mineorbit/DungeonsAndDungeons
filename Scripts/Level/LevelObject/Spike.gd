extends Node3D


@onready var spikeModel: Node3D = $SpikeModel
@onready var hitArea: Area3D = $HitArea

@export var is_open = false:
	set(val):
		if val:
			if spikeModel != null:
				pass
				#spikeModel.switch()
		else:
			if spikeModel != null:
				pass
				#spikeModel.switchback()
		is_open = val

# Called when the node enters the scene tree for the first time.
func _ready():
	close()
	hitArea.body_entered.connect(trystrike)

func trystrike(object):
	object.Hit(25,self)

@rpc
func activate():
	open()
	
@rpc
func deactivate():
	close()

func open():
	is_open = true
	

func close():
	is_open = false
