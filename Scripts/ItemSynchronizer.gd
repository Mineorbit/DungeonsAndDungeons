extends MultiplayerSynchronizer


var default_scene_rep_config

func _ready():
	default_scene_rep_config = replication_config
	get_parent().on_item_attached.connect(ItemAttach)
	get_parent().on_item_dettached.connect(ItemDettach)
	
func ItemAttach(ownerentity):
	replication_config = SceneReplicationConfig.new()
	rpc("ItemAttachRemote",ownerentity.get_path())

func ItemDettach(ownerentity):
	replication_config = default_scene_rep_config
	rpc("ItemDettachRemote",ownerentity.get_path())


@rpc 
func ItemAttachRemote(ownerentity_path):
	get_node(ownerentity_path).Attach(get_parent())
	replication_config = SceneReplicationConfig.new()
	# need to reenable processing so that attachment position can be updated
	get_parent().set_physics_process(true)
	get_parent().set_process(true)

@rpc 
func ItemDettachRemote(old_ownerentity_path):
	print("Dettaching on "+str(old_ownerentity_path))
	get_node(old_ownerentity_path).Dettach(get_parent())
	replication_config = default_scene_rep_config
	get_parent().set_physics_process(false)
	get_parent().set_process(false)
