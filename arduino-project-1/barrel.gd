extends CharacterBody2D

const SPEED=350
var direction=1

func Add_Gravity(delta):
	if not is_on_floor():
		velocity+=get_gravity()*delta
		
func Move_Enemy():
	velocity.x=SPEED*direction
	
func Change_Dir():
	if is_on_wall():
		direction=-direction
		
func _physics_process(delta: float) -> void:
	Add_Gravity(delta)
	Move_Enemy()
	move_and_slide()
	Change_Dir()

func _on_area_2d_body_entered(body: Node2D) -> void:
	if body.is_in_group("Player"):
		body.DestroyActor()
	pass # Replace with function body.
