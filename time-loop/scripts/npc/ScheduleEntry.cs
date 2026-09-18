using Godot;

[GlobalClass]
public partial class ScheduleEntry : Resource
{
    [Export] public double StartTimeSeconds { get; set; }
    [Export] public NpcScheduleAction Action { get; set; }
    [Export] public StringName DestinationId { get; set; } = "";
    // Reserved data only: conditional scheduling is not implemented in Phase 7.
    [Export] public Godot.Collections.Array<StringName> RequiredFacts { get; set; } = new();
    [Export] public Godot.Collections.Array<StringName> BlockedFacts { get; set; } = new();
}
