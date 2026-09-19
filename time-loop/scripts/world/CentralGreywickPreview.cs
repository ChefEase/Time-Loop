using Godot;

public partial class CentralGreywickPreview : Node
{
    private Node2D _map = null!;
    private Node2D _oldMap = null!;
    private Node2D _npcs = null!;
    private Node2D _objects = null!;
    private CanvasLayer _instructions = null!;
    private CanvasLayer _hud = null!;
    private Node2D _player = null!;
    private Camera2D _camera = null!;
    private bool _showing;

    public override void _Ready()
    {
        _map = GetParent().GetNode<Node2D>("CentralGreywickBlockout");
        _oldMap = GetParent().GetNode<Node2D>("MapVisual");
        _npcs = GetParent().GetNode<Node2D>("NPCs");
        _objects = GetParent().GetNode<Node2D>("Objects");
        _instructions = _map.GetNode<CanvasLayer>("Instructions");
        _hud = GetParent().GetNode<CanvasLayer>("UI");
        _player = GetParent().GetNode<Node2D>("Player");
        _camera = GetParent().GetNode<Camera2D>("Camera2D");
        SetPreview(false);
    }

    public override void _Process(double delta)
    {
        if (!_showing) return;

        Vector2 viewport = GetViewport().GetVisibleRect().Size;
        Vector2 halfView = viewport / (2.0f * Mathf.Max(0.1f, _camera.Zoom.X));
        float cameraX = Mathf.Clamp(_player.GlobalPosition.X, halfView.X, 1120.0f - halfView.X);
        float cameraY = Mathf.Clamp(_player.GlobalPosition.Y, halfView.Y, 760.0f - halfView.Y);
        _camera.GlobalPosition = new Vector2(cameraX, cameraY);
        _player.GlobalPosition = new Vector2(
            Mathf.Clamp(_player.GlobalPosition.X, 24.0f, 1096.0f),
            Mathf.Clamp(_player.GlobalPosition.Y, 24.0f, 736.0f)
        );
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
        _hud.Visible = !showing;
        if (showing)
        {
            _player.GlobalPosition = new Vector2(560, 650);
            _camera.GlobalPosition = new Vector2(560, 380);
        }
    }
}
