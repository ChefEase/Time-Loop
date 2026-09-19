using Godot;
using System.Threading.Tasks;

public partial class LoopTransition : CanvasLayer
{
    public static LoopTransition Instance { get; private set; } = null!;
    [Export] public float FadeDuration { get; set; } = 0.4f;
    private ColorRect _fade;
    public override void _Ready()
    {
        Instance = this;
        ProcessMode = ProcessModeEnum.Always;
        _fade = GetNode<ColorRect>("Fade");
        Clear();
    }
    public void Clear() => _fade.Modulate = new Color(1, 1, 1, 0);
    public Task FadeToBlack() => Fade(1);
    public Task FadeFromBlack() => Fade(0);
    private async Task Fade(float alpha)
    {
        var tween = CreateTween().SetPauseMode(Tween.TweenPauseMode.Process);
        tween.TweenProperty(_fade, "modulate:a", alpha, Mathf.Max(0.001f, FadeDuration));
        await ToSignal(tween, Tween.SignalName.Finished);
    }
    public override void _ExitTree()
    {
        if (Instance == this) Instance = null!;
    }
}
