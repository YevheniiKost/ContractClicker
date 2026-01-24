using UnityEngine;

/// <summary>
/// Represents a contract - the fundamental gameplay unit.
/// Tracks progress, completion state, and reward.
/// </summary>
public class Contract
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public float RequiredWork { get; private set; }
    public float CurrentWork { get; private set; }
    public int Reward { get; private set; }
    public bool IsCompleted { get; private set; }
    
    /// <summary>
    /// Creates a new contract with specified parameters.
    /// </summary>
    /// <param name="name">Contract name</param>
    /// <param name="description">Contract description</param>
    /// <param name="requiredWork">Amount of work needed to complete</param>
    /// <param name="reward">Currency reward upon completion</param>
    public Contract(string name, string description, float requiredWork, int reward)
    {
        Name = name;
        Description = description;
        RequiredWork = requiredWork;
        Reward = reward;
        CurrentWork = 0f;
        IsCompleted = false;
    }
    
    /// <summary>
    /// Adds work progress to the contract from manual clicks.
    /// </summary>
    /// <param name="amount">Amount of work to add</param>
    /// <returns>True if contract was completed by this work</returns>
    public bool AddWork(float amount)
    {
        if (IsCompleted)
            return false;
            
        CurrentWork += amount;
        
        if (CurrentWork >= RequiredWork)
        {
            CurrentWork = RequiredWork;
            IsCompleted = true;
            return true;
        }
        
        return false;
    }
    
    /// <summary>
    /// Gets the progress percentage (0-1).
    /// </summary>
    public float GetProgress()
    {
        return Mathf.Clamp01(CurrentWork / RequiredWork);
    }
    
    /// <summary>
    /// Gets the progress percentage as a string (0-100%).
    /// </summary>
    public string GetProgressString()
    {
        return $"{(GetProgress() * 100f):F1}%";
    }
}
