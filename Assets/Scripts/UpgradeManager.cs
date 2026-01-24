using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Represents an upgrade that can be purchased with currency.
/// </summary>
[System.Serializable]
public class Upgrade
{
    public string name;
    public string description;
    public int cost;
    public UpgradeType type;
    public float value;
    public bool isPurchased;
    
    public enum UpgradeType
    {
        ClickPowerMultiplier,
        AutomationWorkPerSecond,
        AutomationMultiplier
    }
}

/// <summary>
/// Manages upgrades that can be purchased with currency.
/// </summary>
public class UpgradeManager : MonoBehaviour
{
    private static UpgradeManager _instance;
    public static UpgradeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UpgradeManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("UpgradeManager");
                    _instance = go.AddComponent<UpgradeManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }
    
    [Header("Available Upgrades")]
    public List<Upgrade> availableUpgrades = new List<Upgrade>();
    
    // Event triggered when an upgrade is purchased
    public event System.Action<Upgrade> OnUpgradePurchased;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        InitializeDefaultUpgrades();
    }
    
    /// <summary>
    /// Initializes a set of default upgrades.
    /// </summary>
    private void InitializeDefaultUpgrades()
    {
        if (availableUpgrades.Count == 0)
        {
            availableUpgrades.Add(new Upgrade
            {
                name = "Better Mouse",
                description = "Doubles your click power",
                cost = 50,
                type = Upgrade.UpgradeType.ClickPowerMultiplier,
                value = 2f,
                isPurchased = false
            });
            
            availableUpgrades.Add(new Upgrade
            {
                name = "Auto-Clicker",
                description = "Generates 1 work per second automatically",
                cost = 100,
                type = Upgrade.UpgradeType.AutomationWorkPerSecond,
                value = 1f,
                isPurchased = false
            });
            
            availableUpgrades.Add(new Upgrade
            {
                name = "AI Assistant",
                description = "Generates 5 work per second automatically",
                cost = 500,
                type = Upgrade.UpgradeType.AutomationWorkPerSecond,
                value = 5f,
                isPurchased = false
            });
        }
    }
    
    /// <summary>
    /// Attempts to purchase an upgrade.
    /// </summary>
    /// <returns>True if purchase was successful</returns>
    public bool PurchaseUpgrade(Upgrade upgrade)
    {
        if (upgrade == null)
        {
            Debug.LogWarning("Cannot purchase null upgrade.");
            return false;
        }
        
        if (upgrade.isPurchased)
        {
            Debug.LogWarning($"Upgrade '{upgrade.name}' is already purchased.");
            return false;
        }
        
        if (!CurrencyManager.Instance.CanAfford(upgrade.cost))
        {
            Debug.LogWarning($"Cannot afford upgrade '{upgrade.name}'. Cost: {upgrade.cost}");
            return false;
        }
        
        if (CurrencyManager.Instance.SpendCurrency(upgrade.cost))
        {
            upgrade.isPurchased = true;
            ApplyUpgrade(upgrade);
            OnUpgradePurchased?.Invoke(upgrade);
            Debug.Log($"Purchased upgrade: {upgrade.name}");
            return true;
        }
        
        return false;
    }
    
    /// <summary>
    /// Applies the effect of an upgrade.
    /// </summary>
    private void ApplyUpgrade(Upgrade upgrade)
    {
        switch (upgrade.type)
        {
            case Upgrade.UpgradeType.ClickPowerMultiplier:
                float currentClickMultiplier = ClickManager.Instance.GetCurrentClickPower() / ClickManager.Instance.baseClickPower;
                ClickManager.Instance.SetClickPowerMultiplier(currentClickMultiplier * upgrade.value);
                break;
                
            case Upgrade.UpgradeType.AutomationWorkPerSecond:
                AutomationManager.Instance.SetBaseWorkPerSecond(
                    AutomationManager.Instance.baseWorkPerSecond + upgrade.value
                );
                AutomationManager.Instance.SetAutomationActive(true);
                break;
                
            case Upgrade.UpgradeType.AutomationMultiplier:
                float currentAutoMultiplier = AutomationManager.Instance.GetCurrentWorkPerSecond() / 
                    (AutomationManager.Instance.baseWorkPerSecond > 0 ? AutomationManager.Instance.baseWorkPerSecond : 1f);
                AutomationManager.Instance.SetWorkPerSecondMultiplier(currentAutoMultiplier * upgrade.value);
                break;
        }
    }
    
    /// <summary>
    /// Gets all available upgrades that haven't been purchased.
    /// </summary>
    public List<Upgrade> GetAvailableUpgrades()
    {
        return availableUpgrades.FindAll(u => !u.isPurchased);
    }
    
    /// <summary>
    /// Gets all purchased upgrades.
    /// </summary>
    public List<Upgrade> GetPurchasedUpgrades()
    {
        return availableUpgrades.FindAll(u => u.isPurchased);
    }
}
