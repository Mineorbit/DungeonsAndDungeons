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
		#print(event)
		if event is InputEventMouseButton:
			pass
			var mouse_pos = get_viewport().get_mouse_position()
			var ray_length = 100
			var from = camera.project_ray_origin(mouse_pos)
			var to = from + camera.project_ray_normal(mouse_pos) * ray_length
			var space = get_world_3d().direct_space_state
			var ray_query = PhysicsRayQueryParameters3D.new()
			ray_query.from = from
			ray_query.to = to
			ray_query.collide_with_areas = true
			var raycast_result = space.intersect_ray(ray_query)
			#print(raycast_result)
			var pos = raycast_result.position
			pos.z = 0
			pos = selectionArea.to_local(pos)
			pos.z = 0
			pos.x *= selectionArea.scale.x
			pos.y *= selectionArea.scale.y
			print(pos)
			event.position.x = event.position.x - get_viewport().size.x*0.25
		subViewport.push_input(event, false)
