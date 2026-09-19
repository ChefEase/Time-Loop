using Godot;

public partial class CausalityTest : Node
{
    public override void _Ready() => CallDeferred(MethodName.RunTests);

    private void RunTests()
    {
        RunScenario("TEST 1 — RUTH AVAILABLE", true);
        GD.Print("\n################################\n");
        RunScenario("TEST 2 — RUTH UNAVAILABLE", false);
        TestCausalQuery();
        GetTree().Quit();
    }

    private void RunScenario(string title, bool ruthAvailable)
    {
        WorldState? world = WorldState.Instance;
        if (world == null) { GD.PushError("WorldState was not loaded."); return; }
        world.ResetToDefaults();
        CausalityManager.Instance?.ResetForNewLoop();
        world.SetBool(WorldFactIds.RuthAvailable, ruthAvailable);
        world.SetBool(WorldFactIds.DanielNearby, true);
        world.SetBool(WorldFactIds.JonahOnDeliveryRoute, true);
        GD.Print($"==============================\n{title}\n==============================");
        EventBus.Instance?.Publish(new GameEvent(GameEventId.TheoReportsDaniel, "Theo", "Ruth"));
        bool chase = world.GetBool(WorldFactIds.DanielFleeing);
        bool injured = world.GetBool(WorldFactIds.JonahInjured);
        bool dropped = world.GetBool(WorldFactIds.RelayDropped);
        GD.Print($"Final: Daniel fleeing={chase}, Jonah injured={injured}, Relay dropped={dropped}");
        bool expected = ruthAvailable;
        if (chase == expected && injured == expected && dropped == expected) GD.Print("PASS: condition changed the timeline.");
        else GD.PushError("FAIL: causal condition did not produce the expected timeline.");
    }

    private void TestCausalQuery()
    {
        GD.Print("\nCAUSES OF JONAH INJURED:");
        if (CausalityManager.Instance == null) return;
        foreach (CausalRule rule in CausalityManager.Instance.GetRulesProducing(GameEventId.JonahInjured))
            GD.Print($"{rule.Trigger} -> JonahInjured");
    }
}
