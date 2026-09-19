using System.Collections.Generic;

public sealed class CausalRule
{
    public string Id { get; }
    public string Description { get; }
    public GameEventId Trigger { get; }
    public IReadOnlyList<WorldCondition> Conditions { get; }
    public IReadOnlyList<CausalEffect> Effects { get; }
    public bool FireOncePerLoop { get; }

    public CausalRule(string id, string description, GameEventId trigger, IEnumerable<WorldCondition> conditions, IEnumerable<CausalEffect> effects, bool fireOncePerLoop = true)
    {
        Id = id;
        Description = description;
        Trigger = trigger;
        Conditions = new List<WorldCondition>(conditions);
        Effects = new List<CausalEffect>(effects);
        FireOncePerLoop = fireOncePerLoop;
    }

    public bool ConditionsAreSatisfied(WorldState worldState)
    {
        foreach (WorldCondition condition in Conditions)
            if (!condition.IsSatisfied(worldState)) return false;
        return true;
    }
}
