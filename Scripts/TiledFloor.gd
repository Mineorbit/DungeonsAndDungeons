extends MeshInstance3D


var levelObjectId

var t
var generating = false
# regenerate mesh at position where stuff changed / in worst case the location can be ignored
func generate():
	print(global_transform.origin)
	#later on should only regenerate those, whose neighbor changed
	for x in get_children():
		x.generate()

var generate_tasks = []

func queue_generate(pos):
	generate_tasks.append(pos)

func start_generate(pos):
	generating = true
	print("Starting Generation")
	t = Thread.new()
	t.start( func():
		generate()
		generating = false
	)
	

func _physics_process(delta):
	if generate_tasks.size() > 0:
		print(generate_tasks)
		if not generating:
			var pos = generate_tasks[0]
			generate_tasks.remove_at(0)
			start_generate(pos)

func _exit_tree():
	if t != null:
		t.wait_to_finish()
