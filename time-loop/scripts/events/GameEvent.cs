public sealed class GameEvent
{
    public GameEventId Id { get; }
    public string SourceId { get; }
    public string TargetId { get; }
    public double GameTimeSeconds { get; }

    public GameEvent(GameEventId id, string sourceId = "", string targetId = "", double gameTimeSeconds = -1)
    {
        Id = id;
        SourceId = sourceId;
        TargetId = targetId;
        GameTimeSeconds = gameTimeSeconds;
    }

    public override string ToString() => $"{Id} | Source: {SourceId} | Target: {TargetId} | Time: {GameTimeSeconds:0.0}";
}
