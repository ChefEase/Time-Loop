public sealed class WorldCondition
{
    public string FactId { get; }
    public bool ExpectedValue { get; }

    public WorldCondition(string factId, bool expectedValue)
    {
        FactId = factId;
        ExpectedValue = expectedValue;
    }

    public bool IsSatisfied(WorldState worldState) => worldState.GetBool(FactId) == ExpectedValue;
    public override string ToString() => $"{FactId} == {ExpectedValue}";
}
