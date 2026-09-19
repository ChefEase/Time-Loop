using Godot;
using System;
using System.Threading.Tasks;
using YarnSpinnerGodot;

// Owns dialogue lifecycle only. Knowledge, world state and simulation remain elsewhere.
public partial class DialogueManager : Node
{
    public static DialogueManager? Instance { get; private set; }
    public static bool IsDialogueActive { get; private set; }
    private DialogueRunner? _runner;
    private bool _clockWasPaused;
    [Export] public string DialogueProjectPath { get; set; } = "res://dialogue/prototype/PrototypeDialogue.yarnproject";

    public override void _EnterTree() => Instance = this;

    public override void _Ready()
    {
        DialogueRunner? runner = GetParent().GetNodeOrNull<DialogueRunner>("DialogueSystem/DefaultDialogueSystem/DialogueRunner");
        if (runner == null) return;
        YarnProject? project = ResourceLoader.Load<YarnProject>(DialogueProjectPath);
        if (project == null)
        {
            GD.PushError($"Dialogue project could not be loaded: {DialogueProjectPath}");
            return;
        }
        runner.SetProject(project);
        RegisterRunner(runner);
    }

    public void RegisterRunner(DialogueRunner runner)
    {
        if (_runner != null)
        {
            _runner.Disconnect(DialogueRunner.SignalName.onDialogueStart, Callable.From(OnDialogueStarted));
            _runner.Disconnect(DialogueRunner.SignalName.onDialogueComplete, Callable.From(OnDialogueCompleted));
        }
        _runner = runner;
        _runner.Connect(DialogueRunner.SignalName.onDialogueStart, Callable.From(OnDialogueStarted));
        _runner.Connect(DialogueRunner.SignalName.onDialogueComplete, Callable.From(OnDialogueCompleted));
    }

    public void Start(string nodeName)
    {
        if (IsDialogueActive)
        {
            return;
        }

        if (_runner == null)
        {
            GD.PushError("DialogueManager has no registered DialogueRunner.");
            return;
        }
        _runner.StartDialogueForget(nodeName);
    }

    public async Task StopForWorldResetAsync()
    {
        if (_runner == null || !IsDialogueActive)
        {
            return;
        }

        try
        {
            await _runner.Stop();
        }
        catch (Exception error)
        {
            GD.PushWarning($"Dialogue stopped during world reset: {error.Message}");
        }

        IsDialogueActive = false;
        PlayerController? player = GetTree().CurrentScene?.GetNodeOrNull<PlayerController>("Player");
        if (player != null)
        {
            player.DialogueLocked = false;
        }
    }

    private void OnDialogueStarted()
    {
        IsDialogueActive = true;
        if (GameClock.Instance == null) return;
        _clockWasPaused = GameClock.Instance.IsPaused;
        GameClock.Instance.PauseClock();
        PlayerController? player = GetTree().CurrentScene?.GetNodeOrNull<PlayerController>("Player");
        if (player != null) player.DialogueLocked = true;
    }

    private void OnDialogueCompleted()
    {
        IsDialogueActive = false;
        PlayerController? player = GetTree().CurrentScene?.GetNodeOrNull<PlayerController>("Player");
        if (player != null) player.DialogueLocked = false;
        if (!_clockWasPaused) GameClock.Instance?.ResumeClock();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!IsDialogueActive || @event is not InputEventKey key || !key.Pressed || key.Echo)
        {
            return;
        }

        if (key.Keycode != Key.E && key.Keycode != Key.Space && key.Keycode != Key.Enter && key.Keycode != Key.KpEnter)
        {
            return;
        }

        GetNodeOrNull<LinePresenterButtonHandler>(
            "../DialogueSystem/DefaultDialogueSystem/LinePresenter/LinePresenterButtonHandler"
        )?.OnClick();

        GetViewport().SetInputAsHandled();
    }

    public override void _ExitTree()
    {
        if (_runner != null)
        {
            if (_runner.IsConnected(DialogueRunner.SignalName.onDialogueStart, Callable.From(OnDialogueStarted)))
                _runner.Disconnect(DialogueRunner.SignalName.onDialogueStart, Callable.From(OnDialogueStarted));
            if (_runner.IsConnected(DialogueRunner.SignalName.onDialogueComplete, Callable.From(OnDialogueCompleted)))
                _runner.Disconnect(DialogueRunner.SignalName.onDialogueComplete, Callable.From(OnDialogueCompleted));
        }
        IsDialogueActive = false;
        if (Instance == this) Instance = null;
    }
}

