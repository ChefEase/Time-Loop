using Godot;

public partial class CentralGreywickPreview : Node
{
    private Node2D _map = null!;
    private Node2D _oldMap = null!;
    private Node2D _npcs = null!;
    private Node2D _objects = null!;
    private CanvasLayer _instructions = null!;
    private bool _showing;

    public override void _Ready()
    {
        _map = GetParent().GetNode<Node2D>("CentralGreywickBlockout");
        _oldMap = GetParent().GetNode<Node2D>("MapVisual");
        _npcs = GetParent().GetNode<Node2D>("NPCs");
        _objects = GetParent().GetNode<Node2D>("Objects");
        _instructions = _map.GetNode<CanvasLayer>("Instructions");
        SetPreview(false);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey key && key.Pressed && !key.Echo && key.Keycode == Key.G)
        {
            SetPreview(!_showing);
            GetViewport().SetInputAsHandled();
        }
    }

    private void SetPreview(bool showing)
    {
        _showing = showing;
        _map.Visible = showing;
        _oldMap.Visible = !showing;
        _npcs.Visible = !showing;
        _objects.Visible = !showing;
        _instructions.Visible = showing;
        if (showing)
        {
            GetParent().GetNode<Node2D>("Player").GlobalPosition = new Vector2(560, 650);
            GetParent().GetNode<Camera2D>("Camera2D").GlobalPosition = new Vector2(560, 380);
        }
    }
}
