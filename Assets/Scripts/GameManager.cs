using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Main game manager that coordinates the core game loop.
/// Handles contract flow: receive → work → complete → reward → next contract.
/// </summary>
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    _instance = go.AddComponent<GameManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }
    
    [Header("Contract Settings")]
    [Tooltip("Difficulty multiplier for each subsequent contract")]
    public float difficultyIncrease = 1.2f;
    
    [Tooltip("Base reward for contracts")]
    public int baseReward = 10;
    
    [Tooltip("Reward multiplier for each subsequent contract")]
    public float rewardIncrease = 1.5f;
    
    private Contract currentContract;
    private int contractsCompleted = 0;
    
    // Events for game loop stages
    public event System.Action<Contract> OnContractReceived;
    public event System.Action<Contract> OnContractCompleted;
    public event System.Action<Contract, float> OnContractProgress;
    
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
    
    private void Start()
    {
        // Subscribe to click and automation events
        ClickManager.Instance.OnClick += OnClickPerformed;
        AutomationManager.Instance.OnAutomationWork += OnAutomationWorkPerformed;
        
        // Start the game loop by receiving the first contract
        ReceiveNextContract();
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from events
        if (ClickManager.Instance != null)
            ClickManager.Instance.OnClick -= OnClickPerformed;
        
        if (AutomationManager.Instance != null)
            AutomationManager.Instance.OnAutomationWork -= OnAutomationWorkPerformed;
    }
    
    /// <summary>
    /// Generates and receives the next contract based on current progress.
    /// </summary>
    public void ReceiveNextContract()
    {
        float requiredWork = 10f * Mathf.Pow(difficultyIncrease, contractsCompleted);
        int reward = Mathf.RoundToInt(baseReward * Mathf.Pow(rewardIncrease, contractsCompleted));
        
        currentContract = new Contract(
            $"Contract #{contractsCompleted + 1}",
            $"Complete this contract to earn {reward} currency",
            requiredWork,
            reward
        );
        
        Debug.Log($"New contract received: {currentContract.Name}");
        Debug.Log($"Required work: {currentContract.RequiredWork:F1}, Reward: {currentContract.Reward}");
        
        OnContractReceived?.Invoke(currentContract);
    }
    
    /// <summary>
    /// Handles manual click work on the current contract.
    /// </summary>
    private void OnClickPerformed(float workAmount)
    {
        if (currentContract != null && !currentContract.IsCompleted)
        {
            bool completed = currentContract.AddWork(workAmount);
            OnContractProgress?.Invoke(currentContract, workAmount);
            
            if (completed)
            {
                CompleteCurrentContract();
            }
        }
    }
    
    /// <summary>
    /// Handles passive automation work on the current contract.
    /// </summary>
    private void OnAutomationWorkPerformed(float workAmount)
    {
        if (currentContract != null && !currentContract.IsCompleted)
        {
            bool completed = currentContract.AddWork(workAmount);
            OnContractProgress?.Invoke(currentContract, workAmount);
            
            if (completed)
            {
                CompleteCurrentContract();
            }
        }
    }
    
    /// <summary>
    /// Completes the current contract and awards the reward.
    /// </summary>
    private void CompleteCurrentContract()
    {
        if (currentContract == null || !currentContract.IsCompleted)
            return;
        
        Debug.Log($"Contract completed: {currentContract.Name}");
        Debug.Log($"Reward earned: {currentContract.Reward} currency");
        
        // Award the reward
        CurrencyManager.Instance.AddCurrency(currentContract.Reward);
        
        // Trigger completion event
        OnContractCompleted?.Invoke(currentContract);
        
        contractsCompleted++;
        
        // Automatically receive the next contract
        ReceiveNextContract();
    }
    
    /// <summary>
    /// Manually performs a click on the current contract.
    /// This method should be called from UI or input handling.
    /// </summary>
    public void PerformManualClick()
    {
        if (currentContract != null && !currentContract.IsCompleted)
        {
            float workDone = ClickManager.Instance.PerformClick();
            // Work is automatically applied through the OnClick event
        }
        else
        {
            Debug.LogWarning("No active contract to work on.");
        }
    }
    
    /// <summary>
    /// Gets the current active contract.
    /// </summary>
    public Contract GetCurrentContract()
    {
        return currentContract;
    }
    
    /// <summary>
    /// Gets the number of contracts completed.
    /// </summary>
    public int GetContractsCompleted()
    {
        return contractsCompleted;
    }
    
    /// <summary>
    /// Resets the game to initial state.
    /// </summary>
    public void ResetGame()
    {
        contractsCompleted = 0;
        currentContract = null;
        
        // Reset managers
        CurrencyManager.Instance.SpendCurrency(CurrencyManager.Instance.CurrentCurrency);
        AutomationManager.Instance.SetBaseWorkPerSecond(0f);
        AutomationManager.Instance.SetAutomationActive(false);
        ClickManager.Instance.SetClickPowerMultiplier(1f);
        
        // Reset upgrades
        foreach (var upgrade in UpgradeManager.Instance.availableUpgrades)
        {
            upgrade.isPurchased = false;
        }
        
        ReceiveNextContract();
        Debug.Log("Game reset!");
    }
}
