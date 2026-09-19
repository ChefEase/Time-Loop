#nullable enable
using Godot;
using System;

public partial class EventBus : Node
{
    public static EventBus Instance { get; private set; } = null!;

    public event Action<DoorOpenedEvent>? DoorOpened;
    public event Action<ItemTakenEvent>? ItemTaken;
    public event Action<GameEvent>? GameEventOccurred;
    public event Action<PrototypeEvent, double, string>? Prototype;
    public Godot.Collections.Array<int> PrototypeEventHistory { get; } = new();

    public override void _EnterTree()
    {
        if (Instance != null && Instance != this)
        {
            GD.PushError("More than one EventBus exists.");
            QueueFree();
            return;
        }

        Instance = this;
    }

    public override void _ExitTree()
    {
        if (Instance == this)
        {
            Instance = null!;
        }
    }

    public void Publish(DoorOpenedEvent eventData)
    {
        // Immediate delivery. Publishers commit state before calling this;
        // subscribers must not depend on another subscriber running first.
        DoorOpened?.Invoke(eventData);
    }

    public void Publish(ItemTakenEvent eventData)
    {
        ItemTaken?.Invoke(eventData);
    }

    public void Publish(GameEvent gameEvent)
    {
        GameEventOccurred?.Invoke(gameEvent);
    }

    public void Publish(PrototypeEvent eventId, double gameTime, string detail = "")
    {
        PrototypeEventHistory.Add((int)eventId);
        GD.Print($"[PROTOTYPE {gameTime:000.0}] {eventId}{(string.IsNullOrEmpty(detail) ? "" : $" — {detail}")}");
        Prototype?.Invoke(eventId, gameTime, detail);
    }

    public void ClearPrototypeHistory() => PrototypeEventHistory.Clear();
}
