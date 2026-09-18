using Godot;

[GlobalClass]
public partial class NpcSchedule : Resource
{
    [Export] public Godot.Collections.Array<ScheduleEntry> Entries { get; set; } = new();
}
