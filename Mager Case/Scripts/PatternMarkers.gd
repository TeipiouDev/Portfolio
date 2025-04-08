extends HBoxContainer

var boss

var PatternMarker = load("res://Scenes/PatternMarker.tscn")

func _on_BossHealthBar_pattern_changed():
	
	for marker in get_children():
		marker.queue_free()
	boss = get_parent().get_parent().find_child("Boss")
	for i in boss.patterns.size():
		add_child(PatternMarker.instantiate())
	pass # Replace with function body.
