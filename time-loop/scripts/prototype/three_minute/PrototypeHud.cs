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
    private Area2D _danielDialogue;
    private Vector2 _lastSize;
    private ProgressBar _timeline;
    private Vector2 _panelHeights;
    private Label _eventLog;
    private Label _causalFeed;
    private Label _impactBanner;
    private Tween _impactTween;
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
        _danielDialogue = GetParent().GetNode<Area2D>("NPCs/Daniel/DialogueArea");
        _timeline = GetNode<ProgressBar>("Screen/Top/Rows/Timeline");
        _eventLog = GetNode<Label>("Screen/EventLog/Rows/Text");
        _causalFeed = GetNode<Label>("Screen/CausalFeed/Rows/Text");
        _impactBanner = GetNode<Label>("Screen/ImpactBanner");
        EventBus.Instance.Prototype += OnPrototypeEvent;
        EventBus.Instance.GameEventOccurred += OnGameEvent;
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
        bool nearDaniel = _player.GetNode<Area2D>("InteractionArea").OverlapsArea(_danielDialogue);
        _prompt.Text = nearTheo && !WorldState.Instance.GetFact(WorldFact.PrototypeTheoDistracted) && clock.CurrentTime < 35
            ? "[E / Space] Ask Theo to search nearby"
            : nearDaniel ? "[E / Space] Talk to Daniel"
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

        switch (eventId)
        {
            case PrototypeEvent.TheftReported:
                ShowCausal("THEO REPORTS  →  RUTH IS AVAILABLE  →  CHASE STARTS", "THEO REPORTED DANIEL");
                break;
            case PrototypeEvent.PoliceChaseStarted:
                ShowCausal("RUTH CHASES  →  DANIEL SWITCHES TO ESCAPE ROUTE", "RUTH STARTED THE CHASE");
                break;
            case PrototypeEvent.CourierCollision:
                ShowCausal("DANIEL FLEES + JONAH ON ROUTE  →  COLLISION", "COLLISION!");
                break;
            case PrototypeEvent.CourierInjured:
                ShowCausal("COLLISION  →  JONAH INJURED  →  RELAY DROPPED", "JONAH WAS INJURED — RELAY DROPPED");
                break;
            case PrototypeEvent.RelayDelivered:
                ShowCausal("JONAH REACHES MARA  →  RELAY DELIVERED", "RELAY DELIVERED");
                break;
            case PrototypeEvent.MachineDestabilized:
                ShowCausal("RELAY DROPPED  →  MACHINE UNSTABLE  →  FAILURE AHEAD", "MACHINE UNSTABLE");
                break;
            case PrototypeEvent.PrototypeExplosion:
                ShowCausal("MACHINE UNSTABLE  →  THREE MINUTES  →  EXPLOSION", "MACHINE FAILURE");
                break;
            case PrototypeEvent.MachineStabilized:
                ShowCausal("RELAY DELIVERED  →  MARA INSTALLS IT  →  MACHINE STABLE", "MACHINE STABLE");
                break;
            case PrototypeEvent.PrototypeSuccess:
                ShowCausal("THE COLLISION WAS PREVENTED  →  RELAY ARRIVED  →  NO EXPLOSION", "THE CHAIN CHANGED");
                break;
        }
    }

    private void OnGameEvent(GameEvent gameEvent)
    {
        if (gameEvent.Id == GameEventId.RuthBeginsChase)
            ShowCausal("THEO REPORTS DANIEL  →  RUTH AVAILABLE  →  RUTH CHASES", "CAUSE FOUND: REPORT → CHASE");
        else if (gameEvent.Id == GameEventId.DanielFlees)
            ShowCausal("RUTH CHASES  →  DANIEL FLEES", "DANIEL CHANGED ROUTE");
        else if (gameEvent.Id == GameEventId.DanielCollidesWithJonah)
            ShowCausal("DANIEL FLEES + JONAH ON DELIVERY ROUTE  →  COLLISION", "CAUSE FOUND: ESCAPE ROUTE → COLLISION");
    }

    private void ShowCausal(string explanation, string banner)
    {
        _causalFeed.Text = explanation;
        _impactBanner.Text = banner;
        _impactBanner.Visible = true;
        Color color = _impactBanner.Modulate;
        color.A = 1.0f;
        _impactBanner.Modulate = color;
        _impactTween?.Kill();
        _impactTween = CreateTween();
        _impactTween.TweenInterval(1.8f);
        _impactTween.TweenProperty(_impactBanner, "modulate:a", 0.0f, 0.8f);
        _impactTween.TweenCallback(Callable.From(() => _impactBanner.Visible = false));
    }

    public override void _ExitTree()
    {
        if (EventBus.Instance != null) EventBus.Instance.Prototype -= OnPrototypeEvent;
        if (EventBus.Instance != null) EventBus.Instance.GameEventOccurred -= OnGameEvent;
    }
}
