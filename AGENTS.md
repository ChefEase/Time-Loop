# 8:15 Project Instructions

## Current phase

PRE-PRODUCTION. Do not start full production or create gameplay code until the documented gates authorize that work. The current authorized work is documentation, design analysis, and paper validation. Do not install the production toolchain or create finished assets now.

## Project

8:15 is a deterministic 2D time-loop detective game planned for Godot .NET and C#. It is developed by one person for Windows first.

## Design rules

- Knowledge is the main progression mechanic.
- Physical world state resets; player knowledge persists separately.
- Critical events must not depend on uncontrolled randomness.
- NPC schedules must be data-driven with bounded conditional deviations.
- Prefer systemic interactions over one-off scripted hacks.
- Support multiple solutions where practical; never gate story by loop number.
- Travel time, information access, and current-loop evidence matter.
- Distinguish developer truth, player observations, hypotheses, and confirmed causal relationships.

## Architecture, when implementation is authorized

- Use C# and composition rather than giant inheritance trees.
- Centralize physical world state; keep knowledge state separate.
- Use events between independent systems and stable event ordering.
- Avoid tightly coupling NPC code or embedding world rules in dialogue scripts.
- Keep presentation separate from authoritative simulation.

## Scope

No multiplayer, online services, combat system, procedural generation, extra towns, or unapproved major systems. No finished character roster, soundtrack, whole-town build, asset purchases, trailers, Steam page, save system, or all-ending implementation during pre-production.

## Workflow

Before large changes, explain the change and affected files. Make the smallest relevant change. For code, build and run appropriate tests; for documentation, validate links, consistency, and affected paper scenarios. Do not refactor unrelated work. No engine build is expected in a documents-only repository.

## Documentation authority

`docs/00_START_HERE.md` defines ownership of design facts. Preserve CANON, PROPOSED, and OPEN labels. Do not silently resolve a source contradiction or mark a human playtest complete. Keep the audit, paper results, decisions, and gate status consistent. Implementation contradictions must be flagged.
