# Greywick map

**CANON v0.1:** conceptual topology, not a measured navigation map.

## Surface layout

```text
                         NORTH
                     OBSERVATORY
                    Observatory Hill
              CEMETERY          SCHOOL
                        CHURCH
          VALE HOUSE          LIBRARY / ARCHIVES
                 TOWN HALL       POLICE
                       TOWN SQUARE
                   CAFÉ       FLORIST + ALLEY
                 WORKSHOP     GENERAL STORE
                SUBSTATION    CLINIC
                    BUS / FREIGHT DEPOT
                         SOUTH
```

This communicates neighborhood order only. It does not establish line of sight, collision points, route lengths between peripheral buildings, or NPC starting positions.

## Source walking times

Approximate real-time player travel, before interaction or waiting:

| Route | Seconds |
| --- | ---: |
| Vale House ↔ Square | 15 |
| Square ↔ Café | 8 |
| Square ↔ Town Hall | 7 |
| Square ↔ Police Station | 12 |
| Square ↔ Library | 18 |
| Square ↔ Church | 22 |
| Square ↔ School | 35 |
| Square ↔ Clinic | 35 |
| Square ↔ Workshop | 35 |
| Square ↔ Depot | 45 |
| Square ↔ Substation | 55 |
| Square ↔ Observatory | 70 |

**PROPOSED measurement convention:** times are bidirectional, from named entrances via the square. Other paths need explicit edges. A sum via the square is a feasible candidate route under this convention, not proof of the shortest route.

The example's 8:00:25 Clara meeting cannot be verified without Clara's starting position. If she starts at school, house → square → school is 50 seconds before speaking.

## Underground topology

Observatory → Meridian service tunnel → old research chamber → old records vault (Town Hall connection) → church crypt access → coolant control → substation.

Exact junctions, locks, grades, access permissions, and traversal times are OPEN. A tunnel reaching the underground system does not automatically bypass every locked door.

## Knowledge shortcuts

| Shortcut | Source purpose | Missing specification |
| --- | --- | --- |
| Café kitchen alley | Saves about 15 seconds | Endpoints and compared route |
| Church cemetery path | Saves 20–25 seconds toward Observatory Hill | Endpoints, comparison route, access |
| Maintenance culvert | Workshop to substation; saves about 20 seconds | Traversal time and hazards/access |
| Florist rear passage | Square to St. Jude Alley; murder-critical | Entrances, length, sight lines |

Shortcuts are physically present each loop. Knowledge reveals them; a discovery flag must not magically unlock a physical gate.

## Geography validation

For each paper route record: departure location/time, edge duration, arrival, interaction duration, target availability, and safety margin. No teleportation. Do not subtract a shortcut saving from an unrelated square route.

Jonah leaves the depot at 8:02:30 yet is still on his route at 8:04:45, although the player can travel depot → square → substation in 100 seconds under the proposed convention. His route needs an authored detour or service stop, not an unexplained delay.

**PROPOSED paper-only route:** depot departure 8:02:30 → square 8:03:15 → 60-second manifest check at the general-store/square delivery point → departure 8:04:15 → collision junction 8:04:45 → substation 8:05:10, with relay handoff complete 8:05:30. This preserves the collision time while explaining the delay. The delivery point and durations require map validation.

## Information has geography

1998 supports landlines, pagers, cassette recorders, disposable cameras, newspapers, handwritten notes, radio dispatch, filing cabinets, physical records, keys, answering machines, and printed photographs. No smartphones, instant messaging, GPS tracking, social media, ubiquitous cameras, or modern web navigation.
