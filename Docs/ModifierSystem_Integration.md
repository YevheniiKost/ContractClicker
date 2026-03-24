# Приклад інтеграції системи модифікаторів

## 🎯 Що потрібно зробити для повної інтеграції

### 1. Оновити ContractProcessorModel

Додайте метод Update для оновлення часових модифікаторів:

```csharp
public class ContractProcessorModel : IContractProcessorModel
{
    // ...існуючий код...

    /// <summary>
    /// Оновлення активних контрактів (викликати в Update/FixedUpdate)
    /// </summary>
    public void Update(float deltaTime)
    {
        foreach (var contract in ActiveContracts)
        {
            contract.UpdateModifiers(deltaTime);
        }
    }
}
```

Додайте до IContractProcessorModel:

```csharp
public interface IContractProcessorModel
{
    // ...існуючі методи...

    void Update(float deltaTime);
}
```

### 2. Оновити обробку завершених контрактів

Використовуйте `FinalReward` замість `Reward`:

```csharp
public void CompleteContract(IContract contract)
{
    if (!contract.IsCompleted)
    {
        return;
    }

    // ✅ Використовуйте FinalReward (з модифікаторами)
    var reward = contract.FinalReward;

    // ❌ НЕ використовуйте contract.Reward (без модифікаторів)

    playerWallet.AddMoney(reward);
    ContractCompleted?.Invoke(contract);
}
```

### 3. Інтеграція в MonoBehaviour

Приклад GameManager або ContractManager:

```csharp
using UnityEngine;
using YeKostenko.ContractClicker.Domain.Contract;

public class ContractManager : MonoBehaviour
{
    private IContractProcessorModel _contractProcessor;

    private void Update()
    {
        // Оновлюємо модифікатори всіх активних контрактів
        _contractProcessor?.Update(Time.deltaTime);
    }
}
```

### 4. Розширення ContractGenerator

Додайте генерацію модифікаторів:

```csharp
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Progress;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Validation;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Reward;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Time;

public class ContractGenerator : IContractGenerator
{
    public IContract Generate()
    {
        var definition = SelectRandomDefinition();
        var contract = _factory.Create(definition);

        // Випадково додаємо модифікатори
        AddRandomModifiers(contract);

        return contract;
    }

    private void AddRandomModifiers(IContract contract)
    {
        // 30% шанс - подвоєння прогресу
        if (Random.value < 0.3f)
        {
            contract.AddModifier(new ProgressMultiplierModifier(2.0f));
        }

        // 20% шанс - тільки ручна робота з бонусом
        if (Random.value < 0.2f)
        {
            contract.AddModifier(new WorkTypeRestrictionModifier(ProgressType.Manual));
            contract.AddModifier(new RewardMultiplierModifier(1.5f));
        }

        // 15% шанс - експрес контракт
        if (Random.value < 0.15f)
        {
            contract.AddModifier(new TimeLimitModifier(60f));
            contract.AddModifier(new TimeBonusRewardModifier(30f, 2.0f));
        }

        // 25% шанс - просто бонус до винагороди
        if (Random.value < 0.25f)
        {
            contract.AddModifier(new RewardBonusModifier(Random.Range(50, 200)));
        }
    }
}
```

### 5. UI для відображення модифікаторів

Приклад компонента для відображення модифікаторів контракту:

```csharp
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using YeKostenko.ContractClicker.Domain.Contract;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers;

public class ContractModifiersView : MonoBehaviour
{
    [SerializeField] private Transform _modifiersContainer;
    [SerializeField] private GameObject _modifierItemPrefab;

    private List<GameObject> _modifierItems = new List<GameObject>();

    public void Display(IContract contract)
    {
        // Очищаємо попередні
        ClearModifiers();

        // Відображаємо кожен модифікатор
        foreach (var modifier in contract.Modifiers)
        {
            if (!modifier.IsActive)
            {
                continue;
            }

            var item = Instantiate(_modifierItemPrefab, _modifiersContainer);
            var text = item.GetComponentInChildren<TMP_Text>();

            if (text != null)
            {
                text.text = modifier.ToString();

                // Колір залежно від типу
                text.color = GetColorForModifierType(modifier.Type);
            }

            _modifierItems.Add(item);
        }
    }

    private void ClearModifiers()
    {
        foreach (var item in _modifierItems)
        {
            Destroy(item);
        }
        _modifierItems.Clear();
    }

    private Color GetColorForModifierType(ModifierType type)
    {
        return type switch
        {
            ModifierType.Progress => new Color(0.2f, 0.8f, 0.2f),    // Зелений
            ModifierType.Validation => new Color(0.8f, 0.2f, 0.2f),  // Червоний
            ModifierType.Reward => new Color(1f, 0.84f, 0f),         // Золотий
            ModifierType.Time => new Color(0.3f, 0.6f, 1f),          // Синій
            ModifierType.Special => new Color(0.8f, 0.2f, 0.8f),     // Пурпурний
            _ => Color.white
        };
    }

    private void Update()
    {
        // Оновлюємо відображення (наприклад, для таймерів)
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        // Можна оновлювати текст для динамічних модифікаторів
        // Наприклад, показувати залишок часу
    }
}
```

### 6. Обробка подій модифікаторів

Приклад обробки подій від модифікаторів:

```csharp
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Time;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Special;

public class ContractEventHandler
{
    public void SetupContract(IContract contract)
    {
        // Обробка закінчення часу
        var timeLimit = contract.GetModifier<TimeLimitModifier>();
        if (timeLimit != null)
        {
            timeLimit.OnExpired += () => OnTimeLimitExpired(contract);
        }

        // Обробка зміни комбо
        var combo = contract.GetModifier<ComboProgressModifier>();
        if (combo != null)
        {
            combo.OnComboChanged += (level) => OnComboChanged(contract, level);
        }

        // Обробка бонусу часу
        var timeBonus = contract.GetModifier<TimeBonusRewardModifier>();
        if (timeBonus != null)
        {
            timeBonus.OnBonusExpired += () => OnTimeBonusExpired(contract);
        }
    }

    private void OnTimeLimitExpired(IContract contract)
    {
        Debug.Log($"Contract {contract.Name} time expired!");

        // Автоматично провалити контракт
        // contractProcessor.SkipContract(contract.Id);

        // Або показати повідомлення
        // ShowNotification("Time's up!", NotificationType.Warning);
    }

    private void OnComboChanged(IContract contract, int comboLevel)
    {
        Debug.Log($"Combo: x{comboLevel}");

        // Показати VFX
        // PlayComboEffect(comboLevel);

        // Відтворити звук
        // PlayComboSound(comboLevel);
    }

    private void OnTimeBonusExpired(IContract contract)
    {
        Debug.Log($"Time bonus expired for {contract.Name}");

        // Оновити UI
        // UpdateContractUI(contract);
    }
}
```

### 7. Збереження та завантаження

Приклад серіалізації модифікаторів (для майбутньої реалізації):

```csharp
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class ContractSaveData
{
    public int Id;
    public string Name;
    public float CurrentProgress;
    public List<ModifierSaveData> Modifiers;
}

[Serializable]
public class ModifierSaveData
{
    public string TypeName;
    public string JsonData;
}

public class ContractSerializer
{
    public ContractSaveData Serialize(IContract contract)
    {
        var data = new ContractSaveData
        {
            Id = contract.Id,
            Name = contract.Name,
            CurrentProgress = contract.CurrentProgress,
            Modifiers = new List<ModifierSaveData>()
        };

        foreach (var modifier in contract.Modifiers)
        {
            var modData = new ModifierSaveData
            {
                TypeName = modifier.GetType().FullName,
                JsonData = JsonConvert.SerializeObject(modifier)
            };
            data.Modifiers.Add(modData);
        }

        return data;
    }

    // Deserialize буде потребувати factory для створення модифікаторів
}
```

---

## 🎮 Повний приклад використання

```csharp
using UnityEngine;
using YeKostenko.ContractClicker.Domain.Contract;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Progress;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Time;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Special;

public class ContractExample : MonoBehaviour
{
    private IContract _currentContract;

    private void Start()
    {
        // Створюємо контракт
        _currentContract = new Contract(1, "Epic Contract", 100f, 500);

        // Додаємо модифікатори
        var combo = new ComboProgressModifier(10, 0.2f, 2f);
        _currentContract.AddModifier(combo);

        var timeLimit = new TimeLimitModifier(120f);
        _currentContract.AddModifier(timeLimit);

        var progressBoost = new ProgressMultiplierModifier(1.5f);
        _currentContract.AddModifier(progressBoost);

        // Підписуємося на події
        combo.OnComboChanged += OnComboChanged;
        timeLimit.OnExpired += OnTimeExpired;
    }

    private void Update()
    {
        // Оновлюємо модифікатори
        _currentContract.UpdateModifiers(Time.deltaTime);

        // Приклад кліку
        if (Input.GetMouseButtonDown(0))
        {
            _currentContract.AddProgress(10f, ProgressType.Manual);
            Debug.Log($"Progress: {_currentContract.CurrentProgress}/{_currentContract.RequiredProgress}");
        }

        // Перевіряємо завершення
        if (_currentContract.IsCompleted)
        {
            CompleteContract();
        }
    }

    private void OnComboChanged(int level)
    {
        Debug.Log($"🔥 Combo x{level}!");
        // Тут можна показати VFX, звук тощо
    }

    private void OnTimeExpired()
    {
        Debug.Log("⏰ Time's up!");
        // Провалити контракт або інша логіка
    }

    private void CompleteContract()
    {
        var reward = _currentContract.FinalReward;
        Debug.Log($"✅ Contract completed! Reward: {reward}");

        // Додати гроші, показати екран винагороди тощо
    }
}
```

---

## ✅ Чеклист інтеграції

- [ ] Додати Update() в IContractProcessorModel
- [ ] Реалізувати Update() в ContractProcessorModel
- [ ] Викликати Update в MonoBehaviour (GameManager/ContractManager)
- [ ] Використовувати FinalReward замість Reward
- [ ] Розширити ContractGenerator для генерації модифікаторів
- [ ] Створити UI для відображення модифікаторів
- [ ] Додати обробку подій (OnExpired, OnComboChanged тощо)
- [ ] Реалізувати серіалізацію (опціонально)
- [ ] Протестувати всі модифікатори в грі

---

**Після виконання цих кроків система модифікаторів буде повністю інтегрована в гру!** 🚀

