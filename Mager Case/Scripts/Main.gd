extends Node2D

var boss_health_label
var boss
var boss_healthbar

# Called when the node enters the scene tree for the first time.
func _ready():
	boss_healthbar = $BossHealthBar/TextureProgressBar
	boss = $Boss

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	if is_instance_valid(boss):
		boss_healthbar.value = boss.get_health()


func _on_Player_game_over():
	var _restart = get_tree().reload_current_scene()


func _on_boss_boss_defeated():
	var game_end_text = $GameEnd
	game_end_text.visible = true
	boss_healthbar.visible = false
	await get_tree().create_timer(5).timeout
	get_tree().quit()
