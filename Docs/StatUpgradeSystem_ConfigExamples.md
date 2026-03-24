# Примеры конфигурации системы улучшений статов

## Простая конфигурация

```csharp
// Все статы улучшаются одинаково часто
var config = new WorkerConfig(
    // ...базовые параметры...
    statModifications: new List<StatModificationConfig>
    {
        new StatModificationConfig(StatId.CritChance, ModificationOperation.Add, 0.01f, levelInterval: 5),
        new StatModificationConfig(StatId.CritMultiplier, ModificationOperation.Add, 0.1f, levelInterval: 5)
    }
);
```

## Разные интервалы для разных статов

```csharp
// Основной стат улучшается часто, второстепенные - реже
var config = new WorkerConfig(
    // ...базовые параметры...
    statModifications: new List<StatModificationConfig>
    {
        // Основной стат - часто
        new StatModificationConfig(StatId.ClickPower, ModificationOperation.Mul, 1.1f, levelInterval: 5),

        // Крит - средне
        new StatModificationConfig(StatId.CritChance, ModificationOperation.Add, 0.01f, levelInterval: 10),

        // Крит множитель - редко
        new StatModificationConfig(StatId.CritMultiplier, ModificationOperation.Add, 0.2f, levelInterval: 15)
    }
);
```

## Многоуровневая прогрессия

```csharp
// Один стат получает улучшения на разных уровнях с разной силой
var config = new WorkerConfig(
    // ...базовые параметры...
    statModifications: new List<StatModificationConfig>
    {
        // Частые малые бонусы
        new StatModificationConfig(StatId.ClickPower, ModificationOperation.Add, 5f, levelInterval: 3),

        // Средние бонусы
        new StatModificationConfig(StatId.ClickPower, ModificationOperation.Add, 20f, levelInterval: 10),

        // Редкие большие бонусы (вехи)
        new StatModificationConfig(StatId.ClickPower, ModificationOperation.Mul, 1.5f, levelInterval: 25),

        // Очень редкие огромные бонусы
        new StatModificationConfig(StatId.ClickPower, ModificationOperation.Mul, 2.0f, levelInterval: 50)
    }
);

// Результат:
// Уровень 3: +5
// Уровень 6: +10
// Уровень 9: +15
// Уровень 10: +15 + 20 = +35
// Уровень 25: значительный скачок благодаря *1.5
// Уровень 50: огромный скачок благодаря *2.0 и *1.5
```

## Конфигурация для ранней игры (быстрый прогресс)

```csharp
var earlyGameConfig = new WorkerConfig(
    // ...базовые параметры...
    statModifications: new List<StatModificationConfig>
    {
        // Частые улучшения для быстрого ощущения прогресса
        new StatModificationConfig(StatId.ClickPower, ModificationOperation.Mul, 1.15f, levelInterval: 3),
        new StatModificationConfig(StatId.CritChance, ModificationOperation.Add, 0.02f, levelInterval: 5),
        new StatModificationConfig(StatId.CritMultiplier, ModificationOperation.Add, 0.15f, levelInterval: 7)
    }
);
```

## Конфигурация для поздней игры (медленный прогресс)

```csharp
var lateGameConfig = new WorkerConfig(
    // ...базовые параметры...
    statModifications: new List<StatModificationConfig>
    {
        // Редкие, но мощные улучшения
        new StatModificationConfig(StatId.ClickPower, ModificationOperation.Mul, 1.5f, levelInterval: 20),
        new StatModificationConfig(StatId.CritChance, ModificationOperation.Add, 0.05f, levelInterval: 25),
        new StatModificationConfig(StatId.CritMultiplier, ModificationOperation.Add, 0.5f, levelInterval: 30)
    }
);
```

## Специализированные конфигурации

### "Машина криты"
```csharp
var critSpecialistConfig = new WorkerConfig(
    // ...базовые параметры...
    statModifications: new List<StatModificationConfig>
    {
        // Фокус на крите
        new StatModificationConfig(StatId.CritChance, ModificationOperation.Add, 0.015f, levelInterval: 3),
        new StatModificationConfig(StatId.CritChance, ModificationOperation.Mul, 1.05f, levelInterval: 10),
        new StatModificationConfig(StatId.CritMultiplier, ModificationOperation.Add, 0.15f, levelInterval: 5),
        new StatModificationConfig(StatId.CritMultiplier, ModificationOperation.Mul, 1.1f, levelInterval: 15),

        // Базовый урон растет медленно
        new StatModificationConfig(StatId.ClickPower, ModificationOperation.Mul, 1.05f, levelInterval: 10)
    }
);
```

### "Стабильный урон"
```csharp
var steadyDamageConfig = new WorkerConfig(
    // ...базовые параметры...
    statModifications: new List<StatModificationConfig>
    {
        // Фокус на постоянном уроне
        new StatModificationConfig(StatId.ClickPower, ModificationOperation.Mul, 1.1f, levelInterval: 3),
        new StatModificationConfig(StatId.ClickPower, ModificationOperation.Add, 25f, levelInterval: 5),

        // Крит минимальный
        new StatModificationConfig(StatId.CritChance, ModificationOperation.Add, 0.005f, levelInterval: 15),
        new StatModificationConfig(StatId.CritMultiplier, ModificationOperation.Add, 0.05f, levelInterval: 20)
    }
);
```

### "Скоростной работник" (для автоматических)
```csharp
var speedWorkerConfig = new WorkerConfig(
    // ...базовые параметры...
    statModifications: new List<StatModificationConfig>
    {
        // Фокус на скорости работы
        new StatModificationConfig(StatId.TickInterval, ModificationOperation.Mul, 0.95f, levelInterval: 5),
        new StatModificationConfig(StatId.TickInterval, ModificationOperation.Mul, 0.9f, levelInterval: 15),

        // Урон растет умеренно
        new StatModificationConfig(StatId.YieldPerTick, ModificationOperation.Mul, 1.1f, levelInterval: 10)
    }
);
```

## Динамический конфиг (программный выбор)

```csharp
public WorkerConfig CreateConfigForWorkerType(string workerType, int playerLevel)
{
    List<StatModificationConfig> modifications = new List<StatModificationConfig>();

    if (workerType == "manual")
    {
        // Конфиг для ручных работников
        modifications.Add(new StatModificationConfig(
            StatId.ClickPower,
            ModificationOperation.Mul,
            playerLevel < 50 ? 1.15f : 1.08f,  // Быстрее в начале, медленнее потом
            levelInterval: playerLevel < 50 ? 5 : 10
        ));

        modifications.Add(new StatModificationConfig(
            StatId.CritChance,
            ModificationOperation.Add,
            0.01f,
            levelInterval: 7
        ));
    }
    else if (workerType == "automatic")
    {
        // Конфиг для автоматических работников
        modifications.Add(new StatModificationConfig(
            StatId.YieldPerTick,
            ModificationOperation.Mul,
            1.12f,
            levelInterval: 5
        ));

        modifications.Add(new StatModificationConfig(
            StatId.TickInterval,
            ModificationOperation.Mul,
            0.97f,
            levelInterval: 10
        ));
    }

    return new WorkerConfig(
        // ...базовые параметры...
        statModifications: modifications
    );
}
```

## Тестирование баланса

```csharp
// Утилита для расчета статов на разных уровнях
public void TestBalanceProgression(WorkerConfig config, int maxLevel)
{
    var worker = factory.CreateManualWorker(stats, 1, config);

    Debug.Log($"Level 1: ClickPower = {worker.GetStat(StatId.ClickPower, withLevel: true)}");

    for (int level = 5; level <= maxLevel; level += 5)
    {
        // Прокачка до следующей вехи
        while (worker.Level < level)
        {
            worker.LevelUpWorker();
        }

        float clickPower = worker.GetStat(StatId.ClickPower, withLevel: true);
        float critChance = worker.GetStat(StatId.CritChance);
        float critMultiplier = worker.GetStat(StatId.CritMultiplier);

        Debug.Log($"Level {level}: " +
                  $"ClickPower = {clickPower:F2}, " +
                  $"CritChance = {critChance:F3}, " +
                  $"CritMultiplier = {critMultiplier:F2}");
    }
}

// Использование:
TestBalanceProgression(myConfig, maxLevel: 100);
```

## Советы по настройке

1. **Используйте простые числа для интервалов**: 3, 5, 7, 10, 15, 20, 25, 50, 100
   - Это создает интересные пересечения улучшений

2. **Тестируйте кривую прогрессии**: Убедитесь, что рост не слишком быстрый и не слишком медленный

3. **Создавайте вехи**: Используйте большие интервалы (25, 50, 100) для значительных улучшений

4. **Балансируйте аддитивные и мультипликативные бонусы**:
   - Аддитивные хороши в начале
   - Мультипликативные становятся мощнее с уровнем

5. **Разные конфиги для разных типов работников**: Manual и Automatic могут иметь совершенно разные прогрессии

