



extends MeshInstance3D


var levelObjectId

var threads = []

var generating = false

@export var surfacematerial: Material

@export_flags_3d_physics var collision


var subgrid = {}
var generating_at = {}
@onready var subgrids = $Subgrids
var previewmesh

func _ready():
	previewmesh = get_node("0")
	if get_parent().get("display") == null:
		previewmesh.hide()
		remove_child(previewmesh)
	for x in subgrids.get_children():
		subgrid[x.name] = x
		generating_at[x.name] = false

# regenerate mesh at position where stuff changed / in worst case the location can be ignored
func generate():
	#later on should only regenerate those, whose neighbor changed
	for x in subgrids.get_children():
		x.generate()

func generate_for(x, force = false):
	if not generating_at[x.name] or force:
		var t = Thread.new()
		generating_at[x.name] = true
		t.start( func():
			x.generate()
			generating_at[x.name] = false
			print("Actual Generate "+str(x))
		)
		
	

var generate_tasks = []

func queue_generate(pos):
	generate_tasks.append(pos)


func get_subgrid(pos):
	var local_pos = pos - global_transform.origin
	var gridname = str(int(local_pos.x > 3))+str(int(local_pos.y > 3))+str(int(local_pos.z > 3))
	#print(gridname)
	#print(str(local_pos)+" "+str(global_transform.origin))
	return subgrid[gridname]

func _physics_process(delta):
	#var set = []
	while generate_tasks.size() > 0:
			var pos = generate_tasks[0]
			var x = get_subgrid(pos)
			if not generating_at[x.name]:
				generate_for(x)
				#set.append(x)
			generate_tasks.remove_at(0)
			#start_generate(pos)

func _exit_tree():
	pass
	#if t != null:
	#	t.wait_to_finish()
