using Godot;

public partial class PrototypeDanielDialogue : Area2D, IInteractable
{
    public void Interact(Node interactor)
    {
        DialogueManager.Instance?.Start("DanielTest");
    }
}
