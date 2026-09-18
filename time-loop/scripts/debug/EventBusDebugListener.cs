using System;
using Godot;

public partial class EventBusDebugListener : Node
{
    private EventBus _bus = null!;

    public override void _EnterTree()
    {
        _bus = EventBus.Instance;
        _bus.DoorOpened += OnDoorOpened;
        _bus.ItemTaken += OnItemTaken;
    }

    public override void _ExitTree()
    {
        // Unsubscribe from the same bus we originally subscribed to.
        if (_bus == null)
            return;

        _bus.DoorOpened -= OnDoorOpened;
        _bus.ItemTaken -= OnItemTaken;
        _bus = null!;
    }

    private void OnDoorOpened(DoorOpenedEvent eventData)
    {
        GD.Print(FormattableString.Invariant(
            $"[EVENT] DoorOpened | Door={eventData.DoorId} | OpenedBy={eventData.OpenedById} | Time={eventData.GameTime}"));
    }

    private void OnItemTaken(ItemTakenEvent eventData)
    {
        GD.Print(FormattableString.Invariant(
            $"[EVENT] ItemTaken | Item={eventData.ItemId} | TakenBy={eventData.TakenById} | PreviousOwner={eventData.PreviousOwnerId} | Time={eventData.GameTime}"));
    }
}
