# 🎮 BomberMan Remake


A modern, object‑oriented recreation of the classic Bomberman built with Unity and C#. The project emphasizes clean architecture, data‑driven design, and performant gameplay systems that scale.

## Key Features

- Core gameplay: player movement, bomb placement, explosions, destructible tiles, power‑ups, win/lose states.
- AI: enemy pathfinding with dynamic obstacle avoidance and target acquisition.
- Architecture: component‑based design, interface‑driven contracts, ScriptableObjects for tunable data.
- Performance: pooling for frequently spawned objects, grid queries, and URP rendering optimizations.
- UI/Audio: real‑time HUD updates, feedback systems, BGM/SFX mixing.

---

## Architecture Overview

- Generic pathfinder (A*/BFS style) over a grid of level blocks; optimized neighbor detection and path reconstruction.
- `ScriptableObject` assets define bombs, enemies, and levels
- Decoupled systems via C# events and interfaces (e.g., `IHittable`), reducing coupling and easing testing.
- Custom serializable dictionary improves Inspector ergonomics for keyed data.
- Object pooling reduces GC pressure; lightweight logs and profiler markers guide optimization.

---

## Project structure

```
Assets/
├─ Scripts/
│  ├─ Core/               # Algorithms, data structures (pathfinding, grid)
│  ├─ Behaviours/         # MonoBehaviours for gameplay
│  ├─ Controllers/        # Input/entity controllers
│  ├─ Management/         # Managers (game, audio, pooling)
│  ├─ UI/                 # HUD and UI logic
│  └─ ScriptableObjects/  # Tunable game data
├─ Art/
├─ GameData/
└─ Prefabs/
```

---


## Tech Stack

- Unity 2022.3+ (URP)
- C# 10
- TextMesh Pro
- Git

