using NUnit.Framework;

using YeKostenko.ContractClicker.Data;
using YeKostenko.ContractClicker.Domain;
using YeKostenko.ContractClicker.Domain.Contract;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Progress;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Validation;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Reward;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Time;
using YeKostenko.ContractClicker.Domain.Contract.Modifiers.Special;

namespace YeKostenko.ContractClicker.Tests.Domain.Modifiers
{
    [TestFixture]
    public class ContractModifierTests
    {
        [Test]
        public void ProgressMultiplier_DoublesProgress()
        {
            // Arrange
            var contract = CreateTestContract();
            contract.AddModifier(new ProgressMultiplierModifier(2.0f));

            // Act
            contract.AddProgress(10f, ProgressType.Manual);

            // Assert
            Assert.AreEqual(20f, contract.CurrentProgress);
        }

        [Test]
        public void ProgressBonus_AddsFixedAmount()
        {
            // Arrange
            var contract = CreateTestContract();
            contract.AddModifier(new ProgressBonusModifier(15f));

            // Act
            contract.AddProgress(10f, ProgressType.Manual);

            // Assert
            Assert.AreEqual(25f, contract.CurrentProgress); // 10 + 15
        }

        [Test]
        public void WorkTypeRestriction_BlocksWrongType()
        {
            // Arrange
            var contract = CreateTestContract();
            contract.AddModifier(new WorkTypeRestrictionModifier(ProgressType.Manual));

            // Act
            contract.AddProgress(10f, ProgressType.Automatic);

            // Assert
            Assert.AreEqual(0f, contract.CurrentProgress); // Заблоковано
        }

        [Test]
        public void WorkTypeRestriction_AllowsCorrectType()
        {
            // Arrange
            var contract = CreateTestContract();
            contract.AddModifier(new WorkTypeRestrictionModifier(ProgressType.Manual));

            // Act
            contract.AddProgress(10f, ProgressType.Manual);

            // Assert
            Assert.AreEqual(10f, contract.CurrentProgress); // Дозволено
        }

        [Test]
        public void RewardMultiplier_MultipliesReward()
        {
            // Arrange
            var contract = CreateTestContract(reward: 100);
            contract.AddModifier(new RewardMultiplierModifier(1.5f));

            // Act
            var finalReward = contract.FinalReward;

            // Assert
            Assert.AreEqual(150, finalReward); // 100 * 1.5
        }

        [Test]
        public void RewardBonus_AddsFixedBonus()
        {
            // Arrange
            var contract = CreateTestContract(reward: 100);
            contract.AddModifier(new RewardBonusModifier(50));

            // Act
            var finalReward = contract.FinalReward;

            // Assert
            Assert.AreEqual(150, finalReward); // 100 + 50
        }

        [Test]
        public void MultipleModifiers_AppliedInOrder()
        {
            // Arrange
            var contract = CreateTestContract(reward: 100);

            // Множник спочатку
            contract.AddModifier(new ProgressMultiplierModifier(2.0f));
            // Потім бонус
            contract.AddModifier(new ProgressBonusModifier(10f));

            // Act
            contract.AddProgress(5f, ProgressType.Manual);

            // Assert
            // (5 * 2.0) + 10 = 20
            Assert.AreEqual(20f, contract.CurrentProgress);
        }

        [Test]
        public void MultipleRewardModifiers_StackCorrectly()
        {
            // Arrange
            var contract = CreateTestContract(reward: 100);
            contract.AddModifier(new RewardMultiplierModifier(2.0f));
            contract.AddModifier(new RewardBonusModifier(50));

            // Act
            var finalReward = contract.FinalReward;

            // Assert
            // (100 * 2.0) + 50 = 250
            Assert.AreEqual(250, finalReward);
        }

        [Test]
        public void TimeLimitModifier_BlocksAfterExpiration()
        {
            // Arrange
            var contract = CreateTestContract();
            var timeLimit = new TimeLimitModifier(1.0f); // 1 секунда
            contract.AddModifier(timeLimit);

            // Act
            contract.UpdateModifiers(1.5f); // Час вичерпано
            contract.AddProgress(10f, ProgressType.Manual);

            // Assert
            Assert.AreEqual(0f, contract.CurrentProgress); // Заблоковано
            Assert.IsTrue(timeLimit.IsExpired);
        }

        [Test]
        public void TimeLimitModifier_AllowsBeforeExpiration()
        {
            // Arrange
            var contract = CreateTestContract();
            var timeLimit = new TimeLimitModifier(2.0f); // 2 секунди
            contract.AddModifier(timeLimit);

            // Act
            contract.UpdateModifiers(1.0f); // Ще є час
            contract.AddProgress(10f, ProgressType.Manual);

            // Assert
            Assert.AreEqual(10f, contract.CurrentProgress);
            Assert.IsFalse(timeLimit.IsExpired);
        }

        [Test]
        public void ComboModifier_IncreasesWithRapidClicks()
        {
            // Arrange
            var contract = CreateTestContract();
            var combo = new ComboProgressModifier(
                maxCombo: 5,
                comboBonus: 0.2f,
                comboResetTime: 2.0f
            );
            contract.AddModifier(combo);

            // Act
            contract.AddProgress(10f, ProgressType.Manual); // x1
            contract.UpdateModifiers(0.5f); // Ще в межах часу
            contract.AddProgress(10f, ProgressType.Manual); // x2

            // Assert
            // Перший: 10 * 1.0 = 10
            // Другий: 10 * 1.2 = 12
            // Разом: 22
            Assert.AreEqual(22f, contract.CurrentProgress);
            Assert.AreEqual(2, combo.CurrentCombo);
        }

        [Test]
        public void ComboModifier_ResetsAfterTimeout()
        {
            // Arrange
            var contract = CreateTestContract();
            var combo = new ComboProgressModifier(
                maxCombo: 5,
                comboBonus: 0.2f,
                comboResetTime: 1.0f
            );
            contract.AddModifier(combo);

            // Act
            contract.AddProgress(10f, ProgressType.Manual); // Комбо 1
            contract.UpdateModifiers(1.5f); // Час вичерпано
            contract.AddProgress(10f, ProgressType.Manual); // Комбо скинулось

            // Assert
            Assert.AreEqual(1, combo.CurrentCombo); // Скинулось до 1
        }

        [Test]
        public void AddModifier_TriggersOnApply()
        {
            // Arrange
            var contract = CreateTestContract();
            var modifier = new ProgressMultiplierModifier(2.0f);

            // Act
            contract.AddModifier(modifier);

            // Assert
            Assert.IsTrue(contract.HasModifier<ProgressMultiplierModifier>());
            Assert.AreEqual(1, contract.Modifiers.Count);
        }

        [Test]
        public void RemoveModifier_TriggersOnRemove()
        {
            // Arrange
            var contract = CreateTestContract();
            var modifier = new ProgressMultiplierModifier(2.0f);
            contract.AddModifier(modifier);

            // Act
            contract.RemoveModifier(modifier);

            // Assert
            Assert.IsFalse(contract.HasModifier<ProgressMultiplierModifier>());
            Assert.AreEqual(0, contract.Modifiers.Count);
        }

        [Test]
        public void DeactivatedModifier_IsNotProcessed()
        {
            // Arrange
            var contract = CreateTestContract();
            var modifier = new ProgressMultiplierModifier(2.0f);
            contract.AddModifier(modifier);
            modifier.Deactivate();

            // Act
            contract.AddProgress(10f, ProgressType.Manual);

            // Assert
            Assert.AreEqual(10f, contract.CurrentProgress); // Модифікатор не спрацював
        }

        [Test]
        public void HasModifier_ReturnsTrueForExistingModifier()
        {
            // Arrange
            var contract = CreateTestContract();
            contract.AddModifier(new ProgressMultiplierModifier(2.0f));

            // Act & Assert
            Assert.IsTrue(contract.HasModifier<ProgressMultiplierModifier>());
            Assert.IsFalse(contract.HasModifier<WorkTypeRestrictionModifier>());
        }

        [Test]
        public void GetModifier_ReturnsCorrectModifier()
        {
            // Arrange
            var contract = CreateTestContract();
            var modifier = new ProgressMultiplierModifier(2.5f);
            contract.AddModifier(modifier);

            // Act
            var retrieved = contract.GetModifier<ProgressMultiplierModifier>();

            // Assert
            Assert.IsNotNull(retrieved);
            Assert.AreEqual(2.5f, retrieved.Multiplier);
        }

        // Helper methods
        private IContract CreateTestContract(float requiredProgress = 100f, int reward = 50)
        {
            return new Contract(1, "Test Contract", requiredProgress, reward);
        }
    }
}

