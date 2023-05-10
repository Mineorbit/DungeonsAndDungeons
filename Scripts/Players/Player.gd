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



var playercontroller


func Hit(damage, hitting_entity,direction = null):
	var modifier = 1
	if direction != null:
		if has_item(1) and item[1] is Shield:
			modifier = item[1].damage_modifier(direction)
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
	if has_item(0):
		item[0].Use()


func StopUseLeft():
	if has_item(0):
		item[0].StopUse()

func UseRight():
	var tried_shot = false
	if has_item(0):
		#check if item in right is bow
		if item[0].has_method("Shoot"):
			tried_shot = true
			item[0].Shoot()
	if has_item(1) and not tried_shot:
		item[1].Use()


func StopUseRight():
	if has_item(1):
		item[1].StopUse()


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
	if has_item(0):
		Dettach(item[0])
	if has_item(1):
		Dettach(item[1])


func start():
	super.start()
	Signals.playerHealthChanged.emit(id,health)


func get_type():
	return "Player"


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
func Attach(item_to_attach):
	super.Attach(item_to_attach)
	item[item_to_attach.attachment] = item_to_attach


@rpc("any_peer", "call_local")
func Dettach(item_to_dettach):
	print("Dettaching "+str(self))
	super.Dettach(item_to_dettach)
	item[item_to_dettach.attachment] = null

