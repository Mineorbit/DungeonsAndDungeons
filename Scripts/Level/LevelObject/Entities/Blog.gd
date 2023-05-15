extends Enemy

@onready var vision: ShapeCast3D = $VisionCheck
var time_since_seen = 0
@export var strike_cooldown: float = 2
var strike_time = 0


func _ready():
	super._ready()


func try_strike():
	if strike_time < 0.001:
		strike_time = strike_cooldown
		if strikeArea.get_overlapping_bodies().size() > 0:
			on_entity_melee_strike.emit(15,0.5)


func _physics_process(delta):
	super._physics_process(delta)
	strike_time = max(strike_time-delta,0)
	pass
	#strike_time += delta
	#if strike_time > 1:
	#	try_strike()
	#	strike_time = 0
	#time_since_seen += delta
	#if time_since_seen > 5:
	#	target_entity = null
