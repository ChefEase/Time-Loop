using System;
using Godot;

public partial class GameClock : Node
{
	// Lets the rest of our C# project access the one global GameClock.
	public static GameClock Instance { get; private set; } = null!;

	// Fires whenever the displayed whole second changes.
	[Signal]
	public delegate void TimeChangedEventHandler(double currentTimeSeconds);

	// Fires ONCE when we reach the end of the loop.
	[Signal]
	public delegate void ReachedLoopEndEventHandler();

	// Prototype = 180 seconds = 3 minutes.
	[Export]
	public double LoopDurationSeconds { get; set; } = 180.0;

	// How many seconds have passed since this loop started.
	public double CurrentTime { get; private set; } = 0.0;

	// 1 = normal
	// 2 = twice as fast
	// 5 = five times as fast
	// 10 = ten times as fast
	public double TimeScale { get; private set; } = 1.0;

	// Pauses only our GAME CLOCK.
	public bool IsPaused { get; private set; } = false;

	// Prevents the ending event from firing repeatedly.
	public bool HasReachedLoopEnd { get; private set; } = false;

	// Used so TimeChanged doesn't fire unnecessarily every frame.
	private int _lastEmittedWholeSecond = -1;


	public override void _Ready()
	{
		Instance = this;

		ResetClock();

		GD.Print("GameClock ready.");
	}


	public override void _PhysicsProcess(double delta)
	{
		// Do nothing while paused or after the loop has ended.
		if (IsPaused || HasReachedLoopEnd)
			return;

		// Advance simulation time.
		CurrentTime += delta * TimeScale;

		// Did we reach the end?
		if (CurrentTime >= LoopDurationSeconds)
		{
			CurrentTime = LoopDurationSeconds;

			EmitTimeChangedIfNeeded(true);

			HasReachedLoopEnd = true;

			EmitSignal(SignalName.ReachedLoopEnd);

			GD.Print("GameClock reached loop end.");

			return;
		}

		EmitTimeChangedIfNeeded();
	}


	private void EmitTimeChangedIfNeeded(bool force = false)
	{
		int wholeSecond = (int)Math.Floor(CurrentTime);

		if (force || wholeSecond != _lastEmittedWholeSecond)
		{
			_lastEmittedWholeSecond = wholeSecond;

			EmitSignal(
				SignalName.TimeChanged,
				CurrentTime
			);
		}
	}


	public void PauseClock()
	{
		IsPaused = true;

		GD.Print("GameClock paused.");
	}


	public void ResumeClock()
	{
		IsPaused = false;

		GD.Print("GameClock resumed.");
	}


	public void TogglePause()
	{
		if (IsPaused)
			ResumeClock();
		else
			PauseClock();
	}


	public void SetTimeScale(double newTimeScale)
	{
		TimeScale = Math.Clamp(newTimeScale, 0.1, 20.0);

		GD.Print($"GameClock speed: {TimeScale}x");
	}


	public void ResetClock()
	{
		CurrentTime = 0.0;

		IsPaused = false;

		HasReachedLoopEnd = false;

		_lastEmittedWholeSecond = -1;

		EmitTimeChangedIfNeeded(true);

		GD.Print("GameClock reset.");
	}
}
