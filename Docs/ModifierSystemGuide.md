# Система модифікаторів контрактів

## 📖 Огляд

Система модифікаторів дозволяє гнучко налаштовувати поведінку контрактів, додаючи різноманітні ефекти та обмеження без зміни базового класу `Contract`.

## 🏗️ Архітектура

### Базові компоненти

- **IContractModifier** - базовий інтерфейс для всіх модифікаторів
- **ContractModifierBase** - абстрактний базовий клас з загальною логікою
- **ModifierContext** - контекст, що передається через ланцюжок модифікаторів
- **ModifierResult** - результат роботи модифікатора
- **ModifierType** - тип модифікатора (визначає пріоритет)

### Категорії модифікаторів

#### 1. Progress Modifiers (Модифікатори прогресу)
Змінюють кількість прогресу, що додається до контракту.

**Доступні класи:**
- `ProgressMultiplierModifier` - множить прогрес на коефіцієнт
- `ProgressBonusModifier` - додає фіксований бонус до прогресу

**Приклад:**
```csharp
var contract = new Contract(1, "Test", 100f, 50);

// Подвоює прогрес
contract.AddModifier(new ProgressMultiplierModifier(2.0f));

// Додає +10 до кожного прогресу
contract.AddModifier(new ProgressBonusModifier(10f));

contract.AddProgress(5f, ProgressType.Manual);
// Результат: (5 * 2.0) + 10 = 20 прогресу
```

#### 2. Validation Modifiers (Модифікатори валідації)
Визначають, чи може прогрес бути доданим до контракту.

**Доступні класи:**
- `WorkTypeRestrictionModifier` - обмежує тип роботи (Manual/Automatic)

**Приклад:**
```csharp
var contract = new Contract(2, "Manual Only", 100f, 100);

// Тільки ручна робота
contract.AddModifier(new WorkTypeRestrictionModifier(ProgressType.Manual));

contract.AddProgress(10f, ProgressType.Manual);      // ✅ Працює
contract.AddProgress(10f, ProgressType.Automatic);   // ❌ Блокується
```

#### 3. Reward Modifiers (Модифікатори винагороди)
Змінюють винагороду за виконання контракту.

**Доступні класи:**
- `RewardMultiplierModifier` - множить винагороду
- `RewardBonusModifier` - додає фіксований бонус до винагороди

**Приклад:**
```csharp
var contract = new Contract(3, "High Reward", 100f, 100);

// x1.5 до винагороди
contract.AddModifier(new RewardMultiplierModifier(1.5f));

// +50 до винагороди
contract.AddModifier(new RewardBonusModifier(50));

var finalReward = contract.FinalReward;
// Результат: (100 * 1.5) + 50 = 200
```

#### 4. Time Modifiers (Часові модифікатори)
Взаємодіють з часом виконання контракту.

**Доступні класи:**
- `TimeLimitModifier` - обмежує час виконання
- `TimeBonusRewardModifier` - додає бонус при швидкому виконанні

**Приклад:**
```csharp
var contract = new Contract(4, "Rush", 50f, 200);

// 60 секунд на виконання
var timeLimit = new TimeLimitModifier(60f);
contract.AddModifier(timeLimit);

// Подвоює винагороду якщо виконано за 30 секунд
var timeBonus = new TimeBonusRewardModifier(30f, 2.0f);
contract.AddModifier(timeBonus);

// Підписка на події
timeLimit.OnExpired += () => {
    // Контракт провалено
};

// Оновлення (кожен кадр)
contract.UpdateModifiers(Time.deltaTime);
```

#### 5. Special Modifiers (Спеціальні модифікатори)
Унікальна логіка, що не підходить під інші категорії.

**Доступні класи:**
- `ComboProgressModifier` - збільшує бонус при послідовному виконанні

**Приклад:**
```csharp
var contract = new Contract(5, "Combo", 200f, 150);

var combo = new ComboProgressModifier(
    maxCombo: 10,           // Максимум x10 комбо
    comboBonus: 0.2f,       // +20% за кожний рівень
    comboResetTime: 2f      // 2 секунди між кліками
);
contract.AddModifier(combo);

combo.OnComboChanged += (level) => {
    Debug.Log($"Combo: x{level}");
};

// Швидкі кліки збільшують комбо
contract.AddProgress(10f, ProgressType.Manual); // x1 (10)
contract.AddProgress(10f, ProgressType.Manual); // x2 (12)
contract.AddProgress(10f, ProgressType.Manual); // x3 (14)
// ... комбо зростає до x10
```

## 🔄 Процес обробки

### Pipeline додавання прогресу:

```
Contract.AddProgress(amount, type)
    ↓
1. Створення ModifierContext
    ↓
2. Сортування модифікаторів за Priority
    ↓
3. Обробка кожного модифікатора:
    - Validation (Priority: 0-99)
    - Progress (Priority: 100-199)
    - Time (Priority: 300-399)
    - Special (Priority: 400+)
    ↓
4. Якщо IsValid:
    - Додати ModifiedAmount до CurrentProgress
    ↓
5. Перевірка завершення контракту
```

### Priority Rules:
- **Менше число = вища пріоритетність**
- Validation модифікатори виконуються першими (0-99)
- Progress модифікатори в середині (100-199)
- Reward модифікатори застосовуються при отриманні FinalReward (200-299)
- Time та Special модифікатори останніми (300+)

## 🛠️ API Reference

### IContract (розширення)

```csharp
// Властивості
IReadOnlyList<IContractModifier> Modifiers { get; }
int FinalReward { get; } // З урахуванням RewardModifiers

// Методи
void AddModifier(IContractModifier modifier);
void RemoveModifier(IContractModifier modifier);
void RemoveModifier(string modifierId);
bool HasModifier<T>() where T : IContractModifier;
T GetModifier<T>() where T : IContractModifier;
void UpdateModifiers(float deltaTime);
```

### IContractModifier

```csharp
ModifierType Type { get; }
int Priority { get; }
bool IsActive { get; }
string Id { get; }

void OnApply(IContract contract);
void OnRemove(IContract contract);
ModifierResult Process(ModifierContext context);
void Update(float deltaTime);
```

### ModifierContext

```csharp
IContract Contract { get; }
ProgressType ProgressType { get; }
float OriginalAmount { get; }
float ModifiedAmount { get; set; }
bool IsValid { get; set; }
Dictionary<string, object> Metadata { get; }

T GetMetadata<T>(string key, T defaultValue = default);
void SetMetadata(string key, object value);
```

### ModifierResult

```csharp
// Статичні методи
static ModifierResult Continue();              // Продовжити обробку
static ModifierResult Stop(string message);    // Зупинити, але валідно
static ModifierResult Invalid(string message); // Невалідно
```

## 📝 Створення власного модифікатора

### Приклад: Модифікатор шансу критичного удару

```csharp
using System;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers;

public class CriticalHitModifier : ContractModifierBase
{
    private readonly float _critChance;  // 0.0 - 1.0
    private readonly float _critMultiplier;
    private readonly Random _random = new Random();

    public CriticalHitModifier(float critChance, float critMultiplier)
    {
        _critChance = critChance;
        _critMultiplier = critMultiplier;
        Type = ModifierType.Progress;
    }

    public override ModifierResult Process(ModifierContext context)
    {
        // Перевіряємо шанс критичного удару
        if (_random.NextDouble() < _critChance)
        {
            context.ModifiedAmount *= _critMultiplier;
            context.SetMetadata("IsCritical", true);
        }

        return ModifierResult.Continue();
    }

    public override string ToString()
    {
        return $"Crit: {_critChance * 100:F0}% x{_critMultiplier:F1}";
    }
}
```

### Використання:

```csharp
var contract = new Contract(1, "Test", 100f, 50);
contract.AddModifier(new CriticalHitModifier(0.25f, 2.0f)); // 25% шанс x2

contract.AddProgress(10f, ProgressType.Manual);
// 75% шанс: 10 прогресу
// 25% шанс: 20 прогресу (критичний удар!)
```

## 🎮 Інтеграція з ContractProcessorModel

Для оновлення часових модифікаторів додайте в `ContractProcessorModel`:

```csharp
public void Update(float deltaTime)
{
    foreach (var contract in ActiveContracts)
    {
        contract.UpdateModifiers(deltaTime);
    }
}
```

## ⚠️ Best Practices

### 1. Пріоритети
- Використовуйте `ModifierType` для базового пріоритету
- Можете перевизначити `Priority` для точного налаштування

### 2. Деактивація
- Використовуйте `Deactivate()` замість видалення, якщо модифікатор може знадобитися пізніше
- Деактивовані модифікатори пропускаються при обробці

### 3. Events
- Використовуйте події для реакції на зміни в модифікаторах
- Не забувайте відписуватися від подій

### 4. Метадані
- Використовуйте `ModifierContext.Metadata` для передачі інформації між модифікаторами
- Корисно для UI та аналітики

### 5. Thread Safety
- Модифікатори НЕ є thread-safe
- Працюйте з ними тільки в головному потоці Unity

## 🔮 Можливі розширення

### 1. Модифікатори складності
```csharp
public class DifficultyScalingModifier : ContractModifierBase
{
    // Збільшує RequiredProgress з часом
}
```

### 2. Модифікатори ризику
```csharp
public class RiskRewardModifier : ContractModifierBase, IRewardModifier
{
    // Шанс провалу, але більша винагорода
}
```

### 3. Глобальні модифікатори
```csharp
public class GlobalModifier : ContractModifierBase
{
    // Застосовується до всіх контрактів
}
```

### 4. Умовні модифікатори
```csharp
public class ConditionalModifier : ContractModifierBase
{
    // Активується при виконанні умови
}
```

## 📊 Тестування

Приклад unit тесту для модифікатора:

```csharp
[Test]
public void ProgressMultiplier_DoublesProgress()
{
    // Arrange
    var contract = new Contract(1, "Test", 100f, 50);
    contract.AddModifier(new ProgressMultiplierModifier(2.0f));

    // Act
    contract.AddProgress(10f, ProgressType.Manual);

    // Assert
    Assert.AreEqual(20f, contract.CurrentProgress);
}
```

---

**Версія:** 1.0
**Автор:** YeKostenko
**Дата:** 2026-02-24

