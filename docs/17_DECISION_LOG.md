# Decision log

**Revision:** v0.2 draft, 2026-09-17. **Authority:** source v0.1 remains canon. Proposals below are reviewable design work, not claims that the author approved changed fiction.

| ID | Status | Decision / proposal | Reason / affected audit |
| --- | --- | --- | --- |
| DEC-01 | Applied housekeeping | Use current `Time loop` workspace as project home | Authorized alternative to `C:\GameDev\815`; avoid a duplicate nested project |
| DEC-02 | Applied housekeeping | Normalize obvious ranges and dimensions | 3,000–5,000 population; 6–10 interiors; 640 × 360 resolution |
| DEC-03 | Proposed | Theo observes 8:03:20, departs 8:03:45 | AUD-01; preserves main timeline |
| DEC-04 | Proposed | Adrian enters alley before Mercer; retains stolen key/ledger after shooting | AUD-02/03; consistent pursuit and ownership |
| DEC-05 | Proposed | Jonah manifest stop; handoff 8:05:30; 120-sec installation + 30-sec test | AUD-04/06; gives delivery concrete duration |
| DEC-06 | Proposed | No-chase Daniel waits for fixed handoff; no-theft Mercer follows shutdown plan | AUD-06; remove events when causes disappear |
| DEC-07 | Proposed | Two-minute coolant task and 8:14:30 readiness deadline | AUD-07/08; define coordination costs |
| DEC-08 | Proposed | Arthur's consent creates holding mode; local control power supports abort after isolation | AUD-08/09/23; needs fiction and failure-state validation |
| DEC-09 | Proposed | Notebook is remembered information; police require current-loop corroboration | AUD-15/16; knowledge differs from physical evidence |
| DEC-10 | Proposed | Two linked November 1984 incidents, tape between | AUD-11; chronology decision still required |
| DEC-11 | Proposed | Town Hall ledger photo; Iris supplies alley evidence; specify film processing | AUD-12; retain distinct clue functions |
| DEC-12 | Proposed | Specify casing-compatible pistol and local phone-record source | AUD-13/14; final details still open |
| DEC-13 | Proposed | Relay handles normal welder disturbance; welder-off alone insufficient | AUD-18; no unexplained substitute for synchronizer |
| DEC-14 | Proposed | Source final route remains illustration; G supplies explicit candidate route | AUD-05/19/20/26; new Sophie/radio/access assumptions labeled |
| DEC-15 | Proposed | Action selection pauses; committed actions consume fixed time | Fair paper timing; digital UX still untested |
| DEC-16 | Applied phase clarification | Gate G permits only bounded prototype, not full production | Source also explicitly requires prototype proof before Greywick software/full game |
| DEC-17 | Proposed | Manual Samuel bell at 8:16 | AUD-22; final communication/task still needed |
| DEC-18 | Open | Common boundary, energy, reset, death, and permanent-ending rules | AUD-10/23/25; not resolved by this pass |
| DEC-19 | Open | Partial theft, police custody, locksmith/crypt, gun removal branches | AUD-24 and remaining alternate solutions |
| DEC-20 | Proposed | Toy prototype cards and three-person formative criteria | Measurable first playtest, not a claim of validation |

## Highest-priority remaining decisions

**DEC-22 — Authorized bounded implementation, 2026-09-19:** user requested Phase 9 persistent binary knowledge and testing through `main.tscn`. Extend the existing Autoload, reserve centralized IDs, emit learned notifications once, and add debug/physical-document tests. Preserve Phase 8's real reset path and exclude saving, notebook, Yarn integration, and advanced knowledge states. The [Phase 9 record](../time-loop/scenes/prototype/PHASE_9.md) separates automated evidence from pending human acceptance. Gate G and proposed story rules are unchanged.

**DEC-21 — Authorized bounded implementation, 2026-09-19:** user requested Phase 8 in the existing `time-loop/` project: three-minute scene reload, persistent knowledge/loop count, fade, and reset verification. This is an explicit exception to the documents-only boundary for this task, not adoption of proposed story rules or closure of Gate G. See the [implementation and verification record](../time-loop/scenes/prototype/PHASE_8.md). Human visual acceptance remains pending.

1. Adopt or revise the shutdown/holding-mode physical rules, including early cuts and 8:15 failure behavior.
2. Resolve Simon/Lydia chronology without weakening the tape's emotional reveal.
3. Validate route assumptions, Sophie access, and credible persuasion on paper.
4. Run unfamiliar-player toy tests and decide whether causal curiosity warrants a software prototype.

Keep proposed choices explicit when revising. After adopting a proposal, update its owning document and rerun affected paper cases; do not just change the status in this table.
