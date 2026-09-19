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

    public override void _Ready()
    {
        // The addon's sample scene is authored for a very large reference
        // viewport. Re-anchor it for the prototype's normal game window.
        CallDeferred(MethodName.ApplyReadableLayout);
    }

    private void ApplyReadableLayout()
    {
        Control line = GetNode<Control>("DefaultDialogueSystem/LinePresenter/PresenterControl/LineText");
        Control name = GetNode<Control>("DefaultDialogueSystem/LinePresenter/PresenterControl/CharacterNameText");
        Control button = GetNode<Control>("DefaultDialogueSystem/LinePresenter/PresenterControl/ContinueButton");
        Control options = GetNode<Control>("DefaultDialogueSystem/OptionsPresenter/PresenterControl/VBoxContainer");

        SetRect(line, 0.08f, 0.67f, 0.92f, 0.91f);
        SetRect(name, 0.09f, 0.59f, 0.42f, 0.68f);
        SetRect(button, 0.78f, 0.825f, 0.91f, 0.895f);
        SetRect(options, 0.10f, 0.28f, 0.90f, 0.82f);

        if (line is RichTextLabel lineText)
        {
            lineText.AddThemeFontSizeOverride("normal_font_size", 24);
            lineText.AddThemeFontSizeOverride("bold_font_size", 24);
            lineText.FitContent = false;
        }

        if (name is RichTextLabel nameText)
        {
            nameText.AddThemeFontSizeOverride("normal_font_size", 18);
            nameText.FitContent = false;
        }

        if (button is Button continueButton)
        {
            continueButton.Text = "Continue  ›";
            continueButton.AddThemeFontSizeOverride("font_size", 18);
        }
    }

    private static void SetRect(Control control, float left, float top, float right, float bottom)
    {
        control.AnchorLeft = left;
        control.AnchorTop = top;
        control.AnchorRight = right;
        control.AnchorBottom = bottom;
        control.OffsetLeft = 0;
        control.OffsetTop = 0;
        control.OffsetRight = 0;
        control.OffsetBottom = 0;
    }
}
