# Contract Clicker - Core Game Loop Documentation

## Overview

This implementation provides a complete core game loop system for the Contract Clicker game. The system follows the specified game loop pattern:

1. **Receive a contract** - Player gets a new contract to work on
2. **Work on the contract** - Through manual clicks or passive automation
3. **Complete the contract** - When work requirement is met
4. **Receive reward** - Currency is awarded
5. **Spend reward on upgrades** - Improve click power and automation
6. **Receive next contract** - Loop continues with increasing difficulty

## Architecture

The implementation follows a modular design with separate managers for each concern:

### Core Classes

#### `Contract.cs`
The fundamental gameplay unit that represents a work contract.
- **Properties**: Name, Description, RequiredWork, CurrentWork, Reward, IsCompleted
- **Methods**: 
  - `AddWork(float amount)` - Adds work progress
  - `GetProgress()` - Returns completion percentage (0-1)
  - `GetProgressString()` - Returns formatted progress string

#### `GameManager.cs`
Coordinates the entire game loop and contract flow.
- **Responsibilities**:
  - Generates contracts with scaling difficulty
  - Manages the current active contract
  - Routes work from clicks and automation to the contract
  - Handles contract completion and reward distribution
  - Automatically starts the next contract
- **Key Methods**:
  - `ReceiveNextContract()` - Generates and activates a new contract
  - `PerformManualClick()` - Processes a manual click
  - `GetCurrentContract()` - Returns the active contract
  - `ResetGame()` - Resets game to initial state
- **Events**:
  - `OnContractReceived` - Fired when a new contract is received
  - `OnContractProgress` - Fired when work is added to a contract
  - `OnContractCompleted` - Fired when a contract is completed

#### `CurrencyManager.cs`
Manages the primary currency system.
- **Responsibilities**:
  - Tracks player's current currency
  - Handles currency transactions (add/spend)
  - Validates purchases
- **Key Methods**:
  - `AddCurrency(int amount)` - Adds currency from rewards
  - `SpendCurrency(int amount)` - Spends currency on upgrades
  - `CanAfford(int amount)` - Checks if player can afford a purchase
- **Events**:
  - `OnCurrencyChanged` - Fired when currency amount changes

#### `ClickManager.cs`
Handles manual clicking mechanics.
- **Responsibilities**:
  - Processes manual clicks
  - Manages click power and multipliers
  - Provides click power upgrades
- **Key Methods**:
  - `PerformClick()` - Executes a click and returns work done
  - `SetClickPowerMultiplier(float multiplier)` - Updates click power from upgrades
  - `GetCurrentClickPower()` - Returns current work per click
- **Events**:
  - `OnClick` - Fired when a click is performed

#### `AutomationManager.cs`
Manages passive automation mechanics.
- **Responsibilities**:
  - Provides continuous work without player input
  - Manages automation power and multipliers
  - Controls automation activation state
- **Key Methods**:
  - `SetBaseWorkPerSecond(float workPerSecond)` - Sets base automation rate
  - `SetWorkPerSecondMultiplier(float multiplier)` - Updates automation multiplier
  - `SetAutomationActive(bool active)` - Enables/disables automation
  - `GetCurrentWorkPerSecond()` - Returns current automation rate
- **Events**:
  - `OnAutomationWork` - Fired continuously when automation is active

#### `UpgradeManager.cs`
Manages the upgrade system.
- **Responsibilities**:
  - Maintains available upgrades
  - Processes upgrade purchases
  - Applies upgrade effects to other systems
- **Key Methods**:
  - `PurchaseUpgrade(Upgrade upgrade)` - Attempts to buy an upgrade
  - `GetAvailableUpgrades()` - Returns unpurchased upgrades
  - `GetPurchasedUpgrades()` - Returns purchased upgrades
- **Upgrade Types**:
  - `ClickPowerMultiplier` - Increases click power
  - `AutomationWorkPerSecond` - Adds passive work generation
  - `AutomationMultiplier` - Multiplies automation effectiveness
- **Events**:
  - `OnUpgradePurchased` - Fired when an upgrade is purchased

#### `GameLoopExample.cs`
Example implementation demonstrating the system usage.
- **Features**:
  - Keyboard controls for testing (Space to click, U to upgrade, R to reset)
  - Event logging for debugging
  - On-screen GUI showing game state
  - Demonstrates proper system integration

## Game Loop Flow

```
[Start Game]
    ↓
[Receive Contract] ← ──────────┐
    ↓                           │
[Work on Contract]              │
    • Manual Clicks             │
    • Passive Automation        │
    ↓                           │
[Contract Progress]             │
    ↓                           │
[Contract Completed?]           │
    ↓                           │
[Receive Reward]                │
    • Add Currency              │
    ↓                           │
[Spend on Upgrades]             │
    • Click Power               │
    • Automation                │
    ↓                           │
[Next Contract] ────────────────┘
```

## Scaling System

### Contract Difficulty
Contracts scale in difficulty using the formula:
```
RequiredWork = 10 × (difficultyIncrease)^contractsCompleted
Default: 10 × (1.2)^n
```

### Rewards
Rewards scale using the formula:
```
Reward = baseReward × (rewardIncrease)^contractsCompleted
Default: 10 × (1.5)^n
```

This creates a balanced progression where:
- Contract #1: 10 work, 10 reward
- Contract #2: 12 work, 15 reward
- Contract #3: 14.4 work, 22 reward
- And so on...

## Default Upgrades

1. **Better Mouse** (50 currency)
   - Doubles click power
   - Type: ClickPowerMultiplier

2. **Auto-Clicker** (100 currency)
   - Generates 1 work per second
   - Type: AutomationWorkPerSecond

3. **AI Assistant** (500 currency)
   - Generates 5 work per second
   - Type: AutomationWorkPerSecond

## Integration Guide

### Basic Setup

1. Create an empty GameObject in your Unity scene
2. Attach the `GameLoopExample` component
3. Press Play
4. Use keyboard controls to test:
   - **Space**: Perform a click
   - **U**: Purchase the next available upgrade
   - **R**: Reset the game

### Custom Integration

To integrate into your own game:

```csharp
// Initialize the game
GameManager.Instance.ReceiveNextContract();

// Handle player clicks
void OnPlayerClick()
{
    GameManager.Instance.PerformManualClick();
}

// Purchase upgrades
void OnUpgradeButtonClicked(Upgrade upgrade)
{
    UpgradeManager.Instance.PurchaseUpgrade(upgrade);
}

// Subscribe to events
GameManager.Instance.OnContractCompleted += (contract) => {
    Debug.Log($"Completed: {contract.Name}");
};

CurrencyManager.Instance.OnCurrencyChanged += (amount) => {
    UpdateCurrencyUI(amount);
};
```

### Creating Custom Upgrades

```csharp
var customUpgrade = new Upgrade
{
    name = "Super Upgrade",
    description = "Triples automation speed",
    cost = 1000,
    type = Upgrade.UpgradeType.AutomationMultiplier,
    value = 3f,
    isPurchased = false
};

UpgradeManager.Instance.availableUpgrades.Add(customUpgrade);
```

## Extensibility

The system is designed to be easily extensible:

- **Add new upgrade types**: Extend the `Upgrade.UpgradeType` enum and add handling in `UpgradeManager.ApplyUpgrade()`
- **Custom contract generation**: Override or extend `GameManager.ReceiveNextContract()`
- **Different reward types**: Add new managers similar to `CurrencyManager` for other resources
- **Special contracts**: Create `Contract` subclasses with additional properties
- **Achievements/Milestones**: Subscribe to manager events and track progress

## Singleton Pattern

All managers use the singleton pattern with lazy initialization:
- Automatically created on first access
- Persist across scene loads (`DontDestroyOnLoad`)
- Thread-safe instance access
- Prevents duplicate instances

## Event System

The implementation uses C# events for loose coupling:
- Managers don't directly depend on each other
- Easy to add new functionality without modifying existing code
- Great for UI updates and analytics tracking
- Example: `GameManager.Instance.OnContractCompleted += MyHandler;`

## Testing

To test the system:

1. Open Unity and load a scene with the `GameLoopExample` component
2. Enter Play mode
3. Observe the Console for event logs
4. Use the on-screen GUI to monitor game state
5. Test the game loop:
   - Click (Space) to work on contracts
   - Complete contracts to earn currency
   - Purchase upgrades (U) when you have enough currency
   - Observe automation working passively
   - Verify difficulty scaling with each new contract

## Future Enhancements

Possible extensions to this core system:
- Save/Load system for persistence
- Multiple contract types with different mechanics
- Contract modifiers (bonus rewards, time limits)
- Prestige/rebirth system
- Achievement system
- Statistics tracking
- UI framework integration
- Mobile touch controls
- Sound effects and visual feedback
- Contract selection/queue system

## Performance Considerations

- All managers are singletons to minimize instantiation overhead
- Event subscriptions properly cleaned up in `OnDestroy`
- Automation uses `Time.deltaTime` for frame-rate independence
- No per-frame allocations in critical paths
- Minimal GC pressure from efficient event handling

## License

This implementation is part of the ContractClicker project.
