# Pre-production gates

**Current state: PRE-PRODUCTION. Gate G OPEN. Full production NOT approved.**

## Source gates Aâ€“F: concept coverage

| Gate | Source status | Current interpretation |
| --- | --- | --- |
| A â€” Vision | Described | Name, genre, hook, platform, perspective, setting, pillars documented |
| B â€” Narrative | Described | Protagonist, murderer, hidden truth, loop and ending concepts documented; chronology/physics unresolved |
| C â€” World | Described | Concept map and travel philosophy documented; measured routes incomplete |
| D â€” Characters | Described | 25-character roster, motives, secrets, relationships documented; full schedules incomplete |
| E â€” Simulation | Described | Default timeline, five chains, ending conditions documented; branches not fully validated |
| F â€” Scope | Described | Included/excluded systems and technology direction documented; compatibility untested |

Source checkmarks indicate authored concepts, not independent acceptance evidence.

## Gate G: validation

- [x] Structured design documents established.
- [x] Contradictions identified with explicit proposed resolutions.
- [x] Guarded causal model and item-conservation rules drafted.
- [x] Aâ€“G analytical paper walkthroughs recorded with assumptions and limitations.
- [x] Three-minute paper prototype designed, with routes, actions, event cards, and candidate solutions.
- [x] Prototype acceptance criteria and neutral playtest record drafted.
- [x] Bounded three-minute software prototype implemented, automated-tested, and human-playtested through `main.tscn`.
- [ ] Proposed rule changes adopted or revised in the owning canon documents.
- [ ] Story contradictions resolved, especially tape chronology and ending/reset physics.
- [ ] Full causal audit completed for partial theft and required alternate solutions.
- [ ] Important routes, NPC availability, interaction costs, and shutdown communications validated.
- [ ] Aâ€“G scenarios rerun against the adopted rules with no unresolved outcome assumptions.
- [ ] Unfamiliar-player paper sessions conducted and evidence recorded.
- [ ] Acceptance criteria reviewed and locked; evidence supports proceeding or redesign.

The analytical paper pass is completed; overall **paper validation is not complete**. Scenario F's failure physics and G's social/access assumptions prevent declaring a fully consistent Greywick simulation.

## Phase boundaries

**Bounded Phase 10 blockout exception, 2026-09-19:** the user authorized section 102 only: create the feature branch, build the small greybox map with six labeled characters, verify player movement and NPC navigation, commit/push, then stop before Daniel behavior. See DEC-23 in the [decision log](17_DECISION_LOG.md) and the [blockout record](../time-loop/scenes/prototype/three_minute/README.md). This historical note was superseded by the later bounded Phase 10 implementation authorization and DEC-25 human-playtest result; it does not close Gate G.

**Bounded Phase 9 exception, 2026-09-19:** the user explicitly authorized extending the existing knowledge manager, centralized IDs, debug keyboard tests, and an interactable test document in `main.tscn`. See DEC-22 in the [decision log](17_DECISION_LOG.md) and the [Phase 9 verification record](../time-loop/scenes/prototype/PHASE_9.md). No save/notebook/dialogue system or full production is authorized. Automated checks do not close Gate G.

**Bounded exception, 2026-09-19:** the user explicitly authorized Phase 8 loop-system implementation in the existing `time-loop/` software prototype. This supersedes the documents-only boundary for that task only. See [DEC-21](17_DECISION_LOG.md) and the [Phase 8 verification record](../time-loop/scenes/prototype/PHASE_8.md). Gate G remains OPEN; no paper playtest or full-production approval is implied.

**Now:** documentation and paper testing only. No Godot project, gameplay code, production assets, installs, saves, or all-ending implementation.

This describes the scope of this work, not a claim that the workspace contains no other work. The separate `time-loop/` directory appeared during the documentation pass and was not modified or included in the documentation commit.

**After Gate G:** record a continue/redesign decision and authorize only the bounded three-minute software prototype. A draft specification is not authorization by itself.

**After software prototype:** repeat player tests, verify deterministic behavior and reset, then decide on a vertical slice. Scope the slice explicitly.

**Full production:** requires prototype and slice evidence plus an explicit recorded decision. Completing this documentation task does not grant that decision.

## Repository delivery checklist

- [x] README and project instructions.
- [x] Requested `00`â€“`13` document set, plus audit and validation documents `14`â€“`18`.
- [x] Source-part coverage index.
- [x] Local Git commit created successfully for the documentation delivery; separate `time-loop/` work excluded.
- [x] GitHub remote connected and documentation pushed to [ChefEase/Time-Loop](https://github.com/ChefEase/Time-Loop), branch `master`, on 2026-09-17. Repository visibility was not independently verified; confirm Private in GitHub settings if required.

Next useful work is the paper session described in [16](16_PROTOTYPE_SPEC.md), while resolving the specific open fiction/route questions. Do not respond to an open gate by building a larger prototype.

**Phase 10 expansion, 2026-09-19:** the user subsequently authorized implementation of the full bounded three-minute causal prototype in `main.tscn`. The implementation is covered by DEC-24 and its automated checks. DEC-25 records the successful human walkthrough; Gate G remains OPEN only for the broader paper, canon, and production-decision requirements listed above.

**Phase 10 software validation result, 2026-09-19:** the user reports completing the `main.tscn` walkthrough and confirming that the tester understood the causal chain. The bounded software prototype is recorded as passing human acceptance. This does not close Gate G; unresolved paper/canon validation, alternate Greywick routes, and production authorization remain separate requirements.


