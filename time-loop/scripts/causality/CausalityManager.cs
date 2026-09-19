using Godot;
using System.Collections.Generic;

public partial class CausalityManager : Node
{
    public static CausalityManager Instance { get; private set; } = null!;
    private CausalRuleRegistry _registry = null!;
    private readonly HashSet<string> _firedRuleIds = new();
    private readonly Queue<GameEvent> _eventQueue = new();
    private bool _isProcessingQueue;

    public override void _Ready()
    {
        Instance = this;
        _registry = new CausalRuleRegistry();
        if (EventBus.Instance != null) EventBus.Instance.GameEventOccurred += OnGameEventOccurred;
        GD.Print($"[Causality] Ready. Loaded {_registry.GetAllRules().Count} rules.");
    }

    private void OnGameEventOccurred(GameEvent gameEvent)
    {
        _eventQueue.Enqueue(gameEvent);
        if (!_isProcessingQueue) ProcessEventQueue();
    }

    private void ProcessEventQueue()
    {
        _isProcessingQueue = true;
        try { while (_eventQueue.Count > 0) ProcessEvent(_eventQueue.Dequeue()); }
        finally { _isProcessingQueue = false; }
    }

    private void ProcessEvent(GameEvent gameEvent)
    {
        GD.Print($"[EVENT] {gameEvent}");
        foreach (CausalRule rule in _registry.GetRulesTriggeredBy(gameEvent.Id)) TryFireRule(rule);
    }

    private void TryFireRule(CausalRule rule)
    {
        if (rule.FireOncePerLoop && !_firedRuleIds.Add(rule.Id))
        {
            GD.Print($"[CAUSALITY] SKIP already fired: {rule.Id}");
            return;
        }

        WorldState? worldState = WorldState.Instance;
        if (worldState == null) { GD.PushError("[Causality] WorldState.Instance is null."); return; }
        if (!rule.ConditionsAreSatisfied(worldState))
        {
            if (rule.FireOncePerLoop) _firedRuleIds.Remove(rule.Id);
            GD.Print($"[CAUSALITY] CONDITIONS FAILED: {rule.Id}");
            return;
        }

        GD.Print($"[CAUSALITY] RULE FIRED: {rule.Id}");
        foreach (CausalEffect effect in rule.Effects) ExecuteEffect(effect, worldState);
    }

    private void ExecuteEffect(CausalEffect effect, WorldState worldState)
    {
        if (effect.Type == CausalEffectType.SetWorldBool)
            worldState.SetBool(effect.FactId, effect.BoolValue);
        else if (effect.Type == CausalEffectType.PublishEvent)
            EventBus.Instance?.Publish(new GameEvent(effect.EventId));
    }

    public void ResetForNewLoop()
    {
        _firedRuleIds.Clear();
        _eventQueue.Clear();
        GD.Print("[Causality] Reset for new loop.");
    }

    public IReadOnlyList<CausalRule> GetRulesTriggeredBy(GameEventId eventId) => _registry.GetRulesTriggeredBy(eventId);
    public IReadOnlyList<CausalRule> GetRulesProducing(GameEventId eventId) => _registry.GetRulesProducing(eventId);

    public override void _ExitTree()
    {
        if (EventBus.Instance != null) EventBus.Instance.GameEventOccurred -= OnGameEventOccurred;
        if (Instance == this) Instance = null!;
    }
}
