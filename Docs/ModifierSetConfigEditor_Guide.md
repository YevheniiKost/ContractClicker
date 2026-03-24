# Custom Editor для ModifierSetConfig

## 🎨 Що це дає

Custom Editor для `ModifierSetConfig` покращує роботу з наборами модифікаторів у Unity Inspector, показуючи наочний огляд всіх активних модифікаторів та їх параметрів.

---

## ✨ Основні функції

### 1. **Кольоровий Header**
- Автоматично підсвічується кольором складності (Easy→Legendary)
- Показує назву сету
- Відображає бейдж складності
- Три статистичні блоки:
  - **Reward**: Фінальний множник винагороди
  - **Level**: Мінімальний рівень контракту
  - **Spawn**: Шанс появи в %

### 2. **Active Modifiers Overview** (Головна фіча)
Згортаємий розділ у верхній частині Inspector, який показує:
- ✅ Кількість активних модифікаторів
- ✅ Всі увімкнені модифікатори згруповані за категоріями
- ✅ Ключові параметри кожного модифікатора

#### Категорії:
- **Progress Modifiers** - модифікатори прогресу
- **Validation Modifiers** - обмеження/валідація
- **Reward Modifiers** - модифікатори винагороди
- **Time Modifiers** - часові модифікатори

#### Відображення параметрів:

**Progress Multiplier:**
```
● Progress Multiplier    ×1.50    "Прогрес йде швидше"
```

**Progress Bonus:**
```
● Progress Bonus         +10.0    "Додатковий прогрес"
```

**Combo System:**
```
● Combo System    Max: 10    +20%/lvl    Reset: 2.0s
```

**Work Type Restriction:**
```
● Work Type Restriction    MANUAL ONLY    "Тільки ручна робота"
```

**Reward Multiplier:**
```
● Reward Multiplier      ×1.50    "Більша винагорода"
```

**Reward Bonus:**
```
● Reward Bonus           +50      "Додаткова винагорода"
```

**Time Limit:**
```
● Time Limit             90s      "Обмеження часу виконання"
```

**Time Bonus Reward:**
```
● Time Bonus Reward      30s  → ×2.00    "Бонус за швидкість"
```

---

## 🎯 Як це виглядає

```
┌─────────────────────────────────────────────────┐
│         ╔═══════════════════════════════╗       │
│         ║    BEGINNER'S LUCK           ║       │ <- Кольоровий header
│         ║         [EASY]                ║       │
│         ╚═══════════════════════════════╝       │
│   ┌─────────┐  ┌─────────┐  ┌─────────┐       │
│   │ Reward  │  │  Level  │  │  Spawn  │       │ <- Статистика
│   │  ×1.50  │  │   ≥1    │  │   80%   │       │
│   └─────────┘  └─────────┘  └─────────┘       │
│                                                 │
│ ▼ Active Modifiers Overview (2 active)        │ <- Згортаємо/розгортаємо
│   ┌─────────────────────────────────────┐      │
│   │ Progress Modifiers                  │      │
│   │ ● Progress Multiplier  ×1.50  ...  │      │
│   │                                     │      │
│   │ Reward Modifiers                   │      │
│   │ ● Reward Bonus        +25     ...  │      │
│   └─────────────────────────────────────┘      │
│                                                 │
│ Configuration                                   │ <- Стандартні поля
│ ├─ Set Name: Beginner's Luck                   │
│ ├─ Description: ...                            │
│ └─ Difficulty: Easy                            │
│                                                 │
│ Balancing                                       │
│ ├─ Reward Multiplier: 1.0                      │
│ ├─ Min Contract Level: 1                       │
│ └─ Spawn Chance: 80                            │
│                                                 │
│ Visual                                          │
│ ├─ Difficulty Color: (Light Green)             │
│ └─ Icon: None                                  │
│                                                 │
│ Modifiers                                       │
│ ├─ Progress Multiplier: (reference)            │
│ ├─ Progress Bonus: None                        │
│ ├─ Combo Progress: None                        │
│ ├─ Work Type Restriction: None                 │
│ ├─ Reward Multiplier Config: None              │
│ ├─ Reward Bonus: (reference)                   │
│ ├─ Time Limit: None                            │
│ └─ Time Bonus Reward: None                     │
└─────────────────────────────────────────────────┘
```

---

## 🔧 Технічні деталі

### Файл
`Assets/YeKostenko/ContractClicker/Scripts/Editor/Modifiers/ModifierSetConfigEditor.cs`

### Залежності
- `UnityEditor`
- `YeKostenko.ContractClicker.Data.Modifiers`

### Автоматично активується
Коли ви відкриваєте `ModifierSetConfig` у Inspector, Unity автоматично використовує цей custom editor завдяки атрибуту `[CustomEditor(typeof(ModifierSetConfig))]`.

---

## 💡 Переваги

### Для геймдизайнерів:
- ✅ **Миттєвий огляд** - бачите всі активні модифікатори одразу
- ✅ **Не треба розкривати кожен** - всі параметри на одному екрані
- ✅ **Швидке порівняння** - легко порівнювати різні сети
- ✅ **Візуальна індикація** - кольори показують складність

### Для програмістів:
- ✅ **Швидка перевірка** - одразу видно що налаштовано
- ✅ **Дебаг** - легко бачити які модифікатори активні
- ✅ **Валідація** - бачите якщо щось не налаштовано

### Робочий процес:
- ✅ **Менше скролінгу** - все важливе зверху
- ✅ **Менше кліків** - не треба розкривати кожне поле
- ✅ **Більше контексту** - бачите загальну картину

---

## 🎨 Кольорова схема

### Складності:
- **Easy**: Світло-зелений (0.6, 0.9, 0.6)
- **Normal**: Синій (0.6, 0.8, 1.0)
- **Hard**: Помаранчевий (1.0, 0.7, 0.3)
- **Expert**: Червоний (0.9, 0.3, 0.3)
- **Legendary**: Золотий (1.0, 0.84, 0.0)

### Елементи UI:
- **Активний модифікатор**: Зелений (●)
- **Work Restriction**: Помаранчевий (попередження)
- **Time Limit**: Червоний (критично)
- **Статистика**: Відповідні пастельні кольори

---

## 📝 Приклади використання

### Створення нового сету:
1. `Right Click → Create → ContractClicker → Modifiers → Modifier Set`
2. Відкрийте створений asset
3. Заповніть базову інформацію (Set Name, Difficulty)
4. Додайте модифікатори (перетягніть ScriptableObject у поля)
5. **Зверху одразу побачите** що увімкнено та які параметри

### Швидка перевірка:
1. Відкрийте ModifierSetConfig
2. Подивіться на **Active Modifiers Overview**
3. Перевірте кількість активних модифікаторів
4. Перевірте параметри кожного

### Порівняння двох сетів:
1. Відкрийте перший сет в Inspector
2. Подивіться на Overview
3. Відкрийте другий сет у новому Inspector (lock перший)
4. Порівняйте Overview обох сетів

---

## 🚀 Що далі

Можна розширити editor додавши:
- **Preview режим** - як виглядатиме в грі
- **Validation warnings** - якщо щось налаштовано неправильно
- **Quick actions** - кнопки для швидких дій
- **Balance calculator** - розрахунок фінального балансу
- **Duplicate detection** - попередження про однакові модифікатори

---

**Custom Editor готовий до використання! Просто відкрийте будь-який ModifierSetConfig у Inspector.** 🎉

