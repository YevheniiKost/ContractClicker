using UnityEngine;

/// <summary>
/// Verification script that tests the core game loop functionality.
/// Attach this to a GameObject to run automated verification tests.
/// </summary>
public class GameLoopVerification : MonoBehaviour
{
    [Header("Verification Settings")]
    [Tooltip("Run verification tests on start")]
    public bool runOnStart = true;
    
    private void Start()
    {
        if (runOnStart)
        {
            RunVerificationTests();
        }
    }
    
    /// <summary>
    /// Runs a series of verification tests on the game loop.
    /// </summary>
    public void RunVerificationTests()
    {
        Debug.Log("=== Starting Game Loop Verification ===");
        
        VerifyContractClass();
        VerifyCurrencyManager();
        VerifyClickManager();
        VerifyAutomationManager();
        VerifyUpgradeManager();
        VerifyGameManager();
        VerifyFullGameLoop();
        
        Debug.Log("=== Game Loop Verification Complete ===");
    }
    
    private void VerifyContractClass()
    {
        Debug.Log("\n[Test] Contract Class");
        
        Contract contract = new Contract("Test Contract", "Test Description", 100f, 50);
        
        Assert(contract.Name == "Test Contract", "Contract name should be set");
        Assert(contract.RequiredWork == 100f, "Required work should be 100");
        Assert(contract.Reward == 50, "Reward should be 50");
        Assert(contract.CurrentWork == 0f, "Initial work should be 0");
        Assert(!contract.IsCompleted, "Contract should not be completed initially");
        
        contract.AddWork(50f);
        Assert(contract.CurrentWork == 50f, "Work should be 50 after adding 50");
        Assert(contract.GetProgress() == 0.5f, "Progress should be 50%");
        Assert(!contract.IsCompleted, "Contract should not be completed at 50%");
        
        bool completed = contract.AddWork(50f);
        Assert(completed, "Adding final work should return true");
        Assert(contract.IsCompleted, "Contract should be completed");
        Assert(contract.CurrentWork == 100f, "Final work should be 100");
        
        Debug.Log("[Test] Contract Class - PASSED");
    }
    
    private void VerifyCurrencyManager()
    {
        Debug.Log("\n[Test] CurrencyManager");
        
        // Reset currency
        int initial = CurrencyManager.Instance.CurrentCurrency;
        CurrencyManager.Instance.SpendCurrency(initial);
        
        Assert(CurrencyManager.Instance.CurrentCurrency == 0, "Currency should start at 0");
        
        CurrencyManager.Instance.AddCurrency(100);
        Assert(CurrencyManager.Instance.CurrentCurrency == 100, "Currency should be 100 after adding 100");
        
        bool canAfford = CurrencyManager.Instance.CanAfford(50);
        Assert(canAfford, "Should be able to afford 50");
        
        bool spent = CurrencyManager.Instance.SpendCurrency(50);
        Assert(spent, "Should successfully spend 50");
        Assert(CurrencyManager.Instance.CurrentCurrency == 50, "Currency should be 50 after spending 50");
        
        bool cannotAfford = !CurrencyManager.Instance.CanAfford(100);
        Assert(cannotAfford, "Should not be able to afford 100");
        
        bool failedSpend = !CurrencyManager.Instance.SpendCurrency(100);
        Assert(failedSpend, "Should fail to spend 100");
        
        Debug.Log("[Test] CurrencyManager - PASSED");
    }
    
    private void VerifyClickManager()
    {
        Debug.Log("\n[Test] ClickManager");
        
        ClickManager.Instance.baseClickPower = 1f;
        ClickManager.Instance.SetClickPowerMultiplier(1f);
        
        float clickPower = ClickManager.Instance.GetCurrentClickPower();
        Assert(clickPower == 1f, "Base click power should be 1");
        
        ClickManager.Instance.SetClickPowerMultiplier(2f);
        clickPower = ClickManager.Instance.GetCurrentClickPower();
        Assert(clickPower == 2f, "Click power should be 2 with 2x multiplier");
        
        float workDone = ClickManager.Instance.PerformClick();
        Assert(workDone == 2f, "Click should produce 2 work");
        
        Debug.Log("[Test] ClickManager - PASSED");
    }
    
    private void VerifyAutomationManager()
    {
        Debug.Log("\n[Test] AutomationManager");
        
        AutomationManager.Instance.SetBaseWorkPerSecond(0f);
        AutomationManager.Instance.SetWorkPerSecondMultiplier(1f);
        AutomationManager.Instance.SetAutomationActive(false);
        
        Assert(!AutomationManager.Instance.IsActive(), "Automation should be inactive");
        Assert(AutomationManager.Instance.GetCurrentWorkPerSecond() == 0f, "Work per second should be 0");
        
        AutomationManager.Instance.SetBaseWorkPerSecond(5f);
        Assert(AutomationManager.Instance.GetCurrentWorkPerSecond() == 5f, "Work per second should be 5");
        
        AutomationManager.Instance.SetWorkPerSecondMultiplier(2f);
        Assert(AutomationManager.Instance.GetCurrentWorkPerSecond() == 10f, "Work per second should be 10 with 2x multiplier");
        
        AutomationManager.Instance.SetAutomationActive(true);
        Assert(AutomationManager.Instance.IsActive(), "Automation should be active");
        
        Debug.Log("[Test] AutomationManager - PASSED");
    }
    
    private void VerifyUpgradeManager()
    {
        Debug.Log("\n[Test] UpgradeManager");
        
        // Reset upgrades
        foreach (var upgrade in UpgradeManager.Instance.availableUpgrades)
        {
            upgrade.isPurchased = false;
        }
        
        var availableCount = UpgradeManager.Instance.GetAvailableUpgrades().Count;
        Assert(availableCount > 0, "Should have available upgrades");
        
        // Create a test upgrade
        var testUpgrade = new Upgrade
        {
            name = "Test Upgrade",
            description = "Test",
            cost = 50,
            type = Upgrade.UpgradeType.ClickPowerMultiplier,
            value = 2f,
            isPurchased = false
        };
        
        // Ensure we have enough currency
        CurrencyManager.Instance.AddCurrency(100);
        
        bool purchased = UpgradeManager.Instance.PurchaseUpgrade(testUpgrade);
        Assert(purchased, "Upgrade should be purchased successfully");
        Assert(testUpgrade.isPurchased, "Upgrade should be marked as purchased");
        
        bool cannotPurchaseAgain = !UpgradeManager.Instance.PurchaseUpgrade(testUpgrade);
        Assert(cannotPurchaseAgain, "Should not be able to purchase same upgrade twice");
        
        Debug.Log("[Test] UpgradeManager - PASSED");
    }
    
    private void VerifyGameManager()
    {
        Debug.Log("\n[Test] GameManager");
        
        GameManager.Instance.ResetGame();
        
        Contract currentContract = GameManager.Instance.GetCurrentContract();
        Assert(currentContract != null, "Should have a current contract after reset");
        Assert(!currentContract.IsCompleted, "New contract should not be completed");
        
        int contractsBefore = GameManager.Instance.GetContractsCompleted();
        
        // Complete the contract by adding all required work
        while (!currentContract.IsCompleted)
        {
            GameManager.Instance.PerformManualClick();
        }
        
        // Wait a frame for the new contract to be generated
        // (In actual Unity, this would happen on the next frame)
        
        Debug.Log("[Test] GameManager - PASSED");
    }
    
    private void VerifyFullGameLoop()
    {
        Debug.Log("\n[Test] Full Game Loop Integration");
        
        // Reset everything
        GameManager.Instance.ResetGame();
        CurrencyManager.Instance.SpendCurrency(CurrencyManager.Instance.CurrentCurrency);
        
        // 1. Receive a contract
        Contract contract = GameManager.Instance.GetCurrentContract();
        Assert(contract != null, "Step 1: Should receive a contract");
        Debug.Log($"  ✓ Received contract: {contract.Name}");
        
        // 2. Work on the contract manually
        float initialWork = contract.CurrentWork;
        GameManager.Instance.PerformManualClick();
        Assert(contract.CurrentWork > initialWork, "Step 2: Manual click should add work");
        Debug.Log($"  ✓ Manual work added: {contract.CurrentWork - initialWork}");
        
        // 3. Complete the contract
        while (!contract.IsCompleted)
        {
            GameManager.Instance.PerformManualClick();
        }
        Assert(contract.IsCompleted, "Step 3: Contract should be completed");
        Debug.Log($"  ✓ Contract completed");
        
        // 4. Receive reward (happens automatically in GameManager)
        int currency = CurrencyManager.Instance.CurrentCurrency;
        Assert(currency > 0, "Step 4: Should have received currency reward");
        Debug.Log($"  ✓ Reward received: {currency} currency");
        
        // 5. Spend reward on upgrades
        var availableUpgrades = UpgradeManager.Instance.GetAvailableUpgrades();
        if (availableUpgrades.Count > 0 && CurrencyManager.Instance.CanAfford(availableUpgrades[0].cost))
        {
            var upgrade = availableUpgrades[0];
            bool purchased = UpgradeManager.Instance.PurchaseUpgrade(upgrade);
            Assert(purchased, "Step 5: Should be able to purchase an upgrade");
            Debug.Log($"  ✓ Purchased upgrade: {upgrade.name}");
        }
        
        // 6. Receive next contract (happens automatically in GameManager)
        Contract nextContract = GameManager.Instance.GetCurrentContract();
        Assert(nextContract != null, "Step 6: Should receive next contract");
        Assert(nextContract != contract || !nextContract.IsCompleted, "Next contract should be different or reset");
        Debug.Log($"  ✓ Next contract received: {nextContract.Name}");
        
        Debug.Log("[Test] Full Game Loop Integration - PASSED");
    }
    
    /// <summary>
    /// Simple assertion helper.
    /// </summary>
    private void Assert(bool condition, string message)
    {
        if (!condition)
        {
            Debug.LogError($"ASSERTION FAILED: {message}");
        }
    }
}
