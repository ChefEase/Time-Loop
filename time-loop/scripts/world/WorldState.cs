using Godot;
using System;
using System.Collections.Generic;

public partial class WorldState : Node
{
    public static WorldState Instance { get; private set; } = null!;

    public event Action<WorldFact, bool>? FactChanged;
    public event Action? WorldReset;

    private readonly Dictionary<WorldFact, bool> _facts = new();
    private readonly Dictionary<string, bool> _causalFacts = new();
    private static readonly Dictionary<string, bool> DefaultCausalFacts = new()
    {
        { WorldFactIds.RuthAvailable, true },
        { WorldFactIds.DanielNearby, true },
        { WorldFactIds.DanielFleeing, false },
        { WorldFactIds.JonahOnDeliveryRoute, true },
        { WorldFactIds.JonahInjured, false },
        { WorldFactIds.RelayDelivered, false },
        { WorldFactIds.RelayDropped, false },
        { WorldFactIds.MercerAlive, true },
        { WorldFactIds.CoolantActive, false },
        { WorldFactIds.GridStable, false },
        { WorldFactIds.ArthurAgreesToAbort, false }
    };

    private static readonly Dictionary<WorldFact, bool> DefaultFacts = new()
    {
        { WorldFact.TestDoorOpen, false },

        { WorldFact.MercerIsAlive, true },
        { WorldFact.DanielHasKey, false },
        { WorldFact.JonahIsInjured, false },
        { WorldFact.RelayDelivered, false },
        { WorldFact.CoolantActive, false }
        ,{ WorldFact.PrototypeDanielHasKey, false }
        ,{ WorldFact.PrototypeTheoDistracted, false }
        ,{ WorldFact.PrototypeTheoWitnessedTheft, false }
        ,{ WorldFact.PrototypeTheoReportedDaniel, false }
        ,{ WorldFact.PrototypeRuthChasingDaniel, false }
        ,{ WorldFact.PrototypeJonahInjured, false }
        ,{ WorldFact.PrototypeRelayDelivered, false }
        ,{ WorldFact.PrototypeMachineStable, false }
        ,{ WorldFact.PrototypeMachineDestabilized, false }
    };

    public override void _Ready()
    {
        Instance = this;

        ResetToDefaults();

        GD.Print("[WorldState] Ready.");
    }

    public bool GetBool(string factId)
    {
        if (_causalFacts.TryGetValue(factId, out bool value)) return value;
        GD.PushWarning($"[WorldState] Requested unknown causal bool fact: {factId}");
        return false;
    }

    public void SetBool(string factId, bool value)
    {
        bool previous = GetBool(factId);
        _causalFacts[factId] = value;
        if (previous != value) GD.Print($"[WorldState] {factId}: {previous} -> {value}");
    }

    public bool HasBool(string factId) => _causalFacts.ContainsKey(factId);

    public bool GetFact(WorldFact fact)
    {
        if (_facts.TryGetValue(fact, out bool value))
        {
            return value;
        }

        return false;
    }

    public void SetFact(WorldFact fact, bool value)
    {
        bool oldValue = GetFact(fact);

        if (oldValue == value)
        {
            return;
        }

        _facts[fact] = value;

        GD.Print($"[WorldState] {fact}: {oldValue} -> {value}");

        FactChanged?.Invoke(fact, value);
    }

    public void ResetToDefaults()
    {
        _facts.Clear();

        foreach (var pair in DefaultFacts)
        {
            _facts[pair.Key] = pair.Value;
        }

        _causalFacts.Clear();
        foreach (var pair in DefaultCausalFacts) _causalFacts[pair.Key] = pair.Value;

        GD.Print("[WorldState] World facts reset to defaults.");
        WorldReset?.Invoke();
    }

    public override void _ExitTree()
    {
        if (Instance == this)
        {
            Instance = null!;
        }
    }
}
