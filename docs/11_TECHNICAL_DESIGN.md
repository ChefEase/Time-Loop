# Technical design direction

**Planning only. No gameplay implementation is authorized by this document.**

## Intended tools

Godot **4.7.2 .NET**, C#, Cursor, Yarn Spinner for Godot C#, Git/GitHub, Git LFS for future large binaries, Aseprite, Krita, Audacity, REAPER or equivalent, OBS. Markdown in this repository is the design source of truth. Notion can be a personal companion, not an independent competing bible.

Checked 2026-09-17: Godot publishes an official [4.7.2 stable archive](https://godotengine.org/download/archive/4.7.2-stable/). Yarn Spinner documents [Godot support](https://yarnspinner.dev/docs/godot/) and a [C# setup procedure](https://github.com/YarnSpinnerTool/YSDocs/blob/main/docs/yarn-spinner-for-godot/godot-csharp/installation-and-setup.md). These establish availability, not a tested compatible project. Record exact Yarn package, .NET SDK, export templates, licenses, and a successful Windows export when a software spike is authorized. Do not install tools now.

## Proposed simulation boundaries

- **Clock:** integer simulation time independent of rendering; baseline 900 seconds.
- **World state:** authoritative ownership, locations, tasks, NPC beliefs, device states, and terminal state.
- **Knowledge state:** observations and learned topics that persist across loop resets; not physical inventory.
- **Schedule data:** default tasks plus bounded guarded branches and explicit fallback actions.
- **Event resolver:** rechecks preconditions at execution time, commits state transitions, records outcomes.
- **Interaction commands:** requests with target, duration, and preconditions, not direct arbitrary state mutation.
- **Presentation:** renders authoritative state; sounds and dialogue follow committed changes.
- **Developer trace:** event ID, timestamp, guards, cause, before/after values, cancellation reason.

Do not design advanced autonomous AI. Most NPCs need one authored route and a few meaningful deviations. Avoid unique scripts for every combination of unrelated changes.

## Proposed deterministic ordering

At each simulation timestamp:

1. Finish valid movement and interaction tasks due at that timestamp.
2. Resolve scheduled world events in stable priority then event-ID order, rechecking guards after each commit.
3. Evaluate derived readiness and terminal conditions.
4. Publish committed presentation events.

A completed intervention at a deadline can affect a later event at the same timestamp; merely starting an interaction cannot. Commands with the same resource conflict must have a stable rule and log the rejected action. Do not depend on node traversal order, frame rate, thread timing, or uncontrolled random numbers.

## Reset contract

Restore every physical snapshot value: positions, item owners, local beliefs, dialogue-local variables, path/task progress, timers, injuries, device states, event queues, and temporary effects. Cancel old-loop pending messages and interactions. Keep only explicitly persistent player knowledge. A developer replay with the same initial knowledge and timed input should produce the same trace.

No save system now. Between-session persistence is a later production problem distinct from between-loop memory.

## Dialogue and time

**PROPOSED paper/prototype rule:** choosing an action pauses the facilitator; committing it consumes its stated simulation duration while other actors continue. Menus and notebook inspection consume no simulation time. This avoids reading speed becoming an implicit puzzle stat while preserving conversation opportunity costs.

For the eventual digital game, exact pause behavior needs UX validation. Never silently pause only some NPCs while their timestamps continue advancing. Yarn may request typed commands and query sanctioned state; it must not own a second competing world-state model.

## Later validation, after authorization

Repeated baseline replay; intervention cancellation; ownership conservation; exact-boundary ordering; knowledge/reset separation; travel and NPC availability; readiness at 8:14:30; alternate route handling. Test causal invariants rather than hard-coding only the expected happy route.

Authoritative simulation should eventually run without final art or full dialogue. No engine files or test harness are required to conduct the current paper pass.
