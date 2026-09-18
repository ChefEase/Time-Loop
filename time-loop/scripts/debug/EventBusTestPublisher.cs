using Godot;

// Synthetic events only: never attach this test helper in normal gameplay.
public partial class EventBusTestPublisher : Node
{
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is not InputEventKey key || !key.Pressed || key.Echo)
            return;

        switch (key.Keycode)
        {
            case Key.T:
                EventBus.Instance.Publish(new DoorOpenedEvent(
                    DoorId: "test_door",
                    OpenedById: "avery",
                    GameTime: 30.0));
                break;
            case Key.Y:
                EventBus.Instance.Publish(new ItemTakenEvent(
                    ItemId: "m03_key",
                    TakenById: "daniel",
                    PreviousOwnerId: "mercer",
                    GameTime: 200.0));
                break;
            default:
                return;
        }

        GetViewport().SetInputAsHandled();
    }
}
