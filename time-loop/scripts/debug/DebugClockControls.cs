using Godot;

public partial class DebugClockControls : Node
{
	public override void _Process(double delta)
	{
		if (GameClock.Instance == null)
			return;


		// Pause / Resume
		if (Input.IsActionJustPressed("debug_clock_pause"))
		{
			GameClock.Instance.TogglePause();
		}


		// Reset
		if (Input.IsActionJustPressed("debug_clock_reset"))
		{
			GameClock.Instance.ResetClock();
		}


		// 1x speed
		if (Input.IsActionJustPressed("debug_speed_1"))
		{
			GameClock.Instance.SetTimeScale(1.0);
		}


		// 2x speed
		if (Input.IsActionJustPressed("debug_speed_2"))
		{
			GameClock.Instance.SetTimeScale(2.0);
		}


		// 5x speed
		if (Input.IsActionJustPressed("debug_speed_5"))
		{
			GameClock.Instance.SetTimeScale(5.0);
		}


		// 10x speed
		if (Input.IsActionJustPressed("debug_speed_10"))
		{
			GameClock.Instance.SetTimeScale(10.0);
		}
	}
}
