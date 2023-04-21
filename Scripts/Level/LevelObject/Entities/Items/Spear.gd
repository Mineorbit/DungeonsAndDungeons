extends ItemEntity


var default_offset
var default_rot
@onready var timer = $Timer

@onready var hitbox:Area3D = $Hitbox

func _ready():
	super._ready()
	hitbox.body_entered.connect(TryDamage)
	default_offset = offset
	default_rot = hold_rot

func TryDamage(body):
	print(body)
	if body != self and body != lastItemOwner and body.has_method("Hit") and in_throw:
		body.Hit(35,self)
		# immediately prevent all collisions
		# remove arrow if hit
		#queue_free()



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
var started_throw = false

var throw_speed = 24


func Throw():
	in_throw = true

func _physics_process(delta):
	if itemOwner == null:
		if in_throw:
			var collision = move_and_collide(-global_transform.basis.z*throw_speed*delta)
			if collision:
				if not ( collision.get_collider() is Entity ):
					print("Arrow was stopped")
					in_throw = false
		
		look_at(global_transform.origin-global_transform.basis.z)
		if not started_throw:
			started_throw = true
			#look_at(global_transform.origin + global_transform.basis.z)
	else:
		super._physics_process(delta)


func _process(delta):
	pass

func SwingFinished():
	offset = default_offset
	hold_rot = default_rot
	

func StopUse():
	super.StopUse()
	
