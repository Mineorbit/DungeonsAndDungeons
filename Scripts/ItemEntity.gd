extends Entity
class_name ItemEntity

var itemOwner

var isEquipped = false


# this later needs to be replaced by itemhandle system
@export var offset = Vector3.ZERO

var collisionlayer
var collisionmask

func _ready():
	super._ready()
	collisionlayer = collision_layer
	collisionmask = collision_mask

func Use():
	pass

func _physics_process(delta):
	super._physics_process(delta)
	print(str(global_transform.origin))
	UpdateRelativePosition()

func _process(delta):
	UpdateRelativePosition()

func UpdateRelativePosition():
	if itemOwner != null and collision_layer == 0:
		transform.basis = itemOwner.global_transform.basis
		#global_transform.origin =	itemOwner.global_transform.origin
		transform.origin =	itemOwner.to_global(offset)
		print(str(itemOwner.global_transform.origin))
		print(str(global_transform.origin)+" "+str(Constants.id))

func OnAttach(new_owner):
	collision_layer = 0
	collision_mask = 0
	_velocity = Vector3.ZERO
	started = false
	isEquipped = true
	itemOwner = new_owner
	motion_mode = 1

func OnDettach():
	collision_layer = collisionlayer
	collision_mask = collisionmask
	_velocity = itemOwner._velocity
	started = true
	isEquipped = true
	itemOwner = null
