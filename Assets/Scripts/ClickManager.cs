using UnityEngine;

/// <summary>
/// Manages manual clicking for working on contracts.
/// </summary>
public class ClickManager : MonoBehaviour
{
    private static ClickManager _instance;
    public static ClickManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ClickManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("ClickManager");
                    _instance = go.AddComponent<ClickManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }
    
    [Header("Click Settings")]
    [Tooltip("Base work done per click")]
    public float baseClickPower = 1f;
    
    private float clickPowerMultiplier = 1f;
    
    // Event triggered when a click is performed
    public event System.Action<float> OnClick;
    
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
    
    /// <summary>
    /// Performs a click, returning the amount of work done.
    /// </summary>
    public float PerformClick()
    {
        float workDone = baseClickPower * clickPowerMultiplier;
        OnClick?.Invoke(workDone);
        Debug.Log($"Click performed! Work done: {workDone}");
        return workDone;
    }
    
    /// <summary>
    /// Sets the click power multiplier (from upgrades).
    /// </summary>
    public void SetClickPowerMultiplier(float multiplier)
    {
        if (multiplier <= 0)
        {
            Debug.LogWarning("Click power multiplier must be positive.");
            return;
        }
        
        clickPowerMultiplier = multiplier;
        Debug.Log($"Click power multiplier set to: {multiplier}x");
    }
    
    /// <summary>
    /// Gets the current work per click.
    /// </summary>
    public float GetCurrentClickPower()
    {
        return baseClickPower * clickPowerMultiplier;
    }
}
