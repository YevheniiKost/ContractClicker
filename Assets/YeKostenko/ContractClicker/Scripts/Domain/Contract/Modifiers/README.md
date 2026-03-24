# Contract Modifiers System

Система модифікаторів для контрактів - гнучке та розширюване рішення для зміни поведінки контрактів.

## 📁 Структура

```
Modifiers/
├── Core/                          # Базові компоненти
│   ├── IContractModifier.cs       # Базовий інтерфейс
│   ├── ContractModifierBase.cs    # Абстрактний базовий клас
│   ├── IRewardModifier.cs         # Інтерфейс для модифікаторів винагороди
│   ├── ModifierContext.cs         # Контекст обробки
│   ├── ModifierResult.cs          # Результат обробки
│   └── ModifierType.cs            # Типи модифікаторів
│
├── Progress/                      # Модифікатори прогресу
│   ├── ProgressMultiplierModifier.cs
│   └── ProgressBonusModifier.cs
│
├── Validation/                    # Модифікатори валідації
│   └── WorkTypeRestrictionModifier.cs
│
├── Reward/                        # Модифікатори винагороди
│   ├── RewardMultiplierModifier.cs
│   └── RewardBonusModifier.cs
│
├── Time/                          # Часові модифікатори
│   ├── TimeLimitModifier.cs
│   └── TimeBonusRewardModifier.cs
│
└── Special/                       # Спеціальні модифікатори
    └── ComboProgressModifier.cs
```

## 🚀 Швидкий старт

```csharp
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Progress;

// Створити контракт
var contract = new Contract(1, "Test", 100f, 50);

// Додати модифікатор
contract.AddModifier(new ProgressMultiplierModifier(2.0f));

// Використовувати
contract.AddProgress(10f, ProgressType.Manual); // → 20 прогресу
```

## 📚 Документація

- **Повний гайд:** `/Docs/ModifierSystemGuide.md`
- **Швидкий старт:** `/Docs/ModifierSystem_QuickStart.md`
- **Cheat Sheet:** `/Docs/ModifierSystem_CheatSheet.md`

## 🎯 Доступні модифікатори

| Категорія | Модифікатор | Опис |
|-----------|------------|------|
| Progress | ProgressMultiplierModifier | Множить прогрес |
| Progress | ProgressBonusModifier | Додає до прогресу |
| Validation | WorkTypeRestrictionModifier | Обмежує тип роботи |
| Reward | RewardMultiplierModifier | Множить винагороду |
| Reward | RewardBonusModifier | Додає до винагороди |
| Time | TimeLimitModifier | Обмежує час |
| Time | TimeBonusRewardModifier | Бонус за швидкість |
| Special | ComboProgressModifier | Система комбо |

## 🛠️ Розширення

### Створення власного модифікатора:

```csharp
public class MyModifier : ContractModifierBase
{
    public MyModifier()
    {
        Type = ModifierType.Progress;
    }

    public override ModifierResult Process(ModifierContext context)
    {
        // Ваша логіка
        context.ModifiedAmount *= 1.5f;
        return ModifierResult.Continue();
    }
}
```

## 📊 Pipeline обробки

```
AddProgress → ModifierContext → [Validation] → [Progress] → [Time] → [Special] → CurrentProgress
```

Модифікатори обробляються за пріоритетом:
1. Validation (0-99)
2. Progress (100-199)
3. Reward (200-299) - застосовуються при FinalReward
4. Time (300-399)
5. Special (400+)

## 🧪 Тести

Unit тести доступні в `/Tests/Domain/ContractModifierTests.cs`

---

**Version:** 1.0.0
**Author:** YeKostenko
**Date:** 2026-02-24

