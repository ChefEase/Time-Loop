using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class NotebookUI : CanvasLayer
{
    public static bool IsOpen { get; private set; }

    private readonly List<NotebookEntryDefinition> _entries = new()
    {
        new(KnowledgeFacts.PersonDaniel, NotebookEntryType.PersonFact, "daniel_cross", "Daniel Cross", "A young man seen around the restricted key rack. Occupation unknown.", sortOrder: 10),
        new(KnowledgeFacts.PersonTheo, NotebookEntryType.PersonFact, "theo_shaw", "Theo Shaw", "A street witness who watches the block carefully.", sortOrder: 20),
        new(KnowledgeFacts.PersonRuth, NotebookEntryType.PersonFact, "ruth_reed", "Ruth Reed", "Officer assigned to the local police station.", sortOrder: 30),
        new(KnowledgeFacts.TimelineDanielStoleKey, NotebookEntryType.TimelineEvent, "daniel_cross", "Daniel took the brass key", "Daniel removed a restricted brass key from Mercer.", 40, 10),
        new(KnowledgeFacts.TimelineTheoReportedDaniel, NotebookEntryType.TimelineEvent, "theo_shaw", "Theo reported Daniel", "Theo reached Officer Ruth and reported what he saw.", 50, 20),
        new(KnowledgeFacts.TimelineJonahInjured, NotebookEntryType.TimelineEvent, "jonah_price", "Jonah was injured", "A collision interrupted Jonah's relay delivery.", 73.5, 30),
        new(KnowledgeFacts.ClueBrassKey, NotebookEntryType.Clue, "daniel_cross", "Brass Maintenance Key", "A heavy brass key stamped M-03.", sortOrder: 10),
        new(KnowledgeFacts.ClueDeliveryParcel, NotebookEntryType.Clue, "jonah_price", "Delivery Parcel", "A replacement relay addressed to Mara at the machine room.", sortOrder: 20),
        new(KnowledgeFacts.ClueWitnessStatement, NotebookEntryType.Clue, "theo_shaw", "Witness Statement", "Theo says he saw Daniel take something from Mercer.", sortOrder: 30)
    };

    private Control _panel = null!;
    private Control _dimmer = null!;
    private TabContainer _tabs = null!;
    private Button _closeButton = null!;
    private VBoxContainer _peopleList = null!;
    private VBoxContainer _peopleDetails = null!;
    private VBoxContainer _timelineList = null!;
    private VBoxContainer _cluesList = null!;
    private Label _debugHint = null!;
    private Label _notification = null!;
    private bool _wasTreePaused;
    private bool _wasClockPaused;
    private string _selectedSubject = "";

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
        _panel = GetNode<Control>("NotebookPanel");
        _dimmer = GetNode<Control>("BackgroundDimmer");
        _tabs = GetNode<TabContainer>("NotebookPanel/MainVBox/Tabs");
        _closeButton = GetNode<Button>("NotebookPanel/MainVBox/Header/CloseButton");
        _peopleList = GetNode<VBoxContainer>("NotebookPanel/MainVBox/Tabs/People/PeopleSplit/PeopleListScroll/PeopleList");
        _peopleDetails = GetNode<VBoxContainer>("NotebookPanel/MainVBox/Tabs/People/PeopleSplit/PeopleDetails");
        _timelineList = GetNode<VBoxContainer>("NotebookPanel/MainVBox/Tabs/Timeline/TimelineScroll/TimelineList");
        _cluesList = GetNode<VBoxContainer>("NotebookPanel/MainVBox/Tabs/Clues/CluesScroll/CluesList");
        _debugHint = GetNode<Label>("NotebookPanel/MainVBox/DebugHint");
        _notification = GetNode<Label>("NewEntryNotification");
        _closeButton.Pressed += CloseNotebook;
        _tabs.TabChanged += _ => RefreshNotebook();
        Visible = true;
        _panel.Visible = false;
        _dimmer.Visible = false;
        IsOpen = false;
        if (KnowledgeManager.Instance != null)
        {
            KnowledgeManager.Instance.KnowledgeLearned += OnKnowledgeLearned;
        }
        RefreshNotebook();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is not InputEventKey key || !key.Pressed || key.Echo)
        {
            return;
        }

        if (key.Keycode == Key.Escape && IsOpen)
        {
            CloseNotebook();
            GetViewport().SetInputAsHandled();
            return;
        }

        if (key.Keycode == Key.N)
        {
            ToggleNotebook();
            GetViewport().SetInputAsHandled();
            return;
        }

        if (!OS.IsDebugBuild()) return;
        switch (key.Keycode)
        {
            case Key.F7: Learn(KnowledgeFacts.PersonDaniel); break;
            case Key.F8: Learn(KnowledgeFacts.DanielStoleKey); break;
            case Key.F9: Learn(KnowledgeFacts.PersonTheo); break;
            case Key.F10: Learn(KnowledgeFacts.ClueBrassKey); break;
            case Key.F11: Learn(KnowledgeFacts.TimelineJonahInjured); break;
            case Key.F12: KnowledgeManager.Instance?.ClearAllForNewGame(); RefreshNotebook(); break;
            default: return;
        }
        GetViewport().SetInputAsHandled();
    }

    public void ToggleNotebook()
    {
        if (IsOpen) CloseNotebook();
        else OpenNotebook();
    }

    public void OpenNotebook()
    {
        if (IsOpen) return;
        _wasTreePaused = GetTree().Paused;
        _wasClockPaused = GameClock.Instance?.IsPaused ?? false;
        IsOpen = true;
        _panel.Visible = true;
        _dimmer.Visible = true;
        GetTree().Paused = true;
        GameClock.Instance?.PauseClock();
        RefreshNotebook();
    }

    public void CloseNotebook()
    {
        if (!IsOpen) return;
        IsOpen = false;
        _panel.Visible = false;
        _dimmer.Visible = false;
        GetTree().Paused = _wasTreePaused;
        if (!_wasClockPaused) GameClock.Instance?.ResumeClock();
    }

    public void RefreshNotebook()
    {
        if (!IsInstanceValid(_peopleList)) return;
        RefreshPeople();
        RefreshTimeline();
        RefreshClues();
    }

    private void RefreshPeople()
    {
        Clear(_peopleList);
        var people = _entries.Where(e => e.EntryType == NotebookEntryType.PersonFact && Knows(e.FactId)).OrderBy(e => e.SortOrder).ToList();
        if (people.Count == 0)
        {
            AddEmpty(_peopleList, "No people discovered yet.");
            Clear(_peopleDetails);
            AddEmpty(_peopleDetails, "Meet someone in Greywick to begin an entry.");
            return;
        }
        foreach (var person in people)
        {
            var button = new Button { Text = person.Title, Alignment = HorizontalAlignment.Left, CustomMinimumSize = new Vector2(0, 42) };
            button.Pressed += () => { _selectedSubject = person.SubjectId; RefreshPersonDetails(person); };
            _peopleList.AddChild(button);
        }
        var selected = people.FirstOrDefault(p => p.SubjectId == _selectedSubject) ?? people[0];
        _selectedSubject = selected.SubjectId;
        RefreshPersonDetails(selected);
    }

    private void RefreshPersonDetails(NotebookEntryDefinition person)
    {
        Clear(_peopleDetails);
        AddHeading(_peopleDetails, person.Title);
        AddBody(_peopleDetails, person.Description);
        AddHeading(_peopleDetails, "Known");
        var facts = _entries.Where(e => e.SubjectId == person.SubjectId && Knows(e.FactId) && e != person).OrderBy(e => e.SortOrder).ToList();
        if (facts.Count == 0) AddBody(_peopleDetails, "No additional information recorded yet.");
        foreach (var fact in facts) AddBody(_peopleDetails, "• " + fact.Title + "\n  " + fact.Description);
        AddHeading(_peopleDetails, "Unknown");
        AddBody(_peopleDetails, "???");
    }

    private void RefreshTimeline()
    {
        Clear(_timelineList);
        var events = _entries.Where(e => e.EntryType == NotebookEntryType.TimelineEvent && Knows(e.FactId)).OrderBy(e => e.TimelineTime).ToList();
        if (events.Count == 0) { AddEmpty(_timelineList, "No observed events yet."); return; }
        foreach (var entry in events)
        {
            string time = $"{(int)entry.TimelineTime / 60:00}:{entry.TimelineTime % 60:00.0}";
            AddBody(_timelineList, $"{time}   {entry.Title}\n          {entry.Description}");
        }
    }

    private void RefreshClues()
    {
        Clear(_cluesList);
        var clues = _entries.Where(e => e.EntryType == NotebookEntryType.Clue && Knows(e.FactId)).OrderBy(e => e.SortOrder).ToList();
        if (clues.Count == 0) { AddEmpty(_cluesList, "No clues recorded yet."); return; }
        foreach (var clue in clues)
        {
            AddHeading(_cluesList, clue.Title);
            AddBody(_cluesList, clue.Description);
        }
    }

    private void OnKnowledgeLearned(string factId)
    {
        NotebookEntryDefinition? entry = _entries.FirstOrDefault(e => e.FactId == factId);
        if (entry == null) return;
        RefreshNotebook();
        _notification.Text = $"NEW NOTEBOOK ENTRY\n{entry.Title}";
        _notification.Visible = true;
        Tween tween = CreateTween();
        tween.TweenInterval(2.5);
        tween.TweenProperty(_notification, "modulate:a", 0.0f, 0.5);
        tween.TweenCallback(Callable.From(() =>
        {
            _notification.Visible = false;
            Color color = _notification.Modulate;
            color.A = 1.0f;
            _notification.Modulate = color;
        }));
    }

    private void Learn(string factId) => KnowledgeManager.Instance?.Learn(factId);
    private bool Knows(string factId) => KnowledgeManager.Instance?.Knows(factId) ?? false;
    private static void Clear(Container container) { foreach (Node child in container.GetChildren()) child.QueueFree(); }
    private static void AddEmpty(Container container, string text) => AddBody(container, text);
    private static void AddHeading(Container container, string text)
    {
        var label = new Label { Text = text };
        label.AddThemeFontSizeOverride("font_size", 20);
        container.AddChild(label);
    }

    private static void AddBody(Container container, string text)
    {
        var label = new Label
        {
            Text = text,
            AutowrapMode = TextServer.AutowrapMode.WordSmart,
            CustomMinimumSize = new Vector2(0, 34)
        };
        container.AddChild(label);
    }

    public override void _ExitTree()
    {
        if (KnowledgeManager.Instance != null) KnowledgeManager.Instance.KnowledgeLearned -= OnKnowledgeLearned;
        IsOpen = false;
    }
}
