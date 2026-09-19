# Three-minute causal prototype

This prototype runs directly from `scenes/core/main.tscn`, which is the configured main scene. `Prototype3Min.tscn` remains a thin compatibility entry point. The entire greybox chain is visible in one small map so it can be tested without loading another scene.

## Player controls

- **WASD / arrow keys**: move Avery.
- **E or Space**: interact with the nearby document or Theo.
- **P**: pause or resume the simulation clock.
- **1 / 2 / 3 / 4**: choose 1x, 2x, 5x or 10x clock speed.
- **R**: reset the current loop.
- **Tab**: toggle the observation panel when available.
- **F1**: toggle the developer event log and clock controls.
- **K / C / N**: learn, check or clear the temporary knowledge test fact.

The HUD explains the controls in-game. The clock is three minutes (180 seconds) and is intentionally accelerated only for developer testing; the simulation remains deterministic at normal speed.

## Default causal chain

1. Daniel walks to the key rack and takes the restricted key at about 00:40.
2. Theo witnesses the theft if he is still at the observation point.
3. Theo runs to the police station and reports Daniel.
4. Ruth leaves the station, and Daniel switches to the escape route.
5. Daniel and Jonah occupy the physical `CollisionTrigger` together; Jonah is injured and the relay is lost.
6. The machine has no relay at 02:30 and destabilizes. At 03:00 the loop displays the failure outcome and resets.

The collision is an Area2D overlap between Daniel and Jonah. It is not decided by checking whether Theo was distracted or whether Ruth is chasing.

## The one intervention

Approach Theo before 00:35 and press **E** or **Space**. Theo moves to the search marker and cannot witness the theft. The report, chase, escape route and collision then never occur; Jonah reaches the machine, Mara installs the relay, and the machine remains stable at 03:00. After 00:35 the interaction reports that it is too late.

## What is reset and what persists

Reloading `main.tscn` recreates the physical map, actor positions, NPC routes, key, relay, machine state and prototype world facts. `KnowledgeManager` is an Autoload, so the prototype observations `prototype.theo_reported_daniel` and `prototype.report_causes_chase` remain known across loop resets. They are cleared only by a new-game operation.

## Automated checks

From `time-loop/`:

```text
dotnet build --no-restore
Godot_console.exe --headless --path . --fixed-fps 60 --script res://tests/causal_prototype_check.gd
Godot_console.exe --headless --path . --fixed-fps 60 --script res://tests/main_hud_check.gd
Godot_console.exe --headless --path . --fixed-fps 60 --script res://tests/blockout_check.gd
```

The causal test runs both branches through `main.tscn`: the untouched timeline must explode, and the Theo distraction timeline must deliver the relay and succeed. The HUD test verifies the visible timer, pause/speed controls, interaction prompt, knowledge persistence and loop reset. A human playtest is still required for the final Phase 10 gate.
