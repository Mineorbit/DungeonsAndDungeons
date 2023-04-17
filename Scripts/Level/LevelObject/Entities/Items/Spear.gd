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
func Use(type = 0):
	super.Use()
	if type == 0:
		Swing()
	else:
		Throw()


func Swing():
	itemOwner.on_entity_melee_strike.emit(15)
	hold_rot = Vector3(0,0,0)
	offset = Vector3(0,0,0)
	timer.start(Constants.SwordStrikeTime)
	

var in_throw = false


func Throw():
	move_direction = global_transform.basis.y
	in_throw = true

func _physics_process(delta):
	if in_throw:
		move_and_collide(8*move_direction*delta)
	else:
		super._physics_process(delta)

func _process(delta):
	pass


func SwingFinished():
	offset = default_offset
	hold_rot = default_rot
	

func StopUse():
	super.StopUse()
	
