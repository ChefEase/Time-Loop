# Three-minute prototype: map blockout

Phase 10 section 102 only, authorized 2026-09-19. This is a static greybox map, not the completed causal prototype. Stop here before Daniel's theft behavior. No external playtest or causal-understanding milestone has passed.

## Run and walk around

Open `Prototype3Min.tscn` in Godot and press **F6** (Run Current Scene). Click the game viewport and use WASD/arrows. F5 still runs the existing Phase 8/9 main scene; it has not been replaced.

- Avery starts at the bottom left, Daniel at the key rack side, Theo above him, Ruth inside police, Jonah near the upper street, and Mara inside the machine room.
- Enter police through its bottom doorway and the machine room through its top doorway.
- Walk around the outside walls, through each entrance, past the key rack, and across the X intersection.
- NPCs are intentionally stationary until later work. Avery can walk through them; NPC body blocking is excluded from this prototype.
- E has no map interactions yet. The existing scene-local 180-second clock still triggers Phase 8's normal fade/reload; there is no failure or success state yet.

## Scene ownership

`Prototype3Min.tscn` contains the street, solid outer/building walls, floors, key-rack and machine placeholders, fixed overview camera, six name labels, 15 Marker2D route points, scene-local clock/world state, and static NavigationRegion2D polygon data. Both buildings are traversable interiors with doorways.

Markers: AveryStart, DanielStart, DanielKeyMarker, DanielNormalExit, DanielEscape01, DanielEscape02, TheoStart, TheoWatchMarker, TheoSearchMarker, PoliceEntrance, RuthStart, JonahStart, CollisionMarker, MachineEntrance, MaraStart.

`GreyboxNpc.tscn` reuses the existing NpcController and NavigationAgent2D without a ScheduleRunner. Per-character NpcDefinition resources are embedded in the map. Avery reuses PlayerController with a small rectangle and interaction area. No existing player/NPC scene or shared controller was changed.

Local collision masks: solid world on layer 1; Avery on layer 2 and NPCs on layer 4 (numeric bit 8). Both actor masks query only world layer 1. Existing interactable layer 3 (numeric bit 4) remains reserved. NPC avoidance is disabled. The navigation mesh leaves 12 pixels of clearance around solid walls for the 18-pixel NPC and 22-pixel player bodies.

The navigation mesh is authored static scene data. If wall placement changes, update navigation geometry and rerun the checks. There is no runtime map generation. Key/machine rectangles are visual placeholders without physical blocking; the building walls are the obstacles under test.

The requested `docs/15_3_MINUTE_PROTOTYPE.md` does not exist. The actual [paper specification](../../../../docs/16_PROTOTYPE_SPEC.md) has different roles, timings, and interventions. This digital map follows the user's Phase 10 blockout request without adopting or rewriting that paper scenario. The named actors do not imply new canon story facts.

## Validation, 2026-09-19

- `dotnet build`: passed, zero errors; three pre-existing warnings (WorldState nullable context twice, Yarn JSON converter once). The first sandbox build could not access SDK/NuGet; the permitted retry passed.
- Installed Godot 4.7.2 .NET headless check passed: all 225 ordered marker pairs have complete paths; each of five NPCs physically visits every marker with the existing controller.
- Automated keyboard-driven Avery traversal visited every marker, including both interiors. Actual wall collision and passage through Daniel were checked.
- Scene replacement restores Avery and all five NPCs, destroys the old map, and preserves KnowledgeManager.
- OpenGL Compatibility rendered launch passed; captured viewport inspected for character names, entrances, map framing, and landmarks. No runtime errors appeared in either run.
- A human free-play walkthrough is not claimed; automated input traversal and rendered inspection are the evidence. External playtesting, causal-chain behavior, and the overall Phase 10 gate remain pending.

From `time-loop/`, replacing `Godot_console.exe` with the installed .NET console executable path:

```text
dotnet build
Godot_console.exe --headless --path . --fixed-fps 60 --script res://tests/blockout_check.gd
```

The test pauses only its test clock while checking routes to prevent the three-minute reset interrupting the long traversal. Normal gameplay remains at 180 seconds. For a rendered snapshot, run the same script without `--headless`, with `--rendering-method gl_compatibility -- --capture-blockout`; that mode writes `tests/blockout_preview.png` and exits without running movement assertions. The image is a temporary verification artifact, not a game asset.
