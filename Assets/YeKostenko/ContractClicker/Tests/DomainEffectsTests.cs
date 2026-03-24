using NUnit.Framework;

using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.ContractClicker.Domain.Work.Effects;

namespace YeKostenko.ContractClicker.Tests
{
    [TestFixture]
    public class DomainEffectsTests
    {
        [Test]
        public void StatModification_Creation_WorksAsExpected()
        {
            StatId statId = StatId.ClickPower;
            float value = 10.0f;
            ModificationOperation operation = ModificationOperation.Add;
            int priority = 5;

            StatModification statModification = new StatModification(statId, value, operation, priority);

            Assert.AreEqual(statId, statModification.Stat);
            Assert.AreEqual(value, statModification.Value);
            Assert.AreEqual(operation, statModification.Operation);
            Assert.AreEqual(priority, statModification.Priority);
        }

        [Test]
        public void ActiveEffect_Creation_WorksAsExpected()
        {
            EffectDefinition effectDefinition = new EffectDefinition("TestEffect", 30.0f,
                StackPolicy.Stack, 10, new []
                {
                    new StatModification(StatId.ClickPower, 10.0f, ModificationOperation.Add),
                });

            ActiveEffect activeEffect = new ActiveEffect(effectDefinition);

            Assert.AreEqual(effectDefinition, activeEffect.Definition);
            Assert.AreEqual(0, activeEffect.CurrentStacks);
            Assert.AreEqual(0.0f, activeEffect.RemainingDurationInSeconds);
        }

        [Test]
        public void EffectDefinition_Creation_WorksAsExpected()
        {
            string id = "TestEffect";
            float durationInSeconds = 30.0f;
            StackPolicy stackPolicy = StackPolicy.Stack;
            int maxStacks = 5;
            StatModification[] modifications = new []
            {
                new StatModification(StatId.ClickPower, 10.0f, ModificationOperation.Add),
            };

            EffectDefinition effectDefinition = new EffectDefinition(id, durationInSeconds, stackPolicy, maxStacks, modifications);

            Assert.AreEqual(id, effectDefinition.Id);
            Assert.AreEqual(durationInSeconds, effectDefinition.DurationInSeconds);
            Assert.AreEqual(stackPolicy, effectDefinition.StackPolicy);
            Assert.AreEqual(maxStacks, effectDefinition.MaxStacks);
            Assert.AreEqual(modifications, effectDefinition.Modifications);
        }

        [Test]
        public void ActiveEffect_AddStackAndRefresh_WorksAsExpected()
        {
            EffectDefinition effectDefinition = new EffectDefinition("TestEffect", 30.0f,
                StackPolicy.Stack, 2, new []
                {
                    new StatModification(StatId.ClickPower, 10.0f, ModificationOperation.Add),
                });
            ActiveEffect activeEffect = new ActiveEffect(effectDefinition);

            activeEffect.AddStack();
            activeEffect.Refresh();

            Assert.AreEqual(1, activeEffect.CurrentStacks);
            Assert.AreEqual(30.0f, activeEffect.RemainingDurationInSeconds);

            activeEffect.Tick(10.0f);
            Assert.AreEqual(20.0f, activeEffect.RemainingDurationInSeconds);

            activeEffect.AddStack();

            Assert.AreEqual(2, activeEffect.CurrentStacks);

            activeEffect.AddStack();

            Assert.AreEqual(2, activeEffect.CurrentStacks); // Should not exceed max stacks
        }
    }
}