extends SceneTree

const SECRET = "test.secret_known"
var learned_events = []
var frames = 0

func _initialize():
    process_frame.connect(watchdog)
    run.call_deferred()

func watchdog():
    frames += 1
    if frames > 2000:
        push_error("FAIL: knowledge test timed out")
        quit(1)

func check(condition, message):
    if not condition:
        push_error("FAIL: " + message)
        quit(1)
        return false
    print("PASS: " + message)
    return true

func press_key(code, echo = false):
    var event = InputEventKey.new()
    event.keycode = code
    event.physical_keycode = code
    event.pressed = true
    event.echo = echo
    Input.parse_input_event(event)
    for i in range(3): await process_frame
    event = InputEventKey.new()
    event.keycode = code
    event.physical_keycode = code
    event.pressed = false
    Input.parse_input_event(event)
    for i in range(3): await process_frame

func run():
    change_scene_to_file("res://scenes/core/main.tscn")
    await scene_changed
    var knowledge = root.get_node("KnowledgeManager")
    var manager = root.get_node("LoopManager")
    knowledge.connect("KnowledgeLearned", func(id): learned_events.append(id))
    await press_key(KEY_C)
    if not check(not knowledge.Knows(SECRET) and knowledge.GetKnownFactCount() == 0, "Fresh main starts with no knowledge"): return
    if not check(not knowledge.Learn("") and not knowledge.Learn("   "), "Empty IDs rejected (two expected warnings)"): return
    await press_key(KEY_K, true)
    if not check(knowledge.GetKnownFactCount() == 0, "Repeated key echo ignored"): return
    await press_key(KEY_K)
    await press_key(KEY_C)
    if not check(knowledge.Knows(SECRET) and learned_events == [SECRET], "K learns and emits once"): return
    await press_key(KEY_K)
    if not check(not knowledge.Learn(SECRET) and learned_events.size() == 1, "Duplicate learn returns false with no event"): return
    var old_world = current_scene
    await press_key(KEY_R)
    while manager.IsResetting: await process_frame
    await press_key(KEY_C)
    if not check(not is_instance_valid(old_world) and manager.LoopNumber == 2 and knowledge.Knows(SECRET), "R reloads main once and preserves knowledge"): return
    await press_key(KEY_N)
    await press_key(KEY_C)
    if not check(knowledge.GetKnownFactCount() == 0 and not knowledge.Knows(SECRET) and manager.LoopNumber == 2, "N clears knowledge only"): return
    var document = current_scene.get_node("SecretDocument")
    current_scene.get_node("player").global_position = document.global_position + Vector2(-70, 0)
    for i in range(4): await process_frame
    await press_key(KEY_E)
    if not check(document.InspectedThisLoop and not document.visible and document.collision_layer == 0, "E inspects and removes document from interaction detection"): return
    if not check(knowledge.Knows(SECRET) and learned_events.size() == 2, "Physical document learns after explicit clear"): return
    document.Interact(current_scene.get_node("player"))
    if not check(learned_events.size() == 2, "Same-loop inspection cannot relearn"): return
    await press_key(KEY_R)
    while manager.IsResetting: await process_frame
    document = current_scene.get_node("SecretDocument")
    if not check(document.visible and not document.InspectedThisLoop and document.collision_layer == 4, "Document physical state resets"): return
    current_scene.get_node("player").global_position = document.global_position + Vector2(-70, 0)
    for i in range(4): await process_frame
    await press_key(KEY_E)
    if not check(document.InspectedThisLoop and learned_events.size() == 2, "Second loop inspection remembers without new event"): return
    current_scene.get_node("TestDoor").Interact(current_scene.get_node("player"))
    await press_key(KEY_C)
    if not check(not current_scene.get_node("TestDoor").visible, "C checks knowledge without closing door"): return
    if not check(knowledge.Learn("event.daniel_stole_key") and knowledge.GetKnownFactCount() == 2, "Distinct IDs stored independently"): return
    await press_key(KEY_N)
    if not check(knowledge.GetKnownFactCount() == 0 and learned_events.size() == 3, "Clear removes every fact without learned events"): return
    print("PASS: Phase 9 acceptance through main.tscn keyboard and physical interactions")
    quit(0)
