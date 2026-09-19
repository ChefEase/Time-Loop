using Godot;

public partial class SecretDocument : Area2D, IInteractable
{
    public bool InspectedThisLoop { get; private set; }

    public void Interact(Node interactor)
    {
        if (LoopManager.Instance.IsResetting || InspectedThisLoop) return;
        InspectedThisLoop = true;
        GD.Print("The document contains a hidden secret.");
        bool newlyLearned = KnowledgeManager.Instance.Learn(KnowledgeFacts.SecretKnown);
        GD.Print(newlyLearned ? "NEW KNOWLEDGE: Avery learned the secret." : "Avery already remembers this.");
        Visible = false;
        // An invisible inspected document must not steal subsequent interactions.
        SetDeferred(PropertyName.CollisionLayer, 0);
    }
}
