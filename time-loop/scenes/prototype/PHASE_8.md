# Phase 8: loop reset

**Entry-point update:** the project main scene is now Phase 10's greybox map in `scenes/core/main.tscn`. The earlier Phase 8/9 fixtures remain in `scenes/prototype/LoopKnowledgeTest.tscn` (F6), also used by `PrototypeWorld.tscn` and the loop regression test.

Phase 9 now centralizes the test secret as `KnowledgeFacts.SecretKnown` (`test.secret_known`) and adds K/C/N knowledge controls plus a document fixture. See [Phase 9](PHASE_9.md) for current controls; R still runs the same loop reset.

Implemented under the user's bounded authorization on 2026-09-19. Gate G remains OPEN and full production remains unapproved. Automated checks pass; visual/manual acceptance is pending, so Phase 8 is not declared fully complete.

Phase 10 reuses this reset path from `main.tscn`: physical actors, objects, clock and prototype facts reload while Autoload managers survive. See the Phase 10 README for causal-chain verification.

## Configuration

Open `time-loop/project.godot` and run the main scene (F5). Main Scene is configured as `res://scenes/core/main.tscn`. The preserved Phase 8/9 fixture is `res://scenes/prototype/LoopKnowledgeTest.tscn`.

Project Settings > Globals > Autoload already contains:

| Name | Path |
| --- | --- |
| KnowledgeManager | `res://scripts/knowledge/KnowledgeManager.cs` |
| LoopManager | `res://scripts/core/LoopManager.cs` |
| LoopTransition | `res://scenes/ui/LoopTransition.tscn` |
| EventBus | Existing event bus, unchanged |

Do not add duplicate Autoloads. GameClock and WorldState have been removed from that list. WorldState is a main-scene child. The single GameClock is under `NpcScheduleTest/GameClock`, initialized before the player/HUD; this keeps the standalone NPC scene runnable too. Static clock access clears when the world exits. The NPC schedule and controller logic are preserved.

At 180 seconds the clock clamps its time and emits `LoopEnded`. Its old `ReachedLoopEnd` signal remains for compatibility. LoopManager immediately guards reset and pauses the tree; both fades run through an always-processing Autoload. It increments the loop, reloads the current scene, waits for `SceneChanged`, fades in, then resumes gameplay. The fresh clock stays at zero during the transition. Duplicate requests are ignored. Reload errors are logged, the overlay cleared, and the previous pause state restored; a rejected reload does not consume a loop number.

Only knowledge IDs and loop count persist. `TestKeyTaken` is a temporary scene-local physical pickup fact; the yellow key disappears when collected and teaches `TEST_SECRET`. This is a test fixture, not a general inventory or notebook. Mutable physical state must not be stored in shared resources or persistent event subscribers. The existing EventBus listener unsubscribes on exit.

The implementation uses Godot's documented [scene reload / scene_changed lifecycle](https://docs.godotengine.org/en/stable/classes/class_scenetree.html) and [pause processing](https://docs.godotengine.org/en/stable/tutorials/scripting/pausing_games.html).

## Controls and manual acceptance

- [ ] F5 starts at `00:00`, Loop 1; move with WASD/arrows and confirm the camera follows.
- [ ] Approach the door on the right and press E/Space. It disappears and becomes passable.
- [ ] Approach the yellow key on the left and press E/Space. It disappears and the HUD reports Secret: True.
- [ ] Let NPC_A follow its schedule: move at 00:20, wait at 00:40, move again at 01:00.
- [ ] At 03:00, verify movement/interactions stop, fade covers the viewport, then gameplay resumes with Loop 2, starting positions, closed door, returned key, and retained secret.
- [ ] Move, interact, and watch the NPC schedule again after reset. Repeat at least ten times.
- [ ] R triggers an early whole-world reset in debug builds. P pauses only the clock; 1–4 select clock speed. These older speed/pause tools do not synchronize NPC movement and are not deterministic speed-validation evidence.
- [ ] For a short visual test, temporarily override `NpcScheduleTest/GameClock.LoopDurationSeconds` to 5, then restore 180. No saved short-duration override is present.
- [ ] Visually inspect the fade at different window sizes and confirm input remains blocked throughout.

Temporary dialogue acceptance is pending: there is no gameplay dialogue runner/NPC conversation state instantiated in this scene. The Yarn addon alone is not a dialogue test. No dialogue system was added just to satisfy this checklist. Physical inventory coverage is limited to the test pickup fact.

## Repeatable checks and results

From `time-loop/`:

```text
dotnet build
Godot_console.exe --headless --path . --fixed-fps 60 --script res://tests/loop_reset_check.gd
Godot_console.exe --headless --path . --fixed-fps 60 --script res://tests/npc_schedule_check.gd
```

Use the installed Godot .NET console executable's full path. A passing script exits 0. Fixed FPS accelerates execution without changing simulation delta or TimeScale. The loop check only changes live clock duration to five seconds; every fresh scene is verified to retain the saved 180-second default.

Verified 2026-09-19 using installed Godot 4.7.2 .NET:

- `dotnet build`: succeeded, zero errors; three existing warnings (two WorldState nullable annotations and one Yarn JSON converter warning).
- 50 automatic resets plus one guarded manual reset passed. Checks cover destroyed old world references, stable node counts, reset player/NPC positions, functioning movement, active camera, door reopening, repeat pickup, cleared physical pickup fact, deduplicated persistent knowledge, frozen input during fade, black overlay at reload, transparent overlay afterward, and fresh clocks initialized at zero.
- Existing standalone NPC full schedule/movement check passed at normal clock speed.
- Clean elevated headless runs produced no runtime errors. Initial sandbox runs encountered SDK/network and Windows certificate-store access restrictions; those were environmental, not passing validation runs.

Stable node counts do not establish a complete memory/performance profile. Human playtests and visual acceptance have not been performed. Commit/push from the phase guide is conditional on full acceptance and remains pending.
