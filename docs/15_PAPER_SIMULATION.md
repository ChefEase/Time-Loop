# Paper simulation: Greywick A–G

**Status:** analytical desk walkthrough completed against the proposed rules below. No human playtest, engine simulation, or measured map traversal has been performed. Results are conditional design deductions, not proof of fun or final feasibility.

Use [the source timeline](05_MASTER_TIMELINE.md), [event contracts](06_CAUSALITY_GRAPH.md), and [map assumptions](03_GREYWICK_MAP.md). All added durations and alternate behavior are **PROPOSED v0.2**.

## Method and assumptions

Set a paper clock to 8:00 and create tokens for actors and items. Copy the initial state from document 06. Advance through every relevant timestamp, recheck guards, move owned items exactly once, and cross out invalid downstream events. Log changed observations separately from developer truth. Repeat from the same initial state for each case.

This pass uses these explicit assumptions:

- Theo sees the theft at 8:03:20; 8:03:45 is departure toward Ruth.
- No chase means Daniel waits behind church for the same 8:05:55 handoff. Nobody automatically replaces Theo as a reporting witness.
- Uninjured Jonah completes relay handoff at 8:05:30 via the proposed manifest-stop route. Mara installs in 120 seconds and tests in 30: ready at 8:08:00.
- A working synchronizer lets Mara control the specified baseline welder disturbance. No additional damage is invented. Turning off the welder alone does not replace the relay.
- Owen needs two minutes after valid access to activate bypass. Mere key possession elsewhere is not access.
- When theft is prevented, Mercer follows his shutdown work route; he does not seek an unrelated alley confrontation.
- Early Arthur consent establishes holding mode, not immediate safe discharge. For incomplete shutdown cases, continued collapse at 8:15 is a provisional assumption needing fiction validation.
- No alternate-ending action is taken. Failure means no safe ending; reset is the working outcome, not a validated model of every possible emergency.

Tests B–F isolate an intervention at a stated point. They do not claim a complete player acquisition/travel route. Test G includes a candidate full route.

## A — No intervention

| Time | State transition |
| --- | --- |
| 8:03:20 | Key and ledger: Mercer → Daniel. Theo witnesses. |
| 8:04:00–8:04:15 | Report reaches Ruth; pursuit starts. |
| 8:04:45 | Jonah injured; relay: Jonah → ground. |
| 8:05:15–8:05:30 | Relay: ground → Naomi; returned to shop. |
| 8:05:55 | Key and ledger: Daniel → Adrian. |
| 8:07:00 | Owen lacks access; bypass stays closed. |
| 8:10:40–8:11:00 | Confrontation and armed struggle occur; Mercer dies. |
| 8:11:10 | Notebook: Mercer → Adrian. Key/ledger stay with Adrian. |
| 8:12:15 | Relay absent; Feeder B synchronization fails. |
| 8:14:30 | Safe prerequisites absent. Arthur still refuses abort. |
| 8:15:00 | Baseline collapse/reset. |

**Deduction:** reproduces the intended failure under corrected item accounting. Source alley-entry order remains documented as a proposed correction, not silently overwritten.

## B — Theo prevented from witnessing

**Input:** a Clara warning completes before 8:03:00; Theo stays supervised through 8:04:30.

| Time | Changed consequence |
| --- | --- |
| 8:03:20 | Theft still succeeds; Theo does not witness. |
| 8:04:00–8:04:45 | No report, pursuit, or collision. Daniel follows normal church route. |
| 8:05:30 | Jonah delivers relay intact. |
| 8:05:40 | Samuel sees Daniel waiting, not running from Ruth. |
| 8:05:55 | Adrian still receives key and ledger. |
| 8:08:00 | Relay installed/tested; grid/feeder readiness possible. |
| 8:11:00 | Unchanged theft investigation still leads to murder. |
| 8:14:30 | Coolant blocked, Mercer dead, Arthur unpersuaded. |

**Deduction:** prevents Jonah's injury and repairs relay delivery. Does not solve key, murder, or consent. Mara's successful isolation test is not permission to cut both feeders uncoordinated.

## C — Daniel never steals either item

**Input:** Daniel abandons the job before 8:03:20. No added shutdown briefing.

| Time | Changed consequence |
| --- | --- |
| 8:03:20 | Key/ledger remain with Mercer; no witnessable theft. |
| 8:04:00–8:04:45 | No report, chase, or collision. |
| 8:05:30 | Relay delivered; ready at 8:08. |
| 8:05:55–8:06:10 | No handoff; Adrian leaves empty-handed after waiting. |
| 8:06:10 | Mercer has no missing item to discover. |
| 8:07:00 | Following his existing shutdown plan, Mercer meets Owen and supplies key. |
| 8:09:00 | Owen completes bypass. |
| 8:10:40–8:11:00 | No pursuit/struggle; Mercer lives. |
| 8:14:30 | Arthur consent absent. Full readiness not inferred from survival. |

**Deduction:** one upstream intervention repairs several chains, but does not grant a true ending. This strong solution must be tested for replay interest. Mercer/Owen rendezvous is a proposed authored fallback, not present in the source timeline.

## D — Mercer survives while theft succeeds

**Input:** a credible warning makes Mercer avoid the alley before 8:10. All earlier theft/chase events remain unchanged.

| Time | Changed consequence |
| --- | --- |
| Through 8:09 | Key/ledger with Adrian; relay with Naomi; bypass closed. |
| 8:10:40 | Mercer does not enter confrontation. |
| 8:11:00 | No shooting; no murder-generated cufflink/casing/discovery. |
| 8:11:10 | Notebook remains with Mercer. |
| 8:12:15 | Missing relay still blocks synchronization. |
| 8:14:30 | Mercer alive, but other conditions fail. |
| 8:15:00 | Provisional collapse/reset: the intended false victory. |

**Deduction:** saving the apparent victim is distinct from saving Greywick. Do not play the baseline gunshot or create murder evidence in this branch.

## E — Relay recovered after collision

**Input:** Avery requests the parcel from Naomi at 8:06:00, finishes the 15-second handoff at 8:06:15, and delivers it to Mara by 8:08:00. The 105-second travel/handoff allowance requires an eventual measured store-to-substation route; this test does not claim it is already measured.

| Time | Changed consequence |
| --- | --- |
| 8:04:45 | Collision and Jonah's injury still occur. |
| 8:05:30 | Relay is in Naomi's shop. |
| 8:06:15 | Relay: Naomi → Avery; identifying Mara as recipient permits handoff in this proposal. |
| 8:08:00 | Relay: Avery → Mara. |
| 8:10:00–8:10:30 | Installation completes, then test passes. |
| 8:11:00 | Mercer still murdered. |
| 8:14:30 | Electrical readiness possible; coolant and consent still fail. |

**Deduction:** a downstream repair is a meaningful alternative to stopping the witness. It cannot retroactively heal Jonah or restore stolen items.

## F — Arthur confronted early

**Input:** credible conversation with Sophie present completes at 8:04:00, and Arthur consents. This isolates the rule; finding Sophie, gaining trust, and reaching the observatory by then are not proven.

| Time | Changed consequence |
| --- | --- |
| 8:04:00 | Arthur enters holding mode and seeks coordinated abort. |
| 8:07:20 onward | Original escalation events are cancelled or replaced by holding-state events. |
| 8:09:40 / 8:13:20 | Do not assume unchanged escalating echoes; holding-mode presentation is unassigned. |
| 8:11:00 | Independent Adrian/Mercer confrontation still kills Mercer. |
| 8:14:30 | Relay absent, bypass closed, secondary operator dead. Safe abort fails. |

**Deduction:** consent alone cannot satisfy the safe-ending contract. **Unresolved:** what physically happens at 8:15 in holding mode. The provisional reset assumption is not established canon and must be decided before this case can pass final validation.

## G — Multiple interventions with candidate travel budget

**Starting knowledge:** Avery knows Daniel's employer and target objects, the coolant plan, Simon's explanation, Lydia, and Sophie's relevance. This is a late-game knowledge state, not a first-loop requirement or loop-number gate.

**Additional proposals for this route:** Clara starts at school; Daniel can be persuaded with specific information in 20 seconds (credibility not yet tested); Sophie is at the library until 8:06 and agrees to accompany Avery after a 20-second explanation; no-theft Mercer is available at Town Hall at 8:04; the observatory has a working radio handset that Arthur permits Avery to use; Sophie moves at Avery's paper travel speed. None is established source canon.

| Start–finish | Avery's action | Calculation / result |
| --- | --- | --- |
| 8:00:00–8:00:50 | House → square → school | 15 + 35 sec |
| 8:00:50–8:01:05 | Warn Clara | 15 sec; 115 sec before deadline |
| 8:01:05–8:01:48 | School → square → café | 35 + 8 sec |
| 8:01:48–8:02:00 | Wait for Daniel | 12 sec |
| 8:02:00–8:02:20 | Persuade Daniel to abandon theft | 20 sec; 60 sec before theft |
| 8:02:20–8:02:35 | Café → square → Town Hall | 8 + 7 sec |
| 8:02:35–8:04:00 | Wait for Mercer | 85 sec; significant idle-time risk |
| 8:04:00–8:04:20 | Brief Mercer on coordinated shutdown | 20 sec; he follows Owen/observatory schedule |
| 8:04:20–8:04:45 | Town Hall → square → library | 7 + 18 sec |
| 8:04:45–8:05:05 | Recruit Sophie | 20 sec; availability assumption |
| 8:05:05–8:06:33 | Library → square → observatory, together | 18 + 70 sec; no unmeasured shortcut used |
| 8:06:33–8:07:33 | Explain Simon's findings with Sophie | 60 sec; Arthur consents, conditional on credibility |
| 8:07:33–8:07:53 | Brief Mara by observatory radio | 20 sec; confirms planned shutdown sequence |
| 8:07:53 onward | Observe and await readiness | Avery need not physically perform every NPC task |

Parallel NPC work:

- Jonah completes handoff 8:05:30; Mara finishes uninterrupted installation at 8:07:30. Avery's radio briefing interrupts the test at 8:07:33. Conservatively, Mara restarts the full 30-second test after the briefing at 8:07:53 and finishes at 8:08:23, still safely before deadline. No simultaneous hands-on work is assumed.
- Mercer has 160 seconds after the 8:04:20 briefing to reach coolant access at 8:07. Via square and substation requires 7 + 55 = 62 seconds, leaving 98 seconds for the proposed entrance/handoff availability. Coolant entrance is provisionally adjacent to substation.
- Owen opens bypass 8:07–8:09; sends confirmation by 8:09:20.
- Mercer coordinates at access 8:07–8:08, then travels 55 + 70 = 125 seconds; arrives observatory 8:10:05. A 20-second operator confirmation ends 8:10:25.
- Mara sends final electrical readiness confirmation by 8:14:30. Arthur and Mercer remain at consoles. Proposed radio confirmations take no more than 20 seconds each and occur sequentially where a shared channel is needed.
- All prerequisites precede 8:14:30; feeder isolation, abort/verification, and unwinding follow the contract in document 06.

**Deduction:** a route fits the stated walking times and added durations, without teleportation or a magical shortcut. It is not yet a validated solution: persuasion, building access, Sophie, radio availability, local map edges, and holding-mode physics remain assumptions. Warning Clara is redundant when theft prevention succeeds; it provides a backup if Daniel refuses but is not a required solution step.

## Outcome comparison at readiness deadline

`Yes` means supported by this working model; `No` means condition fails; `Open` means not established.

| Case | Mercer alive | Bypass active | Grid controlled | Mara can isolate both | Arthur consents | Safe ending |
| --- | --- | --- | --- | --- | --- | --- |
| A Default | No | No | No | No | No | No |
| B Theo retained | No | No | Yes | Yes | No | No |
| C No theft | Yes | Yes | Yes | Yes | No | No |
| D Mercer warned | Yes | No | No | No | No | No |
| E Relay recovered | No | No | Yes | Yes | No | No |
| F Arthur early | No | No | Open | No | Yes | No |
| G Coordinated | Yes | Yes | Yes | Yes | Yes | Conditional candidate |

Readiness additionally requires location, briefing, consent, and confirmations. An ability to isolate does not mean Mara has already isolated the feeders.

## Boundary and combination checks

| Check | Expected result under proposed rules |
| --- | --- |
| Repeat identical initial state/actions | Same ownership and event trace; no random rescue |
| Clara warning finishes exactly 8:03:00 | Completion before departure evaluation; Theo retained |
| Clara warning finishes after Theo witnessed | Cannot erase observation; must affect reporting separately |
| Witness stopped after report reaches Ruth | Does not cancel Ruth's already informed response |
| Key recovered after handoff | Must be acquired from Adrian/current owner, not Daniel |
| Both Theo and theft prevented | One absent theft, no duplicated benefits/items |
| Relay picked up before Naomi | Naomi's pickup guard fails; parcel follows actual owner |
| Mercer alive but away at 8:14:30 | No safe abort; alive is insufficient |
| Bypass finishes exactly 8:14:30 | Accept completion before readiness evaluation |
| Bypass finishes 8:14:31 | Too late for proposed safe sequence |
| Reset after any trace | Restore all physical ownership and tasks; retain only knowledge |

These are desk-derived expectations, not automated test results. Unfinished cases: ledger-only theft, key-only theft, police detention/property access, Vincent, church access, gun removal, late relay, first-loop observatory access, and alternate-ending transitions. Gate G remains open.
