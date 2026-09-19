public enum CausalEffectType
{
    PublishEvent,
    SetWorldBool
}

public sealed class CausalEffect
{
    public CausalEffectType Type { get; }
    public GameEventId EventId { get; }
    public string FactId { get; }
    public bool BoolValue { get; }

    private CausalEffect(CausalEffectType type, GameEventId eventId = GameEventId.None, string factId = "", bool boolValue = false)
    {
        Type = type;
        EventId = eventId;
        FactId = factId;
        BoolValue = boolValue;
    }

    public static CausalEffect PublishEvent(GameEventId eventId) => new(CausalEffectType.PublishEvent, eventId: eventId);
    public static CausalEffect SetWorldBool(string factId, bool value) => new(CausalEffectType.SetWorldBool, factId: factId, boolValue: value);
}
