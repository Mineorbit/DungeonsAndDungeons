extends RigidBody3D
class_name ItemEntity

var itemOwner
var lastItemOwner

var itemAttachmentPoint

var isEquipped = false

# which hand? false = left
@export var attachment: int = 0 

var attachmentPoint: BoneAttachment3D

# this later needs to be replaced by itemhandle system
@export var offset = Vector3.ZERO
@export var hold_rot = Vector3.ZERO

var collisionlayer
var collisionmask

var in_use = false

signal on_item_attached(item_owner)
signal on_item_dettached(old_item_owner)
signal on_item_use


func _ready():
	collisionlayer = collision_layer
	collisionmask = collision_mask

func remove():
	queue_free()

func Use():
	if not in_use:
		if itemOwner != null:
			itemOwner.items_in_use = itemOwner.items_in_use + 1
		in_use = true
		self.on_item_use.emit()
	
func StopUse():
	if in_use:
		itemOwner.items_in_use = itemOwner.items_in_use - 1
		in_use = false

func _physics_process(delta):
	UpdateRelativePosition()

func _process(_delta):
	UpdateRelativePosition()

@export var errorDist = 2

func UpdateRelativePosition(_passive = false):
	if itemOwner != null and collision_layer == 0:
		var attachmentpoint = itemAttachmentPoint.global_transform
		var new_transform: Transform3D = attachmentpoint
		#transform.basis = attachmentpoint.global_transform.basis
		new_transform = new_transform.translated_local(offset)
		new_transform = new_transform.rotated_local(Vector3(1, 0, 0), deg_to_rad(hold_rot.x))
		new_transform = new_transform.rotated_local(Vector3(0, 1, 0), deg_to_rad(hold_rot.y))
		new_transform = new_transform.rotated_local(Vector3(0, 0, 1), deg_to_rad(hold_rot.z))
		new_transform.basis.x = new_transform.basis.x.normalized()
		new_transform.basis.y = new_transform.basis.y.normalized()
		new_transform.basis.z = new_transform.basis.z.normalized()
		global_transform = new_transform


# gravity should be set by global constant

func OnAttach(new_owner):
	collision_layer = 0
	collision_mask = 0
	sleeping  = true
	isEquipped = true
	freeze = true
	itemOwner = new_owner
	lastItemOwner = itemOwner
	itemAttachmentPoint = itemOwner.model.get_node(itemOwner.model.attachmentPoints[attachment])
	on_item_attached.emit(itemOwner)

func OnDettach():
	collision_layer = collisionlayer
	collision_mask = collisionmask
	sleeping  = false
	freeze = false
	isEquipped = true
	var olditemOwner = itemOwner
	itemOwner = null
	in_use = false
	#rotation = Vector3.ZERO
	on_item_dettached.emit(olditemOwner)
