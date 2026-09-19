using Godot;

public partial class PrototypeTheoInteraction : Area2D, IInteractable
{
    private PrototypeController _controller;
    public override void _Ready() => _controller = GetTree().CurrentScene.GetNode<PrototypeController>("PrototypeController");
    public void Interact(Node interactor)
    {
        if (GameClock.Instance.CurrentTime >= 35)
        {
            _controller.SetMessage("Theo is focused on something down the street. Too late.");
            return;
        }
        if (WorldState.Instance.GetFact(WorldFact.PrototypeTheoDistracted)) return;
        WorldState.Instance.SetFact(WorldFact.PrototypeTheoDistracted, true);
        EventBus.Instance.Publish(PrototypeEvent.TheoDistracted, GameClock.Instance.CurrentTime, "Avery asked Theo to search nearby.");
        _controller.SetMessage("Theo is distracted and searching away from the key rack.");
        GetParent<NpcController>().MoveTo(GetTree().CurrentScene.GetNode<Marker2D>("Markers/TheoSearchMarker").GlobalPosition);
    }
}
