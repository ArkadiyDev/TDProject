# TDProject — Tower Defense on Unity 6

> A from-scratch Unity project built to **demonstrate practical C# and Unity engineering**: dependency injection, a layered UI stack, intent-based input, data-driven gameplay tuning, object pooling and async asset loading.

[![Unity](https://img.shields.io/badge/Unity-6000.0.3f1-000000?style=flat&logo=unity&logoColor=white)](https://unity.com)
[![C#](https://img.shields.io/badge/C%23-9.0-239120?style=flat&logo=csharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/)
[![.NET](https://img.shields.io/badge/.NET-Standard_2.1-512BD4?style=flat&logo=dotnet&logoColor=white)](https://learn.microsoft.com/dotnet/standard/net-standard)
[![Zenject](https://img.shields.io/badge/Zenject-9.1.0-3D8BC0?style=flat&logo=diagramsdotjs&logoColor=white)](https://github.com/svermeulen/Zenject)
[![UniTask](https://img.shields.io/badge/UniTask-2.x-2E7D32?style=flat)](https://github.com/Cysharp/UniTask)
[![Addressables](https://img.shields.io/badge/Addressables-2.1.0-007ACC?style=flat&logo=unity&logoColor=white)](https://docs.unity3d.com/Packages/com.unity.addressables@2.1/manual/index.html)
[![Input System](https://img.shields.io/badge/Input%20System-1.8.2-661D4B?style=flat&logo=unity&logoColor=white)](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.8/manual/index.html)
[![License](https://img.shields.io/badge/license-MIT-green?style=flat)](LICENSE)

---

## About this project

This is a **portfolio / demonstration project**, not a shipped game. Its explicit purpose is to showcase how I approach game architecture in C# and Unity: how I structure modules, where I put the seams, how data and behaviour are separated, and how I keep the codebase testable and free of hidden coupling.

Everything here was written from scratch in a single codebase — no third-party gameplay frameworks beyond a DI container, a task library and Unity's own asset pipeline.

| | |
|---|---|
| **Genre** | Tower Defense (waves → towers → economy → win/lose) |
| **Engine** | Unity 6 (`6000.0.3f1`) |
| **Language / runtime** | C# 9, .NET Standard 2.1 |
| **Codebase** | 136 C# files · ~3,900 lines · 6 modules |
| **History** | 31 commits over ~6 weeks, refactored incrementally (see [Project evolution](#project-evolution)) |
| **Scenes** | `Startup` (bootstrap + loading) → `Gameplay` (arena) |

The game loop: invaders spawn in configurable **waves**, walk a **route** toward your **castle**, you spend **gold** to place **towers** that auto-target and shoot them, and killing invaders pays out more gold. Clear the last wave to win; let the castle fall to lose.

---

## Feature highlights

- **Data-driven wave system** — waves, invader groups, routes and inter-group delays are configured in `ScriptableObject` assets, not in code.
- **Tower building with validation** — grid-snapped placement, a translucent ghost preview that turns green/red, per-corner buildability checks, collision detection against existing buildings, and an affordability gate before the purchase is committed.
- **Pluggable targeting** — the target-selection strategy is chosen by data (`ClosestTargetsFinder` / `DefaultTargetsFinder`), not by a hard-coded `if`.
- **Real economy** — a wallet with multiple currencies, atomic (all-or-nothing) purchases, event-driven UI updates, and rewards paid out on kill.
- **Layered UI** — Windows (stack), Popup and Dialog layers, each with its own input context and focus priority.
- **Intent-based input** — the UI and gameplay layers understand only a domain-level `InputIntent` enum; no `InputAction` or `KeyCode` leaks into them. (Four legacy `Input.*` call sites remain — see [Known limitations](#known-limitations--roadmap).)
- **Game speed control** — pause / 1× / 2× / 3× implemented as a single `Time.timeScale` mapping.
- **Arena restart** — a `IResettable` contract + Zenject list injection resets the container-bound services (wallet, towers) in one call. The arena's own wave state and the castle are plain `new`-created objects and are reset separately, outside that list (see [Known limitations](#known-limitations--roadmap)).
- **Floating combat text** — pooled damage numbers rendered with TextMeshPro.
- **Async asset loading & pooling** — all spawnable views are pooled and their prefabs are loaded through Addressables.

---

## Tech stack

| Package | Version | Why it's here |
|---|---|---|
| **Zenject** | vendored in `Assets/Plugins/Zenject` (`9.1.0`) | Dependency injection, per-scene composition roots, automatic `List<T>` injection, and lifecycle hooks (`IInitializable` / `IDisposable`). Picked over a hand-rolled service locator because constructor injection makes the dependency graph explicit and the domain classes testable without a running container. |
| **UniTask** | git (`Cysharp/UniTask`) | `async`/`await` for the wave/game-flow orchestration and cancellable delays (`UniTask.Delay` + `CancellationTokenSource`), instead of nested coroutines. |
| **Addressables** | `2.1.0` | Runtime prefab loading for pooled views (`AssetReference`, `LoadAssetAsync`), so content is decoupled from scene references. |
| **Input System** | `1.8.2` | New input backend with `.inputactions` assets and a generated strongly-typed C# wrapper. |
| **TextMesh Pro / uGUI** | `2.0.0` | UI text and buttons, `TMP_Text.SetText` for allocation-free updates. |
| **SerializeReferenceExtensions** | git (`mackysoft`) | `SubclassSelector` dropdown for polymorphic `[SerializeReference]` fields — this is what makes the ability system inspector-friendly. |
| **Test Framework** | `1.4.4` | Installed and ready; no test assemblies exist yet (see [Known limitations](#known-limitations--roadmap)). |

---

## Architecture

### Module map

```
                 ┌──────────────────────────────────────────┐
                 │               GameState                  │
                 │  Bootstrap · StateMachine                │
                 │  ArenaInstaller — composition root       │
                 └──────────────────┬───────────────────────┘
                                    │ binds & drives
   ┌───────────────┬────────────────┼────────────────┐
   │               │                │                │
┌──▼──────────┐ ┌──▼───────────┐ ┌──▼───────────┐ ┌──▼─────────────────┐
│     UI      │ │ InputSystem  │ │   Economy    │ │        Core        │
│  Mediator   │◄┤ InputIntent  │ │   Wallet     │ │  Arena · Waves     │
│  Observer   │ │   bus        │ │   Rewards    │ │  Invaders          │
│  win. stack │ └──────────────┘ └──┬───────────┘ │  Towers            │
└──────▲──────┘                     │             │  Building          │
       │                            └────────────►│  Damaging          │
       │                                          │  Abilities         │
       └─────────────────────────────────────────►└─────────┬──────────┘
                                                            │
                                             ┌──────────────▼──────────────┐
                                             │           Common            │
                                             │  Object pooling · Floating  │
                                             │  text · Game speed service  │
                                             └─────────────────────────────┘
```

Note: `Common` is not a pure leaf — `GameSpeedService` subscribes to `IInputService`, so `Common → InputSystem` (see the table below).

**Dependency rules (arrows point from consumer to dependency)**

| Module | Depends on | Role |
|---|---|---|
| `Common` | `InputSystem` | Utilities: pooling, floating text, game speed. Not a pure leaf — `GameSpeedService` (`Assets/Scripts/Common/GameSpeed/GameSpeedService.cs`) subscribes to `IInputService` to toggle pause. |
| `Core` | `Common`, `Economy`, `InputSystem` | Gameplay domain. Rewards flow out through `IRewardProvider`. |
| `Economy` | `Core.Arenas` | Wallet & rewards; `WalletService` implements `IResettable` (declared in `namespace Core.Arenas`, file `Core/Arenas/Restart/IResettable.cs`) — the only upward dependency, and a deliberate one (restart is a cross-cutting concern). |
| `InputSystem` | — | Standalone: translates Unity input into `InputIntent`. |
| `UI` | `Economy`, `Common`, `Core.Building`, `InputSystem` | Mediator/Observer/stack over the views; reads wallet events and drives the building service. |
| `GameState` | all | Composition root and scene-flow owner. |

> ⚠️ **The dependency graph contains a cycle.** `Core → Economy → Core.Arenas` is circular, and Unity `.asmdef` files cannot express cycles. Splitting the modules therefore requires first moving the `IResettable` contract down into `Common` — see [Next steps](#next-steps).

**Layering rule:** `MonoBehaviour`s are a thin Unity-facing shell. Gameplay rules live in **plain C# classes** (`Arena`, `Invader`, `Tower`, `Castle`, `WalletService`) that know nothing about `MonoBehaviour`, and views (`*View`, `*Processor`) are dumb. This is what makes the domain layer unit-testable without a running player.

### Scene flow

```
Startup.unity                 Gameplay.unity
┌───────────────────┐         ┌─────────────────────────────────┐
│ GameBootstrapper  │         │ Zenject SceneContext            │
│  └─ Game          │         │  └─ ArenaInstaller (bindings)   │
│      └─ GameState │───────► │      └─ ArenaSystem             │
│          Machine  │  load   │           └─ Arena / ArenaModel │
│  BootstrapState   │         │               ├─ InvadersWave   │
│  LoadLevelState   │         │               ├─ Invaders       │
└───────────────────┘         │               ├─ Towers         │
                              │               └─ Castle         │
                              └─────────────────────────────────┘
```

`StateMachine` (`Assets/Scripts/GameState/StateMachine.cs:9`) owns a `Dictionary<Type, IExitableState>` and transitions are typed: `Enter<GameCoreLoopState>()` — no string keys, no reflection. `GameStateMachine` (`Assets/Scripts/GameState/GameStateMachine.cs:7`) is the concrete registry that populates it.

---

## Key subsystems

### 1. Dependency injection — Zenject as composition root

`ArenaInstaller` (`Assets/Scripts/GameState/ArenaInstaller.cs:24`) is the single place where the object graph is declared. It is a `MonoInstaller` attached to the `SceneContext` in `Assets/Scenes/Gameplay.unity`, so it can bind scene instances and asset references directly.

```csharp
Container.BindInterfacesAndSelfTo<WalletService>()
    .AsSingle()
    .WithArguments(_currenciesRoster, _arenaSettings.StartCurrencies);
```

Techniques used:

- **`BindInterfacesAndSelfTo<T>`** — the concrete type stays resolvable while the interface becomes the contract everyone codes against.
- **`WithArguments(...)`** — injects non-serializable dependencies (the currency roster, a camera, pool references) explicitly.
- **`NonLazy()`** — forces eager construction for services with side effects (`GameSpeedService`, `BuildingService`).
- **Automatic list injection** — `ArenaRestartHandler` declares `List<IResettable>` in its constructor (`Assets/Scripts/Core/Arenas/Restart/ArenaRestartHandler.cs:9`) and Zenject fills it with every **`IResettable` that is bound in the container**. Today that is `WalletService` and `BuildingService` only: `Arena` and `Castle` implement `IResettable` as well, but they are created with `new` (`Assets/Scripts/Core/Arenas/ArenaSystem.cs:19`, `Assets/Scripts/Core/Arenas/Arena.cs:23`) and are therefore **not** collected — they are reset explicitly by `ArenaSystem.RestartArena()` (`Assets/Scripts/Core/Arenas/ArenaSystem.cs:46`). A new resettable needs zero changes to the restart logic **only if it is container-bound**.
- **Lifecycle hooks** — `UIObserver<T>` implements `IInitializable` / `IDisposable` (`Assets/Scripts/UI/UIObserver.cs:6`), so subscribe/unsubscribe is driven by the container and happens automatically on teardown.
- **Method injection on a `MonoBehaviour`** — `ArenaSystem` uses `[Inject] private void Construct(...)` (`Assets/Scripts/Core/Arenas/ArenaSystem.cs:15`) instead of inspector wiring.

### 2. UI service — Mediator + Observer + a window stack

The UI is a four-layer architecture, each layer with one job:

```
IUiView  ────────────  UIView (abstract)         — pure view, knows nothing else
    ▲
UIMediator<TView>  ──  Mediator                  — translates view events → domain events
    ▲
UIObserver<TMed>  ─── Observer                  — decides what a domain event means
    ▲
IUIService  ───────── Facade / Service Locator  — the only type gameplay code sees
```

`UIService` (`Assets/Scripts/UI/UIService.cs:8`) is the facade and the stack manager:

- **Type-keyed registry** — `Dictionary<Type, IUiMediator>` built in the constructor; `OpenWindow<TMediator>()` resolves the concrete mediator from the type key, so call sites never name a concrete UI class.
- **Window stack** — `Stack<IUiMediator>`; opening a window hides the current top and pushes, closing pops and re-shows the new top. The root window can never be closed (`_windowStack.Count <= 1` guard at `Assets/Scripts/UI/UIService.cs:108`).
- **Single-slot overlay layers** — one popup and one dialog at a time; opening a window force-closes both.
- **Facade** — `IUIService` exposes 7 methods over the entire view/mediator graph.

Concrete screens follow a strict 4-file convention:

```
UI/Implementations/HUD/
├── HudMediator.cs         — events + input handling, no decisions
├── HudObserver.cs         — reacts to mediator events
├── HudView.cs             — TMP text + buttons
└── HudInputContext.cs     — InputIntent → Action table
```

A good example of the separation: `HudMediator` raises `PopupButtonPressed`; only `HudObserver` knows that this should open a popup (`Assets/Scripts/UI/Implementations/HUD/HudObserver.cs:34`).

### 3. Context-aware input — one intent, many meanings

The input stack deliberately erases Unity's input types from the rest of the codebase.

```
.inputactions asset  ("Hotkeys" map, 7 actions)
        │  PlayerInput.onActionTriggered
        ▼
CoreInputService  ── Adapter: action-name → InputIntent (Dictionary lookup)
        │  event Action<InputIntent> OnInputActionExecuted
        ▼
UIInputMediator   ── bridges input bus → UI service
        │
        ▼
UIService.ProcessKeyInput(intent)
        │  priority chain: dialog → popup → top window
        ▼
XInputContext.HandleKeyPressed(intent)
        │  Dictionary<InputIntent, Action>
        ▼
domain call  (SetSpeed / SwitchBuildingMode / TryBuildTower / CloseRequest)
```

- **`InputIntent`** (`Assets/Scripts/InputSystem/InputIntent.cs:3`) is the only input type the rest of the game knows: `PauseSwitch`, `SpeedNormal`, `SpeedDouble`, `SpeedTriple`, `BuildMode`, `Escape`, `LeftMouseClick`.
- **Contextual meaning** — `Escape` means "close the pause menu" in `PauseMenuInputContext`, "close the popup" in `PopupInputContext`, and "exit build mode, otherwise open the pause menu" in `HudInputContext` (`Assets/Scripts/UI/Implementations/HUD/HudInputContext.cs:19`). The intent is the same; the meaning follows the focus.
- **Focus routing** — `UIService.ProcessKeyInput` (`Assets/Scripts/UI/UIService.cs:87`) walks a priority chain (dialog → popup → window) and stops at the first handler that consumes the intent. The `bool` return value of `IInputHandler.HandleKeyPressed` *is* the "I consumed this" signal.
- **Command tables** — every context owns a `Dictionary<InputIntent, Action>`; adding a binding is a one-liner, and swapping behaviour is swapping a delegate.
- **Lifecycle correctness** — `CoreInputService` subscribes in `OnEnable` and unsubscribes in `OnDisable` (`Assets/Scripts/InputSystem/CoreInputService.cs:25`), which is why nothing leaks when the object is disabled or destroyed.

### 4. Game state machine — typed transitions, no stringly-typed APIs

```
IExitableState           void Exit()
├── IState               void Enter()
└── IPayloadedState<T>   void Enter(T payload)
```

`StateMachine` (`Assets/Scripts/GameState/StateMachine.cs:7`) stores states in a `Dictionary<Type, IExitableState>` and exposes generic entry points:

```csharp
_stateMachine.Enter<LoadLevelState, string>("Gameplay");   // payload-carrying transition
```

Because `IState` and `IPayloadedState<T>` share the `IExitableState` base, the machine can exit *any* state without knowing its payload type.

The Unity-specific part is kept behind a seam: `SceneLoader` depends on the `ICoroutineRunner` interface (`Assets/Scripts/GameState/Interfaces/ICoroutineRunner.cs`), implemented by the `GameBootstrapper` MonoBehaviour (`Assets/Scripts/GameState/GameBootstrapper.cs:6`). The state machine itself never touches `MonoBehaviour`.

### 5. Gameplay core

| Concern | Implementation | Key file |
|---|---|---|
| Arena / level | `Arena` (flow, events) + `ArenaModel` (wave state) + `ArenaView` (scene refs) | `Assets/Scripts/Core/Arenas/Arena.cs:9` |
| Waves | `InvadersWave` spawns groups per route with delays; cancellable via `CancellationTokenSource` | `Assets/Scripts/Core/Waves/InvadersWave.cs:34` |
| Invaders | Plain C# entity (`Invader`) + `InvaderModel` (pure data) + `InvaderMovementProcessor` (motion) + `InvaderLink` (MonoBehaviour adapter) | `Assets/Scripts/Core/Invaders/Invader.cs:9` |
| Towers | `Tower` (logic) + `TowerModel` (cooldown state) + `TowerView` + projectile chain | `Assets/Scripts/Core/Towers/Tower.cs:10` |
| Building | `BuildingService` (facade) + `BuildingPlacementService` (grid/validation) + `BuildingProcessor` (central tick) + `CollisionChecker` | `Assets/Scripts/Core/Building/BuildingService.cs:9` |
| Damage | `DamageService` — one place computes and applies damage; publishes `OnDamageDealt` | `Assets/Scripts/Core/Damaging/DamageService.cs:5` |
| Castle | `Castle` (health model) + `CastleView` (raises `InvaderEntered`) | `Assets/Scripts/Core/Castles/Castle.cs:8` |

**Notable implementation details**

- **Async game flow** — `Arena.RunGameFlow()` is a `UniTaskVoid` using `UniTask.Delay` for wave delays (`Assets/Scripts/Core/Arenas/Arena.cs:29`), and `InvadersWave` cancels pending spawns through a `CancellationTokenSource` (`Assets/Scripts/Core/Waves/InvadersWave.cs:44`). No coroutine nesting.
- **Central tick instead of scattered `Update()`** — `BuildingProcessor` drives `building.Tick(deltaTime)` for every registered building from a single `Update()` loop (`Assets/Scripts/Core/Building/BuildingProcessor.cs:21`). Towers therefore contain no `MonoBehaviour` logic at all.
- **Deferred removal** — `InvaderSystem` queues removals in a `HashSet<Invader>` and reconciles them at the end of `Update()`, so the live list is never mutated while it is being iterated (`Assets/Scripts/Core/Invaders/InvaderSystem.cs:49`).
- **Grid snapping** — `BuildingPlacementService` snaps to a configurable grid with `Mathf.Round(pos / grid) * grid` and validates **all four corners** of the footprint with separate raycasts (`Assets/Scripts/Core/Building/BuildingPlacementService.cs:107`).
- **Zero-allocation physics** — `TargetsFinder` queries with `Physics.OverlapSphereNonAlloc` into a pre-allocated `Collider[64]` buffer (`Assets/Scripts/Core/Towers/Targeting/TargetsFinder.cs:15`), and `ClosestTargetsFinder` compares **squared** distances to avoid a `sqrt` per candidate (`Assets/Scripts/Core/Towers/Targeting/ClosestTargetsFinder.cs:22`).
- **Try-pattern APIs** — `Route.TryGetNextWaypoints(current, out next)` returns a bool instead of throwing, and never returns null.

### 6. Economy

```
InvaderSettings._rewards
  → Invader.Rewards
  → InvaderDeathHandler → IRewardProvider.ApplyReward
  → ArenaRewardProvider → WalletService.IncreaseCurrencies
  → WalletService.ChangeCurrency → event CurrencyChanged(id, delta)
  → HudMediator.OnMoneyChanged → HudView.SetGoldCounter
```

- **Atomic purchases** — `TryDecreaseCurrencies` verifies the *entire* cost list is affordable before decrementing anything, so a purchase either fully succeeds or fully fails (`Assets/Scripts/Economy/Wallets/WalletService.cs:42`).
- **Delta-based notification** — the event carries the change, not the new total, so listeners never need to re-read state.
- **Strategy seam** — `IRewardProvider` allows an arena-specific implementation to be swapped without touching the death handler.
- **Validation (partial)** — unknown currency ids throw in `ChangeCurrency` (`Assets/Scripts/Economy/Wallets/WalletService.cs:90`) and in `GetCurrency` (`:31`), but are only `Debug.Log`-ed in `CanAfford` (`:74`). Negative amounts throw `ArgumentException` **only** on the purchase path, inside `CanAfford` (`:79`) — `IncreaseCurrencies` accepts a negative amount silently.

### 7. Data-driven configuration (ScriptableObject)

Tuning lives in assets under `Assets/Settings/`, each created via `[CreateAssetMenu(menuName = "TD/Settings/" + nameof(X))]`:

| Asset | Configures |
|---|---|
| `ArenaSettings` | first-wave delay, wave interval, wave composition (groups × route × count × delay), starting currencies |
| `InvaderSettings` | health, damage, speed, prefab `AssetReference`, reward payload |
| `TowerSettings` | range, damage, fire rate, cost, projectile settings, **and the targeting strategy asset** |
| `CastleSettings` | base health |
| `BuildingSettings` | prefab `AssetReference`, footprint size |
| `ProjectileSettings` | prefab `AssetReference`, speed |
| `CurrencySettings` / `CurrencySettingsRoster` | currency id, display name, icon; roster enumerates all valid currencies |
| `AbilitySettings` | polymorphic effect list + targeting strategy (`[SerializeReference]`) |

`TargetsFinder` (`Assets/Scripts/Core/Towers/Targeting/TargetsFinder.cs:7`) is an **abstract ScriptableObject**: the concrete strategy is a separate asset, so which targeting rule a tower uses is a designer decision made in the inspector.

### 8. Object pooling + Addressables

`GameObjectPool<T>` (`Assets/Scripts/Common/Pooling/GameObjectPool.cs:8`) is a generic `MonoBehaviour` pool built on `UnityEngine.Pool.ObjectPool<T>`:

```csharp
_pool = new ObjectPool<T>(
    createFunc: CreateView,        // Instantiate from the loaded Addressable prefab
    actionOnGet: OnGet,
    actionOnRelease: OnRelease,    // SetActive(false)
    actionOnDestroy: OnDestroyItem,
    collectionCheck: true,         // catches double-release bugs in development
    defaultCapacity: 16, maxSize: 150);
```

- **Async prefab loading** — `Addressables.LoadAssetAsync<GameObject>(assetReference)` + `await handle.Task`, with `Addressables.Release(handle)` in `OnDestroy` to avoid leaking the handle.
- **Typed subclasses** — `InvaderViewPool`, `TowerViewPool`, `ProjectileViewPool`, `FloatingTextPool` are one-liners over the generic base.
- **Release driven by lifetime events** — the pool is not polled. `InvaderFactory` subscribes to `invader.Removed` and releases the view (`Assets/Scripts/Core/Invaders/InvaderFactory.cs:34`); `ProjectileFactory` releases the projectile via the completion callback (`Assets/Scripts/Core/Towers/ProjectileFactory.cs:20`). Views return to the pool exactly when their entity dies.

### 9. Abilities (work in progress)

The newest module, built on the SerializeReference extensions. `AbilitySettings` (`Assets/Scripts/Core/Abilities/AbilitySettings.cs:7`) holds two polymorphic fields:

```csharp
[SerializeReference, SubclassSelector] private List<IAbilityEffect> _effects = new();
[SerializeReference, SubclassSelector] private AbilityTargeting _targeting;
```

- **`AbilityTargeting`** is an abstract strategy with `Start` / `Update` / `Stop`; `SelfAbilityTargeting` and `AOEAbilityTargeting` implement it (`AOEAbilityTargeting` spawns a radius preview, raycasts on click and applies effects to everything in an `OverlapSphere`).
- **`AbilityEffect`** is an abstract command; `DamageEffectSettings` applies damage, guarded by a capability check (`if (target is IDamageable damageable)`).
- **`ITarget` / `IDamageable`** are capability interfaces, so the ability system, the tower targeting system and the damage service all speak the same contract.

> ⚠️ This module is **not yet wired into the game loop** — `AbilityCaster` and `AbilityServices` are not referenced by any installer or scene object yet. See [Known limitations](#known-limitations--roadmap).

---

## Design patterns

| Pattern | Where | Why |
|---|---|---|
| **Dependency Injection** | `ArenaInstaller`, all services | Decouples gameplay from construction; enables constructor-injected tests. |
| **Installer / Composition Root** | `ArenaInstaller` | One place per scene declares the whole graph. `BootstrapInstaller` exists but is an empty stub (`Assets/Scripts/GameState/BootstrapInstaller.cs:7`) and is not attached to `Startup.unity` — the startup scene builds its graph by hand in `GameBootstrapper.Awake()`. |
| **Mediator** | `UIMediator<TView>` and 4 implementations | Mediates between a view and the rest of the app; views stay dumb. |
| **Observer** | `UIObserver<T>`, `DamageObserver`, `HudObserver` | Domain events drive reactions without hard references. |
| **Facade** | `IUIService`, `IBuildingService`, `IWalletService` | Narrow, stable API over a complex subsystem. |
| **Service Locator (type-keyed)** | `UIService._mediatorMap` | Resolve a mediator by type without naming concrete classes. |
| **State Machine** | `StateMachine`, `GameStateMachine` | Typed, payload-carrying transitions without string keys. |
| **Strategy** | `TargetsFinder`, `AbilityTargeting`, `IRewardProvider` | Swap algorithm via interface or via a data asset. |
| **Factory** | `InvaderFactory`, `TowerFactory`, `ProjectileFactory` | Centralise complex object construction (pool checkout + wiring). |
| **Command** | `IAbilityEffect`, `Dictionary<InputIntent, Action>` | Behaviour as a first-class, swappable object/delegate. |
| **Adapter** | `CoreInputService`, `InvaderLink` | Translate foreign representations (Unity input, pooled GameObjects) into domain types. |
| **Object Pool** | `GameObjectPool<T>` + 4 subclasses | Avoid per-spawn `Instantiate`/`Destroy` churn. |
| **Template Method** | `UIView.Open/Close`, `AbilityTargeting.Start/Update/Stop` | Fixed skeleton, overridable steps. |
| **Value Object / DTO** | `CurrencyData`, `RewardData`, `InvadersGroup` | Immutable-ish serialisable data passed between layers. |
| **ScriptableObject configuration** | `Assets/Settings/*` | Designer-tunable data, zero code changes to rebalance. |
| **Capability interfaces** | `IDamageable`, `ITarget`, `IBuilding`, `IAttacker` | Small, composable contracts instead of deep type hierarchies. |
| **Event bus** | `IInputService.OnInputActionExecuted` | One-directional publish/subscribe decoupling of the input layer. |

---

## Unity APIs in action

| API | Where | Purpose |
|---|---|---|
| `Physics.OverlapSphereNonAlloc` | `Assets/Scripts/Core/Towers/Targeting/TargetsFinder.cs:15` | Allocation-free target queries |
| `Physics.OverlapSphere` | `Assets/Scripts/Core/Abilities/AOEAbilityTargeting.cs:61` | AOE effect resolution |
| `Physics.Raycast` (×4 corners) | `Assets/Scripts/Core/Building/BuildingPlacementService.cs:112` | Footprint validation |
| `Camera.ScreenPointToRay` | `Assets/Scripts/Core/Building/BuildingPlacementService.cs:73`, `Assets/Scripts/Core/Abilities/TargetingProcessor.cs:36` | Mouse → world picking |
| `SerializeReference` + `SubclassSelector` | `Assets/Scripts/Core/Abilities/AbilitySettings.cs:9` | Inspector-friendly polymorphism |
| `PlayerInput.onActionTriggered` | `Assets/Scripts/InputSystem/CoreInputService.cs:26` | New Input System event bridge |
| `InputAction` code generator | `Assets/Scripts/InputSystem/InputSystem_CoreActions.cs` | Generated strongly-typed wrapper (`IHotkeysActions`) |
| `UniTask.Delay` + `CancellationTokenSource` | `Assets/Scripts/Core/Arenas/Arena.cs:31`, `Assets/Scripts/Core/Waves/InvadersWave.cs:69` | Async, cancellable game flow |
| `UnityEngine.Pool.ObjectPool<T>` | `Assets/Scripts/Common/Pooling/GameObjectPool.cs:48` | Backing store for all pools |
| `Addressables.LoadAssetAsync` / `Release` | `Assets/Scripts/Common/Pooling/GameObjectPool.cs:30` | Async prefab loading + handle cleanup |
| `AssetReference` (serialized) | `Assets/Scripts/Core/**/*Settings.cs` | Asset references stored in data assets |
| `TMP_Text.SetText` | `Assets/Scripts/UI/Implementations/HUD/HudView.cs:18`, `Assets/Scripts/Common/UI/FloatingText/FloatingTextView.cs:20` | Allocation-free text updates |
| `CanvasGroup.alpha` | `Assets/Scripts/GameState/LoadingScreen.cs:18` | Fade transitions |
| `OnDrawGizmos` | `Assets/Scripts/Core/Arenas/ArenaView.cs:15`, `Assets/Scripts/Core/Building/ConstructionSite.cs:27` | Authoring feedback in the editor |
| `OnValidate` | `Assets/Scripts/Core/Building/ConstructionSite.cs:12` | Editor-time grid alignment |
| `ScriptableObject` + `CreateAssetMenu` | `Assets/Scripts/Core/**/*Settings.cs` | Data-driven configuration |
| `HashSet` deferred removal | `Assets/Scripts/Core/Invaders/InvaderSystem.cs:49` | Safe mutation of a live list during iteration |

---

## Repository layout

```
Assets/
├── Scripts/
│   ├── Common/              # Cross-cutting utilities
│   │   ├── GameSpeed/       # Time-scale service
│   │   ├── Pooling/         # Generic GameObjectPool<T>
│   │   └── UI/FloatingText/ # Pooled damage numbers
│   ├── Core/                # Gameplay domain
│   │   ├── Abilities/       # WIP — polymorphic effects & targeting
│   │   ├── Arenas/          # Arena, ArenaModel, Route, Waypoint, Spawner
│   │   │   └── Restart/     # IResettable + ArenaRestartHandler
│   │   ├── Building/        # Placement, ghost, collision, processor
│   │   ├── Castles/         # Castle model + view
│   │   ├── Damaging/        # DamageService, observers, contracts
│   │   ├── Invaders/        # Entity, factory, model, movement, pool
│   │   ├── Towers/          # Tower, projectile, factory
│   │   │   └── Targeting/   # Strategy assets (Closest / Default)
│   │   └── Waves/           # Wave orchestration & wave data
│   ├── Economy/             # Currencies, Wallets, Rewards
│   ├── GameState/           # Bootstrap, state machine, installers
│   │   └── Interfaces/
│   ├── InputSystem/         # InputIntent + Unity Input System adapter
│   └── UI/                  # Service, mediators, observers, views
│       └── Implementations/ # HUD, Popup, PauseMenu, ConfirmationDialog
├── Settings/                # ScriptableObject configuration assets
├── Prefabs/                 # Invaders, Building, Arenas, UI, Abilities
├── Scenes/                  # Startup.unity, Gameplay.unity
├── AddressableAssetsData/   # Addressables groups & schemas
├── Plugins/Zenject/         # Vendored DI container
└── TextMesh Pro/            # TMP resources
```

---

## Getting started

**Requirements:** Unity `6000.0.3f1` (Unity 6). No other tooling required — Zenject is vendored, UniTask and the SerializeReference extensions install from git.

```bash
git clone https://github.com/ArkadiyDev/TDProject.git
```

1. Open the project folder in Unity Hub with the **exact** version above.
2. Wait for Package Manager to resolve `UniTask` and `SerializeReferenceExtensions` from git (first import takes a few minutes).
3. Open `Assets/Scenes/Startup.unity` and press **Play** — it bootstraps, loads `Gameplay.unity` and starts the arena.

Alternatively open `Assets/Scenes/Gameplay.unity` directly and press Play — the `SceneContext` there has its own `ArenaInstaller` and is self-sufficient.

### Controls

| Input | Action |
|---|---|
| `1` / `2` / `3` | Game speed — normal / double / triple |
| `Space` | Pause / resume |
| `B` | Toggle tower build mode |
| `Left Mouse Button` | Place tower (in build mode) |
| `Escape` | Close the top UI layer, or exit build mode |

### How to extend it

| I want to… | I touch |
|---|---|
| Add a new enemy type | `InvaderSettings` asset + `InvaderFactory` binding |
| Change target selection | Create a `TargetsFinder` subclass → assign the asset in `TowerSettings` |
| Add a UI screen | `View` + `Mediator` + `Observer` + `InputContext`, then bind in `ArenaInstaller` |
| Add a gameplay rule that reacts to an event | Subscribe to the existing `event` (e.g. `IDamageService.OnDamageDealt`) in an `IInitializable` |
| Make a system restartable | Implement `IResettable` — it is auto-collected by Zenject |

---

## Known limitations & roadmap

Honest list of what is **not** finished yet. The first group are real defects — the second are design and tooling gaps I know about but have not addressed.

### Bugs to fix

- `Assets/Scripts/GameState/SceneLoader.cs:32` — the wait condition is inverted: `while (waitNextSceneOperation.isDone)` should be `while (!waitNextSceneOperation.isDone)`. Scene loads are therefore not actually awaited before `onLoaded` fires.
- `Assets/Scripts/UI/UIService.cs:23` — the root window is chosen as `allMediators[0]`, an implicit dependency on Zenject binding order. It should be an explicit "root" binding.
- `Assets/Scripts/UI/UIService.cs:45` — `OpenWindow` hides the previous window but never pops it, so the window stack grows monotonically and `CloseCurrentWindow` can never unwind below the root.
- `Assets/Scripts/UI/Implementations/Popup/PopupObserver.cs` and `Assets/Scripts/UI/Implementations/ConfirmationDialog/ConfirmationDialogObserver.cs` — subscribe to `CloseRequested` but do not override `Dispose()`, so subscriptions are not removed on container teardown (`HudObserver` does it correctly).
- `Assets/Scripts/UI/Implementations/Popup/PopupInputContext.cs:21` and `Assets/Scripts/UI/Implementations/ConfirmationDialog/ConfirmationDialogInputContext.cs:21` — `HandleKeyPressed` returns `true` unconditionally, even for unbound intents, which makes those layers greedy input sinks.
- `Assets/Scripts/UI/Implementations/HUD/HudMediator.cs:30` — `CurrencyChanged` is re-subscribed on every `Show()`; calling `Show()` twice without a `Hide()` double-subscribes. The same applies to the `onClick` listeners added at `Assets/Scripts/UI/Implementations/HUD/HudMediator.cs:33`.
- `Assets/Scripts/Core/Arenas/Restart/ArenaRestartHandler.cs:9` — the injected `List<IResettable>` only contains **container-bound** resettables (`WalletService`, `BuildingService`). `Arena` and `Castle` implement `IResettable` too, but are created with `new` and are therefore never collected — they are reset by a separate explicit call in `ArenaSystem.RestartArena()` (`Assets/Scripts/Core/Arenas/ArenaSystem.cs:46`).

### Architecture / tooling gaps

- **No assembly definitions.** All game code compiles into the default `Assembly-CSharp`, so modules are separated by namespace convention only. Splitting `Core`, `UI`, `Economy` and `GameState` into their own `.asmdef` would give compile-time boundaries and enforce dependency direction — but the `Core → Economy → Core.Arenas` cycle must be broken first (see [Next steps](#next-steps)).
- **No automated tests.** `com.unity.test-framework` is installed, but no test assembly exists. The domain layer is plain C# and was written to be testable — `WalletService`, `DamageService` and `Route` are the natural first targets.
- **Legacy input API still present — four call sites.** `Assets/Scripts/Core/Abilities/TargetingProcessor.cs:24` (`Input.GetMouseButtonDown`), `Assets/Scripts/Core/Abilities/TargetingProcessor.cs:36` and `Assets/Scripts/Core/Abilities/AOEAbilityTargeting.cs:38` (`Input.mousePosition`), and `Assets/Scripts/Core/Building/BuildingPlacementService.cs:73` (`Input.mousePosition`) still use the legacy API alongside the new Input System. The plan is to route these through `IInputService` as well.
- **Abilities module is not wired up.** `AbilityCaster` and `AbilityServices` are unreferenced; the module needs an installer binding and a gameplay hook.
- **`InvadersGroup.InvaderSettings` is ignored at runtime.** Per-group enemy variety is configurable in the asset but the factory is bound to a single `InvaderSettings`.
- **No custom inspectors or editor tooling.** The project relies entirely on built-in Unity editors plus `SubclassSelector`.
- **`TowerModel` has a Cyrillic homoglyph** in `СurrentFireTimer` (Cyrillic `С`) — invisible to the compiler, confusing to read.

### Next steps

1. Fix the `SceneLoader` wait condition and add a regression test around it.
2. Break the `Core → Economy → Core.Arenas` cycle — move the `IResettable` contract down into `Common` — and only then introduce `.asmdef` files per module and enforce the dependency direction with `asmdef` references.
3. Add unit tests for `WalletService`, `DamageService` and `Route`.
4. Make the root window explicit in `UIService` and pop (not just hide) windows when opening a new one.
5. Complete the abilities loop and honour per-group `InvaderSettings`.
6. Add a custom editor for wave configuration (curve-based pacing, difficulty presets).

---

## Project evolution

The commit history is effectively a design narrative — each commit is a working vertical slice followed by a refactor:

```
Initial commit → Unity start project → Entrypoint and GameState
  → Addressables package → Zenject plugin
  → Core gameplay: Arena(level), Waypoints, Spawners, Invaders, Invaders waves
  → Castle, invaders route refactor
  → Input System package → Game speed service and input system actions
  → Invaders pool refactor, loading using addressables
  → Buildings placement logic → Tower: building, targeting, shooting
  → Damageable and dying invaders → Refactor Arena MVP
  → Economy: currency, wallet, rewards → Invaders death handler
  → Cleaning arena installer → Tower building condition: cost; wallet service refactor
  → Wave's groups sequence and delay → Invader MVP refactor
  → Targets finding: default, closest
  → Floating text service → Damage service and on damage text
  → TextMesh Pro package
  → UI Service (Windows, Popups, Dialogs)
  → Context-based (active window/popup/dialog) input handler for UI service
  → Clean up CoreInputService
  → Implement decoupled type-based input handling, fix potential memory leaks
  → Implement context-aware input handling and UI stack
  → Restart arena logic after win or defeat
  → Unity serialize reference extensions   (← current HEAD)
```

> **Working tree.** The Abilities module (`Assets/Scripts/Core/Abilities/`, `Assets/Prefabs/Abilities/`, `Assets/Settings/Abilities/`) plus edits to roughly a dozen core gameplay files (`Invader.cs`, `InvaderView.cs`, `InvaderLink.cs`, `InvaderDeathHandler.cs`, `Tower.cs`, `InvadersWave.cs`, `TargetsFinder.cs`, `ClosestTargetsFinder.cs`, `DefaultTargetsFinder.cs`, `DamageObserver.cs`, `IDamageable.cs`, `GameObjectPool.cs`), to `Gameplay.unity`, to `InvaderView.prefab` and to `ProjectSettings/` are **not committed yet**. This README describes that working-tree state, which is ahead of the published repository.

---

## Author

**ArkadiyDev** — [@ArkadiyDev](https://github.com/ArkadiyDev)

## License

Released under the [MIT License](LICENSE).

> This repository is a demonstration project created to showcase C# and Unity development skills. It is not affiliated with, nor represents the work of, any game studio or publisher.
