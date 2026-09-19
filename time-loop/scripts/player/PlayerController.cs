using Godot;

public partial class PlayerController : CharacterBody2D
{
	public bool DialogueLocked { get; set; }
	[Export]
	public float MoveSpeed = 180f;

	private Area2D _interactionArea;

	public override void _Ready()
	{
		_interactionArea = GetNode<Area2D>("InteractionArea");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (DialogueLocked)
		{
			Velocity = Vector2.Zero;
			return;
		}
		Vector2 inputDirection = Input.GetVector(
			"move_left",
			"move_right",
			"move_up",
            "move_down"
		);

		Velocity = inputDirection * MoveSpeed;

		MoveAndSlide();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("interact"))
		{
			TryInteract();

			GetViewport().SetInputAsHandled();
		}
	}

	private void TryInteract()
	{
		IInteractable nearestInteractable = null;
		float nearestDistanceSquared = float.MaxValue;

		// Check physics bodies such as NPCs, doors, signs, etc.
		foreach (Node2D body in _interactionArea.GetOverlappingBodies())
		{
			CheckInteractable(
				body,
				ref nearestInteractable,
				ref nearestDistanceSquared
			);
		}

		// Also check Area2D-based interactables such as items or triggers.
		foreach (Area2D area in _interactionArea.GetOverlappingAreas())
		{
			CheckInteractable(
				area,
				ref nearestInteractable,
				ref nearestDistanceSquared
			);
		}

		if (nearestInteractable != null)
		{
			nearestInteractable.Interact(this);
		}
	}

	private void CheckInteractable(
		Node2D candidate,
		ref IInteractable nearestInteractable,
		ref float nearestDistanceSquared
	)
	{
		if (candidate is not IInteractable interactable)
		{
			return;
		}

		float distanceSquared =
			GlobalPosition.DistanceSquaredTo(candidate.GlobalPosition);

		if (distanceSquared < nearestDistanceSquared)
		{
			nearestDistanceSquared = distanceSquared;
			nearestInteractable = interactable;
		}
	}
}
