#nullable enable
using Godot;
using System;

public partial class EventBus : Node
{
    public static EventBus Instance { get; private set; } = null!;

    public event Action<DoorOpenedEvent>? DoorOpened;
    public event Action<ItemTakenEvent>? ItemTaken;

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
}
