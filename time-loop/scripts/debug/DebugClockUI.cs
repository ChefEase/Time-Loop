using System;
using Godot;

public partial class DebugClockUI : Label
{
	public override void _Ready()
	{
		if (GameClock.Instance == null)
		{
			GD.PushError("DebugClockUI could not find GameClock.");
			return;
		}

		// Listen for time changes.
		GameClock.Instance.TimeChanged += OnTimeChanged;

		// Listen for loop ending.
		GameClock.Instance.ReachedLoopEnd += OnReachedLoopEnd;

		// Immediately show the current time.
		UpdateClockText(GameClock.Instance.CurrentTime);
	}


	public override void _ExitTree()
	{
		// Disconnect safely when this UI disappears.
		if (GameClock.Instance != null)
		{
			GameClock.Instance.TimeChanged -= OnTimeChanged;
			GameClock.Instance.ReachedLoopEnd -= OnReachedLoopEnd;
		}
	}


	private void OnTimeChanged(double currentTimeSeconds)
	{
		UpdateClockText(currentTimeSeconds);
	}


	private void OnReachedLoopEnd()
	{
		GD.Print("DebugClockUI received ReachedLoopEnd event.");
	}


	private void UpdateClockText(double currentTimeSeconds)
	{
		int totalSeconds = (int)Math.Floor(currentTimeSeconds);

		int minutes = totalSeconds / 60;
		int seconds = totalSeconds % 60;

		Text = $"{minutes:00}:{seconds:00}";
	}
}
