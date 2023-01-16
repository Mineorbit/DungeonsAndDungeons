extends Entity
class_name ItemEntity

var itemOwner
var itemAttachmentPoint

var isEquipped = false

# which hand? false = left
@export var hand = false


# this later needs to be replaced by itemhandle system
@export var offset = Vector3.ZERO
@export var hold_rot = Vector3.ZERO

var collisionlayer
var collisionmask

var in_use = false

signal on_item_attached(item_owner)
signal on_item_dettached(old_item_owner)


func _ready():
	collisionlayer = collision_layer
	collisionmask = collision_mask



func Use():
	in_use = true
	
func StopUse():
	in_use = false

func _physics_process(delta):
	super._physics_process(delta)
	UpdateRelativePosition()

func _process(_delta):
	UpdateRelativePosition()

@export var errorDist = 2

func UpdateRelativePosition(_passive = false):
	if itemOwner != null and collision_layer == 0:
		var new_position = itemAttachmentPoint.to_global(offset)
		transform.basis = itemAttachmentPoint.global_transform.basis
		rotate_object_local(Vector3(1, 0, 0), deg_to_rad(hold_rot.x))
		rotate_object_local(Vector3(0, 1, 0), deg_to_rad(hold_rot.y))
		rotate_object_local(Vector3(0, 0, 1), deg_to_rad(hold_rot.z))
		transform.origin = new_position

# gravity should be set by global constant

func OnAttach(new_owner):
	collision_layer = 0
	collision_mask = 0
	_velocity = Vector3.ZERO
	started = false
	gravity = 0
	isEquipped = true
	itemOwner = new_owner
	on_item_attached.emit(itemOwner)

func OnDettach():
	collision_layer = collisionlayer
	collision_mask = collisionmask
	_velocity = itemOwner._velocity
	started = true
	gravity = 25
	isEquipped = true
	var olditemOwner = itemOwner
	itemOwner = null
	in_use = false
	rotation = Vector3.ZERO
	on_item_dettached.emit(olditemOwner)
