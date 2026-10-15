extends CharacterBody2D

var barrel=preload("res://barrel.tscn")

func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

func _on_timer_timeout() -> void:
	var new_barrel = barrel.instantiate()
	add_child(new_barrel)
	new_barrel.scale = Vector2(0.2, 0.2)
	new_barrel.transform=$SpawnPoint.transform
