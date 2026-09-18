using Godot;
using System;
using System.Collections.Generic;

public partial class WorldState : Node
{
    public static WorldState Instance { get; private set; } = null!;

    public event Action<WorldFact, bool>? FactChanged;
    public event Action? WorldReset;

    private readonly Dictionary<WorldFact, bool> _facts = new();

    private static readonly Dictionary<WorldFact, bool> DefaultFacts = new()
    {
        { WorldFact.TestDoorOpen, false },

        { WorldFact.MercerIsAlive, true },
        { WorldFact.DanielHasKey, false },
        { WorldFact.JonahIsInjured, false },
        { WorldFact.RelayDelivered, false },
        { WorldFact.CoolantActive, false }
    };

    public override void _Ready()
    {
        Instance = this;

        ResetToDefaults();

        GD.Print("[WorldState] Ready.");
    }

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
