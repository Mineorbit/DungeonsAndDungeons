extends RigidBody3D


@onready var hitbox:Area3D = $Hitbox
# Called when the node enters the scene tree for the first time.
func _ready():
	hitbox.body_entered.connect(TryStrike)
	

func TryStrike(body):
	print("Test "+str(body))
	if body.has_method("Hit"):
		print("Strike")
		freeze = true
		linear_velocity = Vector3.ZERO
		collision_mask = 0
		collision_layer = 0
		hitbox.collision_layer = 0
		hitbox.collision_mask = 0
		body.Hit(15,self)
		# immediately prevent all collisions
		# remove arrow if hit
		#queue_free()
		despawn()

var can_reflect = true

var max_bounces = 4
var bounces = 0

func _physics_process(delta):
	var result = move_and_collide(Vector3.ZERO)
	if result != null and bounces < max_bounces:
		bounces += 1
		var reflect = result.get_remainder().bounce(result.get_normal())
		linear_velocity = linear_velocity.bounce(result.get_normal())
		move_and_collide(reflect*0.5)
		look_at(position + linear_velocity)
	
func despawn():
	queue_free()

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
