using System;
using System.Collections.Generic;
using System.Linq;

public sealed class CausalRuleRegistry
{
    private readonly List<CausalRule> _rules = new();

    public CausalRuleRegistry() => RegisterPrototypeRules();

    private void RegisterPrototypeRules()
    {
        AddRule(new CausalRule("theo_report_starts_ruth_chase", "Ruth chases after Theo reports Daniel when Ruth is available and Daniel is nearby.", GameEventId.TheoReportsDaniel,
            new[] { new WorldCondition(WorldFactIds.RuthAvailable, true), new WorldCondition(WorldFactIds.DanielNearby, true) },
            new[] { CausalEffect.PublishEvent(GameEventId.RuthBeginsChase) }));

        AddRule(new CausalRule("ruth_chase_makes_daniel_flee", "Daniel flees when Ruth begins chasing him.", GameEventId.RuthBeginsChase,
            Array.Empty<WorldCondition>(), new[] { CausalEffect.SetWorldBool(WorldFactIds.DanielFleeing, true), CausalEffect.PublishEvent(GameEventId.DanielFlees) }));

        AddRule(new CausalRule("daniel_fleeing_causes_collision", "A fleeing Daniel collides with Jonah while Jonah is on his delivery route.", GameEventId.DanielFlees,
            new[] { new WorldCondition(WorldFactIds.JonahOnDeliveryRoute, true), new WorldCondition(WorldFactIds.JonahAtCollisionPoint, true) }, new[] { CausalEffect.PublishEvent(GameEventId.DanielCollidesWithJonah) }));

        AddRule(new CausalRule("collision_injures_jonah", "The collision injures Jonah and drops the relay.", GameEventId.DanielCollidesWithJonah,
            Array.Empty<WorldCondition>(), new[] { CausalEffect.SetWorldBool(WorldFactIds.JonahInjured, true), CausalEffect.SetWorldBool(WorldFactIds.JonahOnDeliveryRoute, false), CausalEffect.SetWorldBool(WorldFactIds.RelayDropped, true), CausalEffect.PublishEvent(GameEventId.JonahInjured), CausalEffect.PublishEvent(GameEventId.RelayDropped) }));
    }

    public void AddRule(CausalRule rule) => _rules.Add(rule);
    public IReadOnlyList<CausalRule> GetRulesTriggeredBy(GameEventId eventId) => _rules.Where(rule => rule.Trigger == eventId).ToList();
    public IReadOnlyList<CausalRule> GetRulesProducing(GameEventId eventId) => _rules.Where(rule => rule.Effects.Any(effect => effect.Type == CausalEffectType.PublishEvent && effect.EventId == eventId)).ToList();
    public IReadOnlyList<CausalRule> GetAllRules() => _rules;
}
