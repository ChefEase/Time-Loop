using Godot;

// Greybox-only vertical-slice map. It owns layout and landmarks, not gameplay logic.
public partial class CentralGreywick : Node2D
{
    [Export] public Vector2 AveryStart = new(560, 650);
    [Export] public Vector2 CafeEntrance = new(290, 250);
    [Export] public Vector2 PoliceEntrance = new(760, 180);
    [Export] public Vector2 FloristAlley = new(860, 390);
    [Export] public Vector2 UtilityAccess = new(930, 570);

    public override void _Ready()
    {
        GD.Print($"[Central Greywick] Cafe -> Police: {AveryStart.DistanceTo(PoliceEntrance):0} px");
        GD.Print($"[Central Greywick] Cafe -> Florist Alley: {CafeEntrance.DistanceTo(FloristAlley):0} px");
        GD.Print($"[Central Greywick] Cafe -> Utility Access: {CafeEntrance.DistanceTo(UtilityAccess):0} px");
        QueueRedraw();
    }

    public override void _Draw()
    {
        DrawRect(new Rect2(0, 0, 1120, 760), new Color("10191f"));
        DrawRect(new Rect2(60, 290, 1000, 180), new Color("263b40"));
        DrawRect(new Rect2(470, 40, 190, 220), new Color("31474a"));
        DrawRect(new Rect2(150, 90, 250, 155), new Color("394d48"));
        DrawRect(new Rect2(760, 90, 220, 170), new Color("344856"));
        DrawRect(new Rect2(120, 535, 190, 110), new Color("3f4944"));
        DrawRect(new Rect2(820, 505, 220, 140), new Color("3d4746"));
        DrawRect(new Rect2(650, 310, 150, 130), new Color("4b4050"));

        DrawLine(new Vector2(60, 380), new Vector2(1060, 380), new Color("9c9070"), 3);
        DrawLine(new Vector2(560, 40), new Vector2(560, 260), new Color("9c9070"), 3);
        DrawLine(new Vector2(560, 470), new Vector2(900, 570), new Color("9c9070"), 3);

        Label("CENTRAL GREYWICK", new Vector2(470, 22), 22, "e8dec1");
        Label("TOWN SQUARE", new Vector2(475, 150), 18, "e8dec1");
        Label("LENA'S CAFÉ", new Vector2(190, 170), 18, "e8dec1");
        Label("POLICE STATION", new Vector2(785, 170), 18, "e8dec1");
        Label("COMMERCIAL ROAD", new Vector2(465, 398), 17, "e8dec1");
        Label("FLORIST ALLEY", new Vector2(665, 380), 15, "d1a7d4");
        Label("GENERAL STORE", new Vector2(145, 595), 16, "e8dec1");
        Label("UTILITY ACCESS", new Vector2(860, 580), 16, "e8dec1");
        Label("CAFÉ KITCHEN SHORTCUT", new Vector2(820, 680), 13, "b7d3c6");

        DrawCircle(AveryStart, 10, new Color("d8c28b"));
        Label("AVERY START", AveryStart + new Vector2(-45, 28), 12, "d8c28b");
        DrawCircle(CafeEntrance, 7, new Color("c7a56b"));
        DrawCircle(PoliceEntrance, 7, new Color("8ab7d1"));
        DrawCircle(FloristAlley, 7, new Color("c89bd1"));
        DrawCircle(UtilityAccess, 7, new Color("e2c56f"));
    }

    private void Label(string text, Vector2 position, int size, string color)
    {
        DrawString(ThemeDB.FallbackFont, position, text, HorizontalAlignment.Left, -1, size, new Color(color));
    }
}
