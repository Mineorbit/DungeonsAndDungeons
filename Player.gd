extends Entity


func _ready():
	Constants.players = [self]
	_velocity = Vector3.ZERO
	_spring_arm= $SpringArm
	_camera_anchor = $CameraAnchor
	_character = $PlayerModel
	

func _physics_process(delta: float) -> void:
	var move_direction := Vector3.ZERO
	move_direction.x = Input.get_action_strength("right") - Input.get_action_strength("left")
	move_direction.z = Input.get_action_strength("back") - Input.get_action_strength("forward")
	move_direction = move_direction.rotated(Vector3.UP, _spring_arm.rotation.y).normalized()
	if not stunned:
		_velocity.x = move_direction.x * speed
		_velocity.z = move_direction.z * speed
	
	if not is_on_floor():
		_velocity.y -= gravity * delta
	var just_landed := is_on_floor() and _snap_vector == Vector3.ZERO
	
	var is_jumping := false
	if not stunned:
		is_jumping = is_on_floor() and Input.is_action_just_pressed("jump")
	if is_jumping:
		_velocity.y = jump_strength
		_snap_vector = Vector3.ZERO
	elif just_landed:
		_snap_vector = Vector3.DOWN
		if stun_done:
			_velocity = Vector3.ZERO
			stunned = false
			stun_done = false
	
	if fresh_kickback:
		fresh_kickback = false
		_snap_vector = Vector3.ZERO
		_velocity = kickback_direction
	velocity = _velocity
	move_and_slide()
	
	if not stunned and velocity.length() > 0.2 and move_direction.length() > 0.4:
		look_direction = Vector2(velocity.x,_velocity.z)
		look_direction = look_direction.normalized()
		var current_rot = Quaternion(transform.basis)
		var target_rot = Quaternion(Vector3(0,1,0), -look_direction.angle())
		var smoothrot = current_rot.slerp(target_rot, turnAngle)
		transform.basis = Basis(smoothrot)


func kickback(direction) -> void:
	stunned = true
	stun_done = false
	fresh_kickback = true
	kickback_direction = direction*speed
	#await get_tree().create_timer(kickbackTime),"timeout"
	stun_done = true
	# starts kickback process:
	# character moves backwards in ballistic arch until

func _process(_delta: float) -> void:
	if Input.is_action_just_pressed("test_kickback"):
		kickback(Vector3.UP+Vector3.BACK)
		#kickback(Vector3.UP)
	_spring_arm.transform.origin = transform.origin + Vector3.UP*0.75
