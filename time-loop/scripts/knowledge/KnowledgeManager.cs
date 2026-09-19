using Godot;
using System.Collections.Generic;

// Only learned information belongs here, never physical inventory/world facts.
public partial class KnowledgeManager : Node
{
    public static KnowledgeManager Instance { get; private set; } = null!;
    private readonly HashSet<string> _knownFacts = new();
    [Signal]
    public delegate void KnowledgeLearnedEventHandler(string factId);

    public override void _EnterTree()
    {
        if (Instance != null && Instance != this)
        {
            GD.PushError("More than one KnowledgeManager exists.");
            QueueFree();
            return;
        }
        Instance = this;
        GD.Print("KnowledgeManager ready.");
    }

    public bool Learn(string factId)
    {
        if (string.IsNullOrWhiteSpace(factId))
        {
            GD.PushWarning("Tried to learn an empty knowledge fact.");
            return false;
        }
        if (!_knownFacts.Add(factId)) return false;
        GD.Print($"KNOWLEDGE LEARNED: {factId}");
        EmitSignal(SignalName.KnowledgeLearned, factId);
        return true;
    }

    public bool Knows(string factId) => _knownFacts.Contains(factId);
    public int GetKnownFactCount() => _knownFacts.Count;
    // Compatibility with the Phase 8 test API.
    public int GetKnowledgeCount() => GetKnownFactCount();

    // Explicit new-game/debug action only; never called by LoopManager.
    public void ClearAllForNewGame()
    {
        _knownFacts.Clear();
        GD.Print("All knowledge cleared for new game.");
    }
    public override void _ExitTree()
    {
        if (Instance == this) Instance = null!;
    }
}
