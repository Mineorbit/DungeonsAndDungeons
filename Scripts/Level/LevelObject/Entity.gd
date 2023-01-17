extends CharacterBody3D
class_name Entity

var health: int = 100:
	set(value):
		health = value
		on_entity_health_changed.emit(health)

var max_health := 100
var in_climb_area = false

@export var speed: float = 5
@export var can_climb: bool = false
@export var climb_velocity: float = 1

var turnAngle := 0.2
var kickbackTime = 2
var jump_strength = 10.0
var gravity := 25

var _velocity := Vector3.ZERO
var _snap_vector := Vector3.DOWN


var started = false




var look_direction = Vector2(1,0)
var fresh_kickback = false
var kickback_direction = Vector3.ZERO
var stun_done = false

var should_jump = false

var move_direction := Vector3.ZERO
var is_jumping = false

var target_rot = Quaternion.IDENTITY
var stunned = false
var input_blocked = false
var allowed_to_move = true
var last_floor = false

var can_shoot = false


# The name of the Model of an entity should allways be named Model
@onready var model = $Model


@onready var meleehitarea = $MeleeHitArea

signal on_entity_remove

signal on_entity_despawn

signal on_entity_pickup

signal on_entity_hit

signal on_entity_died

signal on_entity_landed

signal on_entity_melee_strike(damage)

signal on_entity_shoot

signal on_entity_can_shoot(can)

signal on_entity_aiming(aiming)

signal on_entity_jump

signal on_entity_health_changed(health)


func MeleeStrike(damage):
	meleehitarea.Strike(damage,self)

func ent_can_shoot(v):
	can_shoot = v

func _ready():
	tree_exiting.connect(on_remove)
	target_rot = global_transform.basis
	on_entity_can_shoot.connect(ent_can_shoot)
	on_entity_melee_strike.connect(MeleeStrike)
	_velocity = Vector3.ZERO
	
func start():
	target_rot = global_transform.basis
	health = max_health
	started = true
	
func reset():
	target_rot = global_transform.basis
	started = false

func remove():
	print(str(Constants.id)+" triggering remove ")
	get_parent().remove_child(self)
	queue_free()

func on_remove():
	print(str(Constants.id)+" Removed Entity "+str(self))
	on_entity_remove.emit()

var num_of_climb_areas = 0

func entered_climb_area(area):
	num_of_climb_areas = num_of_climb_areas + 1
	in_climb_area = true


func exited_climb_area(area):
	num_of_climb_areas = max(0,num_of_climb_areas - 1)
	if num_of_climb_areas == 0:
		in_climb_area = false

func jump():
	if not input_blocked and not stunned:
		is_jumping = is_on_floor()
		if is_jumping:
			on_entity_jump.emit()

func _process(_delta):
	var current_rot = Quaternion(transform.basis)
	var smoothrot = current_rot.slerp(target_rot, turnAngle)
	transform.basis = Basis(smoothrot)


var climbing = false

func _physics_process(delta: float) -> void:
	if not started:
		return
	if global_transform.origin.y < Constants.deathplane:
		Kill()
	
	if (not input_blocked) and (not stunned):
		if allowed_to_move:
			_velocity.x = move_direction.x * speed
			_velocity.z = move_direction.z * speed
		else:
			_velocity.x = 0
			_velocity.z = 0
	if (not last_floor)  and is_on_floor():
		on_entity_landed.emit(_velocity.y)
	last_floor = is_on_floor()
	if not is_on_floor():
		_velocity.y -= gravity * delta
	climbing = false
	if can_climb and in_climb_area:
		if move_direction.length() > 0.1:
			_velocity.y = climb_velocity
		else:
			_velocity.y = -climb_velocity
		climbing = true
	
	var just_landed := is_on_floor() and _snap_vector == Vector3.ZERO
	if just_landed:
		is_jumping = false
	if is_jumping:
		_velocity.y = jump_strength
		_snap_vector = Vector3.ZERO
		is_jumping = false
	elif just_landed:
		_snap_vector = Vector3.DOWN
		if stun_done:
			_velocity = Vector3.ZERO
			input_blocked = false
			stunned = false
			stun_done = false
	
	if fresh_kickback:
		fresh_kickback = false
		_snap_vector = Vector3.ZERO
		_velocity = kickback_direction
	velocity = _velocity
	move_and_slide()
	
	if not input_blocked and velocity.length() > 0.2 and move_direction.length() > 0.4:
		# TODO: if climbing, the look direction should be towards the ladder
		
		look_direction = Vector2(velocity.x,_velocity.z)
		look_direction = look_direction.normalized()
		
		target_rot = Quaternion(Vector3(0,1,0), - look_direction.angle())
		
		
		var current_rot = Quaternion(transform.basis)
		var smoothrot = current_rot.slerp(target_rot, turnAngle)
		transform.basis = Basis(smoothrot)


func kickback(direction) -> void:
	input_blocked = true
	stunned = true
	stun_done = false
	fresh_kickback = true
	kickback_direction = direction*speed
	#await get_tree().create_timer(kickbackTime),"timeout"
	stun_done = true
	# starts kickback process:
	# character moves backwards in ballistic arch until

# this is the current method later on do tree removal
func Kill():
	remove()
	on_entity_died.emit()


func Hit(damage, hitting_entity,direction = null):
	on_entity_hit.emit()
	print(str(hitting_entity)+" hit "+str(self)+" and did "+str(damage)+" Damage")
	var offset_dir: Vector3 =global_transform.origin - hitting_entity.global_transform.origin
	if direction != null:
		offset_dir = direction
	
	offset_dir = offset_dir.normalized()
	offset_dir.y = 0
	
	kickback((offset_dir + Vector3.UP)*log(damage)*0.5)
	health = health - damage
	if health < 0:
		Kill()


func Attach(item):
	print("Attaching "+str(item))
	item.OnAttach(self)


func Dettach(item):
	if item == null:
		return
	item.OnDettach()