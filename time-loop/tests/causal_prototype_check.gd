extends SceneTree

var events = []
var manager
var event_bus

func _initialize():
    run.call_deferred()

func require(condition, message):
    if not condition:
        push_error("FAIL: " + message)
        quit(1)
        return false
    print("PASS: " + message)
    return true

func run_until_event(target, budget = 1500):
    while not target in event_bus.PrototypeEventHistory and budget > 0:
        budget -= 1
        await process_frame
    return budget > 0

func run():
    change_scene_to_file("res://scenes/core/main.tscn")
    await scene_changed
    manager = root.get_node("LoopManager")
    event_bus = root.get_node("EventBus")
    event_bus.ClearPrototypeHistory()
    current_scene.get_node("GameClock").SetTimeScale(20.0)
    if not await run_until_event(11): return
    events = event_bus.PrototypeEventHistory
    if not require(1 in events, "Default chain steals key"): return
    if not require(2 in events, "Theo physically witnesses theft"): return
    if not require(4 in events, "Theo reports to Ruth"): return
    if not require(5 in events, "Ruth reacts to report"): return
    if not require(6 in events, "Escape route physically collides with Jonah"): return
    if not require(7 in events, "Collision injures Jonah"): return
    if not require(9 in events, "Missing relay destabilizes machine"): return
    if not require(11 in events, "Default loop reaches failure outcome"): return
    await scene_changed
    while manager.IsResetting: await process_frame
    event_bus.ClearPrototypeHistory()
    current_scene.get_node("GameClock").PauseClock()
    var theo = current_scene.get_node("NPCs/Theo/InteractionArea")
    theo.Interact(current_scene.get_node("Player"))
    if not require(current_scene.get_node("WorldState").GetFact(8), "Theo distraction sets scene-local fact"): return
    current_scene.get_node("GameClock").ResumeClock()
    current_scene.get_node("GameClock").SetTimeScale(20.0)
    if not await run_until_event(12): return
    events = event_bus.PrototypeEventHistory
    if not require(1 in events, "Alternate chain still includes theft"): return
    if not require(not 2 in events and not 4 in events, "Distracted Theo cannot witness or report"): return
    if not require(not 5 in events, "Ruth stays idle without report"): return
    if not require(not 6 in events, "Normal route avoids physical collision"): return
    if not require(8 in events, "Healthy Jonah delivers relay"): return
    if not require(10 in events, "Mara stabilizes after installation"): return
    if not require(12 in events, "Alternate loop reaches success outcome"): return
    print("PASS: complete deterministic default failure and Theo-distraction success chains")
    quit(0)
