extends SceneTree

func _initialize():
    run.call_deferred()

func require(condition, message):
    if not condition:
        push_error("FAIL: " + message)
        quit(1)
        return false
    print("PASS: " + message)
    return true

func run():
    var scene = load("res://scenes/prototype/NpcScheduleTest.tscn").instantiate()
    root.add_child(scene)
    var clock = scene.get_node("GameClock")
    clock.set_physics_process(false)
    var npc = scene.get_node("NPC_A")
    var runner = npc.get_node("ScheduleRunner")
    runner.set_physics_process(false)
    for i in range(3): await process_frame
    if not require(npc.Definition.Id == &"npc_a", "Identity resource loads"): return
    if not require(runner.Schedule.Entries.size() == 4, "Four schedule resources load"): return
    var spawn = scene.get_node("Destinations/Spawn").global_position
    var point_b = scene.get_node("Destinations/PointB").global_position
    var point_c = scene.get_node("Destinations/PointC").global_position
    npc.MoveTo(point_b)
    for i in range(240): await process_frame
    if not require(npc.global_position.distance_to(point_b) <= 4.1, "Direct MoveTo reaches PointB before scheduling"): return
    npc.MoveTo(point_c)
    for i in range(10): await process_frame
    npc.StopMoving()
    var stopped = npc.global_position
    for i in range(20): await process_frame
    if not require(npc.global_position.distance_to(stopped) < 0.01, "StopMoving stops body"): return
    npc.global_position = spawn
    clock.ResetClock()
    clock.SetTimeScale(1.0)
    clock.set_physics_process(true)
    runner.set_physics_process(true)
    while clock.CurrentTime < 19.9: await process_frame
    if not require(npc.global_position.distance_to(spawn) < 0.01, "00:00-00:20 waits at Spawn"): return
    while clock.CurrentTime < 20.5: await process_frame
    if not require(npc.global_position.x > spawn.x + 20, "00:20 starts moving without input"): return
    while clock.CurrentTime < 25.0: await process_frame
    if not require(npc.global_position.distance_to(point_b) <= 4.1, "Arrives at PointB"): return
    while clock.CurrentTime < 40.1: await process_frame
    var wait_position = npc.global_position
    while clock.CurrentTime < 59.9: await process_frame
    if not require(npc.global_position.distance_to(wait_position) < 0.01 and npc.velocity == Vector2.ZERO, "00:40 Wait remains stationary until 01:00"): return
    while clock.CurrentTime < 60.5: await process_frame
    if not require(npc.global_position.distance_to(wait_position) > 20, "01:00 automatically moves toward PointC"): return
    while clock.CurrentTime < 65.0: await process_frame
    if not require(npc.global_position.distance_to(point_c) <= 4.1 and npc.velocity == Vector2.ZERO, "Arrives and stops at PointC"): return
    print("PASS: full schedule at GameClock.TimeScale 1")
    quit(0)
