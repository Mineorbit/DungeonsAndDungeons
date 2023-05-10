extends ItemEntity


var default_offset
var default_rot
@onready var timer = $Timer


var swinging = false
func _ready():
	super._ready()
	default_offset = offset
	default_rot = hold_rot
	self.on_item_use.connect(flash)
	timer.timeout.connect(func():
		StopUse())


func get_type():
	return "Sword"

func OnAttach(new_owner):
	super.OnAttach(new_owner)

# eventuell bei boden kontakt oder so eigenen on_entity_melee_strike triggern
func Use():
	super.Use()
	itemOwner.on_entity_melee_strike.emit(15)
	hold_rot = Vector3(0,0,0)
	offset = Vector3(0,0,0)
	swinging = true
	timer.start(Constants.SwordStrikeTime)
	

func SwingFinished():
	swinging = false
	offset = default_offset
	hold_rot = default_rot
	

func StopUse():
	if not swinging:
		super.StopUse()


@onready var strikeplane: AnimationPlayer = $Model/AnimationPlayer


func flash():
	print("Flashing")
	strikeplane.play("Flash")
