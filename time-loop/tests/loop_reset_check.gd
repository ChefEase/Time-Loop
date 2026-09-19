extends SceneTree

var failed = false
var initialized_at_zero = true

func _initialize():
    run.call_deferred()

func check(condition, message):
    if not condition:
        failed = true
        push_error("FAIL: " + message)
        quit(1)
    return condition

func on_scene_changed():
    initialized_at_zero = initialized_at_zero and current_scene.get_node("NpcScheduleTest/GameClock").CurrentTime == 0.0

func run():
    scene_changed.connect(on_scene_changed)
    change_scene_to_file("res://scenes/prototype/PrototypeWorld.tscn")
    await scene_changed
    var manager = root.get_node("LoopManager")
    var knowledge = root.get_node("KnowledgeManager")
    var transition = root.get_node("LoopTransition")
    var baseline = 0
    for cycle in range(50):
        var world = current_scene
        var clock = world.get_node("NpcScheduleTest/GameClock")
        var player = world.get_node("player")
        var npc = world.get_node("NpcScheduleTest/NPC_A")
        var start = player.position
        var npc_start = npc.position
        if not check(clock.LoopDurationSeconds == 180.0, "Saved duration remains three minutes"): return
        clock.LoopDurationSeconds = 5.0
        for i in range(4): await process_frame
        if baseline == 0: baseline = get_node_count()
        if not check(get_node_count() == baseline, "Stable node count before reset"): return
        if not check(world.get_node("TestDoor").visible, "Door starts closed"): return
        world.get_node("TestDoor").Interact(player)
        if not check(not world.get_node("TestDoor").visible, "Door opens again"): return
        world.get_node("TestKey").Interact(player)
        knowledge.Learn("test.secret_known")
        if not check(knowledge.GetKnowledgeCount() == 1, "Knowledge deduplicates"): return
        Input.action_press("move_right")
        for i in range(15): await process_frame
        Input.action_release("move_right")
        if not check(player.position.distance_to(start) > 10, "Player controls work"): return
        npc.MoveTo(world.get_node("NpcScheduleTest/Destinations/PointB").global_position)
        for i in range(30): await process_frame
        if not check(npc.position.distance_to(npc_start) > 10, "NPC moves"): return
        if not check(not world.has_node("TestKey"), "Collected key disappears"): return
        while not manager.IsResetting: await process_frame
        if not check(paused and clock.CurrentTime == 5.0, "End clamps clock and pauses world"): return
        var stopped_position = player.position
        Input.action_press("move_right")
        manager.RequestReset()
        manager.RequestReset()
        for i in range(3): await process_frame
        if not check(player.position == stopped_position, "Input cannot move player during fade"): return
        Input.action_release("move_right")
        await scene_changed
        if not check(transition.get_node("Fade").modulate.a > 0.99, "Reload hidden by black overlay"): return
        while manager.IsResetting: await process_frame
        if not check(not is_instance_valid(world), "Old physical world destroyed"): return
        if not check(manager.LoopNumber == cycle + 2, "Exactly one loop increment"): return
        if not check(current_scene.get_node("player").position == start, "Player position resets"): return
        if not check(current_scene.get_node("NpcScheduleTest/NPC_A").position == npc_start, "NPC position resets"): return
        if not check(current_scene.has_node("TestKey"), "Item returns"): return
        if not check(current_scene.get_node("TestDoor").visible, "Door resets"): return
        if not check(not current_scene.get_node("WorldState").GetFact(1), "Physical pickup fact resets"): return
        if not check(knowledge.Knows("test.secret_known"), "Knowledge survives"): return
        if not check(root.get_node("LoopManager") == manager and root.get_node("KnowledgeManager") == knowledge, "Same persistent managers"): return
        if not check(transition.get_node("Fade").modulate.a < 0.01 and not paused, "Fade clears and gameplay resumes"): return
        if not check(current_scene.get_node("player/Camera2D").is_current(), "Camera active after reset"): return
        print("PASS: automatic reset ", cycle + 1)
    if not check(initialized_at_zero, "Every scene initializes its clock at zero"): return
    manager.RequestReset()
    manager.RequestReset()
    while manager.IsResetting: await process_frame
    if not check(manager.LoopNumber == 52, "Early manual reset also guarded"): return
    print("PASS: 50 automatic resets plus manual reset; knowledge, input, camera, world lifetime and counts")
    quit(0)
