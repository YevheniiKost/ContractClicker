# Приклад використання ModifierConfigBaseWrapper PropertyDrawer

## Швидкий старт

### 1. Відкрийте існуючий ModifierSetConfig
Знайдіть будь-який існуючий `ModifierSetConfig` asset в проекті або створіть новий:
- Right-click в Project window
- Create → ContractClicker → Modifiers → Modifier Set

### 2. Налаштуйте базову інформацію
```
Set Name: "Швидкий контракт"
Set Description: "Контракт з обмеженим часом але з бонусною нагородою"
Difficulty: Hard
Min Contract Level: 5
Spawn Chance: 30%
```

### 3. Додайте модифікатори
В секції **Modifiers**:

#### Елемент 0:
1. Dropdown → виберіть **TimeLimit**
2. Налаштуйте:
   - Name: "Обмеження часу"
   - Description: "Контракт має бути виконаний за 60 секунд"
   - IsEnabled: ✓
   - Time Limit In Seconds: 60

#### Елемент 1:
1. Dropdown → виберіть **TimeBonusReward**
2. Налаштуйте:
   - Name: "Бонус за швидкість"
   - Description: "Виконай за 30 секунд і отримай x2 нагороду"
   - IsEnabled: ✓
   - Bonus Time In Seconds: 30
   - Bonus Multiplier: 2.0

#### Елемент 2:
1. Dropdown → виберіть **ProgressMultiplier**
2. Налаштуйте:
   - Name: "Прискорення прогресу"
   - Description: "Прогрес виконується на 50% швидше"
   - IsEnabled: ✓
   - Multiplier: 1.5

### 4. Збережіть
Ctrl+S або File → Save Project

## Реальні приклади наборів

### Приклад 1: "Легкий старт" (Easy)
```
📦 Modifier Set: Легкий старт
├─ Progress Multiplier
│  └─ Multiplier: 1.3 (прогрес на 30% швидше)
└─ Reward Bonus
   └─ Bonus Amount: 50 (додаткові 50 монет)
```

### Приклад 2: "Челендж майстра" (Expert)
```
📦 Modifier Set: Челендж майстра
├─ Time Limit
│  └─ Time: 45 seconds
├─ Work Type Restriction
│  └─ Allowed Type: Manual (тільки ручна робота)
├─ Reward Multiplier
│  └─ Multiplier: 3.0 (нагорода x3)
└─ Combo Progress
   ├─ Max Combo: 10
   ├─ Combo Bonus: 0.2 (20% за комбо)
   └─ Combo Reset Time: 2.0 seconds
```

### Приклад 3: "Автоматизація" (Normal)
```
📦 Modifier Set: Автоматизація
├─ Work Type Restriction
│  └─ Allowed Type: Auto (тільки автоматична робота)
├─ Progress Bonus
│  └─ Bonus Amount: 15 (додаткові 15 прогресу)
└─ Reward Multiplier
   └─ Multiplier: 1.2 (нагорода x1.2)
```

### Приклад 4: "Легендарний контракт" (Legendary)
```
📦 Modifier Set: Легендарний контракт
├─ Time Limit
│  └─ Time: 30 seconds
├─ Time Bonus Reward
│  ├─ Bonus Time: 20 seconds
│  └─ Bonus Multiplier: 5.0 (якщо виконаєш за 20 сек - x5 нагорода!)
├─ Progress Multiplier
│  └─ Multiplier: 2.0
├─ Work Type Restriction
│  └─ Allowed Type: Manual
└─ Combo Progress
   ├─ Max Combo: 20
   ├─ Combo Bonus: 0.3
   └─ Combo Reset Time: 1.5 seconds
```

## Поширені кейси

### Як змінити тип модифікатора?
1. Клацніть на dropdown
2. Виберіть новий тип
3. Старі налаштування будуть замінені дефолтними значеннями нового типу

### Як видалити модифікатор?
1. Клацніть на dropdown
2. Виберіть "None"

### Як додати більше модифікаторів?
1. В Inspector знайдіть масив `Progress Modifiers`
2. Збільште Size
3. Для нових елементів виберіть тип з dropdown

### Як скопіювати модифікатор?
1. Збільште Size масиву на +1
2. Вручну скопіюйте налаштування (copy-paste values)
   *(або можна використати context menu → Duplicate якщо додати цей функціонал)*

## Debug Tips

### Як подивитись що насправді збережено?
1. Виберіть asset
2. Правий клік → Show in Explorer
3. Відкрийте .asset файл в текстовому редакторі
4. Шукайте секцію з `managedReferences:`

Приклад:
```yaml
references:
  m_Script: {fileID: 11500000, guid: ...}
  m_Config:
    rid: 2341563904
  rid: 2341563904
    type: {class: ProgressMultiplierConfig, ns: YeKostenko.ContractClicker.Data.Modifiers}
    data:
      _name: Швидкий прогрес
      _description: Прогрес виконується швидше
      _isEnabled: 1
      _multiplier: 1.5
```

### Що робити якщо dropdown порожній?
1. Перевірте що є хоча б один клас що наслідується від `ModifierConfigBase`
2. Перекомпілюйте проект (Assets → Reimport All)
3. Перезапустіть Unity Editor

### Що робити якщо зміни не зберігаються?
1. Натисніть Ctrl+S
2. Перевірте що asset не є read-only
3. Перевірте version control (git status)

## Best Practices

### ✅ Хороші практики:
- Давайте зрозумілі назви (Name) для відображення в UI
- Пишіть детальні описи (Description) що саме робить модифікатор
- Використовуйте IsEnabled для тимчасового вимкнення без видалення
- Групуйте логічно пов'язані модифікатори в один Set
- Тестуйте баланс на різних рівнях складності

### ❌ Антипатерни:
- Не створюйте надто багато модифікаторів в одному Set (оптимально 2-4)
- Не робіть конфлікти (наприклад, Manual + Auto restriction разом)
- Не забувайте про баланс (надто великі multiplier'и)
- Не залишайте порожні елементи (None) в середині масиву

## Інтеграція з кодом

### Як отримати модифікатори з коду?
```csharp
ModifierSetConfig set = // your set
foreach (var config in set.GetActiveModifierConfigs())
{
    Debug.Log($"Modifier: {config.Name} ({config.ModifierType})");

    // Type casting для доступу до специфічних полів
    if (config is ProgressMultiplierConfig pmc)
    {
        Debug.Log($"  Multiplier: {pmc.Multiplier}");
    }
}
```

### Як створити модифікатор з конфігу?
```csharp
ModifierFactory factory = ModifierFactory.Default;
IContractModifier modifier = factory.CreateFromConfig(config);
```

### Як застосувати до контракту?
```csharp
IContract contract = // your contract
ModifierSetConfig set = // your set

foreach (var config in set.GetActiveModifierConfigs())
{
    var modifier = factory.CreateFromConfig(config);
    if (modifier != null)
    {
        contract.AddModifier(modifier);
    }
}
```

## Troubleshooting

| Проблема | Рішення |
|----------|---------|
| Dropdown не показує нові типи | Перекомпілюйте проект |
| Зміни не зберігаються | Ctrl+S після змін |
| Поля не відображаються | Перевірте що тип має [SerializeField] поля |
| Помилки в консолі | Перевірте логи - можливо проблема з reflection |
| Asset corrupted | Створіть новий asset і скопіюйте налаштування |

## Подальше читання

- `ModifierSystem_QuickStart.md` - швидкий старт з системою модифікаторів
- `ModifierSystemGuide.md` - повний гайд по системі модифікаторів
- `ModifierConfigGuide.md` - детальна документація конфігів
- `ModifierConfigBaseWrapper_Guide.md` - технічна документація PropertyDrawer

