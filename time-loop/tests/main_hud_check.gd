extends SceneTree

func _initialize():
    run.call_deferred()

func require(condition, message):
    if not condition:
        push_error("FAIL: " + message)
        quit(1)
    return condition

func press(code):
    var event = InputEventKey.new()
    event.keycode = code
    event.physical_keycode = code
    event.pressed = true
    Input.parse_input_event(event)
    for i in range(4): await process_frame
    event = InputEventKey.new()
    event.keycode = code
    event.physical_keycode = code
    Input.parse_input_event(event)
    for i in range(4): await process_frame

func run():
    change_scene_to_file("res://scenes/core/main.tscn")
    await scene_changed
    for i in range(4): await process_frame
    var clock = current_scene.get_node("GameClock")
    var hud = current_scene.get_node("UI")
    if not require(hud.get_node("Screen/Top/Rows/Time").text.contains("Loop 1"), "Timer and loop visible"): return
    if not require(hud.get_node("Screen/Bottom/Rows/DebugControls").text.contains("R: reset"), "Controls visible"): return
    await press(KEY_F1)
    if not require(hud.get_node("Screen/Bottom/Rows/DebugControls").visible, "F1 shows test controls"): return
    await press(KEY_F1)
    if not require(not hud.get_node("Screen/Bottom/Rows/DebugControls").visible, "F1 hides test controls"): return
    await press(KEY_P)
    var paused_time = clock.CurrentTime
    for i in range(20): await process_frame
    if not require(clock.IsPaused and clock.CurrentTime == paused_time, "P pauses clock"): return
    if not require(hud.get_node("Screen/Top/Rows/Status").text.contains("CLOCK PAUSED"), "Pause feedback visible"): return
    await press(KEY_P)
    await press(KEY_3)
    if not require(clock.TimeScale == 5 and hud.get_node("Screen/Top/Rows/Status").text.contains("5x"), "Speed control and feedback"): return
    await press(KEY_1)
    var document = current_scene.get_node("SecretDocument")
    current_scene.get_node("Player").position = document.position + Vector2(-25, 0)
    for i in range(5): await process_frame
    if not require(hud.get_node("Screen/Bottom/Rows/Prompt").text.contains("Read field note"), "Nearby interaction prompt"): return
    await press(KEY_E)
    if not require(document.InspectedThisLoop and hud.get_node("Screen/Bottom/Rows/Knowledge").text.contains("KNOWN"), "E learns with visible feedback"): return
    await press(KEY_R)
    var budget = 180
    while root.get_node("LoopManager").IsResetting and budget > 0:
        budget -= 1
        await process_frame
    if not require(budget > 0, "Reset completes"): return
    for i in range(4): await process_frame
    hud = current_scene.get_node("UI")
    if not require(hud.get_node("Screen/Top/Rows/Time").text.contains("Loop 2"), "HUD reconnects after reset"): return
    if not require(current_scene.get_node("SecretDocument").visible and hud.get_node("Screen/Bottom/Rows/Knowledge").text.contains("KNOWN"), "Physical document resets, knowledge stays"): return
    await press(KEY_N)
    if not require(hud.get_node("Screen/Bottom/Rows/Knowledge").text.contains("not learned"), "N clear visible"): return
    await press(KEY_K)
    await press(KEY_C)
    if not require(root.get_node("KnowledgeManager").GetKnownFactCount() == 1, "K/C knowledge controls restored"): return
    print("PASS: main timer, controls, document prompt, knowledge feedback and reset")
    quit(0)
