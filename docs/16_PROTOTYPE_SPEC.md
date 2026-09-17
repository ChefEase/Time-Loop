# Three-minute causal prototype

**PROPOSED specification v0.2. Paper first; no implementation authorized.** This tests causal curiosity with a fake scenario, not the whole Greywick plot.

## Question and scope

Will an unfamiliar player observe a repeatable sequence, predict a consequence, and voluntarily replay to change it?

Use five NPC roles: **A caretaker**, **B thief**, **C witness**, **D courier**, **E officer**. Add player, key, component parcel, and machine tokens. No combat, dialogue tree, Meridian lore, save system, art production, or 24-NPC simulation.

The caretaker's key is the theft target. It is not needed to install the machine component. The thief's escape can injure the courier and prevent delivery. The machine needs the component installed by 3:00.

## Materials and map

One paper map, clock, actor/item tokens, event cards below, observation sheet, facilitator-only log. Player starts at plaza. All actors use the same ten-second edge duration. Actors can share a node except when an explicit event says they collide.

```text
                    MACHINE
                       |
                     10 sec
                       |
DEPOT --10 sec-- JUNCTION --10 sec-- PLAZA --10 sec-- POLICE
                       |
                     10 sec
                       |
                    HIDEOUT
```

Each edge is bidirectional. Seeing an event requires being at its node. Adjacent footsteps do not reveal offscreen causes. Machine failure/success is globally signaled at 3:00. Actor positions, visible carried objects, and machine warning label are readable when co-located.

## Initial state

At 0:00: player, caretaker, and witness at plaza; thief at hideout; courier at depot; officer at police; key held by caretaker; component held by courier; machine missing component. No injuries, report, pursuit, or installed component. Caretaker remains at plaza; witness remains there until reporting; machine instruction reads “Replacement component required before 3:00.”

## Facilitator event cards

| Time | Default action and guard |
| --- | --- |
| 0:20–0:40 | Thief travels hideout → junction → plaza. |
| 0:50 | Thief takes key if caretaker still holds it; if already removed, there is no theft. |
| 0:50 | Witness sees successful theft unless occupied by the player's distraction. |
| 0:55–1:05 | Witness goes plaza → police if theft witnessed. |
| 1:05 | Report reaches officer if witnessed and arrival completed. |
| 1:05–1:15 | Officer travels police → plaza after report. |
| 1:15 | Officer confronts thief if key theft occurred and thief still at plaza. Thief flees; otherwise no pursuit. |
| 1:20–1:30 | Fleeing thief travels plaza → junction. Normal route departs plaza at 1:40 instead. |
| 1:20–1:30 | Courier leaves depot and reaches junction. |
| 1:30 | If fleeing thief and courier arrive together, collision injures courier and drops component. If no pursuit, courier continues. |
| 1:30–1:40 | Uninjured courier travels junction → machine. Fleeing thief goes junction → hideout. |
| 1:40–1:50 | Uninjured courier hands component into machine installation tray. |
| 1:40–2:00 | Non-fleeing thief travels plaza → junction → hideout. Arriving junction at 1:50 does not collide with the earlier courier. |
| On tray deposit | Machine automatically installs component over 40 seconds; do not require a sixth NPC. |
| 2:30 | Default uninterrupted delivery's installation completes. |
| 3:00 | Component installed: machine stable. Otherwise machine fails. End run; offer replay. |

Officer stays at plaza after the thief flees; caretaker remains there. Injured courier stays at junction. Witness stays at police after reporting. No substitute witnesses. The officer's presence alone does not produce another theft report. In the no-theft branch the thief still takes the later normal exit, empty-handed.

## Available player actions

Show action availability when co-located; do not advertise downstream consequences.

| Action | Duration | Rule |
| --- | ---: | --- |
| Walk one edge | 10 sec | World events advance during travel |
| Wait | Player chooses 5/10 sec or next visible event | Does not reveal hidden event times |
| Ask caretaker to secure key | 10 sec | Caretaker puts it away; thief cannot take it that run |
| Distract witness | 20 sec | Witness cannot observe during action; starting after observation cannot erase it |
| Pick up dropped component | 10 sec | Only when component at same node on ground |
| Put component in machine tray | 10 sec | Player must carry it; starts 40-second installation |
| Inspect person/object | No simulation cost | Only visible state, not hidden schedule or motives |
| Restart after terminal state | No cost | Restore snapshot; player notes remain |

During action selection the clock pauses. On commitment, resolve time and scheduled events normally. An active distraction covers its start through just before its finish; completing at 0:50 does not hide the 0:50 theft. Securing the key completed at 0:50 takes effect before theft. The facilitator must apply these rules consistently rather than favoring a solution.

Decline unsupported actions consistently and log them as player expectations. Do not invent a special successful action halfway through one player's session.

## Desk solution checks

1. **Default:** witness reports → chase → collision at 1:30 → component remains at junction → machine fails at 3:00.
2. **Witness intervention:** start distraction at 0:40, end 1:00. Theft occurs, observation does not. No report/pursuit. Courier deposits at 1:50; installation ends 2:30. Key still stolen; machine survives.
3. **Key intervention:** secure key 0:30–0:40. No theft, report, or pursuit. Courier installs as above. Player changes an upstream cause.
4. **Downstream recovery:** observe theft at plaza, travel to junction by 1:30, pick up component 1:30–1:40, walk to machine 1:40–1:50, deposit 1:50–2:00, installation completes 2:40. Courier remains injured; machine survives.
5. **Too late:** depositing component at 2:30 finishes installation at 3:10; machine fails at 3:00. Starting an action before deadline is insufficient.
6. **Repeatability:** identical timed actions produce identical trace and object owners. Reset never leaves the key secured, component installed, or courier injured.

These checks follow from the cards; they are not external playtest results. All three solutions are physically possible on the toy map and change different parts of the chain.

## Neutral playtest script

Recruit three people unfamiliar with the design for an initial formative round. Give each up to four runs (about 15–25 minutes including decisions and discussion). This small sample is a design signal, not statistical proof.

Say: “This morning repeats. You can move, inspect, and use the actions shown. Your notes can carry into the next run. Try to understand what happens and change something you care about.” Do not mention stopping the witness, the chase chain, or an optimal solution.

Run one person at a time. Reveal only locally observed events. Ask “What do you expect will happen?” before a chosen intervention, without suggesting a prediction. After each run ask “What changed, and why?” Offer another run neutrally; record whether they already wanted one. Do not rescue a failed plan with hints and then count the result as unprompted.

## Proposed acceptance criteria

The original qualitative criterion remains primary: the player wants to rerun because they are curious about another outcome. The following are proposed observable thresholds for this formative round, not yet locked or passed:

- At least two of three players spontaneously propose a causal experiment within four runs, before the facilitator suggests a solution.
- At least two of three correctly predict one downstream consequence and later explain the observed result.
- At least two of three voluntarily request another run to test an idea, rather than only to comply with a request.
- Across sessions, players discover at least two distinct intervention approaches without a solution hint.
- Identical inputs remain deterministic; no duplicate/lost items or stale events. Any facilitator rule ambiguity is logged and repaired before the next claimed pass.
- Record waiting/frustration and unsupported-action requests. A numeric pass does not override consistent reports that the interaction is confusing or dull.

If curiosity fails, revise information visibility or the causal problem before expanding scope. If causality is understood but tedious, revise timing and waiting. If rules are ambiguous, fix the cards and rerun; do not interpret rule confusion as player failure.

## Session record template

```text
Participant ID (no personal details):
Date / rule revision / facilitator:
Run and starting knowledge:
Timed actions:
Observed events:
Prediction in player's own words:
Outcome and explanation in player's own words:
Spontaneous next experiment / replay request:
Hints given (exact wording):
Idle or frustrating intervals:
Unsupported actions / ambiguous rules:
Acceptance evidence and next revision:
```

Store completed records separately from this specification. No sessions have been run. Do not check the gate based on these empty templates.
