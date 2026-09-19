using Godot;

public partial class PrototypeActorVisual : Node2D
{
    [Export] public Color Coat { get; set; } = new("d2bd8a");
    [Export] public bool IsPlayer { get; set; }
    private CharacterBody2D _actor;
    private float _stride;
    public override void _Ready() => _actor = GetParent<CharacterBody2D>();
    public override void _Process(double delta)
    {
        if (_actor.Velocity.LengthSquared() > 1) _stride += (float)delta * 12;
        else _stride = 0;
        QueueRedraw();
    }
    public override void _Draw()
    {
        DrawEllipse(new Vector2(0, 9), new Vector2(14, 6), new Color(0, 0, 0, 0.3f));
        if (IsPlayer) DrawArc(new Vector2(0, 3), 19, 0, Mathf.Tau, 40, new Color("dec990"), 1.5f, true);
        float step = Mathf.Sin(_stride) * 2;
        DrawRect(new Rect2(-7, 3 + step, 5, 10), new Color("14232a"));
        DrawRect(new Rect2(2, 3 - step, 5, 10), new Color("14232a"));
        DrawCircle(new Vector2(-8, -1), 5, Coat.Darkened(0.12f));
        DrawCircle(new Vector2(8, -1), 5, Coat.Darkened(0.12f));
        DrawRect(new Rect2(-8, -7, 16, 15), Coat);
        DrawLine(new Vector2(0, -3), new Vector2(0, 8), Coat.Darkened(0.3f), 2);
        DrawCircle(new Vector2(0, -10), 7, new Color("c9a888"));
        DrawArc(new Vector2(0, -11), 6, Mathf.Pi, Mathf.Tau, 12, new Color("30322e"), 5, true);
        if (IsPlayer) DrawRect(new Rect2(-5, -3, 10, 3), new Color("f1dec0"));

        if (_actor.Name == "Jonah" && WorldState.Instance != null && WorldState.Instance.GetFact(WorldFact.PrototypeJonahInjured))
        {
            DrawCircle(new Vector2(0, -27), 10, new Color("6b3030"));
            DrawLine(new Vector2(-5, -32), new Vector2(5, -22), new Color("f2c2a6"), 3);
            DrawLine(new Vector2(5, -32), new Vector2(-5, -22), new Color("f2c2a6"), 3);
        }

        if (_actor.Name == "Daniel" && WorldState.Instance != null && WorldState.Instance.GetBool(WorldFactIds.DanielFleeing))
        {
            DrawArc(new Vector2(0, -2), 22, Mathf.Pi, Mathf.Tau, 18, new Color("e3a15c"), 2, true);
        }
    }
    private void DrawEllipse(Vector2 center, Vector2 radius, Color color)
    {
        Vector2[] points = new Vector2[24];
        for (int i = 0; i < points.Length; i++)
        {
            float angle = i * Mathf.Tau / points.Length;
            points[i] = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        }
        DrawColoredPolygon(points, color);
    }
}
