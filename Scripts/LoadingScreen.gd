extends CanvasLayer

var target = true

@export var transition_time:float = 0.5


var t = 1

@onready var panel: Panel = $Panel
@onready var timers = $Timers
@onready var text = $Panel/Label

signal opened


var flavor_texts = [
	"Ey die Hunde!",
	"Gut gebügelt ist halb genäht",
	"Keine Haftung für die Garderobe",
	"Einfach Orangensaft",
	"Es ist halt einfach ein weites Feld",
	"3, 2 ,1 , Frankfurt!",
	"Geisterfahrer sind immer entgegekommend",
	"Wo ist der Sexraum?",
	"ch glaube der Tod hat hier die meisten Kills",
	"Wollen wir das draußen klären?",
	"Amerika ist halt Rom",
	"Es riecht symmetrisch",
	"Taffe Kassee",
	"Scheiß Turnschuhsozialisten",
	"da hab ich mich in der Küche wiedergefunden wie ich das Brot anschreie, dass es so gut schmeckt",
	"Also ich mein wir könnten hier auch zwei Esel mit einer Klappe verprügeln...",
	"Ikea ist literal eine Mensa mit angetackertem Möbelhandel",
	"Ganz ehrlich: Alligatoah ist die Volt Partei des Deutschrap",
	"Das sieht aus wie eine industrielle Hobbithöhle",
	"Zitat: So'n Schnitzel strahlt auch schon so ne Wärme aus",
	"Metakäse",
	"Der schiefe Turm von Zewa",
	"Die holländische Sprache ist wie Valorant: Sie kombiniert zwei Sachen und macht keins von beidem besser",
	"Anfang April ist für mich auch Ende März"
	
	
	
]

var rand
func set_text():
	var n = rand.randi_range(0,flavor_texts.size()-1)
	text.text = flavor_texts[n]

func _ready():
	
	rand = RandomNumberGenerator.new()
	set_text()

func open():
	if target:
		return
	target = true
	t = 0
	set_text()
	var timer = Timer.new()
	timer.one_shot = true
	timers.add_child(timer)
	timer.start(transition_time)
	return timer


func close():
	for timer in timers.get_children():
		remove_child(timer)
	if not target:
		return
	target = false
	t = 1


func _process(delta):
	var dir = -1
	if target:
		dir = 1
	if t > 0:
		visible = true
	else:
		visible = false
	var velocity = (dir*delta)/transition_time
	var last_t = t
	t = clamp(t+velocity,0,1)
	if last_t <= 0.99 and t > 0.99:
		opened.emit()
	panel.modulate.a = t
	
