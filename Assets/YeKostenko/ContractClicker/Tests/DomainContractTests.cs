using NUnit.Framework;

using YeKostenko.ContractClicker.Data;
using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.ContractClicker.Domain.Contract;

namespace YeKostenko.ContractClicker.Tests
{
    [TestFixture]
    public class DomainContractTests
    {
        [Test]
        public void ContractTestSimplePasses()
        {
            const string Name = "Test Contract";
            IContractFactory factory = ContractFactory.Default;
            IContract contract = factory.Create(new ContractDefinition(1, Name, 100f, 50, null));

            Assert.AreEqual(1, contract.Id);
            Assert.AreEqual(Name, contract.Name);
            Assert.AreEqual(100f, contract.RequiredProgress);
            Assert.AreEqual(0f, contract.CurrentProgress);
            Assert.AreEqual(50, contract.Reward);
            Assert.IsFalse(contract.IsCompleted);

            contract.AddProgress(30f, ProgressType.Manual);
            Assert.AreEqual(30f, contract.CurrentProgress);
            Assert.IsFalse(contract.IsCompleted);

            contract.AddProgress(70f, ProgressType.Manual);
            Assert.AreEqual(100f, contract.CurrentProgress);
            Assert.IsTrue(contract.IsCompleted);

            contract.AddProgress(10f, ProgressType.Manual); // Should have no effect
            Assert.AreEqual(100f, contract.CurrentProgress);
        }

        /*[Test]
        public void ContractProcessorModelTest()
        {
            const string Name = "Test Contract Processor";
            IContractFactory factory = ContractFactory.Default;
            IContract contract1 = factory.Create(new ContractDefinition(1, Name, 200f, 100));
            IContract contract2 = factory.Create(new ContractDefinition(2, Name, 200f, 100));

            IContractProcessorModelFactory contractProcessorModelFactory = ContractProcessorModelFactory.Default;
            IContractProcessorModel contractProcessor = contractProcessorModelFactory.Create();

            Assert.AreEqual(0, contractProcessor.ActiveContracts.Count);
            Assert.IsTrue(contractProcessor.UnlockedContractSlots > 0);

            // Test starting new contracts
            bool started1 = contractProcessor.StartNewContract(contract1);
            Assert.IsTrue(started1);
            Assert.AreEqual(1, contractProcessor.ActiveContracts.Count);
            Assert.IsTrue(contractProcessor.ActiveContracts.Contains(contract1));

            bool started2 = contractProcessor.StartNewContract(contract2);
            Assert.IsTrue(started2);
            Assert.AreEqual(2, contractProcessor.ActiveContracts.Count);

            // Test starting more contracts than available slots
            IContract contract3 = factory.Create(new ContractDefinition(3, Name, 200f, 100));
            bool started3 = contractProcessor.StartNewContract(contract3);
            Assert.IsFalse(started3);
            Assert.AreEqual(2, contractProcessor.ActiveContracts.Count);

            // Test adding progress to contract
            contractProcessor.AddProgressToContract(1, 100f, ProgressType.Manual);
            Assert.AreEqual(100f, contract1.CurrentProgress);
            Assert.IsFalse(contract1.IsCompleted);

            // Test adding progress to wrong contract ID
            Assert.Throws<System.Exception>(() => { contractProcessor.AddProgressToContract(999, 50f, ProgressType.Manual); });

            // Complete contract1
            contractProcessor.AddProgressToContract(1, 100f, ProgressType.Manual);
            Assert.IsTrue(contract1.IsCompleted);

            // Clear completed contracts
            contractProcessor.ClearCompletedContracts();
            Assert.AreEqual(1, contractProcessor.ActiveContracts.Count);
            Assert.IsFalse(contractProcessor.ActiveContracts.Contains(contract1));
            Assert.IsTrue(contractProcessor.ActiveContracts.Contains(contract2));

            // Test skipping contract
            contractProcessor.SkipContract(2);
            Assert.AreEqual(0, contractProcessor.ActiveContracts.Count);

            // Test skipping non-existing contract
            Assert.Throws<System.Exception>(() => { contractProcessor.SkipContract(999); });
        }*/



        /*
        [Test]
        public void ContractGeneratorTest()
        {
            int numberOfContractsToGenerate = 5;

            IContractGenerator generator = new ContractGenerator()

            IContractFactory contractFactory = ContractFactory.Default;

            List<ContractDefinition> contracts = generator.GenerateContracts(numberOfContractsToGenerate);

            Assert.AreEqual(numberOfContractsToGenerate, contracts.Count);
            foreach (ContractDefinition contractData in contracts)
            {
                IContract contract = contractFactory.Create(contractData);

                Assert.IsNotNull(contract);
                Assert.AreEqual(contractData.Id, contract.Id);
                Assert.AreEqual(contractData.Name, contract.Name);
                Assert.AreEqual(contractData.RequiredProgress, contract.RequiredProgress);
                Assert.AreEqual(0f, contract.CurrentProgress);
                Assert.AreEqual(contractData.Reward, contract.Reward);
            }
        }*/
    }
}
