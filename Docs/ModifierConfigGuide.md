# 🎮 Гайд по використанню системи конфігурації модифікаторів

## 📋 Огляд системи

Система конфігурації модифікаторів складається з:
1. **ModifierDatabase** - головна база даних всіх наборів
2. **ModifierSetConfig** - ScriptableObject для набору модифікаторів
3. **ModifierFactory** - фабрика для створення модифікаторів з конфігів
4. **ContractModifierService** - сервіс для застосування модифікаторів
5. **ModifierEnabledContractGenerator** - генератор контрактів з модифікаторами

---

## 🚀 Швидкий старт

### Крок 1: Створення ModifierDatabase

1. У Unity: `Right Click → Create → ContractClicker → Modifiers → Modifier Database`
2. Назвіть файл `MainModifierDatabase`
3. Налаштуйте базові параметри:
   - Base Modifier Chance: 60% (шанс появи модифікаторів)
   - Allow Multiple Sets: false (спочатку)
   - Max Sets Per Contract: 1

### Крок 2: Створення ModifierSet для Easy складності

1. `Right Click → Create → ContractClicker → Modifiers → Modifier Set`
2. Назвіть `ModSet_BeginnerLuck`
3. Налаштуйте:

```
═══════════════════════════════════════
БАЗОВА ІНФОРМАЦІЯ
═══════════════════════════════════════
Set Name: Beginner's Luck
Set Description: Легкий старт для новачків. Підвищений прогрес та невеликий бонус.
Difficulty: Easy
Reward Multiplier: 1.0
Min Contract Level: 1
Spawn Chance: 80

═══════════════════════════════════════
PROGRESS MULTIPLIER
═══════════════════════════════════════
✅ Enabled
Name: Fast Progress
Description: Прогрес йде швидше
Multiplier: 1.5

═══════════════════════════════════════
REWARD BONUS
═══════════════════════════════════════
✅ Enabled
Name: Extra Cash
Description: Додаткова винагорода
Bonus Amount: 25

═══════════════════════════════════════
DIFFICULTY COLOR
═══════════════════════════════════════
Color: RGB(153, 230, 153) - Light Green
```

### Крок 3: Додати ModifierSet до Database

1. Відкрийте `MainModifierDatabase`
2. У списку `Modifier Sets` додайте `ModSet_BeginnerLuck`
3. Save

### Крок 4: Використання в коді

```csharp
using YeKostenko.ContractClicker.Data.Modifiers;

public class ContractManager : MonoBehaviour
{
    [SerializeField] private ModifierDatabase _modifierDatabase;
    private ContractModifierService _modifierService;

    private void Start()
    {
        _modifierService = new ContractModifierService(_modifierDatabase);
    }

    private void CreateContract()
    {
        // Створюємо базовий контракт
        var contract = new Contract(1, "Test Contract", 100f, 50);

        // Застосовуємо випадкові модифікатори для рівня 1
        var appliedSets = _modifierService.ApplyRandomModifiers(contract, contractLevel: 1);

        // Показуємо інформацію
        foreach (var set in appliedSets)
        {
            Debug.Log($"Applied: {set.SetName} ({set.Difficulty})");
        }
    }
}
```

---

## 📦 Створення різних типів ModifierSet

### 1. Easy - "Quick Win"

```
Set Name: Quick Win
Difficulty: Easy
Min Level: 1
Spawn Chance: 70

Modifiers:
✅ Progress Bonus
   - Bonus Amount: 15

✅ Time Bonus Reward
   - Bonus Time: 45s
   - Bonus Multiplier: 1.5
```

### 2. Normal - "Manual Focus"

```
Set Name: Manual Focus
Difficulty: Normal
Min Level: 5
Spawn Chance: 50

Modifiers:
✅ Work Type Restriction
   - Allowed Type: Manual

✅ Progress Multiplier
   - Multiplier: 1.5

✅ Reward Multiplier
   - Multiplier: 1.4
```

### 3. Hard - "Time Trial"

```
Set Name: Time Trial
Difficulty: Hard
Min Level: 15
Spawn Chance: 40

Modifiers:
✅ Time Limit
   - Time Limit: 90s

✅ Time Bonus Reward
   - Bonus Time: 45s
   - Bonus Multiplier: 2.5

✅ Progress Multiplier
   - Multiplier: 1.4
```

### 4. Expert - "The Gauntlet"

```
Set Name: The Gauntlet
Difficulty: Expert
Min Level: 30
Spawn Chance: 25

Modifiers:
✅ Work Type Restriction: Manual
✅ Time Limit: 120s
✅ Time Bonus Reward: 60s, x3.0
✅ Combo Progress: Max 15, Bonus 0.3, Reset 1.5s
✅ Progress Multiplier: 1.3
```

### 5. Legendary - "Godlike"

```
Set Name: Godlike
Difficulty: Legendary
Min Level: 50
Spawn Chance: 10

Modifiers:
✅ Work Type Restriction: Manual
✅ Time Limit: 60s
✅ Time Bonus Reward: 30s, x4.0
✅ Combo Progress: Max 25, Bonus 0.4, Reset 1s
✅ Progress Multiplier: 2.0
✅ Reward Multiplier: 2.5
```

---

## 🎯 Інтеграція з ContractGenerator

### Варіант 1: Використання ModifierEnabledContractGenerator

```csharp
using YeKostenko.ContractClicker.Data.Modifiers;
using YeKostenko.ContractClicker.Domain.Contract;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ModifierDatabase _modifierDatabase;
    private ModifierEnabledContractGenerator _generator;

    private void Start()
    {
        var baseGenerator = new ContractGenerator(configProvider, workController);
        var contractFactory = ContractFactory.Default;

        _generator = new ModifierEnabledContractGenerator(
            baseGenerator,
            contractFactory,
            _modifierDatabase
        );

        // Встановлюємо рівень
        _generator.SetLevel(1);
    }

    private void GenerateContracts()
    {
        // Генеруємо 3 контракти з модифікаторами
        var contracts = _generator.GenerateContractsWithModifiers(3);

        foreach (var contractData in contracts)
        {
            var contract = contractData.Contract;
            var sets = contractData.AppliedModifierSets;

            Debug.Log($"Contract: {contract.Name}");
            Debug.Log($"Modifiers: {contractData.GetModifiersDescription()}");
            Debug.Log($"Difficulty: {contractData.GetEffectiveDifficulty()}");

            // Додаємо до процесора
            contractProcessor.StartNewContract(contract);
        }
    }

    private void OnLevelUp()
    {
        _generator.IncreaseLevel();
        Debug.Log($"New level! Recommended difficulty: {_generator.GetCurrentRecommendedDifficulty()}");
    }
}
```

### Варіант 2: Ручне застосування через сервіс

```csharp
public class ContractCreator : MonoBehaviour
{
    [SerializeField] private ModifierDatabase _modifierDatabase;
    private ContractModifierService _modifierService;

    private void Start()
    {
        _modifierService = new ContractModifierService(_modifierDatabase);
    }

    public IContract CreateContractForLevel(int level)
    {
        // Створюємо базовий контракт
        var definition = new ContractDefinition(1, "Test", 100, 50);
        var contract = ContractFactory.Default.Create(definition);

        // Застосовуємо модифікатори
        _modifierService.ApplyRandomModifiers(contract, level);

        return contract;
    }

    public IContract CreateContractWithDifficulty(ModifierDifficulty difficulty, int level)
    {
        var definition = new ContractDefinition(1, "Challenge", 150, 75);
        var contract = ContractFactory.Default.Create(definition);

        // Застосовуємо модифікатори певної складності
        _modifierService.ApplyRandomModifiers(contract, level, difficulty);

        return contract;
    }

    public IContract CreateContractWithSpecificSet(string setName)
    {
        var definition = new ContractDefinition(1, "Custom", 120, 60);
        var contract = ContractFactory.Default.Create(definition);

        // Застосовуємо конкретний набір
        _modifierService.ApplyModifierSetByName(contract, setName);

        return contract;
    }
}
```

---

## 🎨 UI Інтеграція

### Відображення ModifierSet в UI

```csharp
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YeKostenko.ContractClicker.Data.Modifiers;

public class ContractModifierUI : MonoBehaviour
{
    [SerializeField] private Image _difficultyIcon;
    [SerializeField] private TMP_Text _difficultyText;
    [SerializeField] private TMP_Text _setNameText;
    [SerializeField] private TMP_Text _setDescriptionText;
    [SerializeField] private Transform _modifiersContainer;
    [SerializeField] private GameObject _modifierItemPrefab;

    public void DisplayModifierSets(List<ModifierSetConfig> sets)
    {
        ClearModifiers();

        if (sets == null || sets.Count == 0)
        {
            _difficultyText.text = "No Modifiers";
            return;
        }

        // Показуємо перший сет (або найскладніший)
        var mainSet = sets[0];

        // Встановлюємо колір складності
        _difficultyIcon.color = mainSet.DifficultyColor;
        _difficultyText.text = mainSet.Difficulty.ToString();
        _difficultyText.color = mainSet.DifficultyColor;

        // Назва та опис
        _setNameText.text = mainSet.SetName;
        _setDescriptionText.text = mainSet.SetDescription;

        // Показуємо іконку якщо є
        if (mainSet.Icon != null)
        {
            _difficultyIcon.sprite = mainSet.Icon;
        }

        // Показуємо модифікатори
        foreach (var config in mainSet.GetActiveModifierConfigs())
        {
            var item = Instantiate(_modifierItemPrefab, _modifiersContainer);
            var text = item.GetComponentInChildren<TMP_Text>();

            if (text != null)
            {
                text.text = $"• {config.Name}: {config.Description}";
            }
        }
    }

    private void ClearModifiers()
    {
        foreach (Transform child in _modifiersContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
```

---

## ⚙️ Розширені налаштування

### Динамічна зміна шансів появи

```csharp
public class DynamicModifierChance : MonoBehaviour
{
    [SerializeField] private ModifierDatabase _database;

    // Збільшуємо шанс модифікаторів з часом гри
    public void AdjustChanceBasedOnPlaytime(float hoursPlayed)
    {
        // Через Reflection або wrapper можна змінювати шанси
        // Альтернативно - створити декілька баз даних для різних стадій гри

        if (hoursPlayed < 1f)
        {
            // Початок гри - менше модифікаторів
            // Використовуємо EarlyGameModifierDatabase
        }
        else if (hoursPlayed < 10f)
        {
            // Середина гри
            // Використовуємо MidGameModifierDatabase
        }
        else
        {
            // Пізня гра - всі модифікатори
            // Використовуємо LateGameModifierDatabase
        }
    }
}
```

### Створення сетів програмно (для подій/івентів)

```csharp
public ModifierSetConfig CreateEventSet()
{
    var set = ScriptableObject.CreateInstance<ModifierSetConfig>();

    // Налаштовуємо через рефлексію або створюємо окремий метод
    // Це для особливих подій, івентів тощо

    return set;
}
```

---

## 📊 Балансування

### Рекомендовані співвідношення

**Множники винагороди:**
```
Easy:      1.0x - 1.5x
Normal:    1.2x - 1.8x
Hard:      1.5x - 2.5x
Expert:    2.0x - 3.5x
Legendary: 3.0x - 6.0x
```

**З урахуванням difficulty бонусу:**
```csharp
// У ModifierSetConfig.GetFinalRewardMultiplier()
Easy:      x1.0  → Final: 1.0x - 1.5x
Normal:    x1.2  → Final: 1.44x - 2.16x
Hard:      x1.5  → Final: 2.25x - 3.75x
Expert:    x2.0  → Final: 4.0x - 7.0x
Legendary: x3.0  → Final: 9.0x - 18.0x
```

---

## ✅ Чеклист налаштування

- [ ] Створити ModifierDatabase
- [ ] Створити мінімум по 1 ModifierSet для кожної складності
- [ ] Налаштувати Spawn Chance для балансу
- [ ] Додати всі сети до Database
- [ ] Створити ContractModifierService в GameManager
- [ ] Інтегрувати з ContractGenerator
- [ ] Налаштувати UI для відображення модифікаторів
- [ ] Протестувати баланс винагород
- [ ] Створити спеціальні тематичні сети (опціонально)

---

**Детальні пресети дивіться в: `ModifierSetPresets.md`**

