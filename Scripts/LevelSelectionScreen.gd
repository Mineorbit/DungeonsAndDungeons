extends MeshInstance3D

@onready var camera: Camera3D = $Camera3D
@onready var enterArea: Area3D = $Area3D
@onready var levellist = $SubViewport/LevelList
@onready var subViewport: Viewport = $SubViewport
@onready var selectionArea: Area3D = $SelectionArea
# Called when the node enters the scene tree for the first time.
func _ready():
	enterArea.body_entered.connect(player_entered)
	enterArea.body_exited.connect(player_left)

#change to local player inside
var players_inside = 0

func player_entered(player):
	print(str(Constants.id)+"Entered capture area "+str(player.get_parent()))
	camera.current = true
	Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
	players_inside = players_inside + 1
func player_left(player):
	print(str(Constants.id)+"Entered capture area "+str(player.get_parent()))
	camera.current = false
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	players_inside = players_inside - 1

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	#needed for dragging
	levellist.dragging = Input.is_action_pressed("Drag")

func _input(event):
	if players_inside > 0:
		if event is InputEventMouseButton or event is InputEventMouseMotion:
			var relative_pos = Vector2(event.position.x/get_viewport().size.x,event.position.y/get_viewport().size.y)
			#print(relative_pos)
			var from = camera.project_ray_origin(event.position)
			var to = from + camera.project_ray_normal(event.position) * 100
			var raydir = PhysicsRayQueryParameters3D.new()
			raydir.from = from
			raydir.to = to
			raydir.collide_with_areas = true
			raydir.collide_with_bodies = false
			raydir.collision_mask = selectionArea.collision_mask
			
			var result = get_world_3d().direct_space_state.intersect_ray(raydir)
			if result.size() > 0:
				var pos = selectionArea.to_local(result.position)
				var rel_pos = Vector2(pos.x/0.9, pos.y/1.7)
				rel_pos = (rel_pos + Vector2(1,1))/2
				rel_pos.x = clamp(1-rel_pos.x,0,1)
				rel_pos.y = clamp(1-rel_pos.y,0,1)
				
				
				event.position.x = rel_pos.x * subViewport.size.x
				event.position.y = rel_pos.y * subViewport.size.y 
				#print(event.position)
				if event is InputEventMouseButton:
					rpc("button_event",JSON.stringify(event))
				else:
					rpc("motion_event",JSON.stringify(event))


@rpc(any_peer)
func button_event(data):
	var event = InputEventMouseButton.new()
	subViewport.push_input(event, false)


@rpc(any_peer)
func motion_event(data):
	var event = InputEventMouseMotion.new()
	subViewport.push_input(event, false)

