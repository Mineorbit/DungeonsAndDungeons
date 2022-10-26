extends MeshInstance3D

@onready var camera: Camera3D = $Camera3D
@onready var enterArea: Area3D = $EnterArea
@onready var levellist = $SubViewport/LevelList
@onready var subViewport: Viewport = $SubViewport
@onready var selectionArea: Area3D = $SelectionArea
@onready var cursors: Node2D = $SubViewport/Cursors
# Called when the node enters the scene tree for the first time.
func _ready():
	enterArea.body_entered.connect(player_entered)
	enterArea.body_exited.connect(player_left)

#change to local player inside

var localcursor = null
var local_player_inside = false
func player_entered(player):
	if Constants.playerCamera.player == player:
		camera.current = true
		Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
		local_player_inside = true
	if 1 == Constants.id:	
		camera.current = true
		var cursorprefab = load("res://Prefabs/MainMenu/LevelList/LevelListCursor.tscn")
		var cursor = cursorprefab.instantiate()
		localcursor = cursor
		cursor.name = str(player.playercontroller.name)
		cursors.add_child(cursor)

func player_left(player):
	if Constants.playerCamera.player == player:
		camera.current = false
		Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
		local_player_inside = false
	if 1 == Constants.id:
		camera.current = false
		localcursor = null
		local_player_inside = false




func _input(event):
	
	print(str(Constants.id)+" "+str(event))
	if Constants.id == 1:
		subViewport.push_input(event, true)
	if local_player_inside:
		if event is InputEventMouseButton or event is InputEventMouseMotion:
			var relative_pos = Vector2(event.position.x/get_viewport().size.x,event.position.y/get_viewport().size.y)
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
				subViewport.push_input(event, false)



