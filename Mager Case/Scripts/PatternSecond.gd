extends Pattern

class_name PatternSecond

var health = 1400
var spread_timer
var move_timer
var boss_ref
var boss_velocity = Vector2(0, 0)

var t = 0

var start_position = Vector2(128, 96)

var velocity = Vector2(0.01, 0.01)

@export var BossBullet = preload("res://Scenes/BossBullet.tscn")

func enter(boss):
	boss_ref = boss
	boss_ref.health = health
	boss_ref.position = start_position
	
	spread_timer = Timer.new()
	spread_timer.set_one_shot(true)
	spread_timer.set_wait_time(0.175)
	boss_ref.add_child(spread_timer)
	spread_timer.start()
	
	move_timer = boss_ref.get_node("MoveTimer")
	move_timer.set_one_shot(true)
	move_timer.set_wait_time(3.0)
	move_timer.start()


func execute(delta):
	if spread_timer.is_stopped() == true:
		for i in range(30):
			var boss_bullet = BossBullet.instantiate()
			if i % 4 == 0:
				boss_bullet.position = boss_ref.position + Vector2(cos(deg_to_rad(i * 12)), sin(deg_to_rad(i * 12))) * 20
				boss_bullet.direction = (Vector2(cos(deg_to_rad(t)), sin(deg_to_rad(t)))).normalized() * 100
				boss_bullet.modulate = Color("dark_violet")
				boss_ref.get_parent().add_child(boss_bullet)
			elif i % 2 == 1:
				boss_bullet.position = boss_ref.position + Vector2(cos(deg_to_rad(i * 12)), sin(deg_to_rad(i * 12))) * 20
				boss_bullet.direction = (Vector2(sin(deg_to_rad(i*12 * t)), cos(deg_to_rad(i*12 * t)))).normalized() * 100
				boss_bullet.modulate = Color("dodgerblue")
				boss_ref.get_parent().add_child(boss_bullet)
		spread_timer.start()
	
	t += 100 * delta
	if t > 360:
		t = 0
		
	move_boss(delta)

func exit():
	boss_ref.get_tree().call_group("boss_bullets", "queue_free")


func move_boss(delta):
	if move_timer.is_stopped() == false:
		boss_ref.position += boss_velocity.normalized() * delta * 10
	else:
		await boss_ref.get_tree().create_timer(1.0).timeout
		boss_velocity = Vector2(randf_range(-1, 1), randf_range(-0.2, 0.2))
		move_timer.start()
	

