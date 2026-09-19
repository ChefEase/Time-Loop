extends SceneTree

var frames = 0

func _initialize():
    process_frame.connect(func():
        frames += 1
        if frames > 150000:
            push_error("FAIL: blockout test timeout")
            quit(1))
    run.call_deferred()

func check(condition, message):
    if not condition:
        push_error("FAIL: " + message)
        quit(1)
        return false
    return true

func steer(direction):
    Input.action_press("move_right", maxf(direction.x, 0))
    Input.action_press("move_left", maxf(-direction.x, 0))
    Input.action_press("move_down", maxf(direction.y, 0))
    Input.action_press("move_up", maxf(-direction.y, 0))

func release_input():
    for action in ["move_right", "move_left", "move_down", "move_up"]:
        Input.action_release(action)

func run():
    change_scene_to_file("res://scenes/prototype/three_minute/Prototype3Min.tscn")
    await scene_changed
    var scene = current_scene
    scene.get_node("GameClock").set_physics_process(false)
    for i in range(10): await process_frame
    if "--capture-blockout" in OS.get_cmdline_user_args():
        await RenderingServer.frame_post_draw
        root.get_texture().get_image().save_png("res://tests/blockout_preview.png")
        print("PASS: rendered blockout preview captured")
        quit(0)
        return
    var markers = scene.get_node("Markers").get_children()
    var npcs = scene.get_node("NPCs").get_children()
    var map = scene.get_node("NavigationRegion2D").get_navigation_map()
    if not check(markers.size() == 15 and npcs.size() == 5, "Expected map actors and markers"): return
    # Every endpoint and every pair must belong to the same traversable map.
    for source in markers:
        for target in markers:
            var path = NavigationServer2D.map_get_path(map, source.global_position, target.global_position, true)
            if not check(path.size() > 0 and path[-1].distance_to(target.global_position) < 0.1, "Connected route to " + target.name): return
    print("PASS: all 225 marker pairs connected, including building interiors")
    # Use the existing C# controller against real world collisions, not just path queries.
    for npc in npcs:
        if not check(npc.get_node("NameLabel").text == str(npc.name).to_upper(), "NPC name label"): return
        var spawn = npc.position
        for target in markers:
            npc.MoveTo(target.global_position)
            var budget = 1500
            while npc.global_position.distance_to(target.global_position) > 4.1 and budget > 0:
                budget -= 1
                await process_frame
            if not check(budget > 0, str(npc.name) + " reaches " + str(target.name)): return
        npc.StopMoving()
        npc.position = spawn
        print("PASS: ", npc.name, " physically reached every marker")
    var player = scene.get_node("Player")
    if not check(player.collision_mask == 1 and npcs[0].collision_mask == 1, "Player/NPC masks exclude each other"): return
    # Walk Avery through the entire map using actual input and the existing player controller.
    for target in markers:
        var path = NavigationServer2D.map_get_path(map, player.global_position, target.global_position, true)
        for waypoint in path:
            var budget = 600
            while player.global_position.distance_to(waypoint) > 4.0 and budget > 0:
                budget -= 1
                steer(player.global_position.direction_to(waypoint))
                await process_frame
            release_input()
            if not check(budget > 0, "Avery walks to " + str(target.name)): return
    print("PASS: Avery walks all markers, through entrances and around solid walls")
    # Verify that walls actually block the player, while NPCs do not.
    player.position = Vector2(360, 200)
    steer(Vector2.UP)
    for i in range(90): await process_frame
    release_input()
    if not check(player.position.y >= 130 and player.position.y < 140, "Police wall blocks player"): return
    player.position = npcs[0].position + Vector2(-50, 0)
    steer(Vector2.RIGHT)
    for i in range(40): await process_frame
    release_input()
    if not check(player.position.x > npcs[0].position.x + 40, "Avery passes through Daniel"): return
    print("PASS: solid walls and non-blocking NPCs")
    var old_scene = scene
    var knowledge = root.get_node("KnowledgeManager")
    knowledge.Learn("test.blockout_reset")
    root.get_node("LoopManager").RequestReset()
    while root.get_node("LoopManager").IsResetting: await process_frame
    if not check(not is_instance_valid(old_scene), "Whole map reset"): return
    if not check(current_scene.get_node("Player").position == Vector2(160, 680), "Player resets to AveryStart"): return
    for npc in current_scene.get_node("NPCs").get_children():
        if not check(npc.position == current_scene.get_node("Markers/" + str(npc.name) + "Start").position, "NPC spawn resets"): return
    if not check(knowledge.Knows("test.blockout_reset"), "Knowledge persists"): return
    print("PASS: greybox map movement, navigation, collision layers and scene reset")
    quit(0)
