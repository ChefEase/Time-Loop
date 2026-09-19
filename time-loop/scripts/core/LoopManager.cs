using Godot;
using System;
using System.Threading.Tasks;

public partial class LoopManager : Node
{
    public static LoopManager Instance { get; private set; } = null!;
    public int LoopNumber { get; private set; } = 1;
    public bool IsResetting { get; private set; }
    private GameClock _clock;

    public override void _EnterTree()
    {
        Instance = this;
        ProcessMode = ProcessModeEnum.Always;
    }

    public void RegisterClock(GameClock clock)
    {
        if (_clock != null) _clock.LoopEnded -= OnLoopEnded;
        _clock = clock;
        _clock.LoopEnded += OnLoopEnded;
        GD.Print($"Clock registered for loop {LoopNumber}.");
    }

    public void UnregisterClock(GameClock clock)
    {
        if (_clock != clock) return;
        _clock.LoopEnded -= OnLoopEnded;
        _clock = null;
    }

    private void OnLoopEnded() => RequestReset();
    public void RequestReset() => _ = ResetLoopAsync();

    public async Task ResetLoopAsync()
    {
        if (IsResetting) return;
        var tree = GetTree();
        if (tree.CurrentScene == null || string.IsNullOrEmpty(tree.CurrentScene.SceneFilePath))
        {
            GD.PushError("Cannot reset: current scene has no saved scene path.");
            return;
        }
        IsResetting = true;
        bool wasPaused = tree.Paused;
        int previousLoop = LoopNumber;
        bool reloaded = false;
        tree.Paused = true;
        try
        {
            GD.Print($"Ending loop {LoopNumber}.");
            await LoopTransition.Instance.FadeToBlack();
            CausalityManager.Instance?.ResetForNewLoop();
            LoopNumber++;
            Error result = tree.ReloadCurrentScene();
            if (result != Error.Ok)
                throw new InvalidOperationException($"Scene reload failed: {result}");
            reloaded = true;
            await ToSignal(tree, SceneTree.SignalName.SceneChanged);
            await LoopTransition.Instance.FadeFromBlack();
            GD.Print($"Beginning loop {LoopNumber}.");
        }
        catch (Exception error)
        {
            if (!reloaded) LoopNumber = previousLoop;
            GD.PushError($"Loop reset failed: {error.Message}");
            LoopTransition.Instance?.Clear();
        }
        finally
        {
            tree.Paused = wasPaused;
            IsResetting = false;
        }
    }

    public override void _ExitTree()
    {
        if (_clock != null) UnregisterClock(_clock);
        if (Instance == this) Instance = null!;
    }
}
