extends Entity
class_name ItemEntity

var itemOwner

var isEquipped = false

# which hand? false = left
@export var hand = false


# this later needs to be replaced by itemhandle system
@export var offset = Vector3.ZERO
@export var hold_rot = Vector3.ZERO


var collisionlayer
var collisionmask

var in_use = false


func _ready():
	super._ready()
	collisionlayer = collision_layer
	collisionmask = collision_mask



func Use():
	in_use = true
	
func StopUse():
	in_use = false

func _physics_process(delta):
	super._physics_process(delta)
	UpdateRelativePosition()

func _process(delta):
	UpdateRelativePosition()

@export var errorDist = 2

func UpdateRelativePosition(passive = false):
	if itemOwner != null and collision_layer == 0:
		var new_position = itemOwner.to_global(offset)
		transform.basis = Basis(Vector3.UP,hold_rot.x) * itemOwner.global_transform.basis
		transform.origin =	new_position

# gravity should be set by global constant

func OnAttach(new_owner):
	collision_layer = 0
	collision_mask = 0
	_velocity = Vector3.ZERO
	started = false
	gravity = 0
	isEquipped = true
	itemOwner = new_owner

func OnDettach():
	collision_layer = collisionlayer
	collision_mask = collisionmask
	_velocity = itemOwner._velocity
	started = true
	gravity = 25
	isEquipped = true
	itemOwner = null
