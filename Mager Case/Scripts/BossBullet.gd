extends CharacterBody2D

var direction = Vector2(0, 0)

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.
	

func _physics_process(delta):
	var _direction = move_and_collide(direction * delta)


func _on_VisibilityNotifier2D_screen_exited():
	queue_free()
