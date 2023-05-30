extends RigidBody3D


@export var material: LevelObjectData.LevelObjectMaterial = LevelObjectData.LevelObjectMaterial.Default

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
		var normal = result.get_normal()
		if result.get_collider() is GridMap and not is_principal_direction(normal):
			return
		
		var reflect = result.get_remainder().bounce(normal)
		linear_velocity = linear_velocity.bounce(normal)
		move_and_collide(delta*reflect*0.5)
		look_at(position + linear_velocity)

var tolerance = 0.1

func is_principal_direction(vec):
	if((Vector3(1,0,0) - vec).length() < tolerance):
		return true
	if((Vector3(-1,0,0) - vec).length() < tolerance):
		return true
	if((Vector3(0,1,0) - vec).length() < tolerance):
		return true
	if((Vector3(0,-1,0) - vec).length() < tolerance):
		return true
	if((Vector3(0,0,1) - vec).length() < tolerance):
		return true
	if((Vector3(0,0,-1) - vec).length() < tolerance):
		return true
	return false
	


func despawn():
	queue_free()

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
