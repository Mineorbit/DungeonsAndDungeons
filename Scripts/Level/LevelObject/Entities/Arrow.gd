extends RigidBody3D


@onready var hitbox:Area3D = $Hitbox
# Called when the node enters the scene tree for the first time.
func _ready():
	hitbox.body_entered.connect(TryStrike)
	

func TryStrike(body):
	if body.has_method("Hit"):
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

var max_bounces = 16
var bounces = 0

func _physics_process(delta):
	var result = move_and_collide(-basis.z*delta,true,0.001,false,4)
	if result != null and bounces < max_bounces:
		bounces += 1
		print(get_contact_count())
		var normal = result.get_normal()
		print(normal)
		var reflect = result.get_remainder().bounce(normal)
		linear_velocity = linear_velocity.bounce(normal)
		move_and_collide(reflect*0.5)
		look_at(position + linear_velocity)



func despawn():
	queue_free()

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
