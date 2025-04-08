
extends Node2D

var current_pattern
var boss

signal pattern_changed

func change_pattern(new_pattern):
	if current_pattern != null:
		current_pattern.exit()
	current_pattern = new_pattern
	current_pattern.enter(boss)
	emit_signal("pattern_changed")


func _ready():
	boss = get_parent()


func _process(delta):
	current_pattern.execute(delta)


