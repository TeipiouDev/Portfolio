extends Pattern

class_name PatternOutro

var health = 1800
var boss_ref

var spread_timer

var start_position = Vector2(128, 32)

@export var BossBullet = preload("res://Scenes/BossBullet.tscn")

func enter(boss):
	boss_ref = boss
	boss_ref.health = health
	boss_ref.position = start_position

	spread_timer = Timer.new()
	spread_timer.set_one_shot(true)
	spread_timer.set_wait_time(0.5)
	boss_ref.add_child(spread_timer)
	spread_timer.start()


func execute(_delta):	
	if spread_timer.is_stopped() == true:
		spawn_left_bullets(10, Vector2(0, randf_range(23,28)))
		if boss_ref.health <= 900:
			spawn_right_bullets(8, Vector2(256, randf_range(25,35)))
		spread_timer.start()


func exit():
	boss_ref.get_tree().call_group("boss_bullets", "queue_free")


func spawn_left_bullets(amount: int, bullet_position: Vector2):
	for i in range(amount):
		var boss_bullet = BossBullet.instantiate()
		boss_bullet.position = Vector2(bullet_position.x, bullet_position.y * i)
		boss_bullet.direction = Vector2(1, 0) * 50
		boss_bullet.modulate = Color(0.2, 0.2, 1, 1)
		boss_ref.get_parent().add_child(boss_bullet)


func spawn_right_bullets(amount, bullet_position):
	for i in range(amount):
		var boss_bullet = BossBullet.instantiate()
		boss_bullet.transform = Transform2D().scaled(Vector2(1,4))
		boss_bullet.position = Vector2(bullet_position.x, bullet_position.y * i)
		boss_bullet.direction = Vector2(-1, 0) * 50
		boss_bullet.modulate = Color(1, 0.1, 0.1, 1)
		boss_ref.get_parent().add_child(boss_bullet)
