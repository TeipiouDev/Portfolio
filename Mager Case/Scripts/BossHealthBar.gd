extends Control

signal pattern_changed()

var health_bar
var boss

func _on_PatternMachine_pattern_changed():
	emit_signal("pattern_changed")
	health_bar = $TextureProgressBar
	boss = get_parent().find_child("Boss")
	health_bar.max_value = boss.health
