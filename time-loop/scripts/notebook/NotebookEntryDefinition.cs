public sealed class NotebookEntryDefinition
{
    public string FactId { get; }
    public NotebookEntryType EntryType { get; }
    public string SubjectId { get; }
    public string Title { get; }
    public string Description { get; }
    public double TimelineTime { get; }
    public int SortOrder { get; }

    public NotebookEntryDefinition(
        string factId,
        NotebookEntryType entryType,
        string subjectId,
        string title,
        string description,
        double timelineTime = -1,
        int sortOrder = 0)
    {
        FactId = factId;
        EntryType = entryType;
        SubjectId = subjectId;
        Title = title;
        Description = description;
        TimelineTime = timelineTime;
        SortOrder = sortOrder;
    }
}
