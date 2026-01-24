using UnityEngine;

/// <summary>
/// Manages the currency/reward system.
/// Tracks player's current currency and handles transactions.
/// </summary>
public class CurrencyManager : MonoBehaviour
{
    private static CurrencyManager _instance;
    public static CurrencyManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CurrencyManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("CurrencyManager");
                    _instance = go.AddComponent<CurrencyManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }
    
    public int CurrentCurrency { get; private set; }
    
    // Event triggered when currency changes
    public event System.Action<int> OnCurrencyChanged;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
        CurrentCurrency = 0;
    }
    
    /// <summary>
    /// Adds currency (from completing contracts).
    /// </summary>
    public void AddCurrency(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot add negative currency. Use SpendCurrency instead.");
            return;
        }
        
        CurrentCurrency += amount;
        OnCurrencyChanged?.Invoke(CurrentCurrency);
        Debug.Log($"Currency added: +{amount}. Total: {CurrentCurrency}");
    }
    
    /// <summary>
    /// Attempts to spend currency (for upgrades).
    /// </summary>
    /// <returns>True if purchase was successful, false if not enough currency</returns>
    public bool SpendCurrency(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot spend negative currency.");
            return false;
        }
        
        if (CurrentCurrency >= amount)
        {
            CurrentCurrency -= amount;
            OnCurrencyChanged?.Invoke(CurrentCurrency);
            Debug.Log($"Currency spent: -{amount}. Remaining: {CurrentCurrency}");
            return true;
        }
        
        Debug.LogWarning($"Not enough currency. Need {amount}, have {CurrentCurrency}");
        return false;
    }
    
    /// <summary>
    /// Checks if player has enough currency for a purchase.
    /// </summary>
    public bool CanAfford(int amount)
    {
        return CurrentCurrency >= amount;
    }
}
