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
	in_throw = false
	super.OnAttach(new_owner)

func OnDettach():
	stuck_in_wall = false
	super.OnDettach()

# eventuell bei boden kontakt oder so eigenen on_entity_melee_strike triggern
func Use(type = 0,target_pos = null):
	super.Use()
	if type == 0:
		Swing()
	else:
		Throw(target_pos)


func Swing():
	in_throw = false
	itemOwner.on_entity_melee_strike.emit(15)
	timer.start(Constants.SwordStrikeTime)
	

var in_throw = false
var started_throw = false

var throw_speed = 24

var stuck_in_wall = false

func Throw(target_position):
	in_throw = true
	stuck_in_wall = false
	itemOwner.Dettach(self)
	var aim_position = target_position
	aim_position.y = self.global_transform.origin.y
	#self.look_at(aim_position)
	#apply_impulse(-global_transform.basis.z)
	var impulse = (aim_position - global_transform.origin).normalized()
	#collisionlayer = 0
	#collisionmask = 0
	apply_impulse(impulse*24)


# this should be done cleaner
func _physics_process(delta):
	#if in_throw:
	#	gravity_scale = 0
	#else:
	#	gravity_scale = 1
	if itemOwner == null:
		if in_throw:
			var collision = get_colliding_bodies ()
			print(collision)
			if collision:
				if not ( collision[0] is Entity ):
					print("Arrow was stopped")
					in_throw = false
					stuck_in_wall = true
		
			if not started_throw:
				started_throw = true
			#look_at(global_transform.origin + global_transform.basis.z)
	else:
		super._physics_process(delta)


func _process(delta):
	pass

func SwingFinished():
	pass


func StopUse():
	super.StopUse()
