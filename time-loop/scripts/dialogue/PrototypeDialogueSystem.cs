using Godot;
using YarnSpinnerGodot;

// Assigns the compiled project before the supplied presenters enter _Ready.
public partial class PrototypeDialogueSystem : CanvasLayer
{
    public override void _EnterTree()
    {
        DialogueRunner? runner = GetNodeOrNull<DialogueRunner>("DefaultDialogueSystem/DialogueRunner");
        YarnProject? project = ResourceLoader.Load<YarnProject>("res://dialogue/prototype/PrototypeDialogue.yarnproject");
        if (runner == null || project == null)
        {
            GD.PushError("Prototype dialogue system could not load its Yarn Project.");
            return;
        }
        runner.SetProject(project);
    }
}
