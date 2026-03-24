# 🚀 Modifier System - Cheat Sheet

## Швидкий довідник по системі модифікаторів

---

## 📦 Основні класи

```csharp
using YeKostenko.ContractClicker.Domain.Contract.Modifiers;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Progress;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Validation;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Reward;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Time;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Special;
```

---

## 🎯 Доступні модифікатори

| Модифікатор | Що робить | Приклад |
|------------|----------|---------|
| `ProgressMultiplierModifier` | Множить прогрес | `new ProgressMultiplierModifier(2.0f)` |
| `ProgressBonusModifier` | Додає до прогресу | `new ProgressBonusModifier(10f)` |
| `WorkTypeRestrictionModifier` | Обмежує тип | `new WorkTypeRestrictionModifier(ProgressType.Manual)` |
| `RewardMultiplierModifier` | Множить винагороду | `new RewardMultiplierModifier(1.5f)` |
| `RewardBonusModifier` | Додає до винагороди | `new RewardBonusModifier(100)` |
| `TimeLimitModifier` | Обмежує час | `new TimeLimitModifier(60f)` |
| `TimeBonusRewardModifier` | Бонус за швидкість | `new TimeBonusRewardModifier(30f, 2.0f)` |
| `ComboProgressModifier` | Система комбо | `new ComboProgressModifier(10, 0.2f, 2f)` |

---

## 💡 Швидкі сценарії

### Подвоїти прогрес
```csharp
contract.AddModifier(new ProgressMultiplierModifier(2.0f));
```

### Тільки ручна робота + бонус
```csharp
contract.AddModifier(new WorkTypeRestrictionModifier(ProgressType.Manual));
contract.AddModifier(new RewardMultiplierModifier(1.5f));
```

### Обмеження часу
```csharp
var timer = new TimeLimitModifier(60f);
timer.OnExpired += () => Debug.Log("Час вийшов!");
contract.AddModifier(timer);
```

### Бонус за швидкість
```csharp
// x2 винагорода якщо виконано за 30 сек
contract.AddModifier(new TimeBonusRewardModifier(30f, 2.0f));
```

### Комбо система
```csharp
var combo = new ComboProgressModifier(10, 0.2f, 2f);
combo.OnComboChanged += (level) => Debug.Log($"x{level}");
contract.AddModifier(combo);
```

---

## 🛠️ API Reference

### Додавання/видалення
```csharp
// Додати
contract.AddModifier(modifier);

// Видалити
contract.RemoveModifier(modifier);
contract.RemoveModifier("modifierId");

// Перевірити наявність
bool has = contract.HasModifier<ProgressMultiplierModifier>();

// Отримати
var mod = contract.GetModifier<ProgressMultiplierModifier>();
```

### Робота з контрактом
```csharp
// Додати прогрес
contract.AddProgress(10f, ProgressType.Manual);

// Оновити модифікатори (в Update)
contract.UpdateModifiers(Time.deltaTime);

// Отримати фінальну винагороду
int reward = contract.FinalReward;

// Переглянути всі модифікатори
foreach (var mod in contract.Modifiers)
{
    Debug.Log(mod.ToString());
}
```

### Деактивація
```csharp
modifier.Deactivate(); // Тимчасово вимкнути
modifier.Activate();   // Увімкнути назад
```

---

## 🎨 Створення власного модифікатора

### Простий модифікатор прогресу
```csharp
public class MyModifier : ContractModifierBase
{
    public MyModifier()
    {
        Type = ModifierType.Progress;
    }

    public override ModifierResult Process(ModifierContext context)
    {
        context.ModifiedAmount *= 1.5f;
        return ModifierResult.Continue();
    }
}
```

### Модифікатор з параметрами
```csharp
public class ParametrizedModifier : ContractModifierBase
{
    private readonly float _value;

    public ParametrizedModifier(float value)
    {
        _value = value;
        Type = ModifierType.Progress;
    }

    public override ModifierResult Process(ModifierContext context)
    {
        context.ModifiedAmount += _value;
        return ModifierResult.Continue();
    }
}
```

### Модифікатор винагороди
```csharp
public class MyRewardModifier : ContractModifierBase, IRewardModifier
{
    public MyRewardModifier()
    {
        Type = ModifierType.Reward;
    }

    public override ModifierResult Process(ModifierContext context)
    {
        return ModifierResult.Continue();
    }

    public int ModifyReward(int baseReward)
    {
        return baseReward * 2;
    }
}
```

### Часовий модифікатор
```csharp
public class MyTimeModifier : ContractModifierBase
{
    private float _elapsed;

    public MyTimeModifier()
    {
        Type = ModifierType.Time;
    }

    public override void Update(float deltaTime)
    {
        _elapsed += deltaTime;
    }

    public override ModifierResult Process(ModifierContext context)
    {
        return ModifierResult.Continue();
    }
}
```

### Модифікатор з подіями
```csharp
public class EventModifier : ContractModifierBase
{
    public event Action OnSomething;

    public override ModifierResult Process(ModifierContext context)
    {
        OnSomething?.Invoke();
        return ModifierResult.Continue();
    }
}
```

---

## 🔄 ModifierResult options

```csharp
// Продовжити до наступного модифікатора
return ModifierResult.Continue();

// Зупинити, але прогрес валідний
return ModifierResult.Stop();

// Блокувати прогрес (невалідно)
return ModifierResult.Invalid("Причина");

// Позначити невалідним але продовжити (для логування)
return ModifierResult.InvalidButContinue("Помилка");
```

---

## 📊 Priority (порядок виконання)

```csharp
ModifierType.Validation = 0    // Перші
ModifierType.Progress = 100    // Другі
ModifierType.Reward = 200      // Треті (застосовуються при отриманні FinalReward)
ModifierType.Time = 300        // Четверті
ModifierType.Special = 400     // Останні
```

**Менше значення = вища пріоритетність**

Можна перевизначити:
```csharp
public override int Priority => 50; // Буде між Validation та Progress
```

---

## 🎯 Metadata System

```csharp
// В модифікаторі
public override ModifierResult Process(ModifierContext context)
{
    // Встановити
    context.SetMetadata("ComboLevel", 5);
    context.SetMetadata("IsCritical", true);

    // Отримати
    int combo = context.GetMetadata<int>("ComboLevel", 0);
    bool isCrit = context.GetMetadata<bool>("IsCritical", false);

    return ModifierResult.Continue();
}
```

---

## ⚠️ Важливо пам'ятати

1. **Update викликати обов'язково** для часових модифікаторів
   ```csharp
   void Update() {
       contract.UpdateModifiers(Time.deltaTime);
   }
   ```

2. **FinalReward замість Reward**
   ```csharp
   int reward = contract.FinalReward; // ✅ З модифікаторами
   int base = contract.Reward;        // ❌ Без модифікаторів
   ```

3. **Деактивовані модифікатори пропускаються**
   ```csharp
   modifier.Deactivate(); // Більше не обробляється
   ```

4. **Порядок додавання важливий для однакових Priority**
   ```csharp
   contract.AddModifier(multiplier);  // Буде першим
   contract.AddModifier(bonus);       // Буде другим
   ```

5. **Видалення під час обробки - небезпечно**
   ```csharp
   // ❌ НЕ робити під час Process
   contract.RemoveModifier(this);

   // ✅ Краще деактивувати
   this.Deactivate();
   ```

---

## 🧪 Швидке тестування

```csharp
[Test]
public void Test()
{
    var contract = new Contract(1, "Test", 100f, 50);
    contract.AddModifier(new ProgressMultiplierModifier(2.0f));
    contract.AddProgress(10f, ProgressType.Manual);
    Assert.AreEqual(20f, contract.CurrentProgress);
}
```

---

## 📚 Документація

- **Детально:** `Docs/ModifierSystemGuide.md`
- **Швидко:** `Docs/ModifierSystem_QuickStart.md`
- **Приклади:** `Examples/ModifierUsageExamples.cs`
- **Тести:** `Tests/Domain/ContractModifierTests.cs`

---

**Версія:** 1.0 | **Дата:** 2026-02-24

