extends Camera2D

@onready var camera_2d: Camera2D = $"."
@onready var sprite_2d: Sprite2D = $Sprite2D

# CA LAIR OFF MARC QUESTCCE QUI SE PASSE 
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	camera_2d.position += Vector2(1,0)
	sprite_2d.position = position
	pass
