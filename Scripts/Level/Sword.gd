extends ItemEntity


var default_offset
var default_rot
@onready var timer = $Timer

func _ready():
	super._ready()
	default_offset = offset
	default_rot = hold_rot

func OnAttach(new_owner):
	super.OnAttach(new_owner)

# eventuell bei boden kontakt oder so eigenen on_entity_melee_strike triggern
func Use():
	super.Use()
	itemOwner.on_entity_melee_strike.emit(15)
	hold_rot = Vector3(0,0,0)
	offset = Vector3(0,0,0)
	timer.start(0.35)
	

func SwingFinished():
	offset = default_offset
	hold_rot = default_rot
	

func StopUse():
	super.StopUse()
	
