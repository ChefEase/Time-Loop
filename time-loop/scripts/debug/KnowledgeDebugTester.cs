using Godot;

public partial class KnowledgeDebugTester : Node
{
    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (!OS.IsDebugBuild() || @event is not InputEventKey key || !key.Pressed || key.Echo)
            return;
        switch (key.Keycode)
        {
            case Key.K:
                KnowledgeManager.Instance.Learn(KnowledgeFacts.SecretKnown);
                break;
            case Key.C:
                GD.Print($"Does Avery know the secret? {KnowledgeManager.Instance.Knows(KnowledgeFacts.SecretKnown)}");
                break;
            case Key.N:
                KnowledgeManager.Instance.ClearAllForNewGame();
                GD.Print("DEBUG: Knowledge erased; world and loop number unchanged.");
                break;
            default:
                return;
        }
        GetViewport().SetInputAsHandled();
        // R stays owned by DebugClockControls and uses the real LoopManager.
    }
}
