using UnityEngine;

/// <summary>
/// Manages passive automation for working on contracts.
/// Provides work per second without manual interaction.
/// </summary>
public class AutomationManager : MonoBehaviour
{
    private static AutomationManager _instance;
    public static AutomationManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AutomationManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("AutomationManager");
                    _instance = go.AddComponent<AutomationManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }
    
    [Header("Automation Settings")]
    [Tooltip("Base work done per second by automation")]
    public float baseWorkPerSecond = 0f;
    
    private float workPerSecondMultiplier = 1f;
    private bool isAutomationActive = false;
    
    // Event triggered when automation produces work
    public event System.Action<float> OnAutomationWork;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    private void Update()
    {
        if (isAutomationActive && baseWorkPerSecond > 0)
        {
            float workDone = baseWorkPerSecond * workPerSecondMultiplier * Time.deltaTime;
            OnAutomationWork?.Invoke(workDone);
        }
    }
    
    /// <summary>
    /// Sets the base work per second (from upgrades).
    /// </summary>
    public void SetBaseWorkPerSecond(float workPerSecond)
    {
        if (workPerSecond < 0)
        {
            Debug.LogWarning("Work per second cannot be negative.");
            return;
        }
        
        baseWorkPerSecond = workPerSecond;
        Debug.Log($"Base work per second set to: {baseWorkPerSecond}");
    }
    
    /// <summary>
    /// Sets the work per second multiplier (from upgrades).
    /// </summary>
    public void SetWorkPerSecondMultiplier(float multiplier)
    {
        if (multiplier <= 0)
        {
            Debug.LogWarning("Work per second multiplier must be positive.");
            return;
        }
        
        workPerSecondMultiplier = multiplier;
        Debug.Log($"Work per second multiplier set to: {multiplier}x");
    }
    
    /// <summary>
    /// Activates or deactivates automation.
    /// </summary>
    public void SetAutomationActive(bool active)
    {
        isAutomationActive = active;
        Debug.Log($"Automation {(active ? "activated" : "deactivated")}");
    }
    
    /// <summary>
    /// Gets the work per second multiplier.
    /// </summary>
    public float GetWorkPerSecondMultiplier()
    {
        return workPerSecondMultiplier;
    }
    
    /// <summary>
    /// Gets the current work per second.
    /// </summary>
    public float GetCurrentWorkPerSecond()
    {
        return baseWorkPerSecond * workPerSecondMultiplier;
    }
    
    /// <summary>
    /// Checks if automation is currently active.
    /// </summary>
    public bool IsActive()
    {
        return isAutomationActive;
    }
}
