﻿# Пример работы системы автоматических улучшений статов

## Базовый пример

```csharp
// 1. Создаем конфиг с разными интервалами для разных статов
WorkerConfig config = new WorkerConfig(
    criticalChance: 0.05f,
    criticalMultiplier: 2.0f,
    levelLinearProgressionFactor: 0.1f,
    powerCurveProgressionFactor: 1.0f,
    workPower: 10f,
    workPauseDuration: 1f,
    initialUpgradePrice: 100,
    upgradePriceMultiplier: 1.5f,
    statModifications: new List<StatModificationConfig>
    {
        // CritChance улучшается каждые 5 уровней
        new StatModificationConfig(StatId.CritChance, ModificationOperation.Add, 0.01f, levelInterval: 5),
        // CritMultiplier улучшается каждые 10 уровней
        new StatModificationConfig(StatId.CritMultiplier, ModificationOperation.Add, 0.1f, levelInterval: 10)
    }
);

// 2. Создаем работника 1-го уровня
IManualWorker worker = factory.CreateManualWorker(stats, level: 1, config);

// 3. Проверяем статы на разных уровнях
// Уровень 1: CritChance = 0.05, CritMultiplier = 2.0
// Уровень 5: CritChance = 0.06 (+0.01), CritMultiplier = 2.0 (без изменений)
// Уровень 10: CritChance = 0.07 (+0.02), CritMultiplier = 2.1 (+0.1)
// Уровень 15: CritChance = 0.08 (+0.03), CritMultiplier = 2.1 (без изменений)
// Уровень 20: CritChance = 0.09 (+0.04), CritMultiplier = 2.2 (+0.2)
```

## Пример с разными интервалами для одного стата

```csharp
// Можно применять несколько модификаций к одному стату с разными интервалами
WorkerConfig config = new WorkerConfig(
    // ...базовые параметры...
    statModifications: new List<StatModificationConfig>
    {
        // Малое улучшение каждые 5 уровней
        new StatModificationConfig(StatId.ClickPower, ModificationOperation.Add, 10f, levelInterval: 5),
        // Среднее улучшение каждые 10 уровней
        new StatModificationConfig(StatId.ClickPower, ModificationOperation.Add, 50f, levelInterval: 10),
        // Большое улучшение каждые 25 уровней
        new StatModificationConfig(StatId.ClickPower, ModificationOperation.Mul, 1.5f, levelInterval: 25)
    }
);

// Уровень 1: ClickPower = 100
// Уровень 5: ClickPower = 100 + 10 = 110
// Уровень 10: ClickPower = 100 + 20 + 50 = 170
// Уровень 25: ClickPower = (100 + 50 + 100 + 50) * 1.5 = 450
```

## Пример: быстрая и медленная прогрессия

```csharp
WorkerConfig config = new WorkerConfig(
    // ...базовые параметры...
    statModifications: new List<StatModificationConfig>
    {
        // Быстрая прогрессия - каждые 3 уровня
        new StatModificationConfig(StatId.CritChance, ModificationOperation.Add, 0.005f, levelInterval: 3),
        // Средняя прогрессия - каждые 10 уровней
        new StatModificationConfig(StatId.ClickPower, ModificationOperation.Mul, 1.1f, levelInterval: 10),
        // Медленная прогрессия - каждые 20 уровней
        new StatModificationConfig(StatId.CritMultiplier, ModificationOperation.Add, 0.5f, levelInterval: 20)
    }
);

// Это позволяет создавать разнообразные кривые прогрессии
```

## Пример проверки в тестах

```csharp
[Test]
public void Worker_AutoUpgrades_ApplyCorrectly()
{
    // Arrange
    WorkerConfig config = CreateDefaultWorkerConfig();
    config.StatModificationLevelInterval = 5;
    config.StatModifications = new List<StatModificationConfig>
    {
        new StatModificationConfig(StatId.CritChance, ModificationOperation.Add, 0.01f)
    };

    IManualWorker worker = CreateManualWorker(level: 1, config: config);
    float baseCritChance = worker.GetStat(StatId.CritChance);

    // Act - прокачка до 5-го уровня
    for (int i = 0; i < 4; i++)
    {
        worker.LevelUpWorker();
    }

    // Assert
    float upgradedCritChance = worker.GetStat(StatId.CritChance);
    Assert.AreEqual(baseCritChance + 0.01f, upgradedCritChance);
}
```

## Настройка баланса

### Для клик-работников (Manual)
```csharp
statModifications: new List<StatModificationConfig>
{
    // ClickPower улучшается часто, но незначительно
    new StatModificationConfig(StatId.ClickPower, ModificationOperation.Mul, 1.05f, levelInterval: 5),    // +5% силы каждые 5 уровней
    // CritChance улучшается средне
    new StatModificationConfig(StatId.CritChance, ModificationOperation.Add, 0.01f, levelInterval: 7),    // +1% крит каждые 7 уровней
    // CritMultiplier улучшается редко, но значительно
    new StatModificationConfig(StatId.CritMultiplier, ModificationOperation.Add, 0.2f, levelInterval: 15)  // +0.2х крит множитель каждые 15 уровней
}
```

### Для авто-работников (Automatic)
```csharp
statModifications: new List<StatModificationConfig>
{
    // YieldPerTick - основной стат, улучшается часто
    new StatModificationConfig(StatId.YieldPerTick, ModificationOperation.Mul, 1.1f, levelInterval: 5),      // +10% доход каждые 5 уровней
    // TickInterval - улучшается реже
    new StatModificationConfig(StatId.TickInterval, ModificationOperation.Mul, 0.98f, levelInterval: 10),    // -2% интервал каждые 10 уровней
    // CritChance - улучшается редко
    new StatModificationConfig(StatId.CritChance, ModificationOperation.Add, 0.005f, levelInterval: 15),     // +0.5% крит каждые 15 уровней
    // CritMultiplier - улучшается очень редко
    new StatModificationConfig(StatId.CritMultiplier, ModificationOperation.Add, 0.1f, levelInterval: 20)    // +0.1х крит множитель каждые 20 уровней
}
```

### Пример: прогрессия с вехами
```csharp
statModifications: new List<StatModificationConfig>
{
    // Постоянные малые улучшения
    new StatModificationConfig(StatId.ClickPower, ModificationOperation.Add, 5f, levelInterval: 3),
    // Средние улучшения
    new StatModificationConfig(StatId.ClickPower, ModificationOperation.Add, 25f, levelInterval: 10),
    // Важные вехи - большие бонусы
    new StatModificationConfig(StatId.ClickPower, ModificationOperation.Mul, 1.5f, levelInterval: 25),
    // Эпические вехи - огромные бонусы
    new StatModificationConfig(StatId.ClickPower, ModificationOperation.Mul, 2.0f, levelInterval: 50)
}
```

## Советы по балансу

1. **Индивидуальные интервалы для каждого стата**:
   - Важные статы (урон, доход): 3-7 уровней
   - Второстепенные статы (крит): 10-15 уровней
   - Редкие бонусы: 20-50 уровней

2. **Величина улучшений**:
   - Частые улучшения: малые значения (Add: 1-10, Mul: 1.01-1.05)
   - Средние улучшения: средние значения (Add: 10-50, Mul: 1.05-1.15)
   - Редкие улучшения: большие значения (Add: 50+, Mul: 1.2-2.0)

3. **Комбинации**:
   - Один стат может иметь несколько модификаций с разными интервалами
   - Это создает интересную кривую прогрессии

4. **Вехи (Milestones)**:
   - Используй большие интервалы (25, 50, 100) для значительных улучшений
   - Это дает игрокам долгосрочные цели

