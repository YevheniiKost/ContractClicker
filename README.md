# ContractClicker
Clicker management pet project

## Core Game Loop Implementation

This repository contains a complete implementation of the core game loop for a contract-based clicker game.

### Game Loop Overview

```
┌─────────────────────────────────────────────────┐
│                  GAME LOOP                      │
│                                                 │
│  1. Receive Contract                            │
│  2. Work on Contract                            │
│     • Manual Clicks (ClickManager)              │
│     • Passive Automation (AutomationManager)    │
│  3. Complete Contract                           │
│  4. Receive Reward (Currency)                   │
│  5. Spend on Upgrades (UpgradeManager)          │
│  6. Next Contract (repeat)                      │
└─────────────────────────────────────────────────┘
```

### Architecture

The implementation consists of these core components:

- **Contract**: The fundamental gameplay unit representing work to be done
- **GameManager**: Coordinates the entire game loop
- **CurrencyManager**: Manages the primary currency system
- **ClickManager**: Handles manual clicking mechanics
- **AutomationManager**: Manages passive work generation
- **UpgradeManager**: Handles upgrade purchases and effects

### Getting Started

1. Open the project in Unity
2. Add the `GameLoopExample` script to a GameObject in your scene
3. Press Play
4. Use these controls:
   - **Space**: Perform a click to work on the contract
   - **U**: Purchase the next available upgrade
   - **R**: Reset the game

### Documentation

Detailed documentation can be found in `/Assets/Scripts/README.md`, including:
- Architecture details
- API reference
- Integration guide
- Scaling formulas
- Extensibility options

### File Structure

```
Assets/
└── Scripts/
    ├── Contract.cs              # Contract data class
    ├── GameManager.cs           # Core game loop coordinator
    ├── CurrencyManager.cs       # Currency system
    ├── ClickManager.cs          # Manual work system
    ├── AutomationManager.cs     # Passive work system
    ├── UpgradeManager.cs        # Upgrade system
    ├── GameLoopExample.cs       # Example usage/demo
    ├── GameLoopVerification.cs  # Verification tests
    └── README.md                # Detailed documentation
```

### Features

✅ Complete core game loop implementation  
✅ Manual clicking system  
✅ Passive automation system  
✅ Currency and rewards  
✅ Upgrade system with multiple types  
✅ Scaling difficulty and rewards  
✅ Event-driven architecture  
✅ Singleton managers with persistence  
✅ Comprehensive documentation  
✅ Example implementation  
✅ Verification tests
