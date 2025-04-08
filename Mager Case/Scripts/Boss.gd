extends Area2D

@export var health = 1000

var patterns = []
var pattern_machine
var invul_timer

var screen_size: Vector2

signal boss_defeated()

# Called when the node enters the scene tree for the first time.
func _ready():
	
	screen_size = get_viewport_rect().size
	invul_timer = $InvulTimer
	pattern_machine = $PatternMachine
	patterns.push_back(PatternIntro.new())
	patterns.push_back(PatternSecond.new())
	patterns.push_back(PatternOutro.new())
	pattern_machine.change_pattern(patterns.pop_front())
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	if health <= 0 and patterns.size() != 0:
		pattern_machine.change_pattern(patterns.pop_front())
	elif health <= 0 and patterns.size() == 0:
		get_tree().call_group("boss_bullets", "queue_free")
		emit_signal("boss_defeated")
		queue_free()
	
	#skip pattern with R button
	if Input.is_action_just_pressed("debug_skip_pattern"):
		pattern_machine.change_pattern(patterns.pop_front())
	
	position.x = clamp(position.x, 0, screen_size.x)
	position.y = clamp(position.y, 0, screen_size.y)
	
	if invul_timer.is_stopped() == false:
		modulate.a = 0.25 if Engine.get_frames_drawn() % 2 == 0 else 0.5
	else:
		modulate.a = 1.0
	
	

func _on_Boss_body_entered(body) -> void:
	if body.has_method("deal_damage") and invul_timer.is_stopped(): 
		health -= body.deal_damage()
	body.hide()

func get_health() -> int:
	return health

func set_health(amount: int) -> void:
	health = amount


func _on_PatternMachine_pattern_changed():
	invul_timer.start()
