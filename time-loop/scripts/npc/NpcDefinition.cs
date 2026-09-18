using Godot;

[GlobalClass]
public partial class NpcDefinition : Resource
{
    [Export] public StringName Id { get; set; } = "";
    [Export] public string DisplayName { get; set; } = "";
    [Export] public Texture2D Portrait { get; set; }
    [Export] public float DefaultSpeed { get; set; } = 80.0f;
}
