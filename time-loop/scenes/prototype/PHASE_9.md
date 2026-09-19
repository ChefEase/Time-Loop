# Phase 9: persistent knowledge

**Entry-point update:** Phase 10 now occupies `scenes/core/main.tscn`. For the Phase 9 keyboard/document checks below, open `scenes/prototype/LoopKnowledgeTest.tscn` and press F6. The test script has been updated to use that preserved room; its knowledge behavior is unchanged.

Implemented under explicit user authorization on 2026-09-19. Automated acceptance passed; human visual/play acceptance remains pending. Gate G is OPEN and full production is not authorized.

The Phase 10 prototype records `prototype.theo_reported_daniel` and `prototype.report_causes_chase` through this persistent manager. They survive loop reloads and are not save data or final story canon.

## Test directly in main.tscn

1. Stop any running game. Open `scenes/core/main.tscn` in Godot.
2. Press F6 (Run Current Scene), then click the game viewport so it receives keyboard input. Keep Godot's Output panel visible.
3. Follow the keyboard sequence below. No Autoload or node setup is needed: the existing enabled KnowledgeManager Autoload is reused, and the tester/document are already children of main. F5 also works through the inherited PrototypeWorld scene.

| Press | Expected Output/result |
| --- | --- |
| C | `Does Avery know the secret? False` on a fresh run |
| K, then C | One `KNOWLEDGE LEARNED: test.secret_known`, then `True` |
| K again | No second learned message or signal |
| R, wait for fade, then C | World resets; knowledge is still `True` |
| N, then C | Knowledge is cleared; `False` |

R uses the existing guarded LoopManager reset, including fade and loop increment. It does not bypass Phase 8 with a second direct reload handler. N is a debug knowledge-clear test only: it does not reset the world, loop number, or a save file. These keyboard helpers are debug-only and ignore key echoes. C no longer closes the test door; use V for the old close-door debug shortcut (O still opens it).

For the physical-object test, after N walk downward from spawn to the pale rectangular **Secret document** at main-local (458, 650). Approach it and press E/Space. It disappears and prints `NEW KNOWLEDGE: Avery learned the secret.` Press R, wait for the fade, then return to the document and inspect again. It has physically returned but now prints `Avery already remembers this.` No second learned signal occurs. The document uses the player's existing nearest-interactable selection; approach the document itself before pressing E.

The yellow Phase 8 key also teaches the same centralized test fact. If you already collected it or pressed K, the document correctly reports that Avery remembers. N lets you test first discovery again, but use R too if you need to restore an already-inspected document.

Closing the game clears knowledge because no disk saving exists. Run F6 again to check a fresh game starts unknown. Keep the loop duration at 180 seconds; automatic resets preserve knowledge just like R.

## Implementation

- `KnowledgeFacts` centralizes string constants. Phase 8's `TEST_SECRET` callers now use `KnowledgeFacts.SecretKnown` (`test.secret_known`). There are no saved facts to migrate. Story IDs are reserved examples, not new canon decisions or discovery triggers.
- `Learn(id)` rejects blank IDs and returns true only for a new ID. It stores the fact before emitting `KnowledgeLearned(id)` exactly once; duplicate learning returns false.
- `Knows(id)` queries membership; `GetKnownFactCount()` returns the number of distinct facts. `GetKnowledgeCount()` remains a compatibility alias for the Phase 8 test API.
- `ClearAllForNewGame()` explicitly erases knowledge. Only the debug N helper calls it now; LoopManager does not.
- The Autoload's static Instance is established on tree entry. An accidental second manager is rejected without replacing the original; the static reference clears only when its owning node exits.
- SecretDocument owns `InspectedThisLoop`, visibility, and its interaction collision layer. These reset with scene replacement. Learned knowledge lives outside that scene. Inspected invisible documents leave interaction detection, so they cannot block nearby objects.

There is no notebook, save/load, Yarn integration, suspicion model, or automatic learning from remote world events. Discovery is explicit through the debug key, test key, or document interaction. Knowledge IDs represent information learned, not proof that a world event occurred in this loop; future evidence/provenance rules remain separate design work.

## Verification

From `time-loop/`, using the installed Godot .NET console executable in place of `Godot_console.exe`:

```text
dotnet build
Godot_console.exe --headless --path . --fixed-fps 60 --script res://tests/knowledge_check.gd
Godot_console.exe --headless --path . --fixed-fps 60 --script res://tests/loop_reset_check.gd
```

2026-09-19 results:

- Build succeeded with zero errors and three existing warnings: two WorldState nullable-context warnings and one Yarn JSON converter warning.
- Knowledge acceptance passed against `main.tscn` using simulated K/C/R/N/E keyboard input. Covers initially unknown, single learned signal, duplicate/echo suppression, blank-ID rejection, persistence across real reset, explicit clearing, actual player-to-document interaction, restored physical document, repeat inspection without notification, distinct fact IDs, and C not closing the door. Two warnings are intentionally generated by blank-ID rejection tests; no runtime errors occurred.
- Phase 8 regression passed 50 automatic resets plus a guarded manual reset with the new knowledge ID and added document.
- The initial sandbox build could not access the Godot SDK/NuGet; the permitted retry succeeded.

These automated checks do not constitute an unfamiliar-player paper playtest or human visual acceptance. Changes remain uncommitted/unpushed pending review of the combined existing prototype work and manual acceptance.
