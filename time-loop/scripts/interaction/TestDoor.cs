using Godot;

public partial class TestDoor : StaticBody2D, IInteractable
{
	private WorldState _worldState = null!;
	private CollisionShape2D _collision = null!;

	public override void _Ready()
	{
		_collision = GetNode<CollisionShape2D>("CollisionShape2D");
		_worldState = WorldState.Instance;
		_worldState.FactChanged += OnFactChanged;
		_worldState.WorldReset += RefreshFromWorldState;
		RefreshFromWorldState();
	}

	public void Interact(Node interactor)
	{
		if (!_worldState.GetFact(WorldFact.TestDoorOpen))
		{
			// The notification updates the visuals after the truth changes.
			_worldState.SetFact(WorldFact.TestDoorOpen, true);
		}
	}

	private void OnFactChanged(WorldFact fact, bool value)
	{
		if (fact == WorldFact.TestDoorOpen)
			RefreshFromWorldState();
	}

	private void RefreshFromWorldState()
	{
		if (_worldState.GetFact(WorldFact.TestDoorOpen))
			OpenDoor();
		else
			Visible = true;

		// Invisible physics bodies still block movement unless disabled.
		_collision.SetDeferred(CollisionShape2D.PropertyName.Disabled,
			_worldState.GetFact(WorldFact.TestDoorOpen));
	}

	private void OpenDoor()
	{
		Visible = false;
	}

	public override void _ExitTree()
	{
		if (_worldState == null)
			return;

		_worldState.FactChanged -= OnFactChanged;
		_worldState.WorldReset -= RefreshFromWorldState;
	}
}
