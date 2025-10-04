## AI Coding Assistant Instructions

Project: Unity C# RPG prototype (core combat, movement, AI, saving, scene transitions, cinematics).

### High-Level Architecture
- Scripts live under `Assets/Scripts/` grouped by domain: `Core`, `Combat`, `Movement`, `Control`, `Saving`, `SceneManagement`, `Cinematics`.
- Central pattern: an `ActionScheduler` (see `Core/ActionScheduler.cs`) serializes mutually exclusive player/NPC actions. Any action component implements `IAction` with `Cancel()` and calls `GetComponent<ActionScheduler>().StartAction(this)` before taking control. Cancelling current action prevents animation & NavMesh conflicts.
- Player & AI behaviors compose small, single-purpose MonoBehaviours (e.g. `Fighter`, `Mover`, `Health`). No inheritance hierarchies—favor composition.
- Game state persistence uses a JSON token based system: components that persist implement `ISaveable` (methods `CaptureAsJToken` / `RestoreFromJToken`). `SavingSystem` aggregates state for every `SaveableEntity` via a GUID (`uniqueIdentifier`). Each component's fully-qualified type name is the JSON key inside an entity block.
- Scene transitions & persistence: `SceneManagement/Portal` performs fade-out, save, async scene load, restore, reposition player, save again, then fade-in using `Fader` and `SavingWrapper`.

### Key Domain Components
- Core: `Health` triggers death animation & cancels current action; death state disables other behaviors (e.g., movement via `Mover` checks `health.IsDead`).
- Combat: `Fighter` handles approach (delegates pathing to `Mover`), attack timing (`timeBetweenAttacks`), animation triggers (`attack`, `cancelAttack`), and applies damage through target `Health`. `CombatTarget` is a tag component requiring `Health`.
- Movement: `Mover` wraps `NavMeshAgent` controls, animator velocity syncing (`forwardSpeed` param), save/restore of position & rotation (temporarily disables agent when restoring).
- Control: `PlayerController` prioritizes interactions: (1) combat raycast hits (front-to-back via `Physics.RaycastAll`) then (2) movement (single raycast). `AIController` implements a simple state machine: Attack -> Suspicion timer -> Patrol (waypoint cycle with dwell times). Patrol path provided by `PatrolPath` child transforms.
- Saving: `SaveableEntity` auto-assigns deterministic GUIDs in edit mode (`[ExecuteAlways]`). Ensures uniqueness with a static lookup, re-generating if conflicts.
- Cinematics: Timeline events temporarily disable player input via `CinematicControlRemover` (hooks PlayableDirector `played` / `stopped`).

### Conventions & Patterns
- Always gate logic on `health.IsDead` early in `Update()` to short-circuit behavior.
- When starting any new exclusive behavior (movement, combat, cutscene control) call `ActionScheduler.StartAction(this)` first; implement `Cancel()` to revert animator triggers and stop movement.
- Animator parameters in use: `forwardSpeed` (float), triggers `attack`, `cancelAttack`, `die`.
- Use `GetComponent<...>()` on demand; caching only when repeatedly used per frame (e.g., `Mover` caches `NavMeshAgent`).
- Range checks use plain `Vector3.Distance`—keep consistent for new behaviors unless performance proves an issue.
- Saving: Only store primitive state—convert `Vector3` via extension helpers (`JsonStatics.ToToken` / `ToVector3`). For new saveable components, keep payload flat JSON under their type key.
- Scene transitions rely on player tag (`Player`)—new systems that reposition the player should disable the NavMeshAgent before warping then re-enable.

### Typical Workflows
- Add a new action (e.g., Casting): create `CastingAction : MonoBehaviour, IAction`, in `Update()` ensure early exit on death; on start call `ActionScheduler.StartAction(this)`; on cancel stop animations/effects.
- Add persistent component: implement `ISaveable`, return minimal serializable shape (use `JObject` or primitives -> `JToken`), avoid storing references—reconstruct via GUID lookups if needed.
- Extend AI states: modify `AIController.Update()` preserving order (Attack > Suspicion > Patrol). Add timers in `UpdateTimers()` and new behaviors that cancel or start actions appropriately.
- Portals: ensure matching `destination` enum value between paired `Portal` instances and set `sceneToLoad` build index in inspector.

### External Dependencies / APIs
- Unity NavMesh (`NavMeshAgent`) for pathfinding—disable before manual position set (`Warp` or transform changes) and re-enable after.
- Newtonsoft.Json (`JToken`, `JObject`) used instead of Unity's `JsonUtility` for flexible dynamic structures.
- Unity Timeline (`PlayableDirector`) for cinematics event hooks.

### Adding Code Safely
- Always null-check target references (`target == null`) before distance or method calls.
- Maintain consistent animator trigger reset order found in `Fighter.Cancel()` to avoid stuck animations.
- When restoring saved transforms, disable agent, mutate transform, then re-enable and cancel current action to clear stale movement/attack.

### Quick Reference File Map
- Action scheduling: `Core/ActionScheduler.cs`, interface `Core/IAction.cs`
- Health & death handling: `Core/Health.cs`
- Combat logic: `Combat/Fighter.cs`, target marker `Combat/CombatTarget.cs`
- Movement & saving: `Movement/Mover.cs`
- Player input: `Control/PlayerController.cs`
- AI state machine: `Control/AIController.cs`, path data `Control/PatrolPath.cs`
- Persistence: `Saving/SaveableEntity.cs`, `Saving/SavingSystem.cs`, helpers `Saving/ISaveable.cs`
- Scene transitions & fades: `SceneManagement/Portal.cs`, `SceneManagement/Fader.cs`, boot/loading `SceneManagement/SavingWrapper.cs`
- Cinematics gating control: `Cinematics/CinematicControlRemover.cs`, trigger `Cinematics/CinematicTrigger.cs`

### When Unsure
Prefer reusing an existing pattern above; if introducing a new system, integrate with `ActionScheduler` and `ISaveable` only if it needs exclusivity or persistence respectively.

---
Provide feedback if any domain needs deeper guidance or if new subsystems (inventory, quests, stats) are added so instructions can be updated.