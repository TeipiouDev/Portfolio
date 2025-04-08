extends Area2D

@export var speed = 128
@export var PlayerBullet = preload("res://Scenes/PlayerBullet.tscn")
@export var lives_amount = 3

signal game_over()

var lives
var screen_size
var fire_timer
var invul_timer
var rot = 0

# Called when the node enters the scene tree for the first time.
func _ready():
	screen_size = get_viewport_rect().size
	fire_timer = $FireCooldownTimer
	invul_timer = $InvulTimer
	lives = $Lives

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	
	player_control(delta)
	spin_lives(delta)
	
	if invul_timer.is_stopped() == false:
		modulate.a = 0.25 if Engine.get_frames_drawn() % 2 == 0 else 0.5
	else:
		modulate.a = 1.0
	

func fire():
	var player_bullet_left = PlayerBullet.instantiate()
	var player_bullet_right = PlayerBullet.instantiate()
	player_bullet_left.position = $MuzzleLeft.global_position
	player_bullet_right.position = $MuzzleRight.global_position
	get_parent().add_child(player_bullet_left)
	get_parent().add_child(player_bullet_right)

func spin_lives(delta):
	var life1 = $Lives/Life1
	var life2 = $Lives/Life2
	var life3 = $Lives/Life3
	var rotation_transform = Transform2D()
	rot = 4 * delta
	#Perinteinen lineaarikuvaus kierrolle
	rotation_transform.x.x = cos(rot)
	rotation_transform.y.y = cos(rot)
	rotation_transform.x.y = sin(rot)
	rotation_transform.y.x = -sin(rot)
	
	#Kiertävät sopivasti Lives-nimisen Noden sijainnin ympäri.
	#Tämä siksi, jottei spritet vääristy, niinkuin käy jos vaan käännetään
	#suoraan parent-nodea.
	life1.position = rotation_transform * life1.position
	life2.position = rotation_transform * life2.position
	life3.position = rotation_transform * life3.position

func player_control(delta):
	var velocity = Vector2()
	if Input.is_action_pressed("ui_left"):
		velocity.x -= 1
	if Input.is_action_pressed("ui_right"):
		velocity.x += 1
	if Input.is_action_pressed("ui_down"):
		velocity.y += 1
	if Input.is_action_pressed("ui_up"):
		velocity.y -= 1
		
	if Input.is_action_just_pressed("esc"):
		get_tree().quit()
		
	if velocity.length() > 0:
		velocity = velocity.normalized() * speed
		
	position += velocity * delta
	position.x = clamp(position.x, 0, screen_size.x)
	position.y = clamp(position.y, 0, screen_size.y - 8)
	
	if Input.is_action_pressed("ui_fire") and fire_timer.is_stopped():
		fire_timer.start()
		fire()
	
	if lives_amount < 0:
		emit_signal("game_over")

#Bullet collision damage
func _on_Player_body_entered(_body):
	if invul_timer.is_stopped():
		#lives_amount -= 1
		update_lives()
		invul_timer.start()

#Boss collision damage
func _on_Player_area_entered(_area):
	if invul_timer.is_stopped():
		lives_amount -= 1
		update_lives()
		invul_timer.start()


func update_lives():
	match lives_amount:
		3:
			$Lives/Life1.visible = true
			$Lives/Life2.visible = true
			$Lives/Life3.visible = true
		2:
			$Lives/Life1.visible = false
		1:
			$Lives/Life2.visible = false
		0:
			$Lives/Life3.visible = false
			







