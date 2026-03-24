# Система модифікаторів - Швидкий старт

## 🚀 Що створено

### Базова інфраструктура
- ✅ `IContractModifier` - базовий інтерфейс
- ✅ `ContractModifierBase` - базовий клас
- ✅ `ModifierContext` - контекст передачі даних
- ✅ `ModifierResult` - результат обробки
- ✅ `ModifierType` - типи модифікаторів
- ✅ `IRewardModifier` - інтерфейс для модифікаторів винагороди

### Готові модифікатори

#### Progress (Прогрес)
- ✅ `ProgressMultiplierModifier` - множить прогрес (x1.5, x2.0 тощо)
- ✅ `ProgressBonusModifier` - додає фіксований бонус (+10, +20 тощо)

#### Validation (Валідація)
- ✅ `WorkTypeRestrictionModifier` - обмежує тип роботи (Manual/Auto)

#### Reward (Винагорода)
- ✅ `RewardMultiplierModifier` - множить винагороду
- ✅ `RewardBonusModifier` - додає фіксований бонус до винагороди

#### Time (Час)
- ✅ `TimeLimitModifier` - обмеження часу виконання
- ✅ `TimeBonusRewardModifier` - бонус за швидке виконання

#### Special (Спеціальні)
- ✅ `ComboProgressModifier` - система комбо

### Оновлені класи
- ✅ `IContract` - розширено для підтримки модифікаторів
- ✅ `Contract` - реалізовано систему модифікаторів

## 📚 Швидкі приклади

### Приклад 1: Подвоїти прогрес
```csharp
var contract = new Contract(1, "Fast Contract", 100f, 50);
contract.AddModifier(new ProgressMultiplierModifier(2.0f));

contract.AddProgress(10f, ProgressType.Manual);
// Результат: 20 прогресу
```

### Приклад 2: Тільки ручна робота
```csharp
var contract = new Contract(2, "Manual Only", 100f, 100);
contract.AddModifier(new WorkTypeRestrictionModifier(ProgressType.Manual));
contract.AddModifier(new RewardMultiplierModifier(1.5f)); // +50% до винагороди

contract.AddProgress(10f, ProgressType.Manual);      // ✅ Працює
contract.AddProgress(10f, ProgressType.Automatic);   // ❌ Блокується
```

### Приклад 3: Експрес контракт
```csharp
var contract = new Contract(3, "Rush!", 50f, 200);

// 60 секунд на виконання
var timeLimit = new TimeLimitModifier(60f);
contract.AddModifier(timeLimit);

// Подвоює винагороду якщо виконано за 30 секунд
contract.AddModifier(new TimeBonusRewardModifier(30f, 2.0f));

// Оновлення кожен кадр
void Update() {
    contract.UpdateModifiers(Time.deltaTime);
}
```

### Приклад 4: Комбо система
```csharp
var contract = new Contract(4, "Combo Master", 200f, 150);

var combo = new ComboProgressModifier(
    maxCombo: 10,      // До x10
    comboBonus: 0.2f,  // +20% за рівень
    comboResetTime: 2f // 2 сек між кліками
);
contract.AddModifier(combo);

// Швидкі кліки збільшують прогрес
contract.AddProgress(10f, ProgressType.Manual); // 10 (+0%)
contract.AddProgress(10f, ProgressType.Manual); // 12 (+20%)
contract.AddProgress(10f, ProgressType.Manual); // 14 (+40%)
// і так далі до x10
```

## 🎯 Робота з модифікаторами

### Додавання
```csharp
contract.AddModifier(new ProgressMultiplierModifier(1.5f));
```

### Видалення
```csharp
var modifier = contract.GetModifier<ProgressMultiplierModifier>();
contract.RemoveModifier(modifier);
// або
contract.RemoveModifier("modifierId");
```

### Перевірка наявності
```csharp
if (contract.HasModifier<TimeLimitModifier>()) {
    var timeMod = contract.GetModifier<TimeLimitModifier>();
    Debug.Log($"Залишилось: {timeMod.RemainingTime}s");
}
```

### Перегляд всіх модифікаторів
```csharp
foreach (var modifier in contract.Modifiers) {
    Debug.Log(modifier.ToString());
}
```

### Фінальна винагорода
```csharp
var baseReward = contract.Reward;     // 100
var finalReward = contract.FinalReward; // З урахуванням модифікаторів
```

## 📖 Повна документація

Детальна документація: [ModifierSystemGuide.md](./ModifierSystemGuide.md)

Приклади використання: `Assets/YeKostenko/ContractClicker/Scripts/Examples/ModifierUsageExamples.cs`

## 🔧 Створення власного модифікатора

```csharp
using YeKostenko.ContractClicker.Domain.Contract.Modifiers;

public class MyCustomModifier : ContractModifierBase
{
    public MyCustomModifier()
    {
        Type = ModifierType.Progress; // або інший тип
    }

    public override ModifierResult Process(ModifierContext context)
    {
        // Ваша логіка тут
        context.ModifiedAmount *= 1.5f;

        return ModifierResult.Continue();
    }
}
```

## 🎮 Інтеграція з грою

### В ContractProcessorModel
```csharp
public void Update(float deltaTime)
{
    foreach (var contract in ActiveContracts)
    {
        contract.UpdateModifiers(deltaTime);
    }
}
```

### При завершенні контракту
```csharp
if (contract.IsCompleted)
{
    var reward = contract.FinalReward; // З модифікаторами
    playerWallet.AddMoney(reward);
}
```

## 📦 Структура файлів

```
Domain/Contract/Modifiers/
├── ModifierType.cs
├── ModifierContext.cs
├── ModifierResult.cs
├── IContractModifier.cs
├── ContractModifierBase.cs
├── IRewardModifier.cs
├── Progress/
│   ├── ProgressMultiplierModifier.cs
│   └── ProgressBonusModifier.cs
├── Validation/
│   └── WorkTypeRestrictionModifier.cs
├── Reward/
│   ├── RewardMultiplierModifier.cs
│   └── RewardBonusModifier.cs
├── Time/
│   ├── TimeLimitModifier.cs
│   └── TimeBonusRewardModifier.cs
└── Special/
    └── ComboProgressModifier.cs
```

## ✨ Що далі?

### Можливі розширення:
1. **Серіалізація** - збереження модифікаторів у SaveData
2. **UI** - відображення модифікаторів у інтерфейсі
3. **Генератор** - випадкова генерація модифікаторів для контрактів
4. **Нові модифікатори:**
   - `DifficultyScalingModifier` - прогресивна складність
   - `CriticalHitModifier` - шанс критичного удару
   - `RiskRewardModifier` - ризик/винагорода
   - `ChainContractModifier` - зв'язок контрактів

---

**Статус:** ✅ Готово до використання
**Версія:** 1.0
**Дата:** 2026-02-24

