using Godot;

public partial class PrototypeDocumentVisual : Node2D
{
    public override void _Draw()
    {
        DrawCircle(Vector2.Zero, 26, new Color(0.78f, 0.68f, 0.39f, 0.09f));
        DrawRect(new Rect2(-12, -14, 30, 38), new Color(0, 0, 0, 0.3f));
        DrawRect(new Rect2(-15, -20, 28, 36), new Color("d8c9a5"));
        DrawRect(new Rect2(-15, -20, 28, 36), new Color("b99e6a"), false);
        DrawColoredPolygon(new[] { new Vector2(5, -20), new Vector2(13, -12), new Vector2(5, -12) }, new Color("f0e2c2"));
        for (int y = -7; y < 10; y += 5) DrawLine(new Vector2(-9, y), new Vector2(6, y), new Color("837a62"), 1);
        DrawCircle(new Vector2(-8, -14), 2, new Color("92644b"));
    }
}
