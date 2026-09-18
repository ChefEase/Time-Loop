using Godot;

public partial class PlayerController : CharacterBody2D
{
	[Export]
	public float Speed = 200.0f;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 inputDirection = Input.GetVector(
			"move_left",
			"move_right",
			"move_up",
            "move_down"
		);

		Velocity = inputDirection * Speed;

		MoveAndSlide();
	}
}
