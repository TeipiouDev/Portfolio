extends Pattern

class_name PatternIntro

var health: int = 1400
var spread_timer: Timer
var move_timer: Timer
var boss_ref
var boss_velocity: Vector2
var boss_spawner
var player_ref
var player_pos

var velocity = Vector2(0.01, 0.01)

var start_position = Vector2(128, 32)

const BossBullet = preload("res://Scenes/BossBullet.tscn")

func enter(boss):
	
	boss_ref = boss
	boss_ref.health = health
	boss_ref.position = start_position
	boss_velocity = Vector2(0, 0)
	boss_spawner = boss_ref.get_node("Spawner")
	
	player_ref = boss_ref.get_parent().get_node("Player")
	
	spread_timer = Timer.new()
	spread_timer.set_one_shot(true)
	spread_timer.set_wait_time(0.3)
	boss_ref.add_child(spread_timer)
	spread_timer.start()
	
	move_timer = boss_ref.get_node("MoveTimer")
	move_timer.set_one_shot(true)
	move_timer.set_wait_time(3.0)
	move_timer.start()


func execute(delta):
	player_pos = player_ref.position
	if spread_timer.is_stopped() == true:
		for spawner in boss_spawner.get_children():
			for i in range(30):
				if i % 2 == 0:
					var boss_bullet = BossBullet.instantiate()
					boss_bullet.show_behind_parent = true
					boss_bullet.set_script(load("res://Scripts/CustomBulletMovementTest.gd"))
					boss_bullet.position = spawner.global_position
					boss_bullet.direction = Vector2(cos(deg_to_rad(i*12)), sin(deg_to_rad(i*12))) * 50
					boss_ref.get_parent().add_child(boss_bullet)
				if i % 2 == 1:
					var boss_bullet = BossBullet.instantiate()
					boss_bullet.position = spawner.global_position
					boss_bullet.direction = (player_pos - boss_bullet.position).normalized() * 150
					boss_bullet.modulate = Color("crimson")
					boss_ref.get_parent().add_child(boss_bullet)
		
		spread_timer.start()
	
	boss_spawner.rotate(delta * 4)
	move_boss(delta)


func exit():
	boss_ref.get_tree().call_group("boss_bullets", "queue_free")
	for spawner in boss_spawner.get_children():
		spawner.queue_free()


func move_boss(delta):
	if is_instance_valid(move_timer):
		if move_timer.is_stopped() == false:
			boss_ref.position += boss_velocity.normalized() * delta * 10
		else:
			await boss_ref.get_tree().create_timer(1.0).timeout
			boss_velocity = Vector2(randf_range(-1, 1), randf_range(-0.1, 0.1))
			move_timer.start()

	
	
	
