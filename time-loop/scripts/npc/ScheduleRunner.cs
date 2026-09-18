using Godot;
using System.Collections.Generic;

public partial class ScheduleRunner : Node
{
    [Export] public NpcController Npc { get; set; }
    [Export] public NpcSchedule Schedule { get; set; }
    [Export] public Node2D DestinationRoot { get; set; }

    private GameClock _clock;
    private bool _navigationReady;
    private int _activeEntryIndex = -1;
    private readonly Dictionary<StringName, Marker2D> _destinations = new();

    public override async void _Ready()
    {
        ProcessPhysicsPriority = 1;
        _clock = GameClock.Instance;
        if (Npc == null || Schedule == null || DestinationRoot == null || _clock == null)
        {
            GD.PushError($"[{Name}] Assign Npc, Schedule and DestinationRoot; GameClock Autoload is required.");
            SetPhysicsProcess(false);
            return;
        }

        foreach (Node child in DestinationRoot.GetChildren())
            if (child is Marker2D marker)
                _destinations[marker.Name] = marker;

        double previousTime = -1;
        foreach (ScheduleEntry entry in Schedule.Entries)
        {
            if (entry == null || !double.IsFinite(entry.StartTimeSeconds)
                || entry.StartTimeSeconds < 0 || entry.StartTimeSeconds < previousTime)
            {
                GD.PushError($"[{Name}] Schedule entries must be non-null and ordered by nonnegative start time.");
                SetPhysicsProcess(false);
                return;
            }
            previousTime = entry.StartTimeSeconds;
        }

        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        if (!IsInsideTree())
            return;
        _navigationReady = true;
        EvaluateSchedule();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_navigationReady)
            EvaluateSchedule();
    }

    private void EvaluateSchedule()
    {
        int selected = -1;
        for (int i = 0; i < Schedule.Entries.Count; i++)
        {
            if (Schedule.Entries[i].StartTimeSeconds > _clock.CurrentTime)
                break;
            selected = i;
        }
        if (selected == _activeEntryIndex)
            return;
        _activeEntryIndex = selected;
        Npc.StopMoving();
        if (selected < 0)
            return;

        ScheduleEntry entry = Schedule.Entries[selected];
        int seconds = (int)_clock.CurrentTime;
        string action = entry.Action == NpcScheduleAction.Wait ? "WAIT" : $"MOVE {entry.DestinationId}";
        GD.Print($"[{Npc.Name}] {seconds / 60:00}:{seconds % 60:00} -> {action}");
        if (entry.Action == NpcScheduleAction.Wait)
            return;

        if (!_destinations.TryGetValue(entry.DestinationId, out Marker2D destination))
        {
            GD.PushError($"[{Npc.Name}] Unknown destination: {entry.DestinationId}");
            return;
        }
        Npc.MoveTo(destination.GlobalPosition);
    }
}
