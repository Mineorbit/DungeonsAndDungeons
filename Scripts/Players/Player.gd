extends Entity
class_name Player
#need to copy health value to variable here so it can be synchronized
@export var health_ui: int


var id: int = 0:
	get:
		return id
	set(value):
		id = value
		setColor()

var itemLeft
var itemRight
var playercontroller

@onready var camera_offset = $CameraAnchor/CameraOffset

func Hit(damage, hitting_entity,direction = null):
	var modifier = 1
	if direction != null:
		if itemRight is Shield:
			modifier = itemRight.damage_modifier(direction)
	if modifier != 0:
		super.Hit(modifier*damage,hitting_entity,direction)
		Signals.playerHealthChanged.emit(id,health)
	if modifier < 1:
		hitting_entity.Stun(1,self,-direction)


# changing colors
func setColor():
	var mesh = $Model/root/Skeleton3D/Body
	var material = mesh.get_active_material(0)
	var c = Constants.player_colors[id]
	material.albedo_color = c.lightened(0.6)
	material = material.duplicate()
	mesh.set_surface_override_material(0, material)
	
	
	mesh = $Model/root/Skeleton3D/Body
	material = mesh.get_active_material(1)
	material.albedo_color = Constants.alt_player_colors[id]
	material = material.duplicate()
	mesh.set_surface_override_material(1, material)
	
	
	
	mesh = $Model/root/Skeleton3D/Coat
	material = mesh.get_active_material(0)
	material.albedo_color = Constants.player_colors[id]
	material = material.duplicate()
	mesh.set_surface_override_material(0, material)
	
	mesh = $Model/root/Skeleton3D/HeadBand
	material = mesh.get_active_material(0)
	material.albedo_color = Constants.player_colors[id].inverted()
	material = material.duplicate()
	mesh.set_surface_override_material(0, material)
	
	mesh = $Model/root/Skeleton3D/Belt
	material = mesh.get_active_material(1)
	material.albedo_color = Constants.player_colors[id].inverted()
	material = material.duplicate()
	mesh.set_surface_override_material(1, material)

func UseLeft():
	if itemLeft != null:
		itemLeft.Use()


func StopUseLeft():
	if itemLeft != null:
		itemLeft.StopUse()

func UseRight():
	var tried_shot = false
	if itemLeft != null:
		#check if item in right is bow
		if itemLeft.has_method("Shoot"):
			tried_shot = true
			itemLeft.Shoot()
	if itemRight != null and not tried_shot:
		itemRight.Use()


func StopUseRight():
	if itemRight != null:
		itemRight.StopUse()


func _ready():
	super._ready()
	on_entity_despawn.connect(DettachAllItems)
	on_entity_remove.connect(DettachAllItems)
	on_entity_aiming.connect(ChangeMovementState)
	on_entity_landed.connect(LandedOnGround)
	on_entity_health_changed.connect(
		func(health):
			if Constants.player_hud != null:
				Constants.player_hud.updateHealth(health,self.id)
	)

func LandedOnGround(v):
	if input_blocked:
		_velocity.x = 0
		_velocity.z = 0


func ChangeMovementState(aiming):
	input_blocked = aiming
	if aiming and is_on_floor():
		_velocity.x = 0
		_velocity.z = 0

func DettachAllItems():
	if itemLeft != null:
		Dettach(itemLeft)
	if itemRight != null:
		Dettach(itemRight)


func start():
	super.start()
	Signals.playerHealthChanged.emit(id,health)



func _process(delta):
	if playercontroller != null:
		super._process(delta)
		
func _physics_process(delta):
	health_ui = health
	if playercontroller != null:
		move_direction.x = playercontroller.input_direction.x
		move_direction.z = playercontroller.input_direction.z
	super._physics_process(delta)



@rpc("any_peer", "call_local")
func Attach(item):
	super.Attach(item)
	if item.hand:
		itemRight = item
		item.itemAttachmentPoint = model.right_hand()
	else:
		itemLeft = item
		item.itemAttachmentPoint = model.left_hand()


@rpc("any_peer", "call_local")
func Dettach(item):
	print("Dettaching "+str(self))
	super.Dettach(item)
	if item.hand:
		itemRight = null
	else:
		itemLeft = null

