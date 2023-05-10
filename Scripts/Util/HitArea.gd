extends ShapeCast3D

var damage = 0
var striking_object = null

var active = false
var resetTimer: Timer
func _ready():
	resetTimer = Timer.new()
	add_child(resetTimer)
	resetTimer.timeout.connect(StopStrike)

var struck = []

func StopStrike():
	active = false

func Strike(dmg,strike_time,striking_obj = null):
	struck = []
	damage = dmg
	striking_object = striking_obj
	active = true
	resetTimer.start(strike_time)

func _physics_process(delta):
	if active:
		if is_colliding():
			var collided_object = get_collider(0)
		# check if collided has method hit/ is entity
			if collided_object.has_method("Hit"):
				if not collided_object in struck:
					struck.append(collided_object)
					var direction = collided_object.global_position - striking_object.global_position
					collided_object.Hit(damage,striking_object,direction)
