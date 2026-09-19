# Phase 7: one NPC schedule

Phase 8 now configures `PrototypeWorld.tscn` as the main scene. See [Phase 8 setup and tests](PHASE_8.md).

For K/C/R/N knowledge tests and the secret document directly in `main.tscn` (F6), see [Phase 9 setup and tests](PHASE_9.md).

Open `NpcScheduleTest.tscn` in Godot and press **F6** (Run Current Scene).
No node wiring, polygon drawing, or keyboard input is needed. The main game scene is unchanged.

Expected Output:

```text
[NPC_A] 00:00 -> WAIT
[NPC_A] 00:20 -> MOVE PointB
[NPC_A] 00:40 -> WAIT
[NPC_A] 01:00 -> MOVE PointC
```

The scene-local GameClock starts at zero at normal speed in a fresh run.
This scene owns the single clock, including when embedded in the main scene. Markers are at (200,300), (450,300),
and (700,400). The navigation rectangle already contains polygon vertices and
a traversable polygon; it does not require an editor bake before this test.

Identity/speed live in `data/characters/npc_a.tres`; the four instructions live
in `data/schedules/npc_a_schedule.tres`. The runner selects the last entry whose
start time is at or before the clock. It commands the controller only when that
entry changes. NavigationAgent2D supplies the path; the controller moves the body.

`Wait` stops at the current position; it does not teleport to DestinationId.
Movement uses physics delta at 80 pixels/second. This phase validates normal
clock speed only: accelerated clock, coordinated pause/reset, fact conditions,
interaction behavior, animations, and schedule overrides are not implemented.
RequiredFacts and BlockedFacts are reserved empty data, not evaluated conditions.

## Repeatable headless check

After `dotnet build`, run your Godot .NET console executable from the project directory:

```text
Godot_console.exe --headless --path . --fixed-fps 60 --script res://tests/npc_schedule_check.gd
```

Replace the executable name with your installed version's path. Fixed FPS advances
simulation in 1/60-second steps without raising GameClock.TimeScale. The check first
tests direct movement and stopping, then asserts positions at schedule milestones
through 65 game seconds. A passing run exits with code 0 and prints PASS lines.

Validation on 2026-09-18: build succeeded; headless direct movement and full schedule
passed without runtime errors. Existing WorldState nullable and Yarn warnings remain
outside this change. Visual inspection in the editor is still recommended.
