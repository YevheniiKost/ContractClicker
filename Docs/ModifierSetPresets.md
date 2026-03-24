# Приклади ModifierSet конфігів

## 📦 Готові пресети для створення в Unity

### 1. Easy Tier

#### "Beginner's Luck" (Новачок)
```
Set Name: Beginner's Luck
Difficulty: Easy
Reward Multiplier: 1.0
Min Contract Level: 1
Spawn Chance: 80%

Modifiers:
- Progress Multiplier: x1.5
- Reward Bonus: +25

Description: "Легкий старт для новачків. Підвищений прогрес та невеликий бонус."
Color: Light Green (0.6, 0.9, 0.6)
```

#### "Quick Win" (Швидка перемога)
```
Set Name: Quick Win
Difficulty: Easy
Reward Multiplier: 1.2
Min Contract Level: 1
Spawn Chance: 70%

Modifiers:
- Progress Bonus: +15
- Time Bonus Reward: 45s, x1.5

Description: "Додатковий бонус за швидке виконання."
Color: Light Green
```

---

### 2. Normal Tier

#### "Balanced Work" (Збалансована робота)
```
Set Name: Balanced Work
Difficulty: Normal
Reward Multiplier: 1.3
Min Contract Level: 5
Spawn Chance: 60%

Modifiers:
- Progress Multiplier: x1.3
- Reward Multiplier: x1.2
- Progress Bonus: +10

Description: "Збалансований набір для стандартних контрактів."
Color: Blue (0.6, 0.8, 1.0)
```

#### "Manual Focus" (Ручна праця)
```
Set Name: Manual Focus
Difficulty: Normal
Reward Multiplier: 1.5
Min Contract Level: 5
Spawn Chance: 50%

Modifiers:
- Work Type Restriction: Manual Only
- Progress Multiplier: x1.5
- Reward Multiplier: x1.4

Description: "Тільки ручна робота, але з хорошою винагородою."
Color: Blue
```

#### "Combo Master" (Майстер комбо)
```
Set Name: Combo Master
Difficulty: Normal
Reward Multiplier: 1.4
Min Contract Level: 8
Spawn Chance: 45%

Modifiers:
- Combo Progress: Max 8, Bonus 0.15, Reset 2.5s
- Progress Bonus: +8

Description: "Система комбо для швидких кліків."
Color: Blue
```

---

### 3. Hard Tier

#### "Time Trial" (Випробування часом)
```
Set Name: Time Trial
Difficulty: Hard
Reward Multiplier: 1.8
Min Contract Level: 15
Spawn Chance: 40%

Modifiers:
- Time Limit: 90s
- Time Bonus Reward: 45s, x2.5
- Progress Multiplier: x1.4

Description: "Обмежений час, але щедра винагорода за швидкість."
Color: Orange (1.0, 0.7, 0.3)
```

#### "Manual Expert" (Експерт ручної праці)
```
Set Name: Manual Expert
Difficulty: Hard
Reward Multiplier: 2.0
Min Contract Level: 15
Spawn Chance: 35%

Modifiers:
- Work Type Restriction: Manual Only
- Combo Progress: Max 10, Bonus 0.25, Reset 2s
- Reward Multiplier: x1.5
- Progress Multiplier: x1.2

Description: "Тільки ручна робота з комбо системою. Висока винагорода."
Color: Orange
```

#### "Speed Demon" (Демон швидкості)
```
Set Name: Speed Demon
Difficulty: Hard
Reward Multiplier: 1.9
Min Contract Level: 20
Spawn Chance: 30%

Modifiers:
- Time Limit: 60s
- Progress Multiplier: x2.0
- Combo Progress: Max 12, Bonus 0.2, Reset 1.5s
- Reward Bonus: +100

Description: "Екстремальна швидкість. 60 секунд на виконання!"
Color: Orange
```

---

### 4. Expert Tier

#### "The Gauntlet" (Випробування)
```
Set Name: The Gauntlet
Difficulty: Expert
Reward Multiplier: 2.5
Min Contract Level: 30
Spawn Chance: 25%

Modifiers:
- Work Type Restriction: Manual Only
- Time Limit: 120s
- Time Bonus Reward: 60s, x3.0
- Combo Progress: Max 15, Bonus 0.3, Reset 1.5s
- Progress Multiplier: x1.3

Description: "Екстремальний виклик для справжніх майстрів."
Color: Red (0.9, 0.3, 0.3)
```

#### "Double or Nothing" (Все або нічого)
```
Set Name: Double or Nothing
Difficulty: Expert
Reward Multiplier: 3.0
Min Contract Level: 35
Spawn Chance: 20%

Modifiers:
- Time Limit: 90s
- Work Type Restriction: Manual Only
- Reward Multiplier: x2.0
- Progress Multiplier: x1.5
- Combo Progress: Max 20, Bonus 0.25, Reset 2s

Description: "Подвійна винагорода за ризиковане виконання."
Color: Red
```

#### "Auto Master" (Майстер автоматизації)
```
Set Name: Auto Master
Difficulty: Expert
Reward Multiplier: 2.2
Min Contract Level: 30
Spawn Chance: 30%

Modifiers:
- Work Type Restriction: Automatic Only
- Progress Multiplier: x2.5
- Reward Multiplier: x1.8
- Progress Bonus: +50

Description: "Тільки автоматична робота з потужними бонусами."
Color: Red
```

---

### 5. Legendary Tier

#### "Godlike" (Божественний)
```
Set Name: Godlike
Difficulty: Legendary
Reward Multiplier: 5.0
Min Contract Level: 50
Spawn Chance: 10%

Modifiers:
- Work Type Restriction: Manual Only
- Time Limit: 60s
- Time Bonus Reward: 30s, x4.0
- Combo Progress: Max 25, Bonus 0.4, Reset 1s
- Progress Multiplier: x2.0
- Reward Multiplier: x2.5

Description: "Легендарний виклик. Тільки для найкращих!"
Color: Gold (1.0, 0.84, 0.0)
```

#### "Perfect Storm" (Ідеальний шторм)
```
Set Name: Perfect Storm
Difficulty: Legendary
Reward Multiplier: 4.5
Min Contract Level: 45
Spawn Chance: 12%

Modifiers:
- Time Limit: 90s
- Combo Progress: Max 30, Bonus 0.35, Reset 1.2s
- Progress Multiplier: x2.2
- Reward Multiplier: x3.0
- Progress Bonus: +75
- Time Bonus Reward: 45s, x3.5

Description: "Комбінація всіх бонусів. Екстремальна винагорода!"
Color: Gold
```

#### "Ultimate Challenge" (Вищий виклик)
```
Set Name: Ultimate Challenge
Difficulty: Legendary
Reward Multiplier: 6.0
Min Contract Level: 60
Spawn Chance: 5%

Modifiers:
- Work Type Restriction: Manual Only
- Time Limit: 45s
- Time Bonus Reward: 20s, x5.0
- Combo Progress: Max 40, Bonus 0.5, Reset 0.8s
- Progress Multiplier: x3.0
- Reward Multiplier: x3.5
- Reward Bonus: +500

Description: "Вищий виклик гри. Найбільша винагорода за найскладніше виконання!"
Color: Gold
```

---

## 🎯 Спеціальні тематичні сети

### "Weekend Warrior" (Воїн вихідного дня)
```
Set Name: Weekend Warrior
Difficulty: Normal
Reward Multiplier: 1.6
Min Contract Level: 10
Spawn Chance: 50%

Modifiers:
- Progress Multiplier: x1.8
- Reward Bonus: +50
- Time Bonus Reward: 60s, x2.0

Description: "Спеціальний набір для розслабленої гри."
Color: Purple (0.8, 0.2, 0.8)
```

### "Night Owl" (Нічна сова)
```
Set Name: Night Owl
Difficulty: Hard
Reward Multiplier: 2.2
Min Contract Level: 20
Spawn Chance: 25%

Modifiers:
- Combo Progress: Max 15, Bonus 0.3, Reset 3s
- Progress Multiplier: x1.6
- Reward Multiplier: x1.8

Description: "Для тих, хто працює вночі. Розслаблені таймінги комбо."
Color: Dark Purple (0.5, 0.1, 0.5)
```

### "Speedrunner" (Спідранер)
```
Set Name: Speedrunner
Difficulty: Expert
Reward Multiplier: 3.5
Min Contract Level: 40
Spawn Chance: 15%

Modifiers:
- Time Limit: 30s
- Progress Multiplier: x3.0
- Time Bonus Reward: 15s, x6.0
- Combo Progress: Max 50, Bonus 0.6, Reset 0.5s

Description: "Для справжніх спідранерів. Екстремальна швидкість!"
Color: Rainbow Effect
```

---

## 📊 Рекомендації по налаштуванню

### Баланс винагород:
- **Easy**: 1.0 - 1.5x базової винагороди
- **Normal**: 1.2 - 1.8x
- **Hard**: 1.5 - 2.5x
- **Expert**: 2.0 - 3.5x
- **Legendary**: 3.0 - 6.0x

### Spawn Chance:
- **Easy**: 60-80%
- **Normal**: 40-60%
- **Hard**: 25-40%
- **Expert**: 15-30%
- **Legendary**: 5-15%

### Min Contract Level:
- **Easy**: 1-5
- **Normal**: 5-15
- **Hard**: 15-30
- **Expert**: 30-50
- **Legendary**: 45+

---

**Використовуйте ці пресети як шаблони для створення ScriptableObject в Unity!**

