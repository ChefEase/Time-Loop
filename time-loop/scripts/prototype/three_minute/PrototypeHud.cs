using Godot;
using System;
using System.Collections.Generic;

// Read-only presentation of the existing clock, loop and knowledge systems.
public partial class PrototypeHud : CanvasLayer
{
    private Label _time;
    private Label _status;
    private Label _knowledge;
    private Label _prompt;
    private Camera2D _camera;
    private PlayerController _player;
    private SecretDocument _document;
    private Vector2 _lastSize;
    private ProgressBar _timeline;
    private Vector2 _panelHeights;
    private Label _eventLog;
    private readonly List<string> _eventLines = new();

    public override void _Ready()
    {
        _time = GetNode<Label>("Screen/Top/Rows/Time");
        _status = GetNode<Label>("Screen/Top/Rows/Status");
        _knowledge = GetNode<Label>("Screen/Bottom/Rows/Knowledge");
        _prompt = GetNode<Label>("Screen/Bottom/Rows/Prompt");
        _camera = GetParent().GetNode<Camera2D>("Camera2D");
        _player = GetParent().GetNode<PlayerController>("Player");
        _document = GetParent().GetNode<SecretDocument>("SecretDocument");
        _timeline = GetNode<ProgressBar>("Screen/Top/Rows/Timeline");
        _eventLog = GetNode<Label>("Screen/EventLog/Rows/Text");
        EventBus.Instance.Prototype += OnPrototypeEvent;
        GetNode<Label>("Screen/Bottom/Rows/DebugControls").Visible = false;
        GetNode<Control>("Screen/EventLog").Visible = false;
        GetNode<Label>("Screen/Bottom/Rows/Controls").Text = OS.IsDebugBuild()
            ? "WASD / arrows   Move     E / Space   Interact     R   Restart loop     F1   Test controls"
            : "WASD / arrows   Move     E / Space   Interact";
        Refresh();
    }

    public override void _Process(double delta) => Refresh();

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (OS.IsDebugBuild() && @event is InputEventKey key && key.Pressed && !key.Echo && key.Keycode == Key.F1)
        {
            var controls = GetNode<Label>("Screen/Bottom/Rows/DebugControls");
            controls.Visible = !controls.Visible;
            GetNode<Control>("Screen/EventLog").Visible = controls.Visible;
            GetViewport().SetInputAsHandled();
        }
    }

    private void Refresh()
    {
        var clock = GameClock.Instance;
        int seconds = (int)Math.Floor(clock.CurrentTime);
        int remaining = (int)Math.Ceiling(Math.Max(0, clock.LoopDurationSeconds - clock.CurrentTime));
        _time.Text = $"{seconds / 60:00}:{seconds % 60:00}    |    Loop {LoopManager.Instance.LoopNumber}";
        _timeline.Value = clock.LoopDurationSeconds > 0 ? remaining / clock.LoopDurationSeconds * 100 : 0;
        _status.Text = clock.IsPaused ? "CLOCK PAUSED — movement still works" : $"{remaining / 60:00}:{remaining % 60:00} remaining   /   Clock {clock.TimeScale:0.#}x";
        _knowledge.Text = KnowledgeManager.Instance.Knows(KnowledgeFacts.SecretKnown) ? "MEMORY  /  Field note KNOWN · retained across loops" : "MEMORY  /  Field note not learned";
        bool nearby = !_document.InspectedThisLoop && _player.GetNode<Area2D>("InteractionArea").OverlapsArea(_document);
        bool nearTheo = _player.GetNode<Area2D>("InteractionArea").OverlapsArea(GetParent().GetNode<Area2D>("NPCs/Theo/InteractionArea"));
        _prompt.Text = nearTheo && !WorldState.Instance.GetFact(WorldFact.PrototypeTheoDistracted) && clock.CurrentTime < 35
            ? "[E / Space] Ask Theo to search nearby"
            : nearby ? "[E / Space] Read field note" : _document.InspectedThisLoop
            ? "The note is gone. What you learned remains."
            : "Explore the block. Find the field note near the crossing.";

        Vector2 size = GetViewport().GetVisibleRect().Size;
        Vector2 panels = new(GetNode<Control>("Screen/Top").Size.Y, GetNode<Control>("Screen/Bottom").Size.Y);
        if (size != _lastSize || panels != _panelHeights)
        {
            _lastSize = size;
            _panelHeights = panels;
            // Reserve screen space for the controls instead of covering the map.
            float zoom = Mathf.Max(0.1f, Mathf.Min((size.X - 32) / 1120f, (size.Y - panels.X - panels.Y - 36) / 800f));
            _camera.Zoom = Vector2.One * zoom;
            _camera.Offset = new Vector2(0, (panels.Y - panels.X) / (2 * zoom));
        }
    }

    private void OnPrototypeEvent(PrototypeEvent eventId, double time, string detail)
    {
        _eventLines.Add($"{time / 60:00}:{time % 60:00.0}  {eventId}");
        while (_eventLines.Count > 9) _eventLines.RemoveAt(0);
        _eventLog.Text = string.Join("\n", _eventLines);
    }

    public override void _ExitTree()
    {
        if (EventBus.Instance != null) EventBus.Instance.Prototype -= OnPrototypeEvent;
    }
}
