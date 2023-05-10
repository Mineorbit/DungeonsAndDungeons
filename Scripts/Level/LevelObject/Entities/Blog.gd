extends Enemy

@onready var vision: ShapeCast3D = $VisionCheck
var time_since_seen = 0
var strike_time = 0


func _ready():
	super._ready()
	follow_target = true


func try_strike():
	if strikeArea.get_overlapping_bodies().size() > 0:
		on_entity_melee_strike.emit(15)


func _physics_process(delta):
	super._physics_process(delta)
	strike_time += delta
	if strike_time > 1:
		try_strike()
		strike_time = 0
	time_since_seen += delta
	if time_since_seen > 5:
		target_entity = null
