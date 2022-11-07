extends RigidBody3D


@onready var meleeArea: ShapeCast3D = $MeleeHitArea
@onready var hitbox:Area3D = $Hitbox
# Called when the node enters the scene tree for the first time.
func _ready():
	body_entered.connect(Ricochet)
	hitbox.body_entered.connect(TryStrike)
	
func Ricochet(body):
	print("Need to ricochet")

func TryStrike(body):
	if body.has_method("Hit"):
		# currently the striking object is self, could later be itemOwner
		meleeArea.Strike(15,self)
		# immediately prevent all collisions
		collision_mask = 0
		collision_layer = 0
		meleeArea.collision_mask = 0
		hitbox.collision_layer = 0
		hitbox.collision_mask = 0
		# remove arrow if hit
		queue_free()


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
