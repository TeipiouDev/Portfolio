extends CharacterBody2D

var direction := Vector2(0, 0)
var speed: float = 20
var i: int = 0
var rot = 0
var timer: Timer

# Called when the node enters the scene tree for the first time.
func _ready():
	timer = Timer.new()
	timer.set_one_shot(true)
	timer.set_wait_time(2.5)
	add_child(timer)
	timer.start()
	pass


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta) -> void:
	var rotation_transform = Transform2D()
	if timer.is_stopped() == false:
		rot = 5 * delta
		#Perinteinen lineaarikuvaus kierrolle
		rotation_transform.x.x = cos(rot)
		rotation_transform.y.y = cos(rot)
		rotation_transform.x.y = sin(rot)
		rotation_transform.y.x = -sin(rot)
		i = posmod(i + 1, 360)
		speed += 1.0
		direction = rotation_transform * direction
		direction = direction.normalized()
	var _direction = move_and_collide(direction * delta * speed)


func _on_VisibilityNotifier2D_screen_exited() -> void:
	queue_free()

