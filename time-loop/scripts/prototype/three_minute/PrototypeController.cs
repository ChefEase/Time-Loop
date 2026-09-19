using Godot;
using System;

// Owns prototype orchestration; reusable time, facts, events and movement stay in core systems.
public partial class PrototypeController : Node
{
    private GameClock _clock;
    private WorldState _world;
    private EventBus _events;
    private NpcController _daniel, _theo, _ruth, _jonah, _mara;
    private Node2D _player;
    private Node2D _markers;
    private Label _chainStatus;
    private Label _machineStatus;
    private PrototypeCollisionTrigger _collisionTrigger;
    private bool _danielStarted, _keyStolen, _theftWitnessed, _reported, _chaseStarted, _escapeStarted;
    private bool _jonahStarted, _jonahAtCrossing, _machineChecked, _outcomeShown;
    private double _relayDeliveredAt = -1;

    public override void _Ready()
    {
        _clock = GetParent().GetNode<GameClock>("GameClock");
        _world = GetParent().GetNode<WorldState>("WorldState");
        _events = EventBus.Instance;
        _markers = GetParent().GetNode<Node2D>("Markers");
        _player = GetParent().GetNode<Node2D>("Player");
        _daniel = GetParent().GetNode<NpcController>("NPCs/Daniel");
        _theo = GetParent().GetNode<NpcController>("NPCs/Theo");
        _ruth = GetParent().GetNode<NpcController>("NPCs/Ruth");
        _jonah = GetParent().GetNode<NpcController>("NPCs/Jonah");
        _mara = GetParent().GetNode<NpcController>("NPCs/Mara");
        _chainStatus = GetParent().GetNode<Label>("UI/Screen/Bottom/Rows/ChainStatus");
        _machineStatus = GetParent().GetNode<Label>("UI/Screen/Bottom/Rows/MachineStatus");
        _collisionTrigger = GetParent().GetNode<PrototypeCollisionTrigger>("Objects/CollisionTrigger");
        _collisionTrigger.CourierCollision += OnPhysicalCollision;
        _events.Prototype += OnPrototypeEvent;
        _events.Publish(PrototypeEvent.PrototypeLoopStarted, 0, "The machine needs its replacement relay.");
        SetMessage("Observe the block. The machine fails at 03:00.");
    }

    public override void _PhysicsProcess(double delta)
    {
        double t = _clock.CurrentTime;
        // Prototype actors run in simulation time, so debug clock acceleration does not change outcomes.
        float simulationSpeed = Mathf.Max(80.0f, (float)(80.0 * _clock.TimeScale));
        _daniel.Definition.DefaultSpeed = simulationSpeed;
        _theo.Definition.DefaultSpeed = simulationSpeed;
        _ruth.Definition.DefaultSpeed = simulationSpeed;
        _jonah.Definition.DefaultSpeed = simulationSpeed;
        if (!_danielStarted && t >= 15)
        {
            _danielStarted = true;
            Move(_daniel, "DanielKeyMarker");
            Move(_theo, "TheoWatchMarker");
            SetMessage("Daniel is moving toward the key rack. Theo is watching the street.");
        }
        if (!_jonahStarted && t >= 70) { _jonahStarted = true; Move(_jonah, "CollisionMarker"); SetMessage("Jonah is carrying a relay toward the machine."); }
        if (_jonahStarted && !_jonahAtCrossing && _jonah.GlobalPosition.DistanceTo(Marker("CollisionMarker").GlobalPosition) < 32)
        {
            _jonahAtCrossing = true;
            if (!_world.GetFact(WorldFact.PrototypeJonahInjured)) Move(_jonah, "MachineEntrance");
        }
        if (!_keyStolen && t >= 40 && _daniel.GlobalPosition.DistanceTo(Marker("DanielKeyMarker").GlobalPosition) < 28)
            StealKey();
        if (_keyStolen && !_theftWitnessed && !_world.GetFact(WorldFact.PrototypeTheoDistracted) &&
            _theo.GlobalPosition.DistanceTo(Marker("DanielKeyMarker").GlobalPosition) < 85)
        {
            _theftWitnessed = true;
            _world.SetFact(WorldFact.PrototypeTheoWitnessedTheft, true);
            _events.Publish(PrototypeEvent.TheftWitnessed, t, "Theo saw Daniel take the key.");
        }
        if (_theftWitnessed && !_reported && t >= 50 && _theo.GlobalPosition.DistanceTo(Marker("PoliceEntrance").GlobalPosition) < 35)
        {
            _reported = true;
            _world.SetFact(WorldFact.PrototypeTheoReportedDaniel, true);
            _events.Publish(PrototypeEvent.TheftReported, t, "Theo reached Officer Ruth.");
        }
        if (_chaseStarted && _escapeStarted && t >= 67 && t < 70) Move(_daniel, "DanielEscape02");
        if (_chaseStarted && _escapeStarted && t >= 70 && !_world.GetFact(WorldFact.PrototypeJonahInjured)) Move(_daniel, "CollisionMarker");
        if (_keyStolen && !_chaseStarted && t >= 45 && !_world.GetFact(WorldFact.PrototypeDanielHasKey)) Move(_daniel, "DanielNormalExit");
        if (!_chaseStarted && _keyStolen && t >= 45 && _daniel.Velocity.LengthSquared() < 1) Move(_daniel, "DanielNormalExit");
        if (!_world.GetFact(WorldFact.PrototypeJonahInjured) && !_world.GetFact(WorldFact.PrototypeRelayDelivered) &&
            _jonah.GlobalPosition.DistanceTo(Marker("MachineEntrance").GlobalPosition) < 35)
        {
            _world.SetFact(WorldFact.PrototypeRelayDelivered, true);
            _relayDeliveredAt = t;
            _events.Publish(PrototypeEvent.RelayDelivered, t, "Jonah reached Mara with the relay.");
            SetMessage("Relay delivered — Mara is installing it.");
        }
        if (_relayDeliveredAt >= 0 && !_world.GetFact(WorldFact.PrototypeMachineStable) && t >= _relayDeliveredAt + 40)
        {
            _world.SetFact(WorldFact.PrototypeMachineStable, true);
            _events.Publish(PrototypeEvent.MachineStabilized, t, "Mara finished installing the relay.");
            SetMessage("MACHINE STABLE — the relay is installed.");
        }
        if (!_machineChecked && t >= 150)
        {
            _machineChecked = true;
            if (!_world.GetFact(WorldFact.PrototypeMachineStable))
            {
                _world.SetFact(WorldFact.PrototypeMachineDestabilized, true);
                _events.Publish(PrototypeEvent.MachineDestabilized, t, "The relay never reached the machine.");
                SetMessage("MACHINE UNSTABLE — no relay was installed.");
            }
        }
        if (!_outcomeShown && t >= 179.5)
        {
            _outcomeShown = true;
            if (_world.GetFact(WorldFact.PrototypeMachineStable))
            {
                _events.Publish(PrototypeEvent.PrototypeSuccess, t, "The changed chain prevented the failure.");
                SetMessage("03:00 APPROACHING — THE MACHINE REMAINS STABLE.");
            }
            else
            {
                _events.Publish(PrototypeEvent.PrototypeExplosion, t, "The machine destabilized before the loop ended.");
                SetMessage("03:00 APPROACHING — MACHINE FAILURE.");
            }
        }
        UpdateStatus(t);
    }

    private void StealKey()
    {
        _keyStolen = true;
        _world.SetFact(WorldFact.PrototypeDanielHasKey, true);
        _events.Publish(PrototypeEvent.KeyStolen, _clock.CurrentTime, "Daniel took the restricted key.");
        SetMessage("KEY TAKEN — watch who notices.");
    }

    private void OnPrototypeEvent(PrototypeEvent eventId, double time, string detail)
    {
        if (eventId == PrototypeEvent.TheftWitnessed)
        {
            Move(_theo, "PoliceEntrance");
            SetMessage("THEO SAW DANIEL — he is running to police.");
        }
        if (eventId == PrototypeEvent.TheftReported && !_chaseStarted)
        {
            KnowledgeManager.Instance?.Learn(KnowledgeFacts.PrototypeTheoReportedDaniel);
            _chaseStarted = true;
            _world.SetFact(WorldFact.PrototypeRuthChasingDaniel, true);
            _events.Publish(PrototypeEvent.PoliceChaseStarted, time, "Ruth left the station after Theo's report.");
            KnowledgeManager.Instance?.Learn(KnowledgeFacts.PrototypeReportCausesChase);
            Move(_ruth, "CollisionMarker");
            Move(_daniel, "DanielEscape01");
            _escapeStarted = true;
            SetMessage("RUTH IS CHASING DANIEL — the route changed.");
        }
    }

    private void OnPhysicalCollision()
    {
        if (_world.GetFact(WorldFact.PrototypeJonahInjured)) return;
        _world.SetFact(WorldFact.PrototypeJonahInjured, true);
        _events.Publish(PrototypeEvent.CourierCollision, _clock.CurrentTime, "Daniel and Jonah occupied the collision trigger together.");
        _events.Publish(PrototypeEvent.CourierInjured, _clock.CurrentTime, "The relay fell at the collision.");
        _jonah.StopMoving();
        SetMessage("CRASH — Jonah was injured and the relay fell.");
    }

    public void SetMessage(string message) => _chainStatus.Text = message;
    private void UpdateStatus(double t)
    {
        string state = _world.GetFact(WorldFact.PrototypeJonahInjured) ? "RELAY LOST / JONAH INJURED" :
            _world.GetFact(WorldFact.PrototypeRelayDelivered) ? "RELAY DELIVERED" : "RELAY IN TRANSIT";
        _machineStatus.Text = $"MACHINE  /  {( _world.GetFact(WorldFact.PrototypeMachineStable) ? "STABLE" : _world.GetFact(WorldFact.PrototypeMachineDestabilized) ? "UNSTABLE" : "WAITING FOR RELAY")}   ·   {state}";
    }
    private void Move(NpcController npc, string marker) => npc.MoveTo(Marker(marker).GlobalPosition);
    private Marker2D Marker(string name) => _markers.GetNode<Marker2D>(name);

    public override void _ExitTree()
    {
        if (_events != null) _events.Prototype -= OnPrototypeEvent;
        if (_collisionTrigger != null) _collisionTrigger.CourierCollision -= OnPhysicalCollision;
    }
}
