# The Dungeon - Science and Multimedia Tecnology Bachelor degree.
Built as a bachelor's thesis at the University of Udine (Science and Multimedia Technology), The Dungeon is a 2D rogue-like where every floor is procedurally generated at runtime, 
different layouts, enemy spawns, and item drops every run. A hands-on exploration of Procedural Content Generation applied to real game development.

---

## Gameplay
Navigate procedurally generated dungeons in a top-down 2D rogue-like. Each run regenerates the map, enemy and item placement. Poison gas slowly drains your health while enemies hunt you down. The objective is to reach the exit before time runs out.

Core loop:
  1. Explore the procedurally generated dungeon.
  2. Fight enemies with melee and special attacks.
  3. Loot weapons, armor, and potions scattered across the map.
  4. Survive poison gas and enemy encounters.
  5. Reach the exit to advance to the next, harder level.

Mechanics:
  - Equip weapons and armor through a slot-based inventory.
  - Use potions (Health, Mana, Stamina, Attack, Defence) to survive.
  - Level up dungeons grow larger and enemies get stronger each floor.
  - Die and it's back to level 1.
    
---
## Tech Stack
| Layer        | Technology                        |
|---|---|
| Engine       | Unity 2021.3.22f1 (LTS)           |
| Language     | C#                                |
| Architecture | Singleton GameManager, ScriptableObject inventory, Coroutine-based pipelines |
| Rendering    | Universal Render Pipeline (URP)   |
| UI           | TextMesh Pro, UGUI                |
| Navigation   | Custom Breadth-First Search (BFS) pathfinding           |

---
## Architecture Highlights

### Procedural Dungeon Generation — `DungeonManager.cs`

The core of the project. A Random Walker algorithm builds tile-based dungeon layouts at runtime by moving a virtual cursor across a grid and recording floor positions. Supports five distinct map types selectable from the Unity Editor:
| Type        | Description                        |
|---|---|
| Cave       | Organic winding caverns via unconstrained random walking         |
| Room     | Castle-style corridors connecting rectangular rooms                              |
| CrossRoom | Diagonal cross-shaped room patterns |
| VerticalCrossRoom    | Plus-sign shaped room layouts   |
| BossRoom           | Large open arena rooms |

After generation, a coroutine pipeline ( `DelayProgress`) hands off to  `TileSpawner`, which replaces placeholder tiles with floor and wall prefabs, then scatters enemies, items, and weapons across valid positions using configurable spawn probability sliders.


### Tile Spawner — `TileSpawner.cs`
Each placeholder tile runs its own Awake/Start cycle: first it instantiates a floor prefab at its position, then sweeps all 8 surrounding cells and places wall prefabs wherever there is no existing floor or wall. It also updates
the  `DungeonManager`'s bounding box ( `minX` ,  `maxX`,  `minY`,  `maxY`) for use in item and enemy scatter logic. Once done, it destroys itself.

### Enemy AI — `Enemy.cs`, `Node.cs`

Enemies operate in two behaviour states:
- **Patrol**: random tile-by-tile movement when the player is out of alert range, using 4-directional collision checks to find valid moves each tick
- **Chase & Attack**: once the player enters range, a custom **BFS pathfinding** (`FindNextStep`) navigates the enemy toward the player tile by tile, with smooth interpolated movement via coroutines

Within attack range, enemies deal randomised damage with a built-in 15% miss chance. All movement and attack timing is coroutine-driven to avoid frame-rate dependency.

## Player System — `Player.cs`, `PlayerStats.cs`
- **Tile-based movement**: with wall and enemy collision via LayerMasks, directional sprite flipping, and smooth lerp movement
- **Melee combat**: attack on adjacent enemy tiles; cooldown managed via timer
- **Special attack**:area explosion consuming Mana (`SpecialAttackScripts.cs`)
- **Resource management**: HP, Stamina, Mana with coroutine-driven regeneration and drain
- **Stat modifiers**: a `ModifiableFloat` + `FModifiers` system allowing dynamic stat changes from equipment and potions with clean stacking and removal

### Inventory & Equipment System — `InventoryObject.cs`, `UserInterface.cs`

A ScriptableObject-driven inventory fully decoupled from the display layer:
  - Slot-based storage with item stacking support
  - Separate Inventory and Equipment interface types
  - Drag-and-drop UI via DynamicInterface and StaticInterface
  - Items defined as ScriptableObjects (`ItemObject.cs`) with a centralised `ItemDatabaseObject.cs`
  - Binary serialisation for save/load support (`SaveData.cs`)

### Level Progression — `GameManager.cs`, `ExitNext.cs`

GameManager is a Singleton (`DontDestroyOnLoad`) that persists across scene reloads. When the player reaches the exit trigger (`ExitNext.cs`):

  - The current level index increments
  - Enemy stats scale up (`levelEnemy++`)
  - The dungeon tile budget grows by 50% (`totalTileCount += totalTileCount / 100 * 50`
  - EXP is awarded using a logarithmic formula: `exp + 2 * log₂(levelField`)
  - The scene reloads asynchronously, generating a brand new dungeon
    
On death, GameOver.cs resets all level state and returns to level 1 via async scene reload.

## Potion System — `Potions.cs`
Six potion types with four size tiers (`small`, `medium`, `big`, `giant`). Effects scale with both size and current level via `baseValuePotion` (which increases by 5 per level cleared):

| Potion        | Effect   |
|---|---|
| Health       | Restores HP        |
| Mana     | ManaRestores Mana          |
| Stamina | Restores Stamina |
| Attack    | Temporary attack boost   |
| Defence           | Temporary defence boost |
|Random Effect |Random buff or debuff|

---

## Scenes
| Potion        | Effect   |
|---|---|
| MainMenu       | Title screen and game entry point        |
| Game     |Main gameplay level — dungeon generation, combat, and progression          |

---

## Controls

| Input | Action |
|---|---|
| WASD | Move |
| Left Shift | Sprint (consumes Stamina) |
| Space | Attack |
| G | Special Attack (Explosion, consumes Mana)|

---

## Running the Project
1. Install Unity 2021.3.22f1 (LTS) (available via Unity Hub)
2. Clone the repository
3. Open the project folder in Unity Hub
4. Open Assets/Prefabs/Scenes/MainMenu.unity
5. Press Play — or build via File → Build Settings

Changing dungeon type: If you want to change the dungeon layout style, open Assets/Prefabs/Scenes/Game.unity, select the Environment GameObject in the Hierarchy, and set the DungeonType field on the DungeonManager component (Cave, Room, CrossRoom, VerticalCrossRoom, or BossRoom) before pressing Play.


