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
		#spear.global_transform.origin = global_transform.origin + 2*global_transform.basis.x + Vector3(0,0.75,0)
	var aim_position = target_position
	aim_position.y = self.global_transform.origin.y
	self.look_at(aim_position)


# this should be done cleaner
func _physics_process(delta):
	if itemOwner == null:
		if in_throw:
			var collision = move_and_collide(-global_transform.basis.z*throw_speed*delta)
			if collision:
				if not ( collision.get_collider() is Entity ):
					print("Arrow was stopped")
					in_throw = false
					stuck_in_wall = true
		
			look_at(global_transform.origin-global_transform.basis.z)
			if not started_throw:
				started_throw = true
		else:
			if not stuck_in_wall:
				super._physics_process(delta)
			#look_at(global_transform.origin + global_transform.basis.z)
	else:
		super._physics_process(delta)


func _process(delta):
	pass

func SwingFinished():
	pass


func StopUse():
	super.StopUse()
