# ModifierConfigBaseWrapper PropertyDrawer Guide

## Overview
Custom PropertyDrawer для `ModifierConfigBaseWrapper` що автоматично генерує dropdown з усіх доступних типів `ModifierConfigBase` і дозволяє легко вибирати та налаштовувати модифікатори.

## Як використовувати

### 1. В Inspector
Коли ви відкриваєте `ModifierSetConfig` в Inspector, для кожного елемента в масиві `_progressModifiers` ви побачите:

- **Dropdown меню** з опціями:
  - `None` - очистити модифікатор
  - `ProgressMultiplier` - множник прогресу
  - `ProgressBonus` - бонус до прогресу
  - `WorkTypeRestriction` - обмеження типу роботи
  - `RewardMultiplier` - множник нагороди
  - `RewardBonus` - бонус нагороди
  - `TimeLimit` - обмеження часу
  - `TimeBonusReward` - бонусна нагорода за час
  - `ComboProgress` - комбо прогрес

### 2. Вибір типу модифікатора
1. Натисніть на dropdown
2. Виберіть потрібний тип модифікатора
3. Автоматично створюється екземпляр обраного типу
4. Під dropdown'ом з'являються всі поля конфігурації для цього типу

### 3. Налаштування модифікатора
Після вибору типу, ви можете налаштувати:
- `Name` - назва модифікатора для відображення
- `Description` - опис модифікатора
- `IsEnabled` - чи активний модифікатор
- Специфічні параметри для кожного типу (множники, бонуси, тощо)

### 4. Зміна типу
Якщо ви хочете змінити тип модифікатора:
1. Виберіть новий тип з dropdown
2. Попередня конфігурація буде замінена новою

### 5. Видалення модифікатора
Щоб очистити модифікатор:
1. Виберіть `None` з dropdown
2. Всі налаштування будуть видалені

## Автоматичне визначення типів

PropertyDrawer автоматично знаходить всі класи, що наслідуються від `ModifierConfigBase` за допомогою reflection. Це означає:

- ✅ Немає необхідності вручну додавати нові типи до списку
- ✅ При створенні нового `ModifierConfigBase` він автоматично з'явиться в dropdown
- ✅ Типи відсортовані за назвою для зручності

## Приклад нового модифікатора

Якщо ви хочете додати новий тип модифікатора:

```csharp
[Serializable]
public class MyNewModifierConfig : ModifierConfigBase
{
    [SerializeField] private float _myParameter = 1.0f;

    public float MyParameter => _myParameter;
    public override string ModifierType => "MyNewModifier";
}
```

Після компіляції, `MyNewModifier` автоматично з'явиться в dropdown меню!

## Технічні деталі

- **Reflection caching** - список типів кешується при першому використанні
- **SerializeReference** - використовується для polymorphic serialization
- **Null safety** - всі операції перевіряються на null
- **EditorGUI integration** - повна підтримка Undo/Redo системи Unity

## Переваги

1. **Тип-безпека** - неможливо встановити неправильний тип
2. **Зручність** - все в одному місці, без необхідності створювати окремі ScriptableObject
3. **Гнучкість** - легко змінювати типи та параметри
4. **Автоматизація** - нові типи додаються автоматично
5. **Inspector friendly** - зручний UI в Unity Inspector

