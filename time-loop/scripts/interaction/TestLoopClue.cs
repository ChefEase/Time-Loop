using Godot;

// Temporary collectible fixture, not a notebook or general inventory system.
public partial class TestLoopClue : Area2D, IInteractable
{
    public void Interact(Node interactor)
    {
        if (LoopManager.Instance.IsResetting || WorldState.Instance.GetFact(WorldFact.TestKeyTaken)) return;
        WorldState.Instance.SetFact(WorldFact.TestKeyTaken, true);
        KnowledgeManager.Instance.Learn(KnowledgeFacts.SecretKnown);
        QueueFree();
    }
}
