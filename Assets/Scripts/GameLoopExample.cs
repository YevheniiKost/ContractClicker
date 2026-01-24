using UnityEngine;

/// <summary>
/// Example script demonstrating the core game loop usage.
/// This can be attached to a GameObject in a Unity scene to test the system.
/// </summary>
public class GameLoopExample : MonoBehaviour
{
    [Header("Test Controls")]
    [Tooltip("Key to perform a manual click")]
    public KeyCode clickKey = KeyCode.Space;
    
    [Tooltip("Key to purchase the first available upgrade")]
    public KeyCode purchaseUpgradeKey = KeyCode.U;
    
    [Tooltip("Key to reset the game")]
    public KeyCode resetKey = KeyCode.R;
    
    private void Start()
    {
        // Subscribe to game events for logging
        GameManager.Instance.OnContractReceived += OnContractReceived;
        GameManager.Instance.OnContractProgress += OnContractProgress;
        GameManager.Instance.OnContractCompleted += OnContractCompleted;
        
        CurrencyManager.Instance.OnCurrencyChanged += OnCurrencyChanged;
        UpgradeManager.Instance.OnUpgradePurchased += OnUpgradePurchased;
        
        Debug.Log("=== Game Loop Example Started ===");
        Debug.Log($"Press {clickKey} to click and work on the contract");
        Debug.Log($"Press {purchaseUpgradeKey} to purchase an upgrade");
        Debug.Log($"Press {resetKey} to reset the game");
        Debug.Log("Automation will work passively once you purchase automation upgrades");
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from events
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnContractReceived -= OnContractReceived;
            GameManager.Instance.OnContractProgress -= OnContractProgress;
            GameManager.Instance.OnContractCompleted -= OnContractCompleted;
        }
        
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnCurrencyChanged -= OnCurrencyChanged;
        
        if (UpgradeManager.Instance != null)
            UpgradeManager.Instance.OnUpgradePurchased -= OnUpgradePurchased;
    }
    
    private void Update()
    {
        // Handle manual clicking
        if (Input.GetKeyDown(clickKey))
        {
            GameManager.Instance.PerformManualClick();
        }
        
        // Handle upgrade purchase
        if (Input.GetKeyDown(purchaseUpgradeKey))
        {
            TryPurchaseNextUpgrade();
        }
        
        // Handle game reset
        if (Input.GetKeyDown(resetKey))
        {
            GameManager.Instance.ResetGame();
        }
    }
    
    /// <summary>
    /// Tries to purchase the first available upgrade.
    /// </summary>
    private void TryPurchaseNextUpgrade()
    {
        var availableUpgrades = UpgradeManager.Instance.GetAvailableUpgrades();
        
        if (availableUpgrades.Count == 0)
        {
            Debug.Log("No upgrades available to purchase!");
            return;
        }
        
        var upgrade = availableUpgrades[0];
        Debug.Log($"Attempting to purchase: {upgrade.name} (Cost: {upgrade.cost})");
        UpgradeManager.Instance.PurchaseUpgrade(upgrade);
    }
    
    // Event handlers for logging game events
    
    private void OnContractReceived(Contract contract)
    {
        Debug.Log($"[EVENT] Contract Received: {contract.Name}");
        Debug.Log($"  Description: {contract.Description}");
        Debug.Log($"  Required Work: {contract.RequiredWork:F1}");
        Debug.Log($"  Reward: {contract.Reward}");
    }
    
    private void OnContractProgress(Contract contract, float workAdded)
    {
        // Only log every 10% progress to avoid spam
        float progress = contract.GetProgress();
        if (progress % 0.1f < 0.01f || contract.IsCompleted)
        {
            Debug.Log($"[EVENT] Contract Progress: {contract.GetProgressString()} ({contract.CurrentWork:F1}/{contract.RequiredWork:F1})");
        }
    }
    
    private void OnContractCompleted(Contract contract)
    {
        Debug.Log($"[EVENT] Contract Completed: {contract.Name}");
        Debug.Log($"  Reward Earned: {contract.Reward} currency");
        Debug.Log($"  Total Contracts Completed: {GameManager.Instance.GetContractsCompleted()}");
    }
    
    private void OnCurrencyChanged(int newAmount)
    {
        Debug.Log($"[EVENT] Currency Changed: {newAmount}");
    }
    
    private void OnUpgradePurchased(Upgrade upgrade)
    {
        Debug.Log($"[EVENT] Upgrade Purchased: {upgrade.name}");
        Debug.Log($"  Description: {upgrade.description}");
        Debug.Log($"  Current Click Power: {ClickManager.Instance.GetCurrentClickPower():F1}");
        Debug.Log($"  Current Automation: {AutomationManager.Instance.GetCurrentWorkPerSecond():F1} work/sec");
    }
    
    /// <summary>
    /// Displays current game state in the Unity Editor.
    /// </summary>
    private void OnGUI()
    {
        if (!Application.isPlaying)
            return;
        
        GUILayout.BeginArea(new Rect(10, 10, 400, 400));
        GUILayout.Box("=== Contract Clicker - Game State ===");
        
        GUILayout.Label($"Currency: {CurrencyManager.Instance.CurrentCurrency}");
        GUILayout.Label($"Contracts Completed: {GameManager.Instance.GetContractsCompleted()}");
        GUILayout.Label($"Click Power: {ClickManager.Instance.GetCurrentClickPower():F1}");
        GUILayout.Label($"Automation: {AutomationManager.Instance.GetCurrentWorkPerSecond():F1} work/sec");
        
        Contract current = GameManager.Instance.GetCurrentContract();
        if (current != null)
        {
            GUILayout.Space(10);
            GUILayout.Box("=== Current Contract ===");
            GUILayout.Label($"Name: {current.Name}");
            GUILayout.Label($"Progress: {current.GetProgressString()}");
            GUILayout.Label($"Work: {current.CurrentWork:F1} / {current.RequiredWork:F1}");
            GUILayout.Label($"Reward: {current.Reward}");
        }
        
        GUILayout.Space(10);
        GUILayout.Box("=== Controls ===");
        GUILayout.Label($"[{clickKey}] Click to work");
        GUILayout.Label($"[{purchaseUpgradeKey}] Purchase upgrade");
        GUILayout.Label($"[{resetKey}] Reset game");
        
        var availableUpgrades = UpgradeManager.Instance.GetAvailableUpgrades();
        if (availableUpgrades.Count > 0)
        {
            GUILayout.Space(10);
            GUILayout.Box("=== Available Upgrades ===");
            foreach (var upgrade in availableUpgrades)
            {
                bool canAfford = CurrencyManager.Instance.CanAfford(upgrade.cost);
                string affordText = canAfford ? "[CAN AFFORD]" : "[NEED MORE]";
                GUILayout.Label($"{affordText} {upgrade.name} - {upgrade.cost} currency");
            }
        }
        
        GUILayout.EndArea();
    }
}
