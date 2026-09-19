using System;
using Godot;

// Presentation only; GameClock owns time and LoopManager owns reset.
public partial class DebugClockUI : PanelContainer
{
	private GameClock _clock;
	private Label _time;
	private Label _status;
	private Label _remaining;
	private ProgressBar _progress;
	private StyleBoxFlat _fill;
	private string _lastStatus = "";

	public override void _Ready()
	{
		_time = GetNode<Label>("Margin/Rows/TimeRow/Time");
		_status = GetNode<Label>("Margin/Rows/Footer/Status");
		_remaining = GetNode<Label>("Margin/Rows/Footer/Remaining");
		_progress = GetNode<ProgressBar>("Margin/Rows/Progress");
		_fill = (StyleBoxFlat)_progress.GetThemeStylebox("fill").Duplicate();
		_progress.AddThemeStyleboxOverride("fill", _fill);
		_clock = GameClock.Instance;
		if (_clock == null)
		{
			_time.Text = "--:--:--";
			_status.Text = "CLOCK OFFLINE";
			_remaining.Text = "";
			SetProcess(false);
			return;
		}
		Refresh();
	}

	public override void _Process(double delta)
	{
		// Pause/speed have no signals. Read state without changing the simulation.
		Refresh();
	}

	private void Refresh()
	{
		double duration = Math.Max(0, _clock.LoopDurationSeconds);
		double elapsed = Math.Clamp(_clock.CurrentTime, 0, duration);
		int remaining = (int)Math.Ceiling(duration - elapsed);
		int morningSeconds = (int)Math.Floor(elapsed);
		string timeText = $"{morningSeconds / 60 % 60:00}:{morningSeconds % 60:00}";
		if (_time.Text != timeText)
			_time.Text = timeText;
		_remaining.Text = $"{remaining / 60:00}:{remaining % 60:00} LEFT";
		_progress.Value = duration > 0 ? (duration - elapsed) / duration * 100 : 0;
		bool ended = _clock.HasReachedLoopEnd || remaining == 0;
		bool urgent = !ended && remaining <= 30;
		string status = ended ? "LOOP ENDED" : _clock.IsPaused ? "CLOCK PAUSED"
			: urgent ? "FINAL SECONDS" : "MORNING";
		if (!ended && !_clock.IsPaused && _clock.TimeScale != 1)
			status += $"  /  {_clock.TimeScale:0.#}x";
		status = $"Loop: {LoopManager.Instance.LoopNumber} / {status} / Secret: {KnowledgeManager.Instance.Knows(KnowledgeFacts.SecretKnown)}";
		if (status != _lastStatus)
		{
			_lastStatus = status;
			_status.Text = status;
			Color accent = ended || urgent ? new Color("ed9390")
				: _clock.IsPaused ? new Color("a5b1a8") : new Color("d9b77a");
			_status.AddThemeColorOverride("font_color", accent);
			_time.AddThemeColorOverride("font_color", ended || urgent ? new Color("ed9390") : new Color("f2ead9"));
			_fill.BgColor = accent;
		}
	}
}
