using Godot;

public partial class PrototypeDanielDialogue : Area2D, IInteractable
{
    public void Interact(Node interactor)
    {
        KnowledgeManager.Instance?.Learn(KnowledgeFacts.PersonDaniel);
        DialogueManager.Instance?.Start("DanielTest");
    }
}
