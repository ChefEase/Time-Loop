using Godot;
using System.Collections.Generic;

// Presentation only. Geometry and navigation remain owned by the authored scene.
public partial class PrototypeMapVisual : Node2D
{
    private readonly List<Rect2> _walls = new();
    public override void _Ready()
    {
        var geometry = GetParent().GetNode<Node2D>("WorldGeometry");
        foreach (Node node in geometry.GetChildren())
        {
            if (node is Polygon2D polygon) polygon.Hide();
            if (node is Label label) label.Hide();
            if (node is StaticBody2D body)
            {
                var wall = body.GetNode<Polygon2D>("Wall");
                Vector2 min = wall.Polygon[0], max = min;
                foreach (Vector2 p in wall.Polygon) { min = min.Min(p); max = max.Max(p); }
                _walls.Add(new Rect2(min, max - min));
                wall.Hide();
            }
        }
        foreach (Node node in GetParent().GetNode("Objects").GetChildren())
            if (node is CanvasItem item) item.Hide();
    }

    private void Box(Rect2 r, string fill, string edge = "")
    {
        DrawRect(r, new Color(fill));
        if (edge != "") DrawRect(r, new Color(edge), false, 1.5f);
    }

    public override void _Draw()
    {
        Box(new Rect2(-2000, -2000, 5000, 5000), "101a20");
        Box(new Rect2(5, 10, 1120, 800), "090f13");
        Box(new Rect2(0, 0, 1120, 800), "293c40", "60706a");
        // Paving is static linework, with no randomness or physical obstacles.
        for (int y = 40; y < 760; y += 40)
        {
            DrawLine(new Vector2(40, y), new Vector2(1080, y), new Color("32474a"));
            for (int x = 40 + (y / 40 % 2) * 40; x < 1080; x += 80)
                DrawLine(new Vector2(x, y), new Vector2(x, y + 40), new Color("32474a"));
        }
        Box(new Rect2(52, 390, 1016, 220), "1b2a30");
        Box(new Rect2(52, 384, 1016, 6), "879081");
        Box(new Rect2(52, 610, 1016, 6), "879081");
        for (int x = 70; x < 1040; x += 65)
            Box(new Rect2(x, 498, 30, 3), "978c65");
        for (int y = 405; y < 605; y += 28)
            Box(new Rect2(575, y, 50, 12), "77827a");
        // Cutaway interiors and entrance thresholds follow the existing collision walls.
        Box(new Rect2(200, 120, 280, 160), "354b50");
        for (int x = 200; x < 480; x += 35)
            DrawLine(new Vector2(x, 120), new Vector2(x, 280), new Color("40575a"));
        Box(new Rect2(800, 440, 200, 240), "384442");
        for (int y = 440; y < 680; y += 30)
            DrawLine(new Vector2(800, y), new Vector2(1000, y), new Color("47534c"));
        Box(new Rect2(320, 282, 80, 35), "626956", "c4ac72");
        Box(new Rect2(840, 402, 80, 35), "626956", "c4ac72");
        foreach (Rect2 wall in _walls)
        {
            Box(new Rect2(wall.Position + new Vector2(5, 7), wall.Size), "111e24");
            Box(wall, "727b70", "283d40");
            Box(new Rect2(wall.Position + Vector2.One * 3, wall.Size - Vector2.One * 6), "59685f");
            DrawLine(wall.Position + new Vector2(3, 3), wall.Position + new Vector2(wall.Size.X - 3, 3), new Color("9b9e84"), 2);
            if (wall.Size.X > wall.Size.Y)
                for (float x = wall.Position.X + 40; x < wall.End.X; x += 40)
                    DrawLine(new Vector2(x, wall.Position.Y + 4), new Vector2(x, wall.End.Y - 4), new Color("45574f"));
            else
                for (float y = wall.Position.Y + 40; y < wall.End.Y; y += 40)
                    DrawLine(new Vector2(wall.Position.X + 4, y), new Vector2(wall.End.X - 4, y), new Color("45574f"));
        }
        // Key rack: mounted case and brass key rather than a yellow block.
        Box(new Rect2(250, 520, 60, 30), "101d22", "bba578");
        DrawCircle(new Vector2(268, 534), 5, new Color("d6bc78"));
        DrawCircle(new Vector2(268, 534), 2, new Color("101d22"));
        DrawLine(new Vector2(273, 534), new Vector2(295, 534), new Color("d6bc78"), 3);
        DrawLine(new Vector2(290, 534), new Vector2(290, 539), new Color("d6bc78"), 3);
        // Machine cabinet, vents and an amber indicator (not a simulated stable state).
        Box(new Rect2(924, 505, 52, 60), "142226");
        Box(new Rect2(919, 496, 52, 60), "687870", "b1b59b");
        Box(new Rect2(925, 502, 40, 16), "152c32", "879485");
        DrawCircle(new Vector2(958, 510), 3, new Color("d6ad63"));
        for (int y = 527; y < 550; y += 6) DrawLine(new Vector2(928, y), new Vector2(960, y), new Color("344a49"), 2);
        DrawPolyline(new[] { new Vector2(940, 495), new Vector2(940, 470), new Vector2(977, 470), new Vector2(977, 446) }, new Color("939a84"), 5);
        // Signs are fixed to existing walls, so decorative props cannot look like extra obstacles.
        Box(new Rect2(245, 82, 190, 31), "182e38", "b4a67f");
        DrawString(ThemeDB.FallbackFont, new Vector2(293, 104), "POLICE", HorizontalAlignment.Left, -1, 19, new Color("e8dec1"));
        DrawString(ThemeDB.FallbackFont, new Vector2(823, 706), "MACHINE ROOM", HorizontalAlignment.Left, -1, 16, new Color("e8dec1"));
        DrawString(ThemeDB.FallbackFont, new Vector2(255, 567), "KEY RACK", HorizontalAlignment.Left, -1, 12, new Color("c2b793"));
    }
}
