extends RigidBody3D


@onready var hitbox:Area3D = $Hitbox
# Called when the node enters the scene tree for the first time.
func _ready():
	body_entered.connect(Ricochet)
	hitbox.body_entered.connect(TryStrike)
	
func Ricochet(body):
	print("Need to ricochet")

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

func despawn():
	queue_free()

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
