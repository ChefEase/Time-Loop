using Godot;

public partial class WorldStateDebugTester : Node
{
	public override void _UnhandledInput(InputEvent @event)
	{
		if (!OS.IsDebugBuild() || @event is not InputEventKey keyEvent)
		{
			return;
		}

		if (!keyEvent.Pressed || keyEvent.Echo)
		{
			return;
		}

		switch (keyEvent.Keycode)
		{
			case Key.O:
				WorldState.Instance.SetFact(
					WorldFact.TestDoorOpen,
					true
				);
				break;

			case Key.V:
				WorldState.Instance.SetFact(
					WorldFact.TestDoorOpen,
					false
				);
				break;

			case Key.P:
				PrintCurrentState();
				break;
		}
	}

	private void PrintCurrentState()
	{
		bool doorOpen =
			WorldState.Instance.GetFact(WorldFact.TestDoorOpen);

		GD.Print($"[TEST] Door open = {doorOpen}");
	}
}
