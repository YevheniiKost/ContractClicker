﻿using System;
using System.Collections.Generic;

using NUnit.Framework;

using YeKostenko.ContractClicker.Data;
using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.ContractClicker.Domain;
using YeKostenko.ContractClicker.Domain.Work;

using YevheniiKostenko.CoreKit.Time;

namespace YeKostenko.ContractClicker.Tests
{
    [TestFixture]
    public class DomainWorkTests
    {
        [Test]
        public void WorkersFactory_ThrowOnNullStats()
        {
            IWorkersFactory factory = WorkersFactory.Default;
            WorkerConfig config = CreateDefaultWorkerConfig();

            Assert.Throws<ArgumentNullException>(() => factory.CreateManualWorker(null, 1, config));
            Assert.Throws<ArgumentNullException>(() => factory.CreateAutomaticWorker(1, null, 1, config));
        }

        [Test]
        public void ManualWorker_BaseWork()
        {
            float clickPower = 10f;
            IManualWorker worker = CreateManualWorker(clickPower: clickPower);

            float workDone = 0f;
            worker.WorkDone += (work, _) => workDone += work;

            worker.ProcessClick();
            Assert.AreEqual(clickPower, workDone);
            worker.ProcessClick();
            Assert.AreEqual(clickPower * 2, workDone);
            Assert.AreEqual(worker.Type, ProgressType.Manual);
        }

        [Test]
        public void ManualWorker_LevelProgressFactor_MultiplierCorrectly()
        {
            float clickPower = 10f;
            float levelLinearProgressFactor = 0.5f;
            int level = 2;

            IManualWorker worker = CreateManualWorker(
                levelLinearProgressFactor: levelLinearProgressFactor,
                level: level,
                clickPower: clickPower);

            float expectedWorkPerClick = clickPower + (clickPower * levelLinearProgressFactor * (level - 1));
            float workDone = 0f;
            worker.WorkDone += (work, _) => workDone += work;

            worker.ProcessClick();
            Assert.AreEqual(expectedWorkPerClick, workDone);
        }

        [Test]
        public void AutomaticWorker_BaseWork()
        {
            float yieldPerTick = 10f;
            IAutomaticWorker worker = CreateAutomaticWorker(yieldPerTick: yieldPerTick, tickInterval: 0.5f);

            float workDone = 0f;
            worker.WorkDone += (work, _) => workDone += work;

            worker.Tick(0.2f);
            Assert.AreEqual(0f, workDone);
            worker.Tick(0.5f);
            Assert.AreEqual(yieldPerTick, workDone);
            worker.Tick(1.0f);
            Assert.AreEqual(yieldPerTick * 3, workDone);
            Assert.AreEqual(worker.Type, ProgressType.Automatic);
        }

        [Test]
        public void WorkProgressTest()
        {
            WorkProgress progress = new WorkProgress(0, ProgressType.Manual);

            int eventCallCount = 0;
            progress.ProgressChanged += () => eventCallCount++;

            Assert.AreEqual(0, progress.Amount);
            Assert.AreEqual(ProgressType.Manual, progress.Type);

            progress.AddProgress(50f);
            Assert.AreEqual(50f, progress.Amount);

            progress.AddProgress(25f);
            Assert.AreEqual(75f, progress.Amount);

            progress.SpendProgress(40f);
            Assert.AreEqual(35f, progress.Amount);

            Assert.AreEqual(3, eventCallCount);
        }

        /*[Test]
        public void WorkControllerTest()
        {
            IManualWorker manualWorker = CreateManualWorker(clickPower: 10f);
            IAutomaticWorker automaticWorker = CreateAutomaticWorker(yieldPerTick: 20f, tickInterval: 0.5f);
            TestTimeProvider timeProvider = new TestTimeProvider();

            IWorkController workController = new WorkController(timeProvider);

            workController.Initialize(  manualWorker,
                new System.Collections.Generic.List<IAutomaticWorker> { automaticWorker });

            Assert.AreEqual(0f, workController.ManualProgress.Amount);
            Assert.AreEqual(0f, workController.AutomaticProgress.Amount);

            manualWorker.ProcessClick();
            Assert.AreEqual(10f, workController.ManualProgress.Amount);

            timeProvider.AdvanceTime(0.5f);
            Assert.AreEqual(20f, workController.AutomaticProgress.Amount);

            workController.Dispose();
        }*/

        private IManualWorker CreateManualWorker(float critChance = 0f, float critMultiplier = 1f, float levelLinearProgressFactor = 0f,
            float powerCurveProgressionFactory = 1f, int level = 1, float clickPower = 10f, float clickCooldown = 0f)
        {
            ManualWorkerStats stats = new ManualWorkerStats(critChance, critMultiplier, levelLinearProgressFactor, powerCurveProgressionFactory, clickPower, clickCooldown);
            WorkerConfig config = CreateDefaultWorkerConfig();
            IWorkersFactory factory = WorkersFactory.Default;
            IManualWorker worker = factory.CreateManualWorker(stats, level, config);

            return worker;
        }

        private IAutomaticWorker CreateAutomaticWorker(float critChance = 0f, float critMultiplier = 1f, float levelLinearProgressFactor = 0f,
            float powerCurveProgressionFactory = 1f, int level = 1, float yieldPerTick = 10f, float tickInterval = 0f)
        {
            AutomaticWorkerStats stats = new AutomaticWorkerStats(critChance,
                critMultiplier,
                levelLinearProgressFactor,
                powerCurveProgressionFactory,
                yieldPerTick, tickInterval);
            WorkerConfig config = CreateDefaultWorkerConfig();
            IWorkersFactory factory = WorkersFactory.Default;
            IAutomaticWorker worker = factory.CreateAutomaticWorker(1, stats, level, config);

            return worker;
        }

        private WorkerConfig CreateDefaultWorkerConfig()
        {
            return new WorkerConfig(
                criticalChance: 0f,
                criticalMultiplier: 1f,
                levelLinearProgressionFactor: 0f,
                powerCurveProgressionFactor: 1f,
                workPower: 10f,
                workPauseDuration: 0f,
                initialUpgradePrice: 100,
                upgradePriceMultiplier: 1.5f,
                statModifications: new List<StatModificationConfig>
                {
                    new StatModificationConfig(StatId.CritChance, ModificationOperation.Add, 0.01f, levelInterval: 5, 1000),
                    new StatModificationConfig(StatId.CritMultiplier, ModificationOperation.Add, 0.1f, levelInterval: 5, 1000)
                });
        }
    }

    public class TestTimeProvider : ITimeProvider
    {
        private float _currentTime;
        private List<ITimeListener> _listeners = new List<ITimeListener>();

        public float CurrentTime => _currentTime;

        public void AdvanceTime(float deltaTime)
        {
            _currentTime += deltaTime;
            foreach (var listener in _listeners)
            {
                listener.Update(deltaTime);
            }
        }

        public void RegisterTimeListener(ITimeListener listener) => _listeners.Add(listener);

        public void ClearTimeListener(ITimeListener listener) => _listeners.Remove(listener);

        public void SetTimeScale(float timeScale)
        {
        }
    }
}